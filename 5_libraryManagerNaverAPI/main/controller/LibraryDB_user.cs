namespace MainProject
{
    public class LibraryDB_User 
    {   
        public LibraryDB_User(LocalDBmodel<UserVO> localDBmodel) 
        {
            this.localDBmodel = localDBmodel;
        }
        private LocalDBmodel<UserVO> localDBmodel;
        // convinent constants
        /// <summary>
        /// Add the new user to DB.
        /// </summary>
        /// <param name="userVO"></param>
        /// <returns>
        /// return 0 : success code
        /// return -1 : nullable VO.
        /// return c : id already exists
        /// return 2 : email already exists
        /// return 3 : some field is the blank value.
        /// </returns>
        public int UserAdd(UserVO userVO)
        {
            // validate if id is new id
            var query = localDBmodel.Find("id", userVO.id, 0);
            if (query.Count != 0) { return 1; }

            // validate if id is not empty
            if (userVO.id.Length == 0) { return 3;}

            // validate if email is new email
            query = localDBmodel.Find("email", userVO.email, 0);
            if (query.Count != 0) { return 2; }

            // validate if email is not empty
            if (userVO.email.Length == 0) { return 3;}

            // validate if password is not empty
            if (userVO.password.Length == 0) { return 3;}

            // append DB.
            localDBmodel.Append(new List<UserVO>() {userVO});
            return 0;
        }

        /// <summary>
        /// check if id, password matching user is exist
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns>
        /// return UserVO : user exists
        /// return null : no user found
        /// </returns>
        public UserVO? UserLogin(string id, string password)
        {
            // find id there is VO that has the same id
            var query = localDBmodel.Find("id", id, 0);
            if (query.Count == 0) { return null; }
            else if (query.Count > 1) { throw new InvalidDataException("id-matching VO must be unique."); }

            // check if found VO has matching password
            var voFound = query[0];
            if (!String.Equals(password, voFound.password)) { return null; }

            return voFound;
        }

        /// <summary>
        /// stringify the UserVO List to 2D list of string.
        /// </summary>
        /// <param name="voTable"></param>
        /// <returns></returns>
        public List<List<string>> ListVOStringify(List<UserVO> voTable)
        {
            List<string> tableColumn = new List<string>() {"EMAIL", "ID", "PASSWORD"};
            List<List<string>> showableTable = new List<List<string>>() {tableColumn};

            foreach(UserVO user in voTable)
            {
                List<string> line = new List<string>();
                line.Add(user.email);
                line.Add(user.id);
                line.Add(user.password);

                showableTable.Add(line);
            }

            return showableTable;
        }
        public List<UserVO> UserAll()
        {
            return localDBmodel.GetAll();
        }

        public List<List<string>> UserAllString()
        {
            return ListVOStringify(UserAll());
        }

        public List<UserVO> UserSearch(string email)
        {
            return localDBmodel.Find("email", email, 1);
        }

        public List<List<string>> UserSearchString(string email)
        {
            var get = UserSearch(email);
            return ListVOStringify(get);
        }

        /// <summary>
        /// change the target pKey VO user's email.
        /// </summary>
        /// <param name="pKey"></param>
        /// <param name="email"></param>
        /// <returns>
        /// return 0 : success
        /// return -1 : no user exist
        /// </returns>
        /// <exception cref="InvalidDataException"></exception>
        public int UserEmailChange(int pKey, string email)
        {
            // find user.
            List<UserVO> query = localDBmodel.Find("pkey", pKey, 0);
            if (query.Count == 0) { return -1; }
            else if (query.Count > 1) { throw new InvalidDataException("pkey-matching VO must be unique."); }

            // override the user info with email.
            query[0].email = email;
            localDBmodel.Override(query);
            return 0;
        }

        /// <summary>
        /// change user password. require the double-check current password.
        /// </summary>
        /// <param name="pKey"></param>
        /// <param name="password"></param>
        /// <param name="passwordNew"></param>
        /// <returns>
        /// return 0 : success
        /// return -1 : no user
        /// return 1 : password invalid.
        /// </returns>
        /// <exception cref="InvalidDataException"></exception>
        public int UserPasswordChange(int pKey, string password, string passwordNew)
        {
            // find user.
            List<UserVO> query = localDBmodel.Find("pkey", pKey, 0);
            if (query.Count == 0) { return -1; }
            else if (query.Count > 1) { throw new InvalidDataException("pkey-matching VO must be unique."); }
            
            // check if password correct.
            if (query[0].password != password) { return 1; }

            // override the user info with email.
            query[0].password = passwordNew;
            localDBmodel.Override(query);
            return 0;
        }

        /// <summary>
        /// delete user with matching email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>
        /// return 0 : success
        /// return -1 : no user
        /// return 1 : user have unreturned book. foreign key restriction
        /// </returns>
        /// <exception cref="InvalidDataException"></exception>
        public int UserDelete(string email)
        {
            // find user.
            List<UserVO> query = localDBmodel.Find("email", email, 0);
            if (query.Count == 0) { return -1; }
            else if (query.Count > 1) { throw new InvalidDataException("email-matching VO must be unique."); }
            
            // get the user's pKey.
            List<int> pKeyDelete = new List<int>() {query[0].pkey};

            // foreign key restriction : if any book that user borrowed exists, reject to delete.
            var borrowedList = LibraryDB.libraryDB.log.UserBorrowedFind(query[0].pkey);
            foreach (var borrowed in borrowedList)
            {
                if (borrowed[1] != 0)
                {
                    return 1;
                }
            }

            // do the delete 
            localDBmodel.Delete(pKeyDelete);
            return 0;
        }
    }
}