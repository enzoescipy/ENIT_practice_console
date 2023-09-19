using System.Diagnostics;

namespace MainProject
{
    public class LibraryDB_Log : LibraryDBPart
    {   
        public LibraryDB_Log(LocalDBmodel<LogVO> localDBmodel,
                            LocalDBmodel<UserVO> localDBmodel_user,
                            LocalDBmodel<BookVO> localDBmodel_book) 
        {
            this.localDBmodel = localDBmodel;
            this.localDBmodel_user = localDBmodel_user;
            this.localDBmodel_book = localDBmodel_book;
        }
        private LocalDBmodel<LogVO> localDBmodel;
        private LocalDBmodel<UserVO> localDBmodel_user;
        private LocalDBmodel<BookVO> localDBmodel_book;
        


        /// <summary>
        /// find the log that has userPKey, bookPKey matched
        /// </summary>
        /// <param name="userPKey"></param>
        /// <param name="bookPKey"></param>
        /// <returns></returns>
        public List<LogVO> LogSearch(int userPKey, int bookPKey)
        {
            List<LogVO> result = new List<LogVO>();

            var bookPkeySearched = localDBmodel.Find("bookPKey", bookPKey, simpleEquals);
            foreach (var log in bookPkeySearched)
            {
                if (log.userPKey == userPKey)
                {
                    result.Add(log);
                }
            }
            return result;
        }

        /// <summary>
        /// find the log that has userPKey, bookPKey matched
        /// then return the highest logOrder one.
        /// </summary>
        /// <param name="userPKey"></param>
        /// <param name="bookPKey"></param>
        /// <returns>
        /// LogVO : succeed
        /// null : no matched
        /// </returns>
        public LogVO? MaxLogOrderMatched(int userPKey, int bookPKey)
        {
            var logQ = LogSearch(userPKey, bookPKey);

            if (logQ.Count == 0) { return null; }

            int maxLogOrder = 0;
            LogVO? maxLog = null;
            foreach (var log in logQ)
            {
                if (log.logOrder >= maxLogOrder )
                {
                    maxLogOrder = log.logOrder;
                    maxLog = log;
                }
            }

            if (maxLog == null) {throw new Exception("unexpected list counting happens.");}
            
            return maxLog;
        }

        /// <summary>
        /// validate the log.
        /// - do the basic empty check
        /// - check if the borrowing / returning amount is not wrong with the log history
        /// - validation target is, logVO.userPKey, logVO.bookPKey, logVO.count .
        /// </summary>
        /// <param name="logVO"></param>
        /// <returns>
        /// return 0 : succeed
        /// return -1 : wrong borrowing / returning
        /// return 1 : no user or book foregin key matched
        /// return 2 : empty string
        /// </returns>
        public int LogValidate(LogVO logVO)
        {
            // validate if userPKey exists.
            var userQ = localDBmodel_user.Find("primaryKey", logVO.userPKey, simpleEquals);
            if (userQ.Count == 0) { return 1; }
            var userVO = userQ[0];

            // validate if bookPKey exists.
            var bookQ = localDBmodel_book.Find("primaryKey", logVO.bookPKey, simpleEquals);
            if (bookQ.Count == 0) { return 1; }
            var bookVO = bookQ[0];

            return LogValidate(logVO, bookVO);
        }

        /// <summary>
        /// validate the log.
        /// - do NOT CHECK if the userVO-userPKey, bookVO-bookPKey exists.
        /// - check if the borrowing / returning amount is not wrong with the log history
        /// - this overload just check  logVO.count only.
        /// </summary>
        /// <param name="logVO"></param>
        /// <param name="userVO"></param>
        /// <param name="bookVO"></param>
        /// <returns>
        /// return 0 : succeed
        /// return -1 : wrong borrowing / returning
        /// return 2 : empty string
        /// </returns>
        public int LogValidate(LogVO logVO, BookVO bookVO)
        {
            // validate if time string is not empty
            if (logVO.time.Length == 0) { return 2; }

            // validate by borrowing case
            if (logVO.isBorrow == true)
            {
                // borrowing count cannot exceed the book stock
                if (logVO.count < 0 || logVO.count > bookVO.currentStock) { return -1;}
            }
            else
            {
                // returning count cannot exceed the returning left count.
                // // find the log that has  userPKey bookPKey same.
                var maxLog = MaxLogOrderMatched(logVO.userPKey, logVO.bookPKey);
                
                if (maxLog == null) { return -1; } // no history for borrowing but you are trying to return the book

                if ( logVO.count < 0 || logVO.count > maxLog.returnLeft )
                {
                    return -1;
                }
            }

            // passed all test!
            return 0;
        } 
        
        /// <summary>
        /// override the logVO's returnLeft, logOrder parameter properly.
        /// </summary>
        /// <param name="logVO"></param>
        public void LogFix(LogVO logVO)
        {
            var maxLog = MaxLogOrderMatched(logVO.userPKey, logVO.bookPKey);
            int returnLeftDiff = logVO.isBorrow ? logVO.count : -logVO.count;

            if (maxLog == null) // no VO have userPKey bookPKey same
            {
                logVO.returnLeft = returnLeftDiff;
                logVO.logOrder = 0;
            }
            else
            {
                logVO.returnLeft = maxLog.returnLeft + returnLeftDiff;
                logVO.logOrder = maxLog.logOrder + 1;
            }
        }


        /// <summary>
        /// add the log.
        /// </summary>
        /// <param name="logVO"></param>
        /// <returns>
        /// return 0 : succeed
        /// return -1 : wrong borrowing / returning
        /// return 1 : no user or book foregin key matched
        /// return 2 : empty string
        /// </returns>
        private int LogAdd(LogVO logVO)
        {
            // auto-validation
            int validation = LogValidate(logVO);
            if (validation != 0) {return validation;}

            // change the returnLeft
            // // find the log that has  userPKey bookPKey same.
            LogFix(logVO);

            // append DB.
            localDBmodel.Append(new List<LogVO>() {logVO});
            return 0;
        }

        public List<List<string>> ListVOStringify(List<LogVO> voTable)
        {
            List<string> tableColumn = new List<string>() {"USER_EMAIL", "BOOK_NAME", "TYPE", "AMOUNT","TOTAL_BORROWED", "TIME"};
            List<List<string>> showableTable = new List<List<string>>() {tableColumn};

            foreach(LogVO log in voTable)
            {
                List<string> line = new List<string>();

                var user = localDBmodel_user.Find("primaryKey", log.userPKey, simpleEquals)[0];
                var book = localDBmodel_book.Find("primaryKey", log.bookPKey, simpleEquals)[0];
                line.Add(user.email);
                line.Add(book.name);
                line.Add(log.isBorrow ? "BORROW" : "RETURN");
                line.Add(log.count.ToString());
                line.Add(log.returnLeft.ToString());
                line.Add(log.time);

                showableTable.Add(line);
            }

            return showableTable;
        }

        public List<LogVO> LogAll()
        {
            var result = localDBmodel.GetAll();
            return result;
        }

        public List<List<string>> LogAllString()
        {
            return ListVOStringify(LogAll());
        }


        /// <summary>
        /// search for all the book that user borrowed before.
        /// if user has at least one book should return, this function will let you know
        /// by [bookVO.name, logVO.returnLeft] .
        /// </summary>
        /// <param name="userPkey"></param>
        /// <returns>
        /// List [bookVO.name, logVO.returnLeft]  : succeed
        /// </returns>
        public List<dynamic[]> UserBorrowedFind(int userPkey)
        {
            // search for the all book that user had borrowed.
            var query = localDBmodel.Find("userPKey", userPkey, simpleEquals);
            if (query.Count == 0) { return new List<dynamic[]>(); }

            // iterate the list to find the book + user log
            // use the accumulation list method.
            var bookPkeyList = new List<int>();
            var logOrderList = new List<int>();
            var returnLeftList = new List<int>();
            foreach (LogVO log in query)
            {
                
                // if there is already accumulated book exists
                int index = -1;
                if (bookPkeyList.Contains(log.bookPKey))
                {
                    index = bookPkeyList.IndexOf(log.bookPKey);
                    if (logOrderList[index] < log.logOrder)
                    {
                        logOrderList[index] = log.logOrder;
                        returnLeftList[index] = log.returnLeft;
                    }
                }
                else
                {
                    // if there is no accumulated book exists
                    bookPkeyList.Add(log.bookPKey);
                    logOrderList.Add(log.logOrder);
                    returnLeftList.Add(log.returnLeft);
                }
            }


            // packing lists to return value
            var result = new List<dynamic[]>();

            for (int i=0; i < logOrderList.Count; i++)
            {
                if (returnLeftList[i] != 0)
                {
                    var bookQ = localDBmodel_book.Find("primaryKey", bookPkeyList[i], simpleEquals);
                    result.Add(new dynamic[] {bookQ[0].name, returnLeftList[i]});
                }
            }

            return result;
        }
        
        /// <summary>
        /// search for the all user who  had borrowed that book.
        /// if book had at least once borrowed by someone, this function will let you know
        /// by [userVO.email, logVO.returnLeft] .
        /// </summary>
        /// <param name="bookPkey"></param>
        /// <returns>
        /// List [userVO.email, logVO.returnLeft]  : succeed
        /// </returns>
        public List<dynamic[]>? BookBorrowedFind(int bookPkey)
        {
            // search for the all user who  had borrowed that book.
            var query = localDBmodel.Find("bookPKey", bookPkey, simpleEquals);
            if (query.Count == 0) { return new List<dynamic[]>(); }

            // iterate the list to find the book + user log
            // use the accumulation list method.
            var userPkeyList = new List<int>();
            var logOrderList = new List<int>();
            var returnLeftList = new List<int>();
            foreach (LogVO log in query)
            {
                // if there is already accumulated book exists
                int index = -1;
                if (userPkeyList.Contains(log.userPKey))
                {
                    index = userPkeyList.IndexOf(log.userPKey);
                    if (logOrderList[index] < log.logOrder)
                    {
                        logOrderList[index] = log.logOrder;
                        returnLeftList[index] = log.returnLeft;
                    }
                }
                else
                {
                    // if there is no accumulated book exists
                    userPkeyList.Add(log.userPKey);
                    logOrderList.Add(log.logOrder);
                    returnLeftList.Add(log.returnLeft);
                }
            }

            // packing lists to return value
            var result = new List<dynamic[]>();

            for (int i=0; i < logOrderList.Count; i++)
            {
                if (returnLeftList[i] != 0)
                {
                    var userQ = localDBmodel_user.Find("primaryKey", userPkeyList[i], simpleEquals);
                    result.Add(new dynamic[] {userQ[0].email, returnLeftList[i]});
                }
            }

            return result;
        }

        public int DeleteLogForBook(string bookName)
        {
            var bookQ = localDBmodel_book.Find("name", bookName, stringEquals);
            if (bookQ.Count == 0) { return 1; }

            var logQ = localDBmodel.Find("bookPKey", bookQ[0].primaryKey, simpleEquals);
            var deleteTargetLogs = (from log in logQ select log.primaryKey).ToList();
            localDBmodel.Delete(deleteTargetLogs);
            return 0;

        }
    }
}