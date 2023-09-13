using System.ComponentModel;

namespace MainProject
{
    [Serializable]
    public class BasicVO
    {
        public BasicVO() 
        {
        }

        public virtual bool HasNull() { return false; }
        public int primaryKey;
    }

    [Serializable]
    public class UserVO : BasicVO
    {
        public UserVO(string? id, string? password, string? email)
        {
            this.id = id;
            this.password = password;
            this.email = email;
        }

        public UserVO(int primaryKey, string? id, string? password, string? email)
        {
            this.primaryKey = primaryKey;
            this.id = id;
            this.password = password;
            this.email = email;
        }

        public UserVO() {}

        public override bool HasNull()
        {
            if (id == null || password == null || email == null)
            {
                return true;
            }
            return false;
        }
        public string? id;
        public string? password;
        public string? email;
    }

    [Serializable]
    public class BookVO : BasicVO
    {
        public BookVO(string? name, string? description, int? initStock, int? currentStock)
        {
            this.name = name;
            this.description = description;
            this.initStock = initStock;
            this.currentStock = currentStock;
        }
        public BookVO(int primaryKey, string? name, string? description, int? initStock, int? currentStock)
        {
            this.primaryKey = primaryKey;
            this.name = name;
            this.description = description;
            this.initStock = initStock;
            this.currentStock = currentStock;
        }
        public BookVO() {}
        public override bool HasNull()
        {
            if (name == null || description == null || initStock == null || currentStock == null)
            {
                return true;
            }
            return false;
        }
        public string? name;
        public string? description;
        public int? initStock;
        public int? currentStock;
    }

    [Serializable]
    public class LogVO : BasicVO
    {
        public LogVO(int? userPKey, int? bookPKey, bool? isBorrow, string? time, int? count)
        {
            this.userPKey = userPKey;
            this.bookPKey = bookPKey;
            this.isBorrow = isBorrow;
            this.time = time;
            this.count = count;
        }

        public LogVO(int primaryKey, int? userPKey, int? bookPKey, bool? isBorrow, string? time, int? count)
        {
            this.primaryKey = primaryKey;
            this.userPKey = userPKey;
            this.bookPKey = bookPKey;
            this.isBorrow = isBorrow;
            this.time = time;
            this.count = count;
        }   

        public LogVO() {}

        public override bool HasNull()
        {
            if (userPKey == null || bookPKey == null || isBorrow == null || time == null || count == null)
            {
                return true;
            }
            return false;
        }
        public int? userPKey;
        public int? bookPKey;
        public bool? isBorrow;
        public string? time;
        public int? count;

    }
}