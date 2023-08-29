using System.Data;
using System.Diagnostics;

namespace MainProject
{
    public class ClassDB
    {
        public DataTable dataTable = new DataTable(); // DB-like object that store every information of classDB, EXCEPT the start/end time of class.
        public List<ClassTimeWithID> classTimeList = new List<ClassTimeWithID>(); // List of classtime with corresponding id.
        
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
                    // 6th column is for the ClassTime instance
                    string room = rowSplited[9];
                    string teacher = rowSplited[10];
                    string language = rowSplited[11];
                    
                    dataTable.Rows.Add(new Object[] {id, department, cource, division, subject, group, grade, point, room, teacher, language});
                    //classTime works
                    string korTimeExpression = rowSplited[8];
                    classTimeList.Add(new ClassTimeWithID(korTimeExpression, id));
                }
            }
        }
    }
}