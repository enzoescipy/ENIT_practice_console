using System.Data;

namespace MainProject
{
    /// <summary>
    /// public static class PrettyPrint
    /// print some variable very pretty. ex : 2D list.
    /// </summary>
    public static class PrettyPrint
    {
        /// <summary>
        /// public static void Pretty2DList(List<List<dynamic>> list)
        /// return prettier 2D list.
        /// (all elements of each row in 2D list must be simple, immutable type.)
        /// </summary>
        /// <param name="list"></param>
        public static List<List<string>> Prettier2DList(List<List<dynamic>> list)
        {
            // convert the all element to the string type.
            List<List<string>> listString = new List<List<string>>();
            foreach (var row in list)
            {
                var stringRow = new List<string>();
                foreach (var element in row)
                {
                    stringRow.Add(element.ToString());
                }
                listString.Add(stringRow);
            }

            // check the maximum length of each column
            List<int> columnMaxLen = new List<int>();

            foreach (var row in listString)
            {
                foreach (var item in row.Select((value, index) => (value, index)))
                {
                    int itemLength = item.value.Length;
                    int itemIndex = item.index;
                    if (columnMaxLen.Count <= itemIndex)
                    {
                        columnMaxLen.Add(itemLength);
                    }
                    else
                    {
                        if (columnMaxLen[itemIndex] < itemLength)
                        {
                            columnMaxLen[itemIndex] = itemLength;
                        }
                    }
                }
            }

            // put the ' ' every string that has lower length of its column Max Len.
            for (int rowIndex=0; rowIndex < listString.Count; rowIndex++)
            {
                for (int colIndex=0; colIndex < listString[rowIndex].Count; colIndex++)
                {
                    string target = listString[rowIndex][colIndex];
                    if (target.Length < columnMaxLen[colIndex]) 
                    {
                        int difference = columnMaxLen[colIndex] - target.Length;
                        for (int i=0; i < difference; i++) {listString[rowIndex][colIndex] += " ";}
                    }
                }
            }

            return listString;
        }

        /// <summary>
        /// public static void Pprint2DList(List<List<dynamic>> list)
        /// print the PrettyPrint.Prettier2DList(list) simple list.
        /// </summary>
        /// <param name="list"></param>
        public static void Pprint2DList(List<List<dynamic>> list)
        {
            // make the row of string from list
            var prittierList = Prettier2DList(list);
            var prittierRows = new List<string> ();

            foreach (var row in prittierList)
            {
                string stringifiedRow = String.Join("|",row);
                prittierRows.Add("|" + stringifiedRow + "|");
            }

            // find the max length row
            int maxRowLen = 0;
            foreach (var rowString in prittierRows)
            {
                maxRowLen = rowString.Length > maxRowLen ? rowString.Length : maxRowLen;
            }

            // print the table
            string lineSep = ""; 
            for (int i=0; i < maxRowLen; i++) {lineSep += "-";}
            Console.WriteLine(lineSep);

            foreach (var rowString in prittierRows)
            {
                Console.WriteLine(rowString);
            }

            Console.WriteLine(lineSep);

        }

        /// <summary>
        /// public static void PprintDataTable(List<List<dynamic>> list)
        /// print the PrettyPrint.Prettier2DList(list).
        /// but, this function reguards that list is the DataTable.
        /// 
        /// (regard each 1Dlist elements are the row of table.)
        /// </summary>
        /// <param name="list"></param>
        public static void PprintDataTable(List<List<dynamic>>? list)
        {
            if (list == null) {Console.WriteLine(); return;}
            // make the row of string from list
            var prittierList = Prettier2DList(list);
            var prittierRows = new List<string> ();

            foreach (var row in prittierList)
            {
                row[0] = row[0] + "|";
                string stringifiedRow = String.Join("|",row);

                prittierRows.Add("|" + stringifiedRow + "|");
            }

            // find the max length row
            int maxRowLen = 0;
            foreach (var rowString in prittierRows)
            {
                maxRowLen = rowString.Length > maxRowLen ? rowString.Length : maxRowLen;
            }

            // print the table
            string lineSep = ""; 
            for (int i=0; i < maxRowLen; i++) {lineSep += "-";}
            Console.WriteLine(lineSep);
            Console.WriteLine(prittierRows[0]);
            string lineDounbleSep = ""; 
            for (int i=0; i < maxRowLen; i++) {lineDounbleSep += "=";}
            Console.WriteLine(lineDounbleSep);
            foreach (var rowString in prittierRows.GetRange(1, prittierList.Count - 1))
            {
                Console.WriteLine(rowString);
            }

            Console.WriteLine(lineSep);
        }
        /// <summary>
        /// public static void PprintDataTable(DataTable dataTable)
        /// print the dataTable prittier.
        /// </summary>
        /// <param name="dataTable"></param>
        public static void PprintDataTable(DataTable? dataTable, List<string> indexColLine)
        {
            if (dataTable == null) {Console.WriteLine(); return;}
            List<List<dynamic>> dataTableList = new List<List<dynamic>>();
            dataTableList.Add(new List<dynamic> (indexColLine));
            for (int i=0; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                List<dynamic> rowList = new List<dynamic>();
                foreach (var item in row.ItemArray)
                {
                    rowList.Add(item.ToString());
                }
                dataTableList.Add(rowList);
            }

            PprintDataTable(dataTableList);
        }
    }
}