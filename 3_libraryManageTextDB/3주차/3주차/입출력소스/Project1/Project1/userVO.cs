using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    [Serializable]
    class UserVO
    {
        private string userID;
        private string userPassword;
        private string userName;        //이름    
        private string userAge;         //나이
        private string userPhoneNumber; //폰넘버
        private string userAddress;     //주소
        private int userBorrowBookCount;
        public List<string> userBorrowBookList = new List<string>();

        public UserVO(string userId, string userPassword, string userName, string userAge, string userPhoneNumber, string userAddress, int userBorrowBookCount) {

            this.userID = userId;
            this.userPassword = userPassword;
            this.userName = userName;
            this.userAge = userAge;
            this.userPhoneNumber= userPhoneNumber;
            this.userAddress = userAddress;
            this.userBorrowBookCount= userBorrowBookCount;
            this.userBorrowBookList.Clear();

        
        }

        public UserVO() { userBorrowBookCount = 0; this.userBorrowBookList = new List<string>(); }

        public void UserBorrowBookList(string bookName)
        {
            if (userBorrowBookList == null)
                this.userBorrowBookList = new List<string>();
            this.userBorrowBookList.Add(bookName);
        }
        public int UserBorrowBookCount
        {
            get { return userBorrowBookCount; }
            set { userBorrowBookCount = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string UserAge
        {
            get { return userAge; }
            set { userAge = value; }
        }

        public string UserPhoneNumber
        {
            get { return userPhoneNumber; }
            set { userPhoneNumber = value; }
        }

        public string UserAddress
        {
            get { return userAddress; }
            set { userAddress = value; }
        }

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string UserPassword
        {
            get { return userPassword; }
            set { userPassword = value; }
        }

        public override string ToString()
        {
            return userName;
        }

    }
}
