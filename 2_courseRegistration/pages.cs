using System.Data;
using System.Data.Common;

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
        List<int> myClassId;
        List<int> myFlavorId;
        public FunctionalPage(ClassDB classDB, List<int> myClassId, List<int> myFlavorId)
        {
            this.classDB = classDB;
            this.myClassId = myClassId;
            this.myFlavorId = myFlavorId;
        }

        public void debugLists()
        {
            Console.WriteLine($"myflavors : ");
            DebugConsole.D1List(myFlavorId);
            Console.WriteLine();
            Console.WriteLine($"myclasses : ");
            DebugConsole.D1List(myClassId);
            Console.WriteLine();
        }

        public void WindowExit()
        {
            UserInput.Get("press enter to exit ...  (any):");
            System.Environment.Exit(0);
        }

        /// <summary>
        /// public void ClassDBSearch()
        /// just print the current ClassDB All.
        /// </summary>
        public void ClassDBSearch()
        {
            classDB.PrintAll();
            UserInput.Get("이전 메뉴로 돌아가시려면 아무 키나 눌러주세요  : ");
        }


        /// <summary>
        /// public void ClassDBSearch(string columnName)
        /// ask user to get keyword then search about keyword in columnName.
        /// </summary>
        /// <param name="columnName"></param>
        public void ClassDBSearch(string columnName)
        {
            while (true)
            {
                string keyword = UserInput.Get("검색어를 입력해 주세요. : ");
                //debug
                // Console.WriteLine($"kwyword : {keyword}");
                //debug

                bool verify = classDB.PrintSearched(columnName, keyword);
                if (verify == true)
                {
                    UserInput.Get("아무 키나 누르면 이전 메뉴로 돌아갑니다... : ");
                    break;
                }
            }
        }

        /// <summary>
        /// public void AddId(List<int> targetList, int id)
        /// add targetList the id, also block the adding if id is invalid.
        /// - block if there is already the same id exists in the targetList.
        /// - block if some id's pointing classDB rows are overlapping with this id's one. (selectable)
        /// </summary>
        /// <param name="targetList"></param>
        /// <param name="id"></param>
        /// <paramref name="isOverrapDenied"/> if true, block the class time range overlapping. </param>
        /// <returns>
        /// return -1 : null id in DB
        /// return 0 : id added to target List succesfully
        /// return 1 : same id in targetList
        /// return 2 : overlapping id in time in targetList
        /// </returns>
        private int AddId(List<int> targetList, int id, bool isOverrapDenied = true)
        {
            // block if targetList already has the id
            if (targetList.Contains(id))
            {
                return 1;
            }

            var idTime = classDB.FindTime(id);
            if (idTime == null) { return -1; } // if id not found in DB, block return.

            // if isOverrapDenied true, check for time overlapping.
            if (isOverrapDenied)
            {
                foreach (var compareTimeId in targetList)
                {
                    ClassTime? compareTime = classDB.FindTime(compareTimeId);
                    if (compareTime == null) {throw new NullReferenceException("targetList's ID must have its own row inside of the classDB internal database.");}
                    else {
                        if (ClassTime.IsOverlapped(compareTime, idTime) == true)
                        {
                            return 2;
                        }
                    }
                }
            }

            // all test completed. add the id inside of the list.
            targetList.Add(id);
            return 0;
        }

        /// <summary>
        /// public void RegisterClass(string columnName, bool isClassApply = true)
        /// apply the class, or add the flavor class after searched for specific columnName.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="isClassApply"></param>
        public void RegisterClass(string columnName, bool isClassApply = true)
        {
            // loop for column keyword searching
            DataTable? searchedTable;
            while (true)
            {
                string keyword = UserInput.Get($"검색어를 입력해 주세요. : ");

                searchedTable = classDB.ReturnSearched(columnName, keyword);
                if (searchedTable == null) {continue;}
                break;
            }

            string faultString = "";

            while (true)
            {
                PrettyPrint.PprintDataTable(searchedTable, ClassDB.columnNameList);
                Console.Write(faultString); 
                // get keyword then get searched datatable
                // get user applying id
                // if id is not in searchedTable, reject.
                List<int> searchedIdList = new List<int>();
                foreach(DataRow row in searchedTable.Rows) {searchedIdList.Add((int) row["id"]);}
                int userHopeId = UserInput.GetInt("어떤 강의를 신청하시겠습니까?  (id) : ", (x) => {return searchedIdList.Contains(x);});
                
                // add id to the target list.
                var targetList = isClassApply ? myClassId : myFlavorId;
                int applyResult = AddId(targetList, userHopeId, isClassApply);
                if (applyResult == 0) 
                {
                    string applyString = isClassApply ? "신청" : "추가";
                    UserInput.Get($"{applyString} 성공!\n아무 키나 누르면 이전 메뉴로 돌아갑니다... : ");
                    break;
                } else 
                {
                    faultString = isClassApply ? 
                    "이미 해당 강의를 신청했거나 수강 시간이 겹치는 다른 강의가 존재합니다.\n" 
                    : "이미 해당 강의를 관심 과목에 넣었습니다.\n"; 
                    continue; 
                }
            }
        }

        

        /// <summary>
        /// 관심과목 내에서 수강신청
        /// </summary>
        public void ApplyClassWithFlavorList()
        {
            // loop for column keyword searching
            DataTable flavorTable;
            // find flavorTable then print. if length=0 then return
            flavorTable = classDB.FindIdsThenReturn(myFlavorId);
            if (flavorTable.Rows.Count == 0) 
            {
                Console.WriteLine("관심과목이 없습니다."); 
                UserInput.Get("이전 메뉴로 돌아가시려면 아무 키나 누르세요 : ");
                return;
            }
            string faultString = "";

            while (true)
            {
                PrettyPrint.PprintDataTable(flavorTable, ClassDB.columnNameList);
                Console.Write(faultString); 
                
                // get user applying id
                // if id is not in flavorTable, reject.
                int userHopeId = UserInput.GetInt("어떤 강의를 신청하시겠습니까?  (id) : ", (x) => {return myFlavorId.Contains(x);});
                
                int applyResult = AddId(myClassId, userHopeId);
                if (applyResult == 0) 
                {
                    UserInput.Get("지원 성공!\n아무 키나 누르면 이전 메뉴로 돌아갑니다... : ");
                    break;
                } else      
               {
                    faultString = "이미 해당 강의를 신청했거나 수강 시간이 겹치는 다른 강의가 존재합니다.\n"; 
                    continue; 
                }
            }
        }

        /// <summary>
        /// public int? ShowUserSelection(bool isClassApply, bool willAskUser = false)
        /// show the myClassId or myFlavorId then get user input to delete the one row of them.
        /// </summary>
        /// <param name="isClassApply"></param>
        /// <param name="willAskUser"></param>
        /// <returns>
        /// return int : id which can be deleted.
        /// </returns>
        public void ShowUserSelection(bool isClassApply, bool willAskUserWhichDelete = false)
        {
            var myList = isClassApply ? myClassId : myFlavorId;

            // fint mylist id in DB. length=0 then return
            DataTable myTable = classDB.FindIdsThenReturn(myList);
            if (myTable.Rows.Count == 0) 
            {
                string myString = isClassApply ? "신청한 강의가 없습니다." : "추가한 관심과목이 없습니다.";
                Console.WriteLine(myString);
                UserInput.Get("이전 메뉴로 돌아가시려면 아무 키나 누르세요 : ");
                return;
            }

            // print the myTable
            PrettyPrint.PprintDataTable(myTable, ClassDB.columnNameList);

            // if willAskUserWhichDelete, ask user which to delete
            if (willAskUserWhichDelete)
            {   
                int idDeleted = UserInput.GetInt("삭제할 강의의 id를 입력해주세요  : ",(x) => {return myList.Contains(x);});
                myList.RemoveAt(myList.IndexOf(idDeleted));

                Console.WriteLine("삭제되었습니다.");
                UserInput.Get("이전 메뉴로 돌아가시려면 아무 키나 누르세요 : ");
                return ;
            }
            else
            {
                UserInput.Get("이전 메뉴로 돌아가시려면 아무 키나 누르세요 : ");
                return ;
            }
        }

        public void MyTimeSchedule()
        {
            // if empty ClassId, reject.
            if (myClassId.Count == 0)
            {
                Console.WriteLine("신청한 강의가 없습니다.");
                UserInput.Get("이전 메뉴로 돌아가시려면 아무 키나 누르세요 : ");
                return;
            }
            // make and show the schedule table
            var schedule = new MyTimeSchedule();
            foreach (int id in myClassId)
            {
                var classTime = classDB.FindTime(id);
                if (classTime != null) {schedule.ScheduleInsert(classTime);}
                else {throw new NoNullAllowedException("id in myClassId may not verified if it is null in DB");}
            }
            schedule.ScheduleShow();

            DataTable targetTable = classDB.FindIdsThenReturn(myClassId);

            // show the current classIdList
            PrettyPrint.PprintDataTable(targetTable, ClassDB.columnNameList);
            
            // calculate and show the total point
            int totalpoint = 0; 
            foreach (DataRow row in targetTable.Rows)
            {
                totalpoint += (int) row["point"];
            }

            Console.WriteLine($"신청한 학점 합계 : {totalpoint}");

            UserInput.Get("이전 메뉴로 돌아가시려면 아무 키나 누르세요 : ");

        }
    }

    /// <summary>
    /// public class MenuPage
    /// management for menu pages.
    /// </summary>
    public class Page
    {
        FunctionalPage funcPage;

        public Page(ClassDB classDB, List<int> myClassId, List<int> myFlavorId)
        {
            funcPage = new FunctionalPage(classDB, myClassId, myFlavorId);
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
            Console.Clear();

            //debug
            Console.WriteLine("debug");
            funcPage.debugLists();
            Console.WriteLine("debug");
            //debug

            Console.WriteLine("[[수강신청 프로그램에 오신 것을 환영합니다.]]");
            Console.WriteLine("");
            Console.WriteLine("1. 수강 신청");
            Console.WriteLine("2. 관심 과목");
            Console.WriteLine("3. 나의 시간표");
            Console.WriteLine("4. 종료");
            Console.WriteLine();

            var menuConnectionList = new List<Action> 
            {ClassApplyMenu, 
            InterestMenu, 
            funcPage.MyTimeSchedule, 
            funcPage.WindowExit};

            int intMenu = Int32.Parse(UserInput.GetSpecific("진입할 메뉴를 선택해 주세요.  (1 ~ 4) : ", "1234", 1));
            intMenu += -1;

            menuConnectionList[intMenu]();
        }

        /// <summary>
        /// 수강신청 추가 or 관심과목 추가 템플릿
        /// </summary>
        /// <param name="description"></param>
        private void SearchThenRegister(bool isClassApply)
        {
            string description = isClassApply ? "수강 신청" : "관심 과목 추가";
            Console.Clear();
            Console.WriteLine($"[[{description} 메뉴]]");
            Console.WriteLine("");
            Console.WriteLine($"1. 개설 학과 전공으로 검색하여 {description}");
            Console.WriteLine($"2. 학수 번호로 검색하여 {description}");
            Console.WriteLine($"3. 교과목 명으로 검색하여 {description}");
            Console.WriteLine($"4. 강의 대상 학년으로 검색하여 {description}");
            Console.WriteLine($"5. 교수명으로 검색하여 {description}");
            if (isClassApply)
            {
                Console.WriteLine($"6. 관심과목으로 {description}");
                Console.WriteLine($"7. 이전 메뉴로 이동");
                Console.WriteLine($"8. 종료");
                Console.WriteLine();
            } 
            else 
            {
                Console.WriteLine($"6. 이전 메뉴로 이동");
                Console.WriteLine($"7. 종료");
                Console.WriteLine();
            }

            List<Action> menuConnectionList = new List<Action> 
                {() => {funcPage.RegisterClass("department", isClassApply);}, 
                () => {funcPage.RegisterClass("cource", isClassApply);}, 
                () => {funcPage.RegisterClass("subject", isClassApply);}, 
                () => {funcPage.RegisterClass("grade", isClassApply);}, 
                () => {funcPage.RegisterClass("teacher", isClassApply);}};
                // () => {},
                // funcPage.WindowExit};
            if (isClassApply)
            {
                menuConnectionList.Add(() => {funcPage.ApplyClassWithFlavorList();});
            }
            menuConnectionList.Add(() => {});
            menuConnectionList.Add(funcPage.WindowExit);

            string menuString = isClassApply ? "12345678" : "1234567";

            int intMenu = Int32.Parse(UserInput.GetSpecific($"진입할 메뉴를 선택해 주세요.  (1 ~ {menuString[menuString.Length - 1]}) : ", menuString, 1));
            intMenu += -1;

            menuConnectionList[intMenu]();
        }

        /// <summary>
        /// 강의 검색 메뉴
        /// </summary>
        public void SearchMenu()
        {
            Console.Clear();
            Console.WriteLine("[[강의 검색 메뉴]]");
            Console.WriteLine("");
            Console.WriteLine("1. 개설 학과 전공으로 검색");
            Console.WriteLine("2. 학수 번호로 검색");
            Console.WriteLine("3. 교과목 명으로 검색");
            Console.WriteLine("4. 강의 대상 학년으로 검색");
            Console.WriteLine("5. 교수명으로 검색");
            Console.WriteLine("6. 이전 메뉴로 이동");
            Console.WriteLine("7. 종료");

            Console.WriteLine();
            var menuConnectionList = new List<Action> 
            {() => {funcPage.ClassDBSearch("department");}, 
            () => {funcPage.ClassDBSearch("cource");}, 
            () => {funcPage.ClassDBSearch("subject");}, 
            () => {funcPage.ClassDBSearch("grade");}, 
            () => {funcPage.ClassDBSearch("teacher");}, 
            () => {},
            funcPage.WindowExit};

            
            int intMenu = Int32.Parse(UserInput.GetSpecific("진입할 메뉴를 선택해 주세요.  (1 ~ 7) : ", "1234567", 1));
            intMenu += -1;
            menuConnectionList[intMenu]();
        }

        /// <summary>
        /// 수강 신청 메뉴
        /// </summary>
        public void ClassApplyMenu()
        {
            Console.Clear();
            Console.WriteLine("[[수강 신청 메뉴]]");
            Console.WriteLine("");
            Console.WriteLine("1. 수강 강의 추가");
            Console.WriteLine("2. 수강 강의 삭제");
            Console.WriteLine("3. 수강 강의 조회");
            Console.WriteLine("4. 전체 강의 목록");
            Console.WriteLine("5. 강의 검색");
            Console.WriteLine("6. 이전 메뉴로 이동");
            Console.WriteLine("7. 종료");

            Console.WriteLine();
            var menuConnectionList = new List<Action> 
            {() => {SearchThenRegister(true);},
            () => {funcPage.ShowUserSelection(true, true);},
            () => {funcPage.ShowUserSelection(true);},
            () => {funcPage.ClassDBSearch();},
            SearchMenu,
            () => {}, // 이전 메뉴는 빈 함수로 구현
            funcPage.WindowExit};

            int intMenu = Int32.Parse(UserInput.GetSpecific("진입할 메뉴를 선택해 주세요.  (1 ~ 7) : ", "1234567", 1));
            intMenu += -1;

            menuConnectionList[intMenu]();
        }

        /// <summary>
        /// 관심 과목 메뉴
        /// </summary>
        public void InterestMenu()
        {
            Console.Clear();
            Console.WriteLine("[[관심 과목 메뉴]]");
            Console.WriteLine("");
            Console.WriteLine("1. 관심 과목 추가");
            Console.WriteLine("2. 관심 과목 삭제");
            Console.WriteLine("3. 관심 과목 조회");
            Console.WriteLine("4. 전체 강의 목록");
            Console.WriteLine("5. 강의 검색");
            Console.WriteLine("6. 이전 메뉴로 이동");
            Console.WriteLine("7. 종료");
            Console.WriteLine();

            var menuConnectionList = new List<Action> 
            {() => {SearchThenRegister(false);},
            () => {funcPage.ShowUserSelection(false, true);},
            () => {funcPage.ShowUserSelection(false);},
            () => {funcPage.ClassDBSearch();},
            SearchMenu,
            () => {}, // 이전 메뉴는 빈 함수로 구현
            funcPage.WindowExit};

            int intMenu = Int32.Parse(UserInput.GetSpecific("진입할 메뉴를 선택해 주세요.  (1 ~ 7) : ", "1234567", 1));
            intMenu += -1;

            menuConnectionList[intMenu]();
        }
    }
}