namespace MainProject
{
    public class LibraryDB_Log
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

        private Func<dynamic, dynamic, bool> stringEquals = (d, k) => String.Equals(d, k);
        private Func<dynamic, dynamic, bool> simpleEquals = (d, k) => d == k;


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
                if (log.logOrder >= maxLogOrder)
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
            var userQ = localDBmodel_user.Find("userPKey", logVO.userPKey, stringEquals);
            if (userQ.Count == 0) { return 1; }
            var userVO = userQ[0];

            // validate if bookPKey exists.
            var bookQ = localDBmodel_book.Find("bookPKey", logVO.bookPKey, stringEquals);
            if (bookQ.Count == 0) { return 1; }
            var bookVO = bookQ[0];

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
        /// add the log.
        /// </summary>
        /// <param name="logVO"></param>
        /// <returns>
        /// return 0 : succeed
        /// return -1 : wrong borrowing / returning
        /// return 1 : no user or book foregin key matched
        /// return 2 : empty string
        /// </returns>
        public int LogAdd(LogVO logVO)
        {
            // auto-validation
            int validation = LogValidate(logVO);
            if (validation != 0) {return validation;}

            // change the returnLeft
            // // find the log that has  userPKey bookPKey same.
            var maxLog = MaxLogOrderMatched(logVO.userPKey, logVO.bookPKey);

            if (maxLog == null) // no VO have userPKey bookPKey same
            {
                logVO.returnLeft = 0;
                logVO.logOrder = 0;
            }
            else
            {
                logVO.returnLeft = maxLog.returnLeft - logVO.count;
                logVO.logOrder = maxLog.logOrder + 1;
            }

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
            return localDBmodel.GetAll();
        }

        public List<List<string>> LogAllString()
        {
            return ListVOStringify(LogAll());
        }


        /// <summary>
        /// search for all the book that user borrowed before.
        /// if user has at least one book should return, this function will let you know
        /// by [bookVO.primaryKey, logVO.returnLeft] .
        /// </summary>
        /// <returns>
        /// List<[bookVO.primaryKey, logVO.returnLeft]> : succeed
        /// null : failed
        /// </returns>
        public List<int[]>? UserBorrowedFind(int userPkey)
        {
            // search for the all book that user had borrowed.
            var query = localDBmodel.Find("userPKey", userPkey, simpleEquals);
            if (query.Count == 0) { return null; }

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
                    index = bookPkeyList.Find(x => x == log.bookPKey);
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
            var result = new List<int[]>();

            for (int i=0; i < logOrderList.Count; i++)
            {
                if (returnLeftList[i] != 0)
                {
                    result.Add(new int[] {bookPkeyList[i], returnLeftList[i]});
                }
            }

            return result;
        }
    }
}