using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.Collections.Specialized;
using System.Text;

/// <summary>
/// 將所收集到的資料POST到另一個網站的頁面中
/// </summary>
public class clsHttpHelper
{
    /// <summary>
    /// 準備需要的Html表單，並且把資料透過Javascript透過POST傳送到另一個網站的頁面中
    /// </summary>
    /// <param name="url">絕對網址</param>
    /// <param name="data">html表單資料：格式為 ASP.NET 中的 NameValueCollection 型別 </param>
    /// <returns>回傳html結果(當然因為Javascript的作用，此頁面就被Javascript轉入到新的網站中的頁面)</returns>
    private static String PreparePOSTData(string url, NameValueCollection data)
    {
        string formID = "PostForm";
        StringBuilder strForm = new StringBuilder();
        strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
        foreach (string key in data)
        {
            strForm.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + data[key] + "\">");
        }
        strForm.Append("</form>");
        StringBuilder strScript = new StringBuilder();
        strScript.Append("<script language='javascript'>");
        strScript.Append("var v" + formID + " = document." + formID + ";");
        strScript.Append("v" + formID + ".submit();");
        strScript.Append("</script>");
        return strForm.ToString() + strScript.ToString();
    }
    public static void RedirectnPOST(Page page, string destinationUrl, NameValueCollection data)
    {
        string strForm = PreparePOSTData(destinationUrl, data);
        page.Controls.Add(new LiteralControl(strForm));
    }
}