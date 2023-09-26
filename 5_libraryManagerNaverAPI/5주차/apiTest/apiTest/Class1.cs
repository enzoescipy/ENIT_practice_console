using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace apiTest
{
    class Class1
    {
        const string NAVER_DISPLAY_STRING = "&display=";
        const string NAVER_ID = "4djOgPTvaUjxuEqSudHM";
        const string NAVER_SECRET = "bDuwCmqIPr";
        public const string NAVER_URL = "https://openapi.naver.com/v1/search/book_adv.xml?d_titl=";

        static void Main(string[] args)
        {
            WebRequest request;
            WebResponse response;
            Stream stream;
            XmlNode firstNode;
            XmlNode secondNode;
            XmlDocument xmlDocument = new XmlDocument();
            XmlNodeList xmlNodeList;


            string url = NAVER_URL + "검정" + NAVER_DISPLAY_STRING + '5' + "&start=1";
            request = (HttpWebRequest)WebRequest.Create(url);

            request.Headers.Add("X-Naver-Client-Id", NAVER_ID);
            request.Headers.Add("X-Naver-Client-Secret", NAVER_SECRET);           //api 접근

            response = request.GetResponse();
            stream = response.GetResponseStream();

            xmlDocument.Load(stream);

            firstNode = xmlDocument.SelectSingleNode("rss");
            secondNode = firstNode.SelectSingleNode("channel");

            xmlNodeList = secondNode.SelectNodes("item");

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                Console.WriteLine(xmlNode.SelectSingleNode("link").InnerText);  //책 출력.
            }
           

        }
    }
}
