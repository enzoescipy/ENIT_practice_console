using System.Data;
using System.Diagnostics;

namespace MainProject
{
    public class ClassDB
    {
        DataTable dataTable = new DataTable(); // DB-like object that store every information of classDB, EXCEPT the start/end time of class.
        List<ClassTimeWithID> classTimeList = new List<ClassTimeWithID>(); // List of classtime with corresponding id.
        static public List<string> columnNameList {get;} = "NO/개설학과전공/학수번호/분반/교과목명/이수구분/학년/학점/요일 및 강의시간/강의실/메인교수명/강의언어".Split("/").ToList();
        // SECURITY WARNING : deldte public on dataTable, classTimeList on production stage


        /// <summary>
        /// ClassDB(string dbPath)
        /// create ClassDB object and load db.txt automatically.
        /// WARNING : this initiation dosen't verify the db.txt's path.
        /// </summary>
        /// <param name="dbPath"></param>
        public ClassDB(string dbPath)
        {
            dataTable.Columns.Add("id", typeof(int)); // primary key, starting 0. must add +1 when in use directly for user
            dataTable.Columns.Add("department", typeof(string)); // 개설학과전공
            dataTable.Columns.Add("cource", typeof(string)); //학수번호
            dataTable.Columns.Add("division", typeof(string)); //분반
            dataTable.Columns.Add("subject", typeof(string)); //교과목명
            dataTable.Columns.Add("group", typeof(string)); //이수구분
            dataTable.Columns.Add("grade", typeof(string)); //학년
            dataTable.Columns.Add("point", typeof(int)); //학점
            dataTable.Columns.Add("time", typeof(string)); //  요일 및 강의시간 (보여주기용 문자열)
            dataTable.Columns.Add("room", typeof(string)); //  강의실
            dataTable.Columns.Add("teacher", typeof(string)); //  메인교수명
            dataTable.Columns.Add("language", typeof(string)); //  강의언어


            using (StreamReader reader = new StreamReader(dbPath))
            {
                reader.ReadLine(); // skip the first line
                while (true)
                {
                    string? line = reader.ReadLine();
                    if (line == null) {break;}

                    // dataTable works
                    var rowSplited = line.Split("/");

                    int id = Int32.Parse(rowSplited[0]) - 1;
                    string department = rowSplited[1];
                    string cource = rowSplited[2];
                    string division = rowSplited[3];
                    string subject = rowSplited[4];
                    string group = rowSplited[5];
                    string grade = rowSplited[6];
                    double point = Double.Parse(rowSplited[7]);
                    string time = rowSplited[8];// 6th column is for the ClassTime instance
                    string room = rowSplited[9];
                    string teacher = rowSplited[10];
                    string language = rowSplited[11];
                    
                    dataTable.Rows.Add(new Object[] {id, department, cource, division, subject, group, grade, point, time, room, teacher, language});
                    //classTime works
                    classTimeList.Add(new ClassTimeWithID(time, id));

                    // if (String.Equals("컴", department[0])) {Console.WriteLine("matched!!!!!!!");}//debug
                }
            }

            // //debug
            // Console.WriteLine("찾음 : ");
            // Console.WriteLine(dataTable.Select("department LIKE '%컴%'").Length);
            // //debug
        }
    
        public void PrintAll()
        {
            PrettyPrint.PprintDataTable(dataTable, columnNameList);
        }

        /// <summary>
        /// public bool PrintSearched(string colName, string keyWord)
        /// print only searched rows.
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="keyWord"></param>
        /// <returns>
        /// return false : no colName found, or keyward not fitted
        /// return true : works done well (includes no result)
        /// </returns>
        public bool PrintSearched(string colName, string keyWord)
        {
            // validation of param
            if (!dataTable.Columns.Contains(colName)) {return false;}
            foreach(char invalid in "~()#\\/=><+-*%&|^'\"[],")
            {
                if (keyWord.Contains(invalid))
                {
                    return false;
                }
            }
            
            var searched = Search(colName, keyWord);
            PrettyPrint.PprintDataTable(searched, columnNameList);
            return true;
        }

        /// <summary>
        /// public DataTable ReturnSearched(string colName, string keyWord)
        /// search for the keyword in colName, then return the result as DataTable.
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="keyWord"></param>
        /// <returns>
        /// return null : invalid colname or keyword
        /// return DataTable : succeed
        /// </returns>
        public DataTable? ReturnSearched(string colName, string keyWord)
        {
            // validation of param
            if (!dataTable.Columns.Contains(colName)) {return null;}
            foreach(char invalid in "~()#\\/=><+-*%&|^'\"[],")
            {
                if (keyWord.Contains(invalid))
                {
                    return null;
                }
            }
            return Search(colName, keyWord);
        }

        /// <summary>
        /// DataTable? Search(string colName, string keyWord)
        /// search dataTable's column with keyword then return
        /// the compressed dataTable
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="keyWord"></param>
        /// <returns>
        /// return null : inapproprate colName or no search result
        /// </returns>
        private DataTable? Search(string colName, string keyWord)
        {
            // is colName exists
            if (!dataTable.Columns.Contains(colName)) {return null;}

            // is colName not the "point" or "id" (this column is int type, so only matching search allowed.)
            if (String.Equals(colName, "point") || String.Equals(colName, "id")) {return null;}

            // if dataTable Row do not includes keyWord then pop.
            // Console.WriteLine((dataTable.Select($"{colName} LIKE '%{keyWord}%'")).Length); //debug
            // Console.WriteLine($"{keyWord}");
            var searchedTable = dataTable.Select($"{colName} LIKE '%{keyWord}%'");

            if (searchedTable.Length > 0) {return searchedTable.CopyToDataTable();}
            else {return null;}
            
        }


        /// <summary>
        /// public ClassTime Find(int id)
        /// find the corresponding row form classDB, then return the corresponding classTime data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// return null : id not found.
        /// return ClassTime : id found.
        /// </returns>
        public ClassTimeWithID? FindTime(int id)
        {
            foreach (var time in classTimeList)
            {
                int rowId = time.id;
                if (rowId == id) {return time;}
            }
            return null;
        }

        /// <summary>
        /// public DataRow? FindIdsThenReturn(int id)
        /// find the corresponding row form classDB then return
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable FindIdsThenReturn(List<int> idList)
        {
            DataTable foundTable = dataTable.Clone();
            foreach (int id in idList)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    int rowId = (int) row["id"];
                    if (rowId == id) 
                    {
                        foundTable.ImportRow(row);
                    }
                }
            }

            return foundTable;
        }

    }
}