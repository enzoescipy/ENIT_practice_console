namespace MainProject
{
    public class LibraryDB_Log
    {
        private static LibraryDB_Log? _logDB = null;
        public static LibraryDB_Log logDB
        {
            get
            {
                if (_logDB == null)
                {
                    _logDB = new LibraryDB_Log();
                }
                return _logDB;
            }
        }
        
        private LibraryDB_Log() {}
        public void hello()
        {
            Console.WriteLine("hello, world!");
        }
    }
}