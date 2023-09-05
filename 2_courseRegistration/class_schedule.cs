using System.Data;
using System.Data.Common;

namespace MainProject
{
    public class MyTimeSchedule
    {
        List<List<int?>> schedule;
        string[] columnName = ClassTime.dateArray; // {"월","화","수","목","금"}

        string[] RowName = ClassTime.timeArray;  //  {"8:00","8:30","09:00","09: ...
        // actuall row name is like RowName[i] ~ RowName[i+1].
        // for that reason, actuall row length is RowName.Length - 1.

        /// <summary>
        /// public MyTimeSchedule(List<int> myClassList)
        /// myClassList is the List of id which user would listern.
        /// </summary>
        /// <param name="myClassList"></param>
        public MyTimeSchedule()
        {
            // build templet of schedule DataTable
            schedule = new List<List<int?>>();
            // first index is DAY, and last index is TIME.
            // {     10:00 ~ 10:30 ~ 11:00
            // MON :     {id      id      null ...}
            // ... ..    {. ... ... ... ... .}
            // }
            for (int i=0; i < columnName.Length; i++)
            {
                var scheduleCol = new List<int?>();
                for (int j=0; j < RowName.Length - 1; j++) {scheduleCol.Add(null);}
                schedule.Add(scheduleCol);
            }
        }

        /// <summary>
        /// public int ScheduleInsert(ClassTimeWithID classTimeWithID)
        /// DANGER : this method NOT CHECK if id belong with classTimeWithID is verified.
        /// </summary>
        /// <param name="classTimeWithID"></param>
        /// <returns>
        /// return -1 : id already in the myClassList
        /// </returns>
        public void ScheduleInsert(ClassTimeWithID classTimeWithID)
        {
            // each row of dateTimeRangeTable will be like [dateNum, startTimeNum, endTimeNum].
            // ex : 수 18:00~20:00 will became [2, 20, 24].
            
            List<List<int>> timeTable = classTimeWithID.dateTimeRangeTable;
            int id = classTimeWithID.id;

            foreach (var timeRow in timeTable)
            {
                int day = timeRow[0];
                int startTime = timeRow[1];
                int endTime = timeRow[2] - 1; // this is time "pieces" not time "moment", so index should be changed.
                for (int time = startTime; time <= endTime; time++)
                {
                    schedule[day][time] = id;
                }
            }
        }

        public void ScheduleShow()
        {
            string column    = "  시간/요일  |  월   |  화   |  수   |  목   |  금   ";
            string sep       = "=============|=======|=======|=======|=======|=======";
            // string teprow = " 14:00~14:30 |  123  |  012  |  123  |  001  |  123  "

            var rowStringList = new List<string>();
            int willSpaced = 2;
            for (int i=0; i < RowName.Length - 1; i++)
            {
                string rowString = " ";
                // add the time description
                string timeRowName = RowName[i] + "~" + RowName[i+1];
                if (willSpaced > 0) 
                {
                    willSpaced--; 
                    for (int k=willSpaced+1;k>0;k--) {timeRowName += " ";}
                }
                rowString += timeRowName;
                rowString += " ";
                for (int j = 0; j < 5; j++)
                {
                    // add the id
                    rowString += "|  ";
                    int? id =  schedule[j][i];
                    Console.WriteLine($"id : {id}");
                    string idString = "";
                    if (id == null)
                    {
                        idString = "   ";
                    }
                    else if (id < 10)
                    {
                        idString = "00" + id.ToString();
                    }
                    else if (id < 100)
                    {
                        idString = "0" + id.ToString();
                    }
                    else
                    {
                        idString = id.ToString() ?? "   ";
                    }
                    rowString += idString + "  ";
                }
                rowStringList.Add(rowString);
            }
            
            // print the schedule
            Console.WriteLine(column);
            Console.WriteLine(sep);
            foreach(string row in rowStringList)
            {
                Console.WriteLine(row);
            }

        }
    }
}