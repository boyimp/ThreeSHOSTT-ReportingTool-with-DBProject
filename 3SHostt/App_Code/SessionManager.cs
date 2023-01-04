using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SessionManager
/// </summary>
public enum SearchPageEnum
{
    None = 1, 
    BrokerageHome = 2, 
    BrokerageSearchInsured = 3, 
    BrokerageSearchQuoteRequest = 4, 
    BrokerageSearchUser = 5,
    PFCompanyHome = 6,
    PFCompanySearchQuoteRequest = 7,
    PFCompanySearchUser = 8,
    RateFlexSearchUser=9,
    RateFlexSearchQuoteRequest=10,
    RateFlexSearchQuote = 11,
    ErrorConfiguration = 12
}
public class SessionManager
{
    public SessionManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static SearchPageEnum CurrentSearchPage
    {
        get
        {
            if (HttpContext.Current.Session.Contents["CurrentSearchPage"] == null)
            {
                return SearchPageEnum.None;
            }
            else
            {
                return (SearchPageEnum)HttpContext.Current.Session.Contents["CurrentSearchPage"];
            }
        }
        set
        {
            HttpContext.Current.Session.Contents["CurrentSearchPage"] = value;
        }
    }
    public static Dictionary<string, string> SearchParams
    {
        get
        {
            return (Dictionary<string, string>)HttpContext.Current.Session.Contents["SearchParamsProduct"];
        }
        set
        {
            HttpContext.Current.Session.Contents["SearchParamsProduct"] = value;
        }
    }

    public static int CurrentTermID
    {
        get
        {
            if (HttpContext.Current.Session.Contents["CurrentTermID"] == null)
            {
                return 0;
            }
            else
            {
                return (int)HttpContext.Current.Session.Contents["CurrentTermID"];
            }
        }
        set
        {
            HttpContext.Current.Session.Contents["CurrentTermID"] = value;
        }
    }

    public static string CurrentUser
    {
        get
        {
            if (HttpContext.Current.Session.Contents["CurrentUser"] == null)
            {
                return null;
            }
            else
            {
                return (string)HttpContext.Current.Session.Contents["CurrentUser"];
            }
        }
        set
        {
            HttpContext.Current.Session.Contents["CurrentUser"] = value;
        }
    }

}
