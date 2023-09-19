namespace MainProject
{
    public class Page
    {
        private static Page? _page = null;
        public static Page page
        {
            get
            {
                if (_page == null)
                {
                    _page = new Page();
                }
                return _page;
            }
        }

        /// <summary>
        /// public void FrontMenu()
        /// show the frontpage.
        /// </summary>
        public void FrontMenu()
        {
            // Console.Clear();

            // //debug
            // Console.WriteLine("debug");
            // FunctionalPage.functionalPage.debugLists();
            // Console.WriteLine("debug");
            // //debug

            Console.WriteLine("[[도서관리 프로그램에 온걸 환영해.]]");
            Console.WriteLine("");
            Console.WriteLine("1. 회원 메뉴");
            Console.WriteLine("2. 로그아웃");
            Console.WriteLine("3. 회원가입");
            Console.WriteLine("4. 관리자 메뉴");
            Console.WriteLine("5. 종료");
            Console.WriteLine();

            var menuConnectionList = new List<Action> 
            {FunctionalPage.functionalPage.UserLogin, 
            FunctionalPage.functionalPage.UserLogOut, 
            FunctionalPage.functionalPage.UserRegister, 
            FunctionalPage.functionalPage.ManagerLogin, 
            FunctionalPage.functionalPage.WindowExit};

            int intMenu = Int32.Parse(UserInput.GetSpecific("진입할 메뉴를 선택해 주세요.  (1 ~ 4) : ", "1234", 1));
            intMenu += -1;

            menuConnectionList[intMenu]();
        }

        public void UserMenu()
        {
            // Console.Clear();

            // //debug
            // Console.WriteLine("debug");
            // FunctionalPage.functionalPage.debugLists();
            // Console.WriteLine("debug");
            // //debug

            Console.WriteLine("[[회원 메뉴]]");
            Console.WriteLine("");
            Console.WriteLine("1. 책 검색");
            Console.WriteLine("2. 책 대출");
            Console.WriteLine("3. 책 반납");
            Console.WriteLine("4. 책 리스트");
            Console.WriteLine("5. 나의 회원 정보");
            Console.WriteLine("6. 되돌아가기");
            Console.WriteLine("7. 종료");
            Console.WriteLine();

            var menuConnectionList = new List<Action> 
            {FunctionalPage.functionalPage.BookSearch, 
            FunctionalPage.functionalPage.UserBorrow, 
            FunctionalPage.functionalPage.UserReturn, 
            FunctionalPage.functionalPage.BookList, 
            FunctionalPage.functionalPage.UserInfo, 
            () => {}, 
            FunctionalPage.functionalPage.WindowExit};

            int intMenu = Int32.Parse(UserInput.GetSpecific("진입할 메뉴를 선택해 주세요.  (1 ~ 7) : ", "1234567", 1));
            intMenu += -1;

            menuConnectionList[intMenu]();
        }

        public void ManagerMenu()
        {
            // Console.Clear();

            // //debug
            // Console.WriteLine("debug");
            // FunctionalPage.functionalPage.debugLists();
            // Console.WriteLine("debug");
            // //debug

            Console.WriteLine("[[관리자 메뉴]]");
            Console.WriteLine("");
            Console.WriteLine("1. 회원 리스트");
            Console.WriteLine("2. 책 리스트");
            Console.WriteLine("3. 회원 검색");
            Console.WriteLine("4. 회원 삭제");
            Console.WriteLine("5. 책 정보 수정");
            Console.WriteLine("6. 신규 책 등록");
            Console.WriteLine("7. 책 삭제");
            Console.WriteLine("8. 책 대여 반납 기록");
            Console.WriteLine("9. 되돌아가기");
            Console.WriteLine("10. 종료");
            Console.WriteLine();

            var menuConnectionList = new List<Action> 
            {FunctionalPage.functionalPage.UserList,
            FunctionalPage.functionalPage.BookList,
            FunctionalPage.functionalPage.UserSearch,
            FunctionalPage.functionalPage.UserDelete,
            FunctionalPage.functionalPage.BookInfoChange,
            FunctionalPage.functionalPage.BookAdd,
            FunctionalPage.functionalPage.BookDelete,
            FunctionalPage.functionalPage.LogList,
            () => {}, 
            FunctionalPage.functionalPage.WindowExit};

            int intMenu;
            while (true)
            {
                intMenu = UserInput.GetInt("진입할 메뉴를 선택해 주세요.  (1 ~ 11) : ", (s) => {return 0 < s && s < 11;});
                if (intMenu > 11 || intMenu < 1) { continue; } 
                break;
            }
            intMenu += -1;

            menuConnectionList[intMenu]();
        }
    }
}