using System;
using System.Web;
using System.Net.Mail;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Web.Configuration;
using System.Configuration;
using System.Net.Configuration;

/// <summary>
/// ExtensionMethods 集合了所有在VBSCRIPT程式碼中常用但是C#.NET卻沒有的函數
/// 常看到的包含 DATEDIFF, LEFT, RIGHT 等
/// </summary>
namespace ExtensionMethods
{
    public static class MyExtensions 
    {
        /*移除特定的HTML標籤*/
        public static int RemoveNodesButKeepChildren(this HtmlNode rootNode, string xPath)
        {
            HtmlNodeCollection nodes = rootNode.SelectNodes(xPath);
            if (nodes == null)
                return 0;
            foreach (HtmlNode node in nodes)
                node.RemoveButKeepChildren();
            return nodes.Count;
        }
        
        public static void RemoveButKeepChildren(this HtmlNode node)
        {
            foreach (HtmlNode child in node.ChildNodes)
                node.ParentNode.InsertBefore(child, node);
            node.Remove();
        }
        /*移除特定的HTML標籤中的Style屬性*/
        public static void RemoveStyle(this HtmlNode rootNode)
        {
            HtmlNodeCollection nodes = rootNode.SelectNodes("//@style");
            try
            {
                foreach (HtmlNode node in nodes)
                    node.Attributes["style"].Remove();
            }
            catch
            {
                
            }
        }
        /*移除特定的HTML標籤中的Class屬性*/
        public static void RemoveClass(this HtmlNode rootNode)
        {
            HtmlNodeCollection nodes = rootNode.SelectNodes("//@class");
            try
            {
                foreach (HtmlNode node in nodes)
                    node.Attributes["class"].Remove();
            }
            catch
            {
                
            }
        }
        /// <summary>
        /// 判斷是否為正確的日期格式
        /// </summary>
        public static bool IsDate(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                DateTime dt;
                return (DateTime.TryParse(input, out dt));
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 清除所有的Cache資料
        /// </summary>
        public static void Clear(this Cache x)
        {
            List<string> cacheKeys = new List<string>();
            IDictionaryEnumerator cacheEnum = x.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cacheKeys.Add(cacheEnum.Key.ToString());
            }
            foreach (string cacheKey in cacheKeys)
            {
                x.Remove(cacheKey);
            }
        }
        /// <summary>
        /// Returns the last few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the 
        /// given length the complete string is returned. If length is zero or 
        /// less an empty string is returned
        /// http://www.extensionmethod.net/
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Right(this string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
            {
                return s.Substring(s.Length - length, length);
            }
            else
            {
                return s;
            }
        }
        
        /// <summary>
        /// Returns the first few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the 
        /// given length the complete string is returned. If length is zero or 
        /// less an empty string is returned
        /// http://www.extensionmethod.net/
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Left(this string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
            {
                return s.Substring(0, length);
            }
            else
            {
                return s;
            }
        }

        /// <summary>
        /// Try to parse any given string to Integer
        /// http://www.extensionmethod.net/
        /// </summary>
        /// <param name="current">the string to process</param>
        /// <returns></returns>
        public static int ToInt(this string current)
        {
            int convertedValue;

            int.TryParse(current, out convertedValue);

            return convertedValue;
        }

        /// <summary>
        /// Prefix any given number with the right number of zeros
        /// http://madskristensen.net/post/Force-length-on-integer-in-C.aspx
        /// </summary>
        /// <param name="number">the number to process</param>
        /// <param name="length">Number of zeros to add as prefix</param>
        /// <returns></returns>
        public static string ForceLength(this int number, int length)
        {
            string s = number.ToString();
            string returnString = "";

            for (int i = s.Length; i < length; i++)
            {
                returnString += "0";
            }

            return returnString + s;
        }
        
        /// <summary>
        /// Send an email using the supplied string.
        /// 寄通知信功能
        /// </summary>
        /// <param name="body">String that will be used is the body of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="sender">The email address from which the message was sent.</param>
        /// <param name="recipient">The receiver of the email.</param> 
        /// <param name="server">The server from which the email will be sent.</param>  
        /// <returns>A boolean value indicating the success of the email send.</returns>
        public static bool Email(this string body, string subject, string sender, string recipient, string server)
        {
            Configuration config =
             WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings =
                (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            //Response.Write("SMTP 主機: " + settings.Smtp.Network.Host + "<br />");
            //Response.Write("SMTP 埠號: " + settings.Smtp.Network.Port + "<br />");
            //Response.Write("SMTP 帳號: " + settings.Smtp.Network.UserName + "<br />");
            //Response.Write("SMTP 密碼: " + settings.Smtp.Network.Password + "<br />");
            //Response.Write("預設寄件者:" + settings.Smtp.From + "<br />");
            try
            {
                // To
                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(recipient);

                // From
                MailAddress mailAddress = new MailAddress(sender);
                mailMsg.From = mailAddress;

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.Body = body;
                mailMsg.IsBodyHtml = true;
                mailMsg.Priority = MailPriority.Normal;

                // Init SmtpClient and send
                //SmtpClient smtpClient = new SmtpClient(server);
                SmtpClient smtpClient = new SmtpClient(settings.Smtp.Network.Host);
                smtpClient.Port = settings.Smtp.Network.Port;
                if (smtpClient.Port != 25)
                {
                    smtpClient.EnableSsl = true;
                }
                //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                //smtpClient.Credentials = credentials;
                smtpClient.Credentials = new NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        //public static bool Emails(this string body, string subject, string sender, string recipient)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DicQuesText">Email內容</param>
        /// <param name="HiLiteID">公司LiteID 14</param>
        /// <param name="LoginID">登入代碼 0</param>
        /// <param name="MemberID">會員編號 0</param>
        /// <param name="DicQuesCNm">姓名</param>
        /// <param name="DicQuesEmail">Email</param>
        /// <param name="DicQuesTelD">電話</param>
        /// <param name="DicQuesTelM">手機</param>
        /// <param name="DicQuesCat">1商品預約單 2聯繫需求單 3電話諮詢紀錄 4企業會員需求單 3獎旅需求單</param>
        /// <param name="DicQuesPPL">訂單編號 沒有請填0</param>
        /// <param name="DicQuesSubject">信件主題</param>
        /// <param name="DicQuesNm">問題名稱 商品項目名稱</param>
        /// <param name="DicQuesReply">回覆內容 無使用</param>
        /// <param name="DicQuesNotes">註記 商品項目內容 注意上限4000字元</param>
        /// <param name="WriteDB">寫入資料庫</param>
        /// <param name="MailSample">信件範本 暫時未實作 請填0</param>
        /// <returns>g是否出錯誤</returns>
        public static bool Email2DB(this string DicQuesText,
                    //DicQuesID , //流水號
                    int HiLiteID, //公司代碼 14 16 17 19  <asp:ListItem Selected="True" Value="14">台北總公司</asp:ListItem>
                                  //< asp:ListItem Value = "19" > 台中分公司華揚旅行社 </ asp:ListItem >
                                  //< asp:ListItem Value = "16" > 高雄分公司 </ asp:ListItem >
                    int LoginID,//登入代碼
                    int MemberID,//會員編號
                                 //string ProductExtNo ,// 產品代碼 未使用
                                 //string DicLangCode ,
                    string DicQuesCNm,
                    string DicQuesEmail,
                    string DicQuesTelD,//電話
                    string DicQuesTelM,//手機
                    string DicQuesCat, //DicCatCode	DicCatNm   1   商品預約單 2   聯繫需求單 3   電話諮詢紀錄 4   企業會員需求單 3   獎旅需求單
                                       //string DicQuesArea, //地區
                    int DicQuesPPL,//訂單編號 沒有請填0
                    string DicQuesSubject,//信件主題 limit 254
                                          //string DicQuesText , //字元無上限
                                          //string DicQuesCoun ,//固定值
                    string DicQuesNm, //問題名稱 商品項目名稱
                    string DicQuesReply,//字元 4000內
                    string DicQuesNotes,//無使用 4000內 商品或行程內容
                                        //DateTime DicQuesAD , //訊息記錄日期
                                        //DateTime DicQuesRD ,//回復日期
                                        //int DicQuesStatus //DicCatCode	DicCatNm  0   未回覆 1   已完成 2   處理中 3   取消
                    bool WriteDB,//寫入資料庫
                    int MailSample //保留項目 寄信內容範本跟寄信列表類型
                    
            )
        {
            //取得MailServerIP 
            bool hasError = false;
            string MailServerIP = WebConfigurationManager.AppSettings["MailServerIP"];
            using (DataModel dm = new DataModel())
            {

                //選擇email樣板

                //0
                string mailtitle = DicQuesSubject;
                //1 客服：  顯示在標題
                string contactinfoshort = "";
                //2 項目標題
                string itemtitle = "";
                //3
                string username = DicQuesCNm;
                //4 單號:
                string orderno = DicQuesPPL.ToString();
                //5 項目名稱
                string itemname = DicQuesNm;
                //6 
                string bodytitle = "您的"+itemtitle+"訂單如下：";
                //7 您的"票券"{itemtitle}訂單如下：
                string itembody = DicQuesNotes;

                //8 "台北02-25815859" 聯絡資訊 顯示在標題結尾
                string contactinfo = "";

                #region 信件內容

                string mailtemplate = @"<html>
                                            <head>
                                                <title>《大榮旅遊》{0}需求通知 {1} </title>
                                                <meta http-equiv=Content-Type content='text/html; charset=utf-8'>
                                                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css'>
                                            </head>
                                            <body>
                                                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                                                    <tr>
                                                        <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                                                        <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                                                        <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                                                    </tr>
                                                    <tr>
                                                        <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                                                        <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                                                            <table width=509 border=0 cellspacing=0 cellpadding=0>
                                                                <tr>
                                                                    <td>
                                                                        <table border=0 cellspacing=1 cellpadding=0>
                                                                            <tr>
                                                                                <td align=left class=wording11px>Complete Notification</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><font color=#990066 style=font-weight:bolder>{2}需求通知</font></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                                                                    <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                                                        <td align=left valign=top bgcolor=#FFFFFF class=wording13px>
                                                            <br />{3}&nbsp;您好：<br />
                                                            大榮旅遊客服中心已收到您的訂購與詢問需求單，{4}，我們將會儘速與您聯絡，處理{5}相關事宜。本確認函僅通知您所需求之內容，不代表您所訂購與需求已成立，所有細節確認項目將以客服人員所回覆為準，若造成您的不便與困擾敬請見諒。
                                                            <br />
                                                            {7}
                                                            <br />
                                                            {6}
                                                            <br />
                                                            <br />
                                                            如有任何疑問或需修改訂單相關資訊，請來電大榮旅遊 {8} ，客服人員將竭誠為您服務！
                                                            <br />
                                                            <br />
                                                            大榮旅遊 敬祝 鈞安<br /><br />
                                                        </td>
                                                        <td bgcolor=#FFFFFF>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                                                        <td bgcolor=#FFFFFF>&nbsp;</td>
                                                        <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                                                        <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                                                        <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";

                #endregion

                switch (MailSample) {
                    //取得密碼
                    case 1:
                        break;
                    //需求單
                    case 2:
                        break;
                    //訂購單
                    case 3:
                        break;
                    //特殊訂單
                    case 4:
                        break;
                }


                #region mail樣版

                //非需求類型

                #region 2.修改密碼

                //string myEmailBody = @"<html>
                //            <head>
                //                <title>《大榮旅遊》同業修改密碼通知 客服：02-25679315 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>修改密碼通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                您的新密碼為：{1}
                //                <br /><br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";

                #endregion




                #region 4.同業加入會員

                //if (vLoginEmail != "")
                //{
                //    myEmailBody2.Email("大榮旅遊 - 加入同業會員通知信 ", "service@gogojp.com.tw", vLoginEmail, mailip);
                //}
                //myEmailBody2.Email("大榮旅遊 - 加入同業會員通知信 ", "service@gogojp.com.tw", "lin1688@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 加入同業會員通知信 ", "service@gogojp.com.tw", "elain.ho@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 加入同業會員通知信 ", "service@gogojp.com.tw", "henrywung@gogojp.com.tw", mailip);

                //string myEmailBody = @"<html>
                //                <head>
                //                <title>《大榮旅遊》加入同業會員通知信 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>加入同業會員通知信</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                我們已收到您的加入同業會員通知信，我們將會儘速與您聯絡。<br /><br />
                //                會員編號：{1}<br />
                //                會員姓名：{2}<br />
                //                身分證字號：{3}<br />
                //                同業公司名稱：{4}<br />
                //                同業公司統編：{5}<br />
                //                如有任何疑問或相關資訊，請來電大榮旅遊台北02-25679315；台中04-22994567；高雄07-2727690，同業客服人員將竭誠為您服務！
                //                <br />
                //                <br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";

                #endregion
                //lin1688@gogojp.com.tw
                //elain.ho@gogojp.com.tw
                //henrywung@gogojp.com.tw

                #region 5.同業忘記密碼

                //string myEmailBody = @"<html>
                //            <head>
                //                <title>《大榮旅遊》同業忘記密碼通知 客服：02-25679315 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>忘記密碼通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                您的密碼為：{1}
                //                <br /><br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";
                //string myEmailBody2 = string.Format(myEmailBody, rdrMemberCFNm + rdrMemberCLNm, rdrMemberPWD);
                //string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
                //myEmailBody2.Email("大榮旅遊 - 忘記密碼通知信函", "service@gogojp.com.tw", vEmail, mailip);

                #endregion


                //需求類型

                #region  1.通用活動需求單

                //活動頁 email對象
                //vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", email, mailip);//消費者
                //vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "service@gogojp.com.tw", mailip);//正式機   

                //vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "wuchia@gogojp.com.tw", mailip);//嘉嘉

                //vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "charly@gogojp.com.tw", mailip);//鉅晏
                //vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "ck@gogojp.com.tw", mailip);//卡伯

                //string vEmailMsg = @"<html>
                //                    <head>
                //                        <title>《大榮旅遊》" + ProductNm + @"訂單需求通知</title>
                //                        <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                        <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                    </head>
                //                <body>
                //                    <table width=586 border=0 cellpadding=0 cellspacing=0>
                //                        <tr>
                //                            <td><img src=http://www.dtsgroup.com.tw/imgs/header.jpg  /></td>
                //                        </tr>
                //                        <tr>
                //                            <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />
                //                            您好：<br />
                //                            大榮旅遊客服中心已收到您的詢問需求單，我們將會儘速與您聯絡，處理旅遊相關事宜<br />
                //                            本確認函僅通知您所需求之內容，不代表您所需求已成立，所有細節確認項目將以客服<br />
                //                            人員所回覆為準。若造成您的不便與困擾敬請見諒。<br />" +
                //                            sProductDetail + @"
                //                            姓名： " + name + @"<br>
                //                            信箱： " + email + @"<br>
                //                            電話： " + tel + @"<br>
                //                            主題： " + MailTitle + " " + @"<br>
                //                            留言： " + msg + @"<br>
                //                            <br />
                //                            如有任何疑問，請來電大榮旅遊台北網路部 02-25223219<br />
                //                            大榮旅遊將竭誠為您服務！
                //                            <br />
                //                            <br />
                //                            大榮旅遊 敬祝 鈞安<br />
                //                            <br /></td>
                //                        </tr>
                //                        <tr>
                //                            <td><img src=http://www.dtsgroup.com.tw/imgs/footer.jpg /></td>
                //                        </tr>
                //                    </table>
                //                </body>
                //                </html>";

                #endregion
                //service@gogojp.com.tw
                //wuchia@gogojp.com.tw
                //charly@gogojp.com.tw
                //ck@gogojp.com.tw

                #region 15.一般行程需求單
                //string myEmailBody = @"<html>
                //                <head>
                //                <title>《大榮旅遊》訂單需求通知 客服：02-25223219 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>訂單需求通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                大榮旅遊客服中心已收到您的訂購與詢問需求單，單號&nbsp;{1}&nbsp;，我們將會儘速與您聯絡，處理旅遊相關事宜。本確認函僅通知您所需求之內容，不代表您所訂購與需求已成立，所有細節確認項目將以客服人員所回覆為準，若造成您的不便與困擾敬請見諒。
                //                <!--
                //                本系統將寄發此訂單之查詢密碼至您所填寫的email信箱，您可使用身分證字號&nbsp;{2}&nbsp;及此組查詢密碼&nbsp;{3}&nbsp;於大榮旅遊網站首頁右上方之【訂單查詢】(如下圖所示)查詢此訂單處理的狀況<br />
                //                <br />
                //                <img src=http://www.dtsgroup.com.tw/imgs/order_compelet_img.gif width=517 height=110 /><br />
                //                <br />
                //                使用【訂單查詢】功能，您將可了解您所訂購之旅遊產品的服務人員、需繳金額(訂金及尾款)、選擇付款的方式(現金、傳真刷卡、匯款)等訂單詳細資訊。
                //                -->
                //                如有任何疑問或需修改訂單相關資訊，請來電大榮旅遊台北02-25223219 / 02-25429288；台中04-22982299/04-22994567；高雄07-2727690，客服人員將竭誠為您服務！
                //                <br />
                //                <br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";
                //string myEmailBody2 = string.Format(myEmailBody, vOrderCNm, vOrderID, vMemberPID, vMemberPWD);
                //string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", vMemberEmail, mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", vLoginEmail, mailip);
                ////myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "charly@gogojp.com.tw", mailip);
                //switch (vHiLiteID)
                //{
                //    case "14":
                //        myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "leo@gogojp.com.tw", mailip);
                //        break;
                //    case "16":
                //        myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "babu0521@gogojp.com.tw", mailip);
                //        break;
                //    case "17":
                //        myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "clara@gogojp.com.tw", mailip);
                //        break;
                //    default:
                //        myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "leo@gogojp.com.tw", mailip);
                //        break;
                //}

                #endregion
                //charly@gogojp.com.tw
                //switch (vHiLiteID)
                //{
                //    case "14":
                //leo@gogojp.com.tw
                //        break;
                //    case "16":
                //babu0521@gogojp.com.tw
                //        break;
                //    case "17":
                //clara@gogojp.com.tw
                //        break;
                //    default:
                //leo@gogojp.com.tw
                //        break;
                //}

                #region 3.同業需求單

                //switch (vHiLiteID)
                //{
                //    case "14":
                //        if (vLoginEmail != "lin1688@gogojp.com.tw")
                //        {
                //            myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", vLoginEmail, mailip);
                //        }
                //        myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "lin1688@gogojp.com.tw", mailip);
                //        break;
                //    case "17":
                //        if (vLoginEmail != "MIAO@GOGOJP.COM.TW")
                //        {
                //            myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", vLoginEmail, mailip);
                //        }
                //        myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "MIAO@GOGOJP.COM.TW", mailip);
                //        break;
                //    case "16":
                //        if (vLoginEmail != "babu0521@gogojp.com.tw")
                //        {
                //            myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", vLoginEmail, mailip);
                //        }
                //        myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "babu0521@gogojp.com.tw", mailip);
                //        break;
                //}
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", vMemberEmail, mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "ck@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "6524@gogojp.com.tw", mailip);

                //string myEmailBody = @"<html>
                //                <head>
                //                <title>《大榮旅遊》同業訂單需求通知 客服：02-25223219 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>訂單需求通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                大榮旅遊客服中心已收到您的訂購與詢問需求單，單號&nbsp;{1}&nbsp;，我們將會儘速與您聯絡，處理旅遊相關事宜。本確認函僅通知您所需求之內容，不代表您所訂購與需求已成立，所有細節確認項目將以客服人員所回覆為準，若造成您的不便與困擾敬請見諒。
                //                <!--
                //                本系統將寄發此訂單之查詢密碼至您所填寫的email信箱，您可使用身分證字號&nbsp;{2}&nbsp;及此組查詢密碼&nbsp;{3}&nbsp;於大榮旅遊網站首頁右上方之【訂單查詢】(如下圖所示)查詢此訂單處理的狀況<br />
                //                <br />
                //                <img src=http://www.dtsgroup.com.tw/imgs/order_compelet_img.gif width=517 height=110 /><br />
                //                <br />
                //                使用【訂單查詢】功能，您將可了解您所訂購之旅遊產品的服務人員、需繳金額(訂金及尾款)、選擇付款的方式(現金、傳真刷卡、匯款)等訂單詳細資訊。
                //                -->
                //                如有任何疑問或需修改訂單相關資訊，請來電大榮旅遊台北02-25679315 / 02-25429288；台中04-22982299/04-22994567；高雄07-2727690，客服人員將竭誠為您服務！
                //                <br />
                //                <br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";

                #endregion
                //switch (vHiLiteID)
                //{
                //    case "14":
                //        if (vLoginEmail != "lin1688@gogojp.com.tw")
                //        {
                //        vLoginEmail, mailip);
                //        }
                //        lin1688@gogojp.com.tw
                //        break;
                //    case "17":
                //        if (vLoginEmail != "MIAO@GOGOJP.COM.TW")
                //        {
                //        vLoginEmail, mailip);
                //        }
                //        MIAO@GOGOJP.COM.TW
                //        break;
                //    case "16":
                //        if (vLoginEmail != "babu0521@gogojp.com.tw")
                //        {
                //        vLoginEmail, mailip);
                //        }
                //        babu0521@gogojp.com.tw
                //        break;
                //}
                //vMemberEmail
                //ck@gogojp.com.tw
                //6524@gogojp.com.tw


                #region 6.自由行需求

                //string myEmailBody = @"<html>
                //                <head>
                //                <title>《大榮旅遊》自由行需求通知 客服：02-25815859 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>自由行需求通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                大榮旅遊客服中心已收到您的訂購與詢問需求單，我們將會儘速與您聯絡，處理票券相關事宜。本確認函僅通知您所需求之內容，不代表您所訂購與需求已成立，所有細節確認項目將以客服人員所回覆為準，若造成您的不便與困擾敬請見諒。
                //                <br />
                //                您的票券訂單如下：
                //                <br />
                //                {1}
                //                <br />
                //                <br />
                //                如有任何疑問或需修改訂單相關資訊，請來電大榮旅遊台北02-25815859 ，客服人員將竭誠為您服務！
                //                <br />
                //                <br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";
                //string myEmailBody2 = string.Format(myEmailBody, v姓名, vOrderDetails);
                //string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", v電子郵件, mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "chloe@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "sale35@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "ck@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "leo@gogojp.com.tw", mailip);

                #endregion
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", v電子郵件, mailip);
                //chloe@gogojp.com.tw
                //sale35@gogojp.com.tw
                //ck@gogojp.com.tw
                //leo@gogojp.com.tw

                #region 7.自由行需求

                //string myEmailBody = @"<html>
                //                <head>
                //                <title>《大榮旅遊》自由行訂單需求通知 客服：02-25815859 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>飯店訂單需求通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                大榮旅遊客服中心已收到您的訂購與詢問需求單，我們將會儘速與您聯絡，處理飯店訂房相關事宜。本確認函僅通知您所需求之內容，不代表您所訂購與需求已成立，所有細節確認項目將以客服人員所回覆為準，若造成您的不便與困擾敬請見諒。
                //                <br />
                //                您的票券訂單如下：
                //                <br />
                //                {1}
                //                <br />
                //                <br />
                //                如有任何疑問或需修改訂單相關資訊，請來電大榮旅遊台北02-25815859 ，客服人員將竭誠為您服務！
                //                <br />
                //                <br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";
                //string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
                //string myEmailBody2 = string.Format(myEmailBody, vNameF + vNameL, vOrderDetails);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", vEmail, mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "chloe@gogojp.com.tw", mailip); //芳羽
                //                                                                                               //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "sale35@gogojp.com.tw", mailip); //自由行倩如
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "ck@gogojp.com.tw", mailip); //卡伯
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "leo@gogojp.com.tw", mailip); //東謀
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "joan@gogojp.com.tw", mailip); //瓊如


                #endregion
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", vEmail, mailip);
                //chloe@gogojp.com.tw
                //sale35@gogojp.com.tw
                //ck@gogojp.com.tw
                //leo@gogojp.com.tw
                //joan@gogojp.com.tw

                #region 8.旅行需求 企業

                //組合需求單Email
                //string vEmailMsg = @"<html>
                //                <head>
                //                <title>《大榮旅遊》訂單需求通知</title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>訂單需求通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />";
                //vEmailMsg += vDicQuesText + "<br />";
                //vEmailMsg += "      如有任何疑問或需修改訂單相關資訊，請來電大榮旅遊台北";
                //if (v旅遊需求 == "員工旅遊")
                //{
                //    vEmailMsg += "網路部 02-25223219";
                //}
                //else
                //{
                //    vEmailMsg += "獎旅部 02-25674988";
                //}
                //vEmailMsg += @"<br />大榮旅遊將竭誠為您服務！
                //                <br />
                //                <br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";
                //string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
                //string myEmailBody2 = string.Format(vEmailMsg, "", "", "", "");
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", v信箱, "192.168.201.94");
                //if (v旅遊需求 == "員工旅遊")
                //{
                //    myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "leo@gogojp.com.tw", mailip);
                //    myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "joy0222@gogojp.com.tw", mailip);
                //    myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "chloe@gogojp.com.tw", mailip);
                //}
                //else
                //{
                //    myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "chloe@gogojp.com.tw", mailip);
                //    myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "joy0222@gogojp.com.tw", mailip);
                //    myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "leo@gogojp.com.tw", mailip);
                //}

                #endregion
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", v信箱, "192.168.201.94");
                //if (v旅遊需求 == "員工旅遊")
                //{
                //chloe@gogojp.com.tw
                //joy0222@gogojp.com.tw
                //leo@gogojp.com.tw
                //}
                //else
                //{
                //chloe@gogojp.com.tw
                //joy0222@gogojp.com.tw
                //leo@gogojp.com.tw


                //訂購類型

                #region 10.沖繩訂房

                //string myEmailBody = @"<html>
                //                <head>
                //                <title>《大榮旅遊》沖繩訂房需求通知 客服：02-25815859 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>沖繩訂房需求通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                大榮旅遊客服中心已收到您的訂購與詢問需求單，我們將會儘速與您聯絡，處理票券相關事宜。本確認函僅通知您所需求之內容，不代表您所訂購與需求已成立，所有細節確認項目將以客服人員所回覆為準，若造成您的不便與困擾敬請見諒。
                //                <br />
                //                您的票券訂單如下：
                //                <br />
                //                {1}
                //                <br />
                //                <br />
                //                如有任何疑問或需修改訂單相關資訊，請來電大榮旅遊台北02-25815859 ，客服人員將竭誠為您服務！
                //                <br />
                //                <br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";
                //string myEmailBody2 = string.Format(myEmailBody, vNameF + vNameL, vOrderDetails);
                //string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", vEmail, mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "chloe@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "sale35@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "ck@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "leo@gogojp.com.tw", mailip);

                #endregion
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", vEmail, mailip);
                //chloe@gogojp.com.tw
                //sale35@gogojp.com.tw
                //ck@gogojp.com.tw
                //leo@gogojp.com.tw

                #region 11.舊手機板沖繩訂房

                //string myEmailBody = @"<html>
                //                <head>
                //                <title>《大榮旅遊》自由行訂單需求通知 客服：02-25815859 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>團體訂單需求通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                大榮旅遊客服中心已收到您的訂購與詢問需求單，我們將會儘速與您聯絡，處理參團相關事宜。本確認函僅通知您所需求之內容，不代表您所訂購與需求已成立，所有細節確認項目將以客服人員所回覆為準，若造成您的不便與困擾敬請見諒。
                //                <br />
                //                您的團體訂單如下：
                //                <br />
                //                {1}
                //                <br />
                //                <br />
                //                如有任何疑問或需修改訂單相關資訊，請來電大榮旅遊台北02-25815859 ，客服人員將竭誠為您服務！
                //                <br />
                //                <br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";
                //string myEmailBody2 = string.Format(myEmailBody, vName + vPhone, vOrderDetails);
                //string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "chloe@gogojp.com.tw", mailip); //芳羽        
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "ck@gogojp.com.tw", mailip); //卡伯
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "leo@gogojp.com.tw", mailip); //東謀
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "tim00363@gogojp.com.tw", mailip); //祥

                #endregion
                //chloe@gogojp.com.tw //芳羽        
                //ck@gogojp.com.tw //卡伯
                //leo@gogojp.com.tw //東謀
                //tim00363@gogojp.com.tw //祥

                #region 12.沖繩租車

                //string vTicket = "訂購明細:" + v訂購明細 + " <br/>出發日期:" + v出發日期 + " " + v出發小時 + "點" + v出發分鐘 + "分<br />承租台數：" + v承租台數 + "<br />取車地點：" + v取車地點 + "<br />姓名：" + v姓名 + "<br />電話：" + v電話 + "<br />電子郵件：" + v電子郵件 + "<br />";
                //string myEmailBody = @"<html>
                //                <head>
                //                <title>《大榮旅遊》租車訂單需求通知 客服：02-25815859 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>租車訂單需求通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                大榮旅遊客服中心已收到您的訂購與詢問需求單，我們將會儘速與您聯絡，處理租車相關事宜。本確認函僅通知您所需求之內容，不代表您所訂購與需求已成立，所有細節確認項目將以客服人員所回覆為準，若造成您的不便與困擾敬請見諒。
                //                <br />
                //                您的租車訂單如下：
                //                <br />
                //                {1}
                //                <br />
                //                <br />
                //                如有任何疑問或需修改訂單相關資訊，請來電大榮旅遊台北02-25223219 / 02-25429288；台中04-22982299/04-22994567；高雄07-2727690，客服人員將竭誠為您服務！
                //                <br />
                //                <br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";
                //string myEmailBody2 = string.Format(myEmailBody, v姓名, vTicket);
                //string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", v電子郵件, mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "ck@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "chloe@gogojp.com.tw", mailip);
                ////myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "sale35@gogojp.com.tw@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "joan@gogojp.com.tw@gogojp.com.tw", mailip);

                #endregion
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", v電子郵件, mailip);
                //ck@gogojp.com.tw
                //chloe@gogojp.com.tw
                ////myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "sale35@gogojp.com.tw", mailip);
                //joan@gogojp.com.tw", mailip);

                #region 14.票券訂購

                //string myEmailBody = @"<html>
                //                <head>
                //                <title>《大榮旅遊》票券訂單需求通知 客服：02-25815859 </title>
                //                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                //                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                //                </head>
                //                <body>
                //                <table width=586 border=0 align=center cellpadding=0 cellspacing=0>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                <tr>
                //                <td width=66><img src=http://www.dtsgroup.com.tw/imgs/pop_logo.gif width=66 height=60 /></td>
                //                <td background=http://www.dtsgroup.com.tw/imgs/pop_t_bg.gif>
                //                <table width=509 border=0 cellspacing=0 cellpadding=0>
                //                <tr>
                //                <td>
                //                <table border=0 cellspacing=1 cellpadding=0>
                //                <tr>
                //                <td align=left class=wording11px>Complete Notification</td>
                //                </tr>
                //                <tr>
                //                <td><font color=#990066 style=font-weight:bolder>票券訂單需求通知</font></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=5><img src=http://www.dtsgroup.com.tw/imgs/pop_subend.gif width=5 height=60 /></td>
                //                <td width=76><a href=# onclick=window.close()><img src=http://www.dtsgroup.com.tw/imgs/pop_close.gif width=76 height=60 border=0 /></a></td>
                //                </tr>
                //                </table>
                //                </td>
                //                <td width=11><img src=http://www.dtsgroup.com.tw/imgs/pop_rt.gif width=11 height=60 /></td>
                //                </tr>
                //                <tr>
                //                <td valign=top bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_l.gif width=66 height=55 /></td>
                //                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />{0}&nbsp;您好：<br />
                //                大榮旅遊客服中心已收到您的訂購與詢問需求單，我們將會儘速與您聯絡，處理票券相關事宜。本確認函僅通知您所需求之內容，不代表您所訂購與需求已成立，所有細節確認項目將以客服人員所回覆為準，若造成您的不便與困擾敬請見諒。
                //                <br />
                //                您的票券訂單如下：
                //                <br />
                //                {1}
                //                <br />
                //                <br />
                //                如有任何疑問或需修改訂單相關資訊，請來電大榮旅遊台北02-25815859 ，客服人員將竭誠為您服務！
                //                <br />
                //                <br />
                //                大榮旅遊 敬祝 鈞安<br /><br /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                </tr>
                //                <tr>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_lb.gif width=66 height=20 /></td>
                //                <td bgcolor=#FFFFFF>&nbsp;</td>
                //                <td valign=bottom bgcolor=#FFFFFF><img src=http://www.dtsgroup.com.tw/imgs/pop_rb.gif width=11 height=20 /></td>
                //                </tr>
                //                <tr>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                <td><img src=http://www.dtsgroup.com.tw/imgs/spacer.gif width=10 height=6 /></td>
                //                </tr>
                //                </table>
                //                </form>
                //                </body>
                //                </html>";
                //string myEmailBody2 = string.Format(myEmailBody, v姓名, vTicket);
                //string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", v電子郵件, mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "chloe@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "sale35@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "ck@gogojp.com.tw", mailip);
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", "leo@gogojp.com.tw", mailip);

                #endregion
                //myEmailBody2.Email("大榮旅遊 - 訂購需求通知信函", "service@gogojp.com.tw", v電子郵件, mailip);
                //chloe@gogojp.com.tw
                //sale35@gogojp.com.tw
                //ck@gogojp.com.tw
                //leo@gogojp.com.tw

                //特殊類型

                #region 9.婚宴需求

                //string v教堂名稱 = txt教堂名稱_DDL.SelectedValue;
                //string v婚禮日期 = txt婚禮日期.Text;
                //string v預計人數 = txt預計人數.Text;
                //string v聯絡姓名 = txt聯絡姓名.Text;
                //string v聯絡電話1 = txt聯絡電話1.Text;
                //string v聯絡電話2 = txt聯絡電話2.Text;
                //string v聯絡電話 = v聯絡電話1 + v聯絡電話2;
                //string v行動電話 = txt行動電話.Text;
                //string v電子郵件 = txt電子郵件.Text;
                //string v聯絡地址 = txt聯絡地址.Text;
                //string v其他備註 = txt其他備註.Text;
                //string v角色 = txt角色.Text;
                //string v業務端 = "婚禮專題" + "<br>教堂名稱：" + v教堂名稱 + "<br>婚禮日期：" + v婚禮日期 + "<br>預計人數：" + v預計人數 + "<br>角色：" + v角色 + "<br>聯絡姓名：" + v聯絡姓名 + "<br>聯絡電話：" + v聯絡電話 + "<br>行動電話：" + v行動電話 + "<br>電子郵件：" + v電子郵件 + "<br>聯絡地址：" + v聯絡地址 + "<br>其他備註：" + v其他備註 + "<br><br>請盡快回覆";
                //string v客戶端 = "親愛的客戶您好~<br>本公司已經收到您的需求會請業務人員盡快與您聯繫" + "<br><br>婚禮專題-我有興趣" + "<br><br>教堂名稱：" + v教堂名稱 + "<br>婚禮日期：" + v婚禮日期 + "<br>預計人數：" + v預計人數 + "<br>聯絡姓名：" + v聯絡姓名 + "<br>聯絡電話：" + v聯絡電話 + "<br>行動電話：" + v行動電話 + "<br>電子郵件：" + v電子郵件 + "<br>聯絡地址：" + v聯絡地址 + "<br>其他備註：" + v其他備註;
                //v業務端.Email("客戶需求~婚禮專題 大榮旅遊集團", "service@gogojp.com.tw", "Sale35@gogojp.com.tw", "192.168.201.94");
                //// v業務端.Email("客戶需求~婚禮專題 大榮旅遊集團", "service@gogojp.com.tw", "netdep@gogojp.com.tw", "192.168.201.94"); 

                //v客戶端.Email("客戶需求~婚禮專題 大榮旅遊集團", "service@gogojp.com.tw", v電子郵件, "192.168.201.94");

                #endregion
                //Sale35@gogojp.com.tw

                #region 13.高爾夫球

                //string myEmailBody = "出發日期：" + v出發日期 + "<br>預計人數：" + v預計人數 + "<br>聯絡姓名：" + v聯絡姓名 + "<br>聯絡電話：" + v聯絡電話 + "<br>行動電話：" + v行動電話 + "<br>電子郵件：" + v電子郵件 + "<br>聯絡地址：" + v聯絡地址 + "<br>其他備註：" + v其他備註 + "<br>需求球場：" + v球場;
                //string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
                //myEmailBody.Email("高爾夫專題需求單", "service@gogojp.com.tw", "polohuang@gogojp.com.tw", mailip);
                //myEmailBody.Email("高爾夫專題需求單", "service@gogojp.com.tw", "sale35@gogojp.com.tw", mailip);
                //myEmailBody.Email("高爾夫專題需求單", "service@gogojp.com.tw", "6524@gogojp.com.tw", mailip);
                //myEmailBody.Email("高爾夫專題需求單", "service@gogojp.com.tw", "tim00363@gogojp.com.tw", mailip);

                #endregion

                //polohuang@gogojp.com.tw
                //sale35@gogojp.com.tw
                //6524@gogojp.com.tw
                //tim00363@gogojp.com.tw

                //無類型

                #endregion

                #region 處理寄信
                //// From //service@gogojp.com.tw 
                //MailAddress mailAddress = new MailAddress("service@gogojp.com.tw");
                //MailMessage mailMsg = new MailMessage();
                //mailMsg.From = mailAddress;
                //// Subject and Body
                //mailMsg.Subject = DicQuesSubject;

                ////可能需要分離欄位跟內容
                ////mailMsg.Body = DicQuesText;
                //mailMsg.Body = String.Format(mailtemplate, 
                //    DicQuesSubject, contactinfoshort, username,
                //    orderno, itemname, bodytitle,
                //    itembody, contactinfo
                //    );


                //mailMsg.IsBodyHtml = true;
                //mailMsg.Priority = MailPriority.Normal;
                ////Init SmtpClient and send
                //SmtpClient smtpClient = new SmtpClient(MailServerIP);
                //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                //smtpClient.Credentials = credentials;

                //List<MailAddress> mAs = new List<MailAddress>();
                //List<string> s = new List<string>();
                //// To
                ////需要提供其他接收者

                ////!!要從資料庫取得!!!
                ////string[] s = "".Split(',');


                //switch (MailSample) {
                //    case 1:
                //        //自由行 list.aspx
                //        s.Add("chloe@gogojp.com.tw");
                //        s.Add("sale35@gogojp.com.tw");
                //        s.Add("ck@gogojp.com.tw");
                //        s.Add("leo@gogojp.com.tw");
                //        break;
                //    case 2:
                //        break;
                //    case 3:
                //        break;
                //    case 4:
                //        break;
                //    case 5:
                //        break;
                //    case 6:
                //        break;
                //    case 7:
                //        break;
                //    case 8:
                //        break;
                //}



                ////可能會出錯
                //s.Add(DicQuesEmail);

                #endregion

                

                #region 寫入資料庫

                tblDicQues tdq = new tblDicQues()
                {
                    //DicQuesID = 1, //流水號
                    HiLiteID = HiLiteID, //公司代碼 14 16 17 19  <asp:ListItem Selected="True" Value="14">台北總公司</asp:ListItem>
                                         //< asp:ListItem Value = "19" > 台中分公司華揚旅行社 </ asp:ListItem >
                                         //< asp:ListItem Value = "16" > 高雄分公司 </ asp:ListItem >
                    LoginID = LoginID,//登入代碼
                    MemberID = MemberID,//會員編號
                    ProductExtNo = "0",// 產品代碼 未使用
                    DicLangCode = "BIG5",
                    DicQuesCNm = DicQuesCNm,
                    DicQuesEmail = DicQuesEmail,
                    DicQuesTelD = DicQuesTelD,//電話
                    DicQuesTelM = DicQuesTelM,//手機
                    DicQuesCat = DicQuesCat, //DicCatCode	DicCatNm   1   商品預約單 2   聯繫需求單 3   電話諮詢紀錄 4   企業會員需求單 3   獎旅需求單
                    DicQuesArea = "",//暫時沒用到
                    DicQuesPPL = DicQuesPPL,//可能是某種單據號碼
                    DicQuesSubject = DicQuesSubject,//信件主題 limit 254
                    DicQuesText = DicQuesText.HtmlStrip().Trim(), //字元無上限
                    DicQuesCoun = "TW",//固定值
                    DicQuesNm = DicQuesNm, //無使用
                    DicQuesReply = DicQuesReply,//字元 4000內
                    DicQuesNotes = DicQuesNotes.HtmlStrip(),//無使用
                    DicQuesAD = DateTime.Now,
                    DicQuesRD = DateTime.Now,//回復日期
                    DicQuesStatus = 1//DicCatCode	DicCatNm  0   未回覆 1   已完成 2   處理中 3   取消
                };

                if (WriteDB)
                {
                    dm.tblDicQues.Add(tdq);
                    dm.SaveChanges();
                }

                #endregion


                //佔不處理email


                //try
                //{


                //    foreach (string ss in s)
                //    {

                //        //mailMsg.To.Add(ss);
                //        mailMsg.To.Clear();
                //        mailMsg.To.Add(ss);
                //        smtpClient.Send(mailMsg);


                //    }
                //}
                //catch (Exception ex)
                //{
                //    hasError = true;
                //}
            }
            return hasError;
        }

        

        /// <summary>
        /// Removes all html tags from string and leaves only plain text
        /// Removes content of <xml></xml> and <style></style> tags as aim to get text content not markup /meta data.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HtmlStrip(this string input)
        {
            input = Regex.Replace(input, "<style>(.|\n)*?</style>",string.Empty);
            input = Regex.Replace(input, @"<xml>(.|\n)*?</xml>", string.Empty); // remove all <xml></xml> tags and anything inbetween.  
            return Regex.Replace(input, @"<(.|\n)*?>", string.Empty); // remove any tags but not there content "<p>bob<span> johnson</span></p>" becomes "bob johnson"
        }
        /// <summary>
        /// </summary>
        public static string GetUserIP()
        {
            string ipList = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
    
            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }
    
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        /// 把色碼轉換成系統的顏色物件
        /// </summary>
        /// <param name="hex">色碼格式 "#FFFFFF" </param>
        /// <returns></returns>
        public static Color HexColor(String hex)
        {
            //將井字號移除
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;
            int start = 0;

            //處理ARGB字串 
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            // 將RGB文字轉成byte
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }
    }
}