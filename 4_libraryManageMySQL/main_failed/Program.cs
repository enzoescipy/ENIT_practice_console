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
            // Console.OutputEncoding = System.Text.Encoding.Unicode;
            // Console.InputEncoding = System.Text.Encoding.Unicode;
            // while (true) {Page.page.FrontMenu();}
            // Debug();
        }
        public static void Debug()
        {
            string strConn = "Server=localhost;DataBase=library;Uid=root;Pwd=1111;Charset=utf8";
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM book",conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine(rdr["name"]);
                }
                rdr.Close();
            }
            
        }
    }

}