namespace MainProject
{
    public class LibraryDB
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
            LocalDBmodel<UserVO> localDBmodel_user = new LocalDBmodel<UserVO>("data/userDB.db");
            LocalDBmodel<BookVO> localDBmodel_book = new LocalDBmodel<BookVO>("data/bookDB.db");
            LocalDBmodel<LogVO> localDBmodel_log = new LocalDBmodel<LogVO>("data/logDB.db");

            // invoke the LibraryDB partition then put LocalDBmodel
            this.user = new LibraryDB_User(localDBmodel_user);
            this.book = new LibraryDB_Book(localDBmodel_book);
            this.log = new LibraryDB_Log(localDBmodel_log, localDBmodel_user, localDBmodel_book);
        }
        public LibraryDB_User user; // = LibraryDB_User.userDB;
        public LibraryDB_Book book; // = LibraryDB_Book.bookDB;
        public LibraryDB_Log log; // = LibraryDB_Log.logDB;
    }
}