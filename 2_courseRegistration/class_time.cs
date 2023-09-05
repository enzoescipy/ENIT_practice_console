namespace MainProject
{
    /// <summary>
    /// public class ClassTimeWithID : ClassTime
    /// ClassTime with id parameter. end
    /// </summary>
    public class ClassTimeWithID : ClassTime
    {
        public int id;
        public ClassTimeWithID(string korClassTimeExpression, int id) : base(korClassTimeExpression)
        {
            this.id = id;
        }
    }

    
    /// <summary>
    /// public class ClassTime
    /// DateTime object for the class hours
    /// </summary>
    public class ClassTime
    {
        // constant Arrays. Array's index will be directly matched to the corresponding string expression.
        static public string[] dateArray {get;} = new string[] {"월","화","수","목","금"};
        private string[] _dateArray {get;} = dateArray;
        static public string[] timeArray {get;} = new string[] {"8:00","8:30","09:00","09:30","10:00","10:30","11:00","11:30",
        "12:00","12:30","13:00","13:30","14:00","14:30","15:00","15:30","16:00","16:30","17:00","17:30",
        "18:00","18:30","19:00","19:30","20:00","20:30","21:00"};
        private string[] _timeArray = timeArray;

        // each row of dateTimeRangeTable will be like [dateNum, startTimeNum, endTimeNum].
        // ex : 수 18:00~20:00 will became [2, 20, 24].
        public List<List<int>> dateTimeRangeTable {get;} = new List<List<int>>(); // security warn : delete public in production

        /// <summary>
        /// public ClassTime(string korClassTimeExpression)
        /// convert korClassTimeExpression to comparable ClassTime object.
        /// WARNING : this dosen't check if expression itself has fallacy, like "월 수 09:00~10:30, 수 9:00~11:00".
        /// </summary>
        /// <param name="korClassTimeExpression">"월 수 09:00~10:30, 수 18:00~20:00" order.</param>
        /// 
        public ClassTime(string korClassTimeExpression)
        {
            var parsedKor = korClassTimeExpression.Split(",");

            // split expr into ',' section.
            foreach (string dateTimeRowString in parsedKor)
            {
                var parsedRow = dateTimeRowString.Split(" ");

                var timeString = parsedRow[parsedRow.Length - 1]; // "18:00~20:00"
                var timeSplitedString = timeString.Split("~");

                var startTime = Array.IndexOf(this._timeArray, timeSplitedString[0]);
                var endTime = Array.IndexOf(this._timeArray, timeSplitedString[1]);

                // iter the target parsed expression like : ["월", "수"] except "18:00~20:00"
                for (int i=0; i < parsedRow.Length - 1; i++)
                {
                    var targetDate = parsedRow[i]; // 월
                    if (String.Equals("", targetDate)) {continue;}
                    var targetDateNum = Array.IndexOf(this._dateArray, targetDate); // 0
                    
                    int[] dateTimeRow = new int[] {targetDateNum, startTime, endTime}; // [0, 20, 24]
                    
                    dateTimeRangeTable.Add(new List<int>(dateTimeRow));
                }
            }
        }

        /// <summary>
        /// public static bool IsOverlapped(ClassTime classTime1, ClassTime classTime2)
        /// check if the two ClassTime object is overlapped in time.
        /// </summary>
        /// <param name="classTime1"></param>
        /// <param name="classTime2"></param>
        /// <returns>
        /// true : if overlapped
        /// false: if not overlapped
        /// </returns>
        public static bool IsOverlapped(ClassTime classTime1, ClassTime classTime2)
        {
            foreach (var c1DateRow in classTime2.dateTimeRangeTable)
            {
                // compare the each classTime2's dateRow is overlapping the classTime1's one.
                foreach (var c2DateRow in classTime1.dateTimeRangeTable)
                {
                    if (c1DateRow[0] != c2DateRow[0]) {continue;} // if date is diff then no overlap

                    var c1startTime = c1DateRow[1];
                    var c1endTime = c1DateRow[2];
                    var c2startTime = c2DateRow[1];
                    var c2endTime = c2DateRow[2];

                    if ((c1startTime <= c2startTime && c2startTime < c1endTime) //c2 start time check
                    || (c1startTime < c2endTime && c2endTime <= c1endTime))   // c2 end time check
                    {return true;}
                }
            }
            return false;
        }
    }
}