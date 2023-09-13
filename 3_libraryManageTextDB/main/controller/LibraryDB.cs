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

        public static LibraryDB_Book book = LibraryDB_Book.bookDB;
    }
}