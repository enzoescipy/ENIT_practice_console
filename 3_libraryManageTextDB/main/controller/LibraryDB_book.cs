namespace MainProject
{
    public class LibraryDB_Book
    {
        private static LibraryDB_Book? _bookDB = null;
        public static LibraryDB_Book bookDB
        {
            get
            {
                if (_bookDB == null)
                {
                    _bookDB = new LibraryDB_Book();
                }
                return _bookDB;
            }
        }
        
        private LibraryDB_Book() {}

        public void hello()
        {
            Console.WriteLine("hello, world!");
        }
    }
}