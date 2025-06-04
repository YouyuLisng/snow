using System;
using System.Web;
using ExtensionMethods;

/// <summary>
/// clsSession：操作Session物件的類別
/// </summary>
public class clsSession
{
    /// <summary>
    /// 預設值；
    /// </summary>
    private clsSession()
    {
        sLoginID = "0";
        sLoginNm = "";
        sLoginCNm = "";
        sLoginTelD = "";
        sLoginTelM = "";
        sLoginEmail = "";
        
        sMemberID = "0";
        sMemberNm = "";
        sMemberTelD = "";
        sMemberTelM = "";
        sMemberEmail = "";
        sCapchaCode = "";
    }

    /// <summary>
    /// 取得/設定Session；
    /// </summary>
    public static clsSession Current
    {
        get
        {
            clsSession session =
              (clsSession)HttpContext.Current.Session["__MySession__"];
            if (session == null)
            {
                session = new clsSession();
                HttpContext.Current.Session["__MySession__"] = session;
            }
            return session;
        }
    }

    public string sLoginID { get; set; }
    public string sLoginNm { get; set; }
    public string sLoginCNm { get; set; }
    public string sLoginTelD { get; set; }
    public string sLoginTelM { get; set; }
    public string sLoginEmail { get; set; }
    
    public string sMemberID { get; set; }
    public string sMemberNm { get; set; }
    public string sMemberTelD { get; set; }
    public string sMemberTelM { get; set; }
    public string sMemberEmail { get; set; }
    public string sCapchaCode { get; set; }
}
/*
    //Get
    string sLoginID = clsSession.Current.sLoginID;
    string sLoginNm = clsSession.Current.sLoginNm;
    string sLoginCNm = clsSession.Current.sLoginCNm;
    string sLoginTelD = clsSession.Current.sLoginTelD;
    string sLoginTelM = clsSession.Current.sLoginTelM;
    string sLoginEmail = clsSession.Current.sLoginEmail;

    //Set
    clsSession.Current.sLoginID = rdrLoginID.ToString();
    clsSession.Current.sLoginNm = rdrLoginNm;
    clsSession.Current.sLoginCNm = rdrLoginCNm;
    clsSession.Current.sLoginTelD = rdrLoginTelD;
    clsSession.Current.sLoginTelM = rdrLoginTelM;
    clsSession.Current.sLoginEmail = rdrLoginEmail; 
*/