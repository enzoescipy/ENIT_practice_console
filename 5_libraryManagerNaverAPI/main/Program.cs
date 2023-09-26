using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MainProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Debug();
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            while (true) {Page.page.FrontMenu();}
        }
        public static void Debug()
        {
            DebugConsole.Debug(Convert.ToBoolean((sbyte) 1));
        }
    }

}