using System.ComponentModel;

namespace MainProject
{
    [Serializable]
    public class BasicVO
    {
        public BasicVO() 
        {
        }

        public int pkey;
    }

    [Serializable]
    public class UserVO : BasicVO
    {
        public UserVO() 
        {
            this.id = "";
            this.password = "";
            this.email = "";
        }
        public UserVO(string id, string password, string email)
        {
            this.id = id;
            this.password = password;
            this.email = email;
        }

        public UserVO(int pkey, string id, string password, string email)
        {
            this.pkey = pkey;
            this.id = id;
            this.password = password;
            this.email = email;
        }

        public string id;
        public string password;
        public string email;
    }

    [Serializable]
    public class BookVO : BasicVO
    {
        public BookVO()
        {
            this.name = "";
            this.description =  "";
        }
        public BookVO(string name, string description, int initStock)
        {
            this.name = name;
            this.description = description;
            this.initStock = initStock;
        }
        public BookVO(int pkey, string name, string description, int initStock)
        {
            this.pkey = pkey;
            this.name = name;
            this.description = description;
            this.initStock = initStock;
        }
        public string name;
        public string description;
        public int initStock;
        public int currentStock;
    }

    [Serializable]
    public class LogVO : BasicVO
    {
        public LogVO()
        {
            this.time = "";
        }
        public LogVO(int userPKey, int bookPKey, bool isBorrow, string time, int count)
        {
            this.userPKey = userPKey;
            this.bookPKey = bookPKey;
            this.isBorrow = isBorrow;
            this.time = time;
            this.count = count;
        }

        public LogVO(int pkey, int userPKey, int bookPKey, bool isBorrow, string time, int count)
        {
            this.pkey = pkey;
            this.userPKey = userPKey;
            this.bookPKey = bookPKey;
            this.isBorrow = isBorrow;
            this.time = time;
            this.count = count;
        }   

        public int userPKey;
        public int bookPKey;
        public bool isBorrow;
        public string time;
        public int count;
        public int returnLeft; // how many book you have to return
        public int logOrder; // time order of the log, which count separately by userPkey and bookPkey.
    }
}