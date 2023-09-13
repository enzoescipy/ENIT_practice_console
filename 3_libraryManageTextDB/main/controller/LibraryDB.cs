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
        private LibraryDB() {}

        public LibraryDB_User user = LibraryDB_User.userDB;
        public LibraryDB_Book book = LibraryDB_Book.bookDB;
        public LibraryDB_Log log = LibraryDB_Log.logDB;
    }
}