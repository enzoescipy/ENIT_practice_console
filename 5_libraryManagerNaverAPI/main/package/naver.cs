using System;
using System.Net;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Xml;
using MainProject;

public class NaverBookSearchAPI
{   
    private const string basicURL = "https://openapi.naver.com/v1/search/book.xml?";
    private string clientID;
    private string clientPW;
    private string query;
    private int display;
    private int start;
    private bool isReachedEnd = false;

    public NaverBookSearchAPI(string clientID, string clientPW, string keyword, int display)
    {
        this.clientID = clientID;
        this.clientPW = clientPW;
        this.query = keyword;
        if (keyword.Length > 0)
        {
            this.query = keyword;
        }
        else
        {
            throw new ArgumentException("keyword must be over length 1");
        }
        if (0 < display && display <= 100)
        {
            this.display = display;
        }
        else
        {
            throw new ArgumentException("display, start parameter should be range in 1 ~ 100");
        }
        this.start = 1 - display;
    }

    /// <summary>
    /// return the next searched book xml pages.
    /// if reached end then return just null.
    /// </summary>
    /// <returns>
    /// state -1 : instance reached its end of page.
    /// state 0 : succeed code
    /// state 1 : request rejected
    /// state 2 : xml parsing failed
    /// 
    /// xmlNodeList null : failed case
    /// else : succeed case
    /// </returns>
    public (XmlNodeList? xmlNodeList, int state) Next()
    {
        // validation of termination
        if (start + display > 100)
        {
            isReachedEnd = true;
            return (null, -1);
        }
        else if (isReachedEnd == true)
        {
            return (null, -1);
        }

        // request perform
        start = start + display;

        string url = basicURL + "query=" + query + "&display=" + display.ToString() + "&start=" + start.ToString();
        // DebugConsole.Debug(url);
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
        request.Headers.Add("X-Naver-Client-Id", clientID);
        request.Headers.Add("X-Naver-Client-Secret", clientPW);
        HttpWebResponse response = (HttpWebResponse) request.GetResponse();

        // failed request case
        if (response.StatusCode.ToString() != "OK")
        {
            return (null, 1);
        }

        // load xml to xmldocument
        XmlDocument xmlDocument = new XmlDocument();
        StreamReader responseStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        xmlDocument.LoadXml(responseStream.ReadToEnd());

        // get XmlElement
        if (xmlDocument.DocumentElement == null)
        {
            return (null, 2);
        }
        else
        {
            return (xmlDocument.DocumentElement.ChildNodes, 0);
        }
    }
}