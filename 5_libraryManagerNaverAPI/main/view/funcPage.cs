using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace MainProject
{
    public class FunctionalPage
    {
        private static FunctionalPage? _functionalPage = null;
        public static FunctionalPage functionalPage
        {
            get
            {
                if (_functionalPage == null)
                {
                    _functionalPage = new FunctionalPage();
                }
                return _functionalPage;
            }
        }
        private LibraryDB libraryDB = LibraryDB.libraryDB;
        private UserVO? currentLogInUser = null;
        private bool isManagerOnline = false;
        private const string escape = "back";
        public void WindowExit()
        {
            UserInput.Get("press enter to exit ...  (any):");
            System.Environment.Exit(0);
        }

        public void UserList()
        {
            var userListString = libraryDB.user.UserAllString();
            PrettyPrint.Pprint2DStringList(userListString);
            UserInput.Get(" press any to return back ... (any)  : ");
        }

        public void BookList()
        {
            var bookListString = libraryDB.book.BookAllString();
            PrettyPrint.Pprint2DStringList(bookListString);
            UserInput.Get(" press any to return back ... (any)  : ");
        }

        public void LogList()
        {
            var logListString = libraryDB.log.LogAllString();
            PrettyPrint.Pprint2DStringList(logListString);
            UserInput.Get(" press any to return back ... (any)  : ");
        }

        public void UserSearch()
        {
            string get = UserInput.GetSpecific("please put the keyword you want to search,\nthen program will find user with matching EMAIL. 'back' to return back. (keyword/back)  :  ",
                                                UserInput.alphabetString + "@." + "1234567890");
            if (String.Equals(escape, get))
            {
                return;
            }

            var userSearchedString = libraryDB.user.UserSearchString(get);
            if (userSearchedString.Count == 0)
            {
                Console.WriteLine("no user has found ...");
            }
            else
            {
                PrettyPrint.Pprint2DStringList(userSearchedString);
            }
            UserInput.Get(" press any to return back ... (any)  : ");
        }

        public void BookSearch()
        {
            string get = UserInput.GetExcept("please put the keyword you want to search,\nthen program will find book with matching NAME or DESCRIPTION. \n 'back' to return back. (keyword/back)  :  ",
                                                UserInput.basicInvalidString);
            if (String.Equals(escape, get))
            {
                return;
            }

            var bookSearchedString = libraryDB.book.BookSearch(get);
            if (bookSearchedString.Count == 0)
            {
                Console.WriteLine("no book has found ...");
            }
            else
            {
                PrettyPrint.Pprint2DStringList(libraryDB.book.ListVOStringify(bookSearchedString));
            }
            UserInput.Get(" press any to return back ... (any)  : ");
        }

        public void UserDelete()
        {
            var userListString = libraryDB.user.UserAllString();
            while (true)
            {
                // print user
                PrettyPrint.Pprint2DStringList(userListString);

                // main logic
                string get = UserInput.GetSpecific("please put the EMAIL of the user you want to delete. 'back' to return back.  (user email/back)  :  ",
                                                    UserInput.alphabetString + "@." + "1234567890");
                if (String.Equals(escape, get))
                {
                    return;
                }

                var deleteState = libraryDB.user.UserDelete(get);
                if (deleteState == 0)
                {
                    Console.WriteLine("user delete succeed.");
                    UserInput.Get(" press any to return back ... (any)  : ");
                    break;
                }
                else if (deleteState == 1)
                {
                    Console.WriteLine("user must return the all of its borrowed book before deleted ...");
                    continue;
                }
                else
                {
                    Console.WriteLine("no user found ...");
                    continue;
                }
            }
        }

        public void BookDelete()
        {
            var bookListString = libraryDB.book.BookAllString();
            while (true)
            {
                // print book
                PrettyPrint.Pprint2DStringList(bookListString);

                // main logic
                string get = UserInput.GetExcept("please put the NAME of the book you want to delete. 'back' to return back. (book name/back)  :  ",
                                                    UserInput.basicInvalidString);
                if (String.Equals(escape, get))
                {
                    return;
                }

                var deleteState = libraryDB.book.bookDelete(get);
                if (deleteState != 0)
                {
                    Console.WriteLine("no book has found ...");
                    continue;
                }
                else
                {
                    Console.WriteLine("book delete succeed.");
                    UserInput.Get(" press any to return back ... (any)  : ");
                    break;
                }
            }
        }

        public void UserRegister()
        {
            while (true)
            {
                string id = UserInput.GetExcept("please put the id you want to use. 'back' to return back.  (user id/back)  :  ",
                                                        UserInput.basicInvalidString);
                if (String.Equals(escape, id))
                {
                    return;
                }
                string password = UserInput.GetExcept("please put the password you want to use. 'back' to return back.  (user password/back)  :  ",
                                                        UserInput.basicInvalidString);
                if (String.Equals(escape, password))
                {
                    return;
                }
                string email = UserInput.GetSpecific("please put the email you want to use. 'back' to return back.  (user email/back)  :  ",
                                                        UserInput.alphabetString + "@." + "1234567890");
                if (String.Equals(escape, email))
                {
                    return;
                }
                int createState = libraryDB.user.UserAdd(new UserVO(id, password, email));

                if (createState == 0)
                {
                    Console.WriteLine("user register succeed.");
                    UserInput.Get(" press any to return back ... (any)  : ");
                    break;
                }
                else if (createState == 1)
                {
                    Console.WriteLine("id already using by other user ...");
                    continue;
                }
                else if (createState == 2)
                {
                    Console.WriteLine("email already using by other user ...");
                    continue;
                }
                else if (createState == 3)
                {
                    Console.WriteLine("you entered some field blank ...");
                    continue;
                }
            }
        }

        public void BookAdd()
        {
            while (true)
            {
                string name = UserInput.GetExcept("please put the name of book. 'back' to return back.  (book name/back)  :  ",
                                                        UserInput.basicInvalidString);
                if (String.Equals(escape, name))
                {
                    return;
                }
                string description = UserInput.GetExcept("please put the description of book. 'back' to return back.  (book description/back)  :  ",
                                                        UserInput.basicInvalidString);
                if (String.Equals(escape, description))
                {
                    return;
                }

                int initStockParsed;
                while (true)
                {
                    string initStock = UserInput.GetSpecific("please put the initial stock of book. 'back' to return back.  (book initial stock/back)  :  ",
                                                            UserInput.numberString + escape );
                    if (String.Equals(escape, initStock))
                    {
                        return;
                    }
                    bool initStockParseState = Int32.TryParse(initStock, out initStockParsed);
                    if (initStockParseState)
                    {
                        break;
                    }
                }

                int createState = libraryDB.book.BookAdd(new BookVO(name, description, initStockParsed));

                if (createState == 0)
                {
                    Console.WriteLine("book creation succeed.");
                    UserInput.Get(" press any to return back ... (any)  : ");
                    break;
                }
                else if (createState == 1)
                {
                    Console.WriteLine("name already using by other book ...");
                    continue;
                }
                else if (createState == 2)
                {
                    Console.WriteLine("you entered some field blank ...");
                    continue;
                }
            }
        }

        public void UserInfo()
        {
            // get user check
            if (currentLogInUser == null) {throw new Exception("no user information found! login problem");}
            
            // print user basic information
            Console.WriteLine($"ID : {currentLogInUser.id}");
            Console.WriteLine($"EMAIL : {currentLogInUser.email}");

            // print user book borrow information
            var borrowState = libraryDB.log.UserBorrowedFind(currentLogInUser.pkey);
            if (borrowState != null)
            {
                foreach (var borrowline in borrowState)
                {
                    string name = borrowline[0].ToString();
                    string count = borrowline[1].ToString();
                    Console.WriteLine($"you borrowed {name} : {count} book(s).");
                }
            }

            while (true)
            {
                string get = UserInput.GetSpecific("do you want to change your infomation? \n 1:change email, 2:change password, back:return back. (1/2/back)  :  ",
                                        "12back");
                if (String.Equals(escape, get))
                {
                    return;
                }
                else if (String.Equals("1", get))
                {
                    // change email.
                    string email = UserInput.GetSpecific("please put the email you want to change. 'back' to return back.  (user email/back)  :  ",
                                                            UserInput.alphabetString + "@." + "1234567890");
                    if (String.Equals(escape, email))
                    {
                        return;
                    }

                    int state = libraryDB.user.UserEmailChange(currentLogInUser.pkey, email);
                    if (state != 0)
                    {
                        throw new Exception("no user found. this can't be happend");
                    }
                    else
                    {
                        Console.WriteLine("email change succeed.");
                    }

                    UserLogOut();
                    return;
                }
                else if (String.Equals("2", get))
                {
                    //verifty pass.
                    string password_verify = UserInput.GetExcept("please put the current password. 'back' to return back.  (user password/back)  :  ",
                                                            UserInput.basicInvalidString);
                    if (String.Equals(escape, password_verify))
                    {
                        return;
                    }

                    // change passwrod.
                    string password = UserInput.GetExcept("please put the password you want to change. 'back' to return back.  (user password/back)  :  ",
                                                            UserInput.basicInvalidString);
                    if (String.Equals(escape, password))
                    {
                        return;
                    }
                    int state = libraryDB.user.UserPasswordChange(currentLogInUser.pkey, password_verify, password);
                    if (state == 0)
                    {
                        Console.WriteLine("password changed succeed.");
                    }
                    else if (state == 1)
                    {
                        Console.WriteLine("invalid password.");
                    }
                    else if (state == -1)
                    {
                        throw new Exception("no user foun. this can't be happend");
                    }

                    UserLogOut();
                    return;

                }
                else
                {
                    Console.WriteLine("Wrong input.");
                }
            }
        }

        public void BookInfoChange()
        {
            var bookListString = libraryDB.book.BookAllString();
            while (true)
            {
                // print book
                PrettyPrint.Pprint2DStringList(bookListString);

                // main logic
                string nameInit = UserInput.GetExcept("please put the NAME of the book you want to change/correct. \n'back' to return back, press enter to keep the current value. (book name/back)  :  ",
                                                    UserInput.basicInvalidString);
                if (String.Equals(escape, nameInit))
                {
                    return;
                }
                string name = UserInput.GetExcept("please put the New NAME of book. 'back' to return back. \n'back' to return back, press enter to keep the current value.  (book name/back)  :  ",
                                                        UserInput.basicInvalidString);
                if (String.Equals(escape, name))
                {
                    return;
                }
                string description = UserInput.GetExcept("please put the description of book. 'back' to return back. \n'back' to return back, press enter to keep the current value.  (book description/back)  :  ",
                                                        UserInput.basicInvalidString);
                if (String.Equals(escape, description))
                {
                    return;
                }

                int createState = libraryDB.book.BookInfoChange(nameInit, 
                                                                name.Length == 0 ? null : name, 
                                                                description.Length == 0 ? null : description, null);
                if (createState != 0)
                {
                    Console.WriteLine("no book has found ...");
                    continue;
                }
                else
                {
                    Console.WriteLine("book correction succeed.");
                    UserInput.Get(" press any to return back ... (any)  : ");
                    break;
                }
            }
        }

        public void UserLogin()
        {
            if (currentLogInUser != null)
            {
                Page.page.UserMenu();
                return;
            }
            while (true)
            {
                string id = UserInput.GetExcept("please put the id. 'back' to return back.  (user id/back)  :  ",
                                                        UserInput.basicInvalidString);
                if (String.Equals(escape, id))
                {
                    break;
                }
                string password = UserInput.GetExcept("please put the password. 'back' to return back.  (user password/back)  :  ",
                                                        UserInput.basicInvalidString);
                if (String.Equals(escape, password))
                {
                    break;
                }

                currentLogInUser = libraryDB.user.UserLogin(id, password);
                if (currentLogInUser == null)
                {
                    Console.WriteLine("login failed ...");
                    continue;
                }
                else
                {
                    Console.WriteLine("welcome!");
                    break;
                }
            }

            Page.page.UserMenu();
        }

        public void UserLogOut()
        {
            currentLogInUser = null;
            Console.WriteLine("로그아웃 되었습니다.");
            UserInput.Get(" press any to return back ... (any)  : ");
        }

        public void ManagerLogin()
        {
            if (isManagerOnline == false)
            {
                while (true)
                {
                    string password = UserInput.GetExcept("please put the password. 'back' to return back.  (user password/back)  :  ",
                                                            UserInput.basicInvalidString);
                    if (String.Equals(escape, password))
                    {
                        break;
                    }
                    else if (String.Equals(password, "1111"))
                    {
                        Console.WriteLine("welcome!");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("login failed ...");
                        continue;
                    }
                }
            }
            isManagerOnline = true;
            Page.page.ManagerMenu();
        }

        public void UserBorrow()
        {
            var bookListString = libraryDB.book.BookAllString();
            var borrowState = libraryDB.log.UserBorrowedFind(currentLogInUser.pkey);

            while (true)
            {
                // print book
                PrettyPrint.Pprint2DStringList(bookListString);
                if (borrowState != null)
                {
                    foreach (var borrowline in borrowState)
                    {
                        string bookname = borrowline[0].ToString();
                        string count = borrowline[1].ToString();
                        Console.WriteLine($"you borrowed {bookname} : {count} book(s).");
                    }
                }
                string name = UserInput.GetExcept("please put the name of book which you want to borrow.\n 'back' to return back.  (book name/back)  :  ",
                                                        UserInput.basicInvalidString);
                if (String.Equals(escape, name))
                {
                    return;
                }

                int amountParsed;
                while (true)
                {
                    string amount = UserInput.GetSpecific("please put the amount of the book you want to borrow. 'back' to return back.  (book initial stock/back)  :  ",
                                                            UserInput.numberString + escape );
                    if (String.Equals(escape, amount))
                    {
                        return;
                    }
                    bool amountParseState = Int32.TryParse(amount, out amountParsed);
                    if (amountParseState)
                    {
                        break;
                    }
                }
                

                var state = libraryDB.Borrow(currentLogInUser.pkey, name, amountParsed);

                if (state == 0)
                {
                    Console.WriteLine("borrowing complete!");
                    return;
                }
                else if (state == -1)
                {
                    Console.WriteLine("wrong input. please put the right amount and book name.");
                    continue;
                }
                else if (state == 1)
                {
                    Console.WriteLine("no user or bookname found");
                    continue;
                }
            }
        }

        public void UserReturn()
        {
            var bookListString = libraryDB.book.BookAllString();
            var borrowState = libraryDB.log.UserBorrowedFind(currentLogInUser.pkey);

            while (true)
            {
                // print book
                PrettyPrint.Pprint2DStringList(bookListString);
                if (borrowState != null)
                {
                    foreach (var borrowline in borrowState)
                    {
                        string bookname = borrowline[0].ToString();
                        string count = borrowline[1].ToString();
                        Console.WriteLine($"you borrowed {bookname} : {count} book(s).");
                    }
                }
                string name = UserInput.GetExcept("please put the name of book which you want to return.\n 'back' to return back.  (book name/back)  :  ",
                                                        UserInput.basicInvalidString);
                if (String.Equals(escape, name))
                {
                    return;
                }

                int amountParsed;
                while (true)
                {
                    string amount = UserInput.GetSpecific("please put the amount of the book you want to return. 'back' to return back.  (book initial stock/back)  :  ",
                                                            UserInput.numberString + escape );
                    if (String.Equals(escape, amount))
                    {
                        return;
                    }
                    bool amountParseState = Int32.TryParse(amount, out amountParsed);
                    if (amountParseState)
                    {
                        break;
                    }
                }

                var state = libraryDB.Return(currentLogInUser.pkey, name, amountParsed);

                if (state == 0)
                {
                    Console.WriteLine("returning complete!");
                    return;
                }
                else if (state == 1)
                {
                    Console.WriteLine("no user or bookname found");
                    continue;
                }
                else if (state == -1)
                {
                    Console.WriteLine("wrong input. please put the right amount and book name.");
                    continue;
                }
            }
        }
    }
}