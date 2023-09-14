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

        public int LogValidate(LogVO logVO)
        {
            // validate if hasNull.
            if (logVO.HasNull()) { return -1; }

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
                if (logVO.count < 0 || logVO.count > bookVO.currentStock) { return 2;}
            }
            else
            {
                // returning count cannot exceed the returning left count.
                // // find the log that has  userPKey bookPKey same.
                LogSearch(logVO.userPKey, logVO.bookPKey)
            }




        }
    }
}