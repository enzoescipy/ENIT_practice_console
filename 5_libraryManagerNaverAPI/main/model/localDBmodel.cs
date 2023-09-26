using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using MySql.Data.MySqlClient;
using System.Data;
using MySqlX.XDevAPI.Relational;
using System.Data.Common;

namespace MainProject
{
    public class LocalDBmodel<VO>
        where VO : BasicVO , new()
    {
        public MySqlConnection mySqlConnection; 
        string myTableName;
        FieldInfo[] voFieldsInfo;
        FieldInfo[] voFieldsInfoExceptPkey;
        public LocalDBmodel(MySqlConnection mySqlConnection, string myTableName)
        {
            this.mySqlConnection = mySqlConnection;
            this.myTableName = myTableName;
            this.voFieldsInfo = typeof(VO).GetFields();
            this.voFieldsInfoExceptPkey  = (from vo in voFieldsInfo where !String.Equals(vo.Name, "pkey") select vo).ToArray();
        }

        private string GetFromVO(VO vo, FieldInfo info)
        {
            var value = info.GetValue(vo);
            if (value == null)
            {
                return "";
            }
            else if (value.GetType() == typeof(string))
            {
                return $"'{value}'";
            }
            else
            {
                return $"{value}";
            }
        }

        private void SetForVO(VO vo, FieldInfo info, dynamic value)
        {
            info.SetValue(vo, Convert.ChangeType(value,info.FieldType));
        }

        /// <summary>
        /// run query then execute actionEac() for each row of found.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="actionEach"></param>
        private void ReadQueryEach(string query, Action<MySqlDataReader> actionEach)
        {
            try
            {
                // transaction 넣어라
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        actionEach(reader);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("unexpected mySQL exception happened.");
            }
        }

        /// <summary>
        /// run query, get the result data, then put it to the resultSet
        /// </summary>
        /// <param name="query"></param>
        /// <param name="resultSet"></param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the DataSet
        /// </returns>
        private int ReadQueryThen(string query, DataSet resultSet)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, mySqlConnection);
                return adapter.Fill(resultSet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("unexpected mySQL exception happened.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>
        /// numbers of the row affected
        /// </returns>
        /// <exception cref="Exception"></exception>
        private int ExecuteQuery(string query)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("unexpected mySQL exception happened.");
            }
        }

        /// <summary>
        /// run query then change the reader of row to the VO
        /// then collect the VO to the resultList
        /// </summary>
        /// <param name="query"></param>
        /// <param name="resultList"></param>
        private void ReadEachThen(string query, List<VO> resultList)
        {
            // launch query
            ReadQueryEach(query, (q) => {
                VO vo = new VO();
                foreach(FieldInfo info in voFieldsInfo)
                {
                    SetForVO(vo, info, q[info.Name]);
                }
                resultList.Add(vo);
            });
        }

        /// <summary>
        /// append newVOList to the current DB row.
        /// can create new DB. can be called if current DB file not exists or empty.
        /// </summary>
        /// <param name="newVOList"></param>
        /// <returns>
        /// num of rows added
        /// </returns>
        public int Append(List<VO> newVOList)
        {
            // VO field name string
            string fieldNameQuery = $"INSERT INTO `library`.`{myTableName}` ( " +  String.Join(",",from voInfo in voFieldsInfoExceptPkey select $"`{voInfo.Name}`") +  " ) VALUES ";

            // VO field name : value string list
            List<string> queryList = new List<string>();
            foreach (VO vo in newVOList)
            {
                string valueQuery = "( " + String.Join(",",from voInfo in voFieldsInfoExceptPkey select $"{GetFromVO(vo, voInfo)}") + " );";
                queryList.Add(fieldNameQuery + valueQuery);
            }

            // save DB
            DebugConsole.Debug(String.Join("\n", queryList));
            return ExecuteQuery(String.Join("\n", queryList));
        }

        public List<VO> GetAll()
        {
            string query = $"SELECT * FROM library.{myTableName};";
            var newVOList = new List<VO>();
            ReadEachThen(query, newVOList);
            return newVOList;
        }


        /// <summary>
        /// find the matching property of the targetVO which is not null, in the DB then return the VO List.
        /// 
        /// mode 0 : simple equal.
        /// mode 1 : keyword search for string
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="compareFunc"> function that compare DB value and keyword value, then return bool. first param is DBvalue, sec param is keyword value.</param>
        /// <returns></returns>
        public List<VO> Find(string fieldName, dynamic fieldValue, int mode)
        {
            // make query   
            string modeString;
            if (mode == 0) { modeString = "=" ;}
            else { modeString = "LIKE" ;}
            string findQuery = $"SELECT * FROM library.{myTableName} where {fieldName} {modeString} '{fieldValue.ToString()}';";

            // launch query
            var resultList = new List<VO>();
            ReadEachThen(findQuery, resultList);

            return resultList;
        }

        /// <summary>
        /// search for keyword in 2 column
        /// </summary>
        /// <param name="fieldName1"></param>
        /// <param name="fieldName2"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<VO> Find(string fieldName1, string fieldName2, string keyword)
        {
            // make query   
            string findQuery = $"SELECT * FROM library.{myTableName} where {fieldName1} LIKE '{keyword.ToString()}' OR  {fieldName2} LIKE '{keyword.ToString()}';";

            // launch query
            var resultList = new List<VO>();
            ReadEachThen(findQuery, resultList);

            return resultList;
        }


        /// <summary>
        /// override the DB by the VO in the newVOList, which has same Pkey.
        /// if Pkey not matched, just skip.
        /// </summary>
        /// <param name="newVOList"></param>
        /// <returns>
        /// numbers of the row affected
        /// </returns>
        public int Override(List<VO> newVOList)
        {
            // make query
            string updateQuery = $"UPDATE `library`.`{myTableName}` SET "; // + String.Join(",",from voInfo in voFieldsInfoExceptPkey select $"`{voInfo.Name}`") + ") = ";

            // VO field name : value string list
            List<string> queryList = new List<string>();
            foreach (VO vo in newVOList)
            {
                string valueQuery = String.Join(",",from voInfo in voFieldsInfoExceptPkey select $"{voInfo.Name} = {GetFromVO(vo, voInfo)}");
                string pkeyQuery = $" WHERE (`pkey` = {vo.pkey});";
                queryList.Add(updateQuery + valueQuery + pkeyQuery);
            }

            DebugConsole.Debug(String.Join("\n", queryList));
            return ExecuteQuery(String.Join("\n", queryList));
        }


        /// <summary>
        /// naga
        /// </summary>
        /// <param name="deletePkeyList"></param>
        /// <returns>
        /// numbers of the row affected
        /// </returns>
        public int Delete(List<int> deletePkeyList)
        {
            // make query
            string updateQuery = $"DELETE FROM `library`.`{myTableName}`";

            // VO field name : value string list
            List<string> queryList = new List<string>();
            foreach (int pkey in deletePkeyList)
            {
                string pkeyQuery = $" WHERE (`pkey` = '{pkey}');";
                queryList.Add(updateQuery + pkeyQuery);
            }

            return ExecuteQuery(String.Join("\n", queryList));
        }


    }
}