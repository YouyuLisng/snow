using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// HtmlHelper 的摘要描述
/// </summary>
public class HtmlHelper
{
    public HtmlHelper()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public static string StripHTML(string input)
    {
        return Regex.Replace(input, "<.*?>", String.Empty);
    }
    public static string fronthost = WebConfigurationManager.AppSettings["fronthost"] ?? "http://testwww.dtsgroup.com.tw/newweb";
    public static string basehost = WebConfigurationManager.AppSettings["basehost"] ?? "";
    public static string imgpath = WebConfigurationManager.AppSettings["imgpath"] ?? "Images";
    
    

}
public class HeadSetting
{
    public HeadSetting()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public static string title(string input)
    {
        return Regex.Replace(input, "<.*?>", String.Empty);
    }
    
}