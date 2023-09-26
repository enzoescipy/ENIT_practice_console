using System.Xml;
using MainProject;

public class BookSearchFromNaver
{
    private static BookSearchFromNaver? _bookSearchFromNaver = null;
    public static BookSearchFromNaver bookSearchFromNaver
    {
        get
        {
            if (_bookSearchFromNaver == null)
            {
                _bookSearchFromNaver = new BookSearchFromNaver();
            }
            return _bookSearchFromNaver;
        }
    }
    private NaverBookSearchAPI? naverBookSearchAPI;
    private string clientID = "8CG2J7yPuOxb4x2_E9Sy";
    private string cliendPW = "mecMAx7RJa";

    /// <summary>
    /// override the current NaverBookSearchAPI instance and get search result from the page 1 again
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="display"></param>
    /// <returns>
    /// state -1 : instance reached its end of page. 
    /// state 0 : succeed code 
    /// state 1 : request rejected 
    /// state 2 : xml parsing failed
    /// 
    /// bookVOs null : failed code occoured
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public (List<BookVO>? bookVOs, int state) initialSearch(string keyword, int display)
    {
        // validation of param
        if (!(keyword.Length > 0 && 0 < display && display <= 100))
        {
            throw new ArgumentException("똑바로써랴 앙?");
        }

        // make API instance and call Next()
        naverBookSearchAPI = new NaverBookSearchAPI(clientID, cliendPW, keyword, display);
        return NextSearch();
    }

    /// <summary>
    /// run the Next() for the current instance of naverAPI
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="display"></param>
    /// <returns>
    /// state -1 : instance reached its end of page. 
    /// state 0 : succeed code 
    /// state 1 : request rejected 
    /// state 2 : xml parsing failed
    /// state 5 : initialSearch() not called before or reached the pages end
    /// 
    /// bookVOs null : failed code occoured
    /// </returns>
    public (List<BookVO>? bookVOs, int state) NextSearch()
    {
        // if naverBookSearchAPI is null, block the call.
        if (naverBookSearchAPI == null)
        {
            return (null, 5);
        }
        // make API instance and call Next()
        var nextResult = naverBookSearchAPI.Next();
        int state = nextResult.state;
        if (state != 0)
        {
            return (null, state);
        }
        XmlNodeList nodeList = nextResult.xmlNodeList;

        // parse XML then make VOs
        var voList = new List<BookVO>();
        foreach (XmlNode nodeElement in nodeList)
        {
            foreach (XmlNode nodeTag in nodeElement)
            {
                if (nodeTag.Name == "item")
                {
                    string description = nodeTag["description"].InnerText;
                    if (description.Length > 25) {description = description.Substring(0,25) + " ...";}
                    description = description.Replace('\n', ' ');
                    var vo = new BookVO(
                        nodeTag["title"].InnerText,
                        description,
                        0,
                        nodeTag["author"].InnerText,
                        nodeTag["isbn"].InnerText
                    );
                    voList.Add(vo);
                }
            }
        }

        return (voList, 0);
    }
}