using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtensionMethods;
using System.Web.Configuration;

public partial class _contactform_index : System.Web.UI.Page
{
    private string vCheck = "0";
    private string vReturn = "0";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        string vSubject = Request.QueryString["qsubject"] ?? "";
        Txt主題.Attributes.Add("placeholder", vSubject);
        Txt主題.Text = vSubject;
    }
   
    protected void Button1_Click(object sender, EventArgs e)
    {
        //string 主題 = Txt主題.Text;
        string 主題 = "大榮旅遊線上活動::冬季線上旅展";
        string 姓名 = Txt姓名.Text;
        string 信箱 = Txt信箱.Text;
        string 電話 = Txt電話.Text;      
        string 留言 = Txt留言.Text;
       
        //判斷資訊是否空白
        if (姓名 == "") { TxtError.Visible = true; TxtError.Text = "＊請輸入姓名！"; vCheck = "0";return; } else { vCheck = "1"; }
        if (電話== "") { TxtError.Visible = true; TxtError.Text = "＊請輸入聯絡電話！"; vCheck = "0";return; } else { vCheck = "1"; }
        //if(主題 == "") { TxtError.Visible = true; TxtError.Text = "＊請輸入主題！"; vCheck = "0";return; } else { vCheck = "1"; }
        if (留言== "") { TxtError.Visible = true; TxtError.Text = "＊請輸入留言！"; vCheck = "0";return; } else { vCheck = "1"; }
        // 暫時不用if (信箱 == "") { TxtError.Visible = true; TxtError.Text = "＊請輸入信箱！"; vCheck = "0";return; } else { vCheck = "1"; }

        if (vCheck == "1")
        {
            //組合需求訊息
            string vDicQuesText = "您好：<br /> 大榮旅遊客服中心已收到您的詢問需求單，我們將會儘速與您聯絡，處理旅遊相關事宜<br />本確認函僅通知您所需求之內容，不代表您所需求已成立，所有細節確認項目將以客服<br />人員所回覆為準。若造成您的不便與困擾敬請見諒。<br /> ";
            vDicQuesText += "姓名： " + 姓名 + "<br>";
            vDicQuesText += "信箱： " + 信箱 + "<br>";
            vDicQuesText += "電話： " + 電話 + "<br>";
            vDicQuesText += "主題： " + 主題 + "<br>";
            vDicQuesText += "留言： " + 留言 + "<br>";
            //組合需求單Email
            string vEmailMsg = @"<html>
                                <head>
                                <title>《大榮旅遊》訂單需求通知</title>
                                <meta http-equiv=Content-Type content='text/html; charset=utf-8' >
                                <link href='http://www.dtsgroup.com.tw/dtsoverall.css' rel=stylesheet type='text/css' >
                                </head>
                                <body>
                                <table width=586 border=0 cellpadding=0 cellspacing=0>
                                <tr>
                                <td><img src=http://www.dtsgroup.com.tw/imgs/header.jpg  /></td>
                                </tr>
                                <tr>
                                <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />";
            vEmailMsg += vDicQuesText + "<br />";
            vEmailMsg += "如有任何疑問，請來電大榮旅遊台北網路部 02-25223219";
            vEmailMsg += @"<br />大榮旅遊將竭誠為您服務！
                                <br />
                                <br />
                                大榮旅遊 敬祝 鈞安<br /><br /></td>
                                </tr>
                                <tr>
                                <td><img src=http://www.dtsgroup.com.tw/imgs/footer.jpg /></td>
                                </tr>
                                </table>
                                </form>
                                </body>
                                </html>";

            string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
            vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", 信箱, mailip);//消費者
            vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "charly@gogojp.com.tw", mailip);//鉅晏
            vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "wuchia@gogojp.com.tw", mailip);//嘉嘉
            vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "service@gogojp.com.tw", mailip);//正式機   
            vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "ck@gogojp.com.tw", mailip);//卡伯
            //vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "samchien@gogojp.com.tw", mailip);//sam
            //clsSMS.SendSMS("DTS前台", "冬季線上旅展-需求通知信函");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowStatus", "javascript:alert('已收到您的需求！將盡快與您聯絡！')", true);
        }
        else
        {
            TxtError.Visible = true; 
            TxtError.Text = "＊請輸入以上資料 ~ ";
            return;
        }
    }
}