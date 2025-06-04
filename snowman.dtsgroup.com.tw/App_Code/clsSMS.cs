using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Collections;
using System.Collections.Specialized;

/// <summary>
/// clsSMS 的摘要描述
/// </summary>
public class clsSMS
{
    
    /// <summary>
    /// 傳送簡訊；
    /// </summary>
    /// <param name="pMobile">手機</param>
    /// <param name="pNm">收件人</param>
    /// <param name="pText">內容</param>
    /// <returns>回傳成功/失敗</returns>
    public static bool SendSMS(string pMobile, string pNm, string pText)
    {
        Encoding myEncoding = Encoding.GetEncoding("Big5");
        string vAddr = "http://smexpress.mitake.com.tw:9600/SmSendGet.asp?username=70550442&password=80559415&dstaddr=" + pMobile + "&EnCodeing=Big5&DestName=" + HttpUtility.UrlEncode(pNm, myEncoding) + "&smbody=" + HttpUtility.UrlEncode(pText, myEncoding) + "";
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(vAddr);
        req.Method = "GET";
        using (WebResponse wr = req.GetResponse())
        {
            using (StreamReader myStreamReader = new StreamReader(wr.GetResponseStream(), myEncoding))
            {
                string vRawData = myStreamReader.ReadToEnd();
                bool vStrData = ParseSMSStatus(vRawData, pMobile);
                return vStrData;
            }
        } 
    }

    /// <summary>
    /// 精簡版 只發送給ck 
    /// </summary>
    /// <param name="pNm"></param>
    /// <param name="pText"></param>
    /// <returns></returns>
    public static bool SendSMS(string pNm, string pText)
    {
        //發給ck的手機
        //SendSMS("0932312026", pNm,  pText);
        return true;
    }


    public static bool ParseSMSStatus(string pStatus, string pMobile)
    {
        string vReturn = "";
        using (StringReader reader = new StringReader(pStatus))
        {
            string vLine = string.Empty;
            do
            {
                vLine = reader.ReadLine();
                if (vLine != null)
                {
                    // do something with the line
                    if (vLine.IndexOf("=") > 0)
                    {
                        string vName = vLine.Split('=')[0];
                        string vVal = vLine.Split('=')[1];
                        if (vName == "statuscode")
                        {
                            switch (vVal)
                            {
                                case "*":
                                    vReturn = "系統發生錯誤";
                                    return false;
                                case "a":
                                    vReturn = "簡訊發送功能暫時停止服務，請稍候再試";
                                    return false;
                                case "b":
                                    vReturn = "簡訊發送功能暫時停止服務，請稍候再試";
                                    return false;
                                case "c":
                                    vReturn = "請輸入帳號";
                                    return false;
                                case "d":
                                    vReturn = "請輸入密碼";
                                    return false;
                                case "e":
                                    vReturn = "帳號、密碼錯誤";
                                    return false;
                                case "f":
                                    vReturn = "帳號已過期";
                                    return false;
                                case "h":
                                    vReturn = "帳號已被停用";
                                    return false;
                                case "k":
                                    vReturn = "無效的連線位址";
                                    return false;
                                case "m":
                                    vReturn = "必須變更密碼，在變更密碼前，無法使用簡訊發送服務";
                                    return false;
                                case "n":
                                    vReturn = "密碼已逾期，在變更密碼前，將無法使用簡訊發送服務";
                                    return false;
                                case "p":
                                    vReturn = "沒有權限使用外部Http程式";
                                    return false;
                                case "r":
                                    vReturn = "系統暫停服務，請稍後再試";
                                    return false;
                                case "s":
                                    vReturn = "帳務處理失敗，無法發送簡訊";
                                    return false;
                                case "t":
                                    vReturn = "簡訊已過期";
                                    return false;
                                case "u":
                                    vReturn = "簡訊內容不得為空白";
                                    return false;
                                case "v":
                                    vReturn = "無效的手機號碼";
                                    return false;
                                case "0":
                                    vReturn = "預約傳送中";//預約傳送中
                                    return true;
                                case "1":
                                    vReturn = "已送達業者";//已送達業者
                                    return true;
                                case "2":
                                    vReturn = "已送達業者";//已送達業者
                                    return true;
                                case "3":
                                    vReturn = "已送達業者";//已送達業者
                                    return true;
                                case "4":
                                    vReturn = "已送達手機";//已送達手機
                                    return true;
                                case "5":
                                    vReturn = "無效的手機號碼";//內容有錯誤
                                    return false;
                                case "6":
                                    vReturn = "無效的手機號碼";//門號有錯誤
                                    return false;
                                case "7":
                                    vReturn = "無效的手機號碼";//簡訊已停用
                                    return false;
                                case "8":
                                    vReturn = "無效的手機號碼";//逾時無送達
                                    return false;
                                case "9":
                                    vReturn = "無效的手機號碼";//預約已取消
                                    return false;
                                default:
                                    return false;
                            }
                        }
                    }
                }

            } while (vLine != null);
        }
        return false;
    }
}
/*
*	系統發生錯誤，請聯絡三竹資訊窗口人員
a	簡訊發送功能暫時停止服務，請稍候再試
b	簡訊發送功能暫時停止服務，請稍候再試
c	請輸入帳號
d	請輸入密碼
e	帳號、密碼錯誤
f	帳號已過期
h	帳號已被停用
k	無效的連線位址
m	必須變更密碼，在變更密碼前，無法使用簡訊發送服務
n	密碼已逾期，在變更密碼前，將無法使用簡訊發送服務
p	沒有權限使用外部Http程式
r	系統暫停服務，請稍後再試
s	帳務處理失敗，無法發送簡訊
t	簡訊已過期
u	簡訊內容不得為空白
v	無效的手機號碼
0	預約傳送中
1	已送達業者
2	已送達業者
3	已送達業者
4	已送達手機
5	內容有錯誤
6	門號有錯誤
7	簡訊已停用
8	逾時無送達
9	預約已取消
*/