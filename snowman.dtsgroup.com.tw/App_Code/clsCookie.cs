using System;
using System.Web;
using ExtensionMethods;

/// <summary>
/// clsCookie：操作Cookie物件的類別
/// </summary>
public class clsCookie
{
    /// <summary>
    /// 寫入Cookie；
    /// </summary>
    /// <param name="cookieName">string: 名稱</param>
    /// <param name="cookieVal">string: 值</param>
    /// <param name="cookieHR">int: 小時</param>
    /// <returns>成功：回傳所帶入的值；失敗：回傳空白</returns>
    public static string SetCookie(string cookieName, string cookieVal, Int32 cookieHR)
    {
        try
        {
            //HttpCookie cookie = new HttpCookie(cookieName) { Value = clsCrypto.Encrypt(cookieVal, "80559415"), Expires = DateTime.Now.AddHours(cookieHR) };
            HttpCookie cookie = new HttpCookie(cookieName) { Value = cookieVal, Expires = DateTime.Now.AddHours(cookieHR) };
            HttpContext.Current.Response.Cookies.Add(cookie);
            return cookieVal.ToString();
        }
        catch (Exception)
        {
            return "";
        }
    }

    /// <summary>
    /// 取得Cookie；
    /// </summary>
    /// <param name="cookieName">string: 名稱</param>
    /// <returns>成功：回傳所設定的值</returns>
    public static string GetCookie(string cookieName)
    {
        String vGetCookie = "";
        try
        {
            HttpCookie cookie = new HttpCookie(cookieName);
        
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                vGetCookie = HttpContext.Current.Request.Cookies[cookieName].Value.ToString();
                //vGetCookie = clsCrypto.Decrypt(vGetCookie, "80559415");
                return vGetCookie;
            }
            else
            {
                return "";
            }
        }
        catch (Exception ex)
        {
            //return ex.ToString();
            return "";
        }
    }
}
/*
    //====單一值 cookie== 
    //add 
    System.Web.HttpContext.Current.Response.Cookies["cookie_name"].Value = set_value;
    //get 
    get_valeu = System.Web.HttpContext.Current.Response.Cookies["cookie_name"].Value;
    //clear 
    System.Web.HttpContext.Current.Response.Cookies["cookie_name"].Value  = "";
    //判斷是否存在 
    if(Request.Cookies["cookie_name"].Equals(null)){ 
    }
    //====cookie多值== 
    //add 
    HttpCookie appndCookie = System.Web.HttpContext.Current.Request.Cookies["cookie_name"]; 
    appndCookie.Values.Set("user_lang" , lang); 
    System.Web.HttpContext.Current.Response.AppendCookie(appndCookie);
    //get 
    HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies["cookie_name"];   
    get_value = authCookie.Values["user_lang"].ToString();
    //delete 
    System.Web.HttpContext.Current.Response.Cookies.Remove("cookie_name"); 
    System.Web.HttpContext.Current.Response.Cookies["cookie_name"].Expires = DateTime.Now; //到期時間 
    System.Web.HttpContext.Current.Response.Cookies["cookie_name"].Values["user_lang"] = ""; 
*/