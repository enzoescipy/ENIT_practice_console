using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace testx
{
    class Program
    {
        static void Main(string[] args)
        {

            XmlDocument doc = new XmlDocument();

            
            doc.Load("http://openapi.naver.com/search?key=ae0b502421e326a37a09c432b0ecfc1d&query=nexearch&target=rank"); //API 주소




            XmlNodeList nodelist1 = doc.GetElementsByTagName("K"); //k 안에 있는
            XmlNodeList nodelist2 = doc.GetElementsByTagName("V"); //v안에 있는
            XmlNodeList nodelist3 = doc.GetElementsByTagName("S"); //s 안에 있는 


            for (int i = 0; i < nodelist1.Count;i++ ) //출력
            {

                Console.WriteLine(nodelist1.Item(i).InnerText + " " + " " + nodelist3.Item(i).InnerText + " " +nodelist2.Item(i).InnerText);


            }
        }
    }
}
