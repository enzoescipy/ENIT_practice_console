using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
            var bookSearch = new BookSearchFromNaver();
            PrettyPrint.Pprint2DStringList(LibraryDB.libraryDB.book.ListVOStringify(bookSearch.initialSearch("책", 5).bookVOs));
            PrettyPrint.Pprint2DStringList(LibraryDB.libraryDB.book.ListVOStringify(bookSearch.NextSearch().bookVOs));

        }
        
    }

}