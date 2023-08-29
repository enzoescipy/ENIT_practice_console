using System.Data;

namespace MainProject
{
    /// <summary>
    /// public class FunctionalPage
    /// management of functional pages. 
    /// (not manage the non-functional menu pages.)
    /// </summary>
    public class FunctionalPage
    {
        ClassDB classDB;
        public FunctionalPage(ClassDB classDB)
        {
            this.classDB = classDB;
        }
        public void WindowExit()
        {
            UserInput.Get("press enter to exit ...  (any):");
            System.Environment.Exit(0);
        }

        public void ClassDBSearch()
        {
            classDB.PrintAll();
            // UserInput.Get("Esc 키를 누르면 홈 화면으로 돌아갑니다.");
        }

        public void MyTimeSchedule()
        {

        }
    }

    /// <summary>
    /// public class MenuPage
    /// management for menu pages.
    /// </summary>
    public class Page
    {
        FunctionalPage funcPage;
        public Page(ClassDB classDB)
        {
            funcPage = new FunctionalPage(classDB);
        }
        private Action debugEmpty(string text)
        {
            void test()
            {
                Console.WriteLine($"TEST {text}");
            }
            return test;
        }

        
        /// <summary>
        /// public void FrontMenu()
        /// show the frontpage.
        /// </summary>
        public void FrontMenu()
        {
            Console.WriteLine("[[수강신청 프로그램에 오신 것을 환영합니다.]]");
            Console.WriteLine("");
            Console.WriteLine("1. 수강 신청");
            Console.WriteLine("2. 관심 과목");
            Console.WriteLine("3. 나의 시간표");
            Console.WriteLine("4. 종료");

            var menuConnectionList = new List<Action> 
            {ClassApplyMenu, InterestMenu, debugEmpty("나의 시간표"), funcPage.WindowExit};

            int intMenu = Int32.Parse(UserInput.GetSpecific("진입할 메뉴를 선택해 주세요.  (1 ~ 4) : ", "1234", 1));
            intMenu += -1;

            menuConnectionList[intMenu]();
        }

        /// <summary>
        /// 수강 신청 메뉴
        /// </summary>
        public void ClassApplyMenu()
        {
            Console.WriteLine("[[수강 신청 메뉴]]");
            Console.WriteLine("");
            Console.WriteLine("1. 수강 강의 추가");
            Console.WriteLine("2. 수강 강의 삭제");
            Console.WriteLine("3. 수강 강의 조회");
            Console.WriteLine("4. 전체 강의 목록");
            Console.WriteLine("5. 강의 검색");
            Console.WriteLine("6. 이전 메뉴로 이동");
            Console.WriteLine("7. 종료");

            var menuConnectionList = new List<Action> 
            {debugEmpty("추가"),debugEmpty("삭제"),debugEmpty("조회"),
            debugEmpty("전체목록"),debugEmpty("전체검색"),() => {},funcPage.WindowExit};

            int intMenu = Int32.Parse(UserInput.GetSpecific("진입할 메뉴를 선택해 주세요.  (1 ~ 7) : ", "1234567", 1));
            intMenu += -1;

            menuConnectionList[intMenu]();
        }

        /// <summary>
        /// 관심 과목 메뉴
        /// </summary>
        public void InterestMenu()
        {
            Console.WriteLine("[[관심 과목 메뉴]]");
            Console.WriteLine("");
            Console.WriteLine("1. 관심 과목 추가");
            Console.WriteLine("2. 관심 과목 삭제");
            Console.WriteLine("3. 관심 과목 조회");
            Console.WriteLine("4. 전체 강의 목록");
            Console.WriteLine("5. 강의 검색");
            Console.WriteLine("6. 이전 메뉴로 이동");
            Console.WriteLine("7. 종료");

            var menuConnectionList = new List<Action> {};

            int intMenu = Int32.Parse(UserInput.GetSpecific("진입할 메뉴를 선택해 주세요.  (1 ~ 7) : ", "1234567", 1));
            intMenu += -1;

            menuConnectionList[intMenu]();
        }
    }
}