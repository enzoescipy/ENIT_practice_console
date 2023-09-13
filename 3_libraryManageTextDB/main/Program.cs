using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            // var page = new Page(new ClassDB("db.txt"), new List<int>(), new List<int>());
            // while (true) {page.FrontMenu();}

            Debug();

        }
        public static void Debug()
        {
            var UserVOList = new List<UserVO>();
            UserVOList.Add(new UserVO(1,"boringa", "boring1", "boring@gmail.com"));
            UserVOList.Add(new UserVO(2,"boringb", "boring2", "boring@gmail.com"));
            UserVOList.Add(new UserVO(3,"boringc", "boring3", "boring@gmail.com"));


            // var UserVOList_2 = new List<UserVO>();
            // UserVOList_2.Add(new UserVO());
            // UserVOList_2.Add(new UserVO());
            // UserVOList_2.Add(new UserVO());

            var DBmodel = new LocalDBmodel<UserVO>("sample.db");

            // DBmodel.Override(UserVOList);
            DBmodel.Delete(new List<int>() {1});
            // DebugConsole.D1List(DBmodel.Find("id", "boringa", (i, t) => String.Equals(i, t)), (s) => {return s.id;});
            DebugConsole.D1List(DBmodel.GetAll(), (s) => {return s.password;});
            // DebugConsole.D1List(DBmodel.GetAll());
        }
    }

}