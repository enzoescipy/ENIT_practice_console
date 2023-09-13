namespace MainProject
{
    public class LibraryDB_User
    {
        private static LibraryDB_User? _userDB = null;
        public static LibraryDB_User userDB
        {
            get
            {
                if (_userDB == null)
                {
                    _userDB = new LibraryDB_User();
                }
                return _userDB;
            }
        }
        
        private LibraryDB_User() {}
        public void hello()
        {
            Console.WriteLine("hello, world!");
        }
    }
}