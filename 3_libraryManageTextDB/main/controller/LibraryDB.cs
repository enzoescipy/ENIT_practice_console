namespace MainProject
{
    public class LibraryDB : LibraryDBPart
    {
        private static LibraryDB? _libraryDB = null;
        public static LibraryDB libraryDB
        {
            get
            {
                if (_libraryDB == null)
                {
                    _libraryDB = new LibraryDB();
                }
                return _libraryDB;
            }
        }
        private LibraryDB() 
        {
            // create each of LocalDBmodel
            localDBmodel_user = new LocalDBmodel<UserVO>("data/userDB.db");
            localDBmodel_book = new LocalDBmodel<BookVO>("data/bookDB.db");
            localDBmodel_log = new LocalDBmodel<LogVO>("data/logDB.db");

            // invoke the LibraryDB partition then put LocalDBmodel
            this.user = new LibraryDB_User(localDBmodel_user);
            this.book = new LibraryDB_Book(localDBmodel_book);
            this.log = new LibraryDB_Log(localDBmodel_log, localDBmodel_user, localDBmodel_book);
        }
        private LocalDBmodel<UserVO> localDBmodel_user;
        private LocalDBmodel<BookVO> localDBmodel_book;
        private LocalDBmodel<LogVO> localDBmodel_log;
        public LibraryDB_User user; // = LibraryDB_User.userDB;
        public LibraryDB_Book book; // = LibraryDB_Book.bookDB;
        public LibraryDB_Log log; // = LibraryDB_Log.logDB;

        /// <summary>
        /// borrow the book by user.
        /// this method makes the both logDB and bookDB changes together.
        /// </summary>
        /// <param name="userPkey"></param>
        /// <param name="bookName"></param>
        /// <param name="amount"></param>
        /// <returns>
        /// return 0 : succeed
        /// return -1 : amount is invalid by some fallacy
        /// return 1 : no user or bookname found
        /// </returns>
        public int Borrow(int userPkey, string bookName, int amount)
        {
            // validate if userPKey exists.
            var userQ = localDBmodel_user.Find("primaryKey", userPkey, simpleEquals);
            if (userQ.Count == 0) { return 1; }
            var userVO = userQ[0];

            // validate if bookname exists.
            var bookQ = localDBmodel_book.Find("name", bookName, stringEquals);
            if (bookQ.Count == 0) { return 1; }
            var bookVO = bookQ[0];

            // validate & fix the log
            var newLogVO = new LogVO(userPkey, bookVO.primaryKey, true, DateTime.Now.ToString(), amount);
            var logState = log.LogValidate(newLogVO);
            if (logState != 0)
            {
                return logState;
            }
            log.LogFix(newLogVO);

            // borrowing amount cannot exceed the current stock
            if (amount > bookVO.currentStock) { return -1; }

            // change currentStock
            bookVO.currentStock = bookVO.currentStock - amount;


            // overrides the bookVO, add log.
            localDBmodel_log.Append(new List<LogVO>() {newLogVO});
            localDBmodel_book.Override(bookQ);
            return 0;
        }

        /// <summary>
        /// return the book by user.
        /// this method makes the both logDB and bookDB changes together.
        /// </summary>
        /// <param name="userPkey"></param>
        /// <param name="bookName"></param>
        /// <param name="amount"></param>
        /// <returns>
        /// return 0 : succeed
        /// return -1 : amount is invalid by some fallacy
        /// return 1 : no user or bookname found
        /// </returns>
        public int Return(int userPkey, string bookName, int amount)
        {
            // validate if userPKey exists.
            var userQ = localDBmodel_user.Find("primaryKey", userPkey, simpleEquals);
            if (userQ.Count == 0) { return 1; }
            var userVO = userQ[0];

            // validate if bookname exists.
            var bookQ = localDBmodel_book.Find("name", bookName, stringEquals);
            if (bookQ.Count == 0) { return 1; }
            var bookVO = bookQ[0];

            //  validate & fix the log
            var newLogVO = new LogVO(userPkey, bookVO.primaryKey, false, DateTime.Now.ToString(), amount);
            var logState = log.LogValidate(newLogVO);
            if (logState != 0)
            {
                return logState;
            }
            log.LogFix(newLogVO);

            // returning amount cannot exceed the free stock (initStock - currentStock)
            if (amount > bookVO.initStock - bookVO.currentStock) {return -1;}

            // change currentStock
            bookVO.currentStock = bookVO.currentStock + amount;
            
            // overrides the bookVO, add log.
            localDBmodel_log.Append(new List<LogVO>() {newLogVO});
            localDBmodel_book.Override(bookQ);


            return 0;
        }
    }

    public class LibraryDBPart
    {
        public Func<dynamic, dynamic, bool> stringEquals = (d, k) => String.Equals(d, k);
        public Func<dynamic, dynamic, bool> simpleEquals = (d, k) => d == k;
        public bool KeywordSearch(dynamic targetString, dynamic keyword)
        {
            string sTargetString = targetString.ToString();
            string sKeyword = keyword.ToString();

            return sTargetString.Contains(sKeyword);
        }
    }
}