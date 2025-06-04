using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class contactform_control_contactform2 : System.Web.UI.UserControl
{
    private string vCheck = "0";
    private string vReturn = "0";

    /// <summary>
    /// 只發送信件給測試人員 預設關閉
    /// </summary>
    private bool mailtotest = false;

    /// <summary>
    /// 發送簡訊通知 預設開啟
    /// </summary>
    private bool smsenable = true;

    /// <summary>
    /// 信件標題
    /// </summary>
    [Browsable(true)]
    public string MailTitle { get; set; }

    /// <summary>
    /// 直接傳入 送出按鈕的顏色 範例#FFFFFF
    /// </summary>
    [Browsable(true)]
    public Color ButtonColor { get; set; }

    /// <summary>
    /// 滑鼠移到按鈕時顯示的顏色 目前還沒想到怎麼加進去...
    /// </summary>
    public Color ButtonColorMouseOn { get; set; }


    /// <summary>
    /// 直接設定CSS路徑 預設為/contactform/bootstrap.css
    /// </summary>
    [Browsable(true)]
    public string cssstyle1
    {
        get
        {
            return css1.Attributes["href"];
        }
        set
        {
            css1.Attributes["href"] = value;
        }

    }

    /// <summary>
    /// 直接設定CSS路徑 預設為/contactform/contactform.css
    /// </summary>
    [Browsable(true)]
    public string cssstyle2
    {
        get
        {
            return css2.Attributes["href"];
        }
        set
        {
            css2.Attributes["href"] = value;
        }

    }

    /// <summary>
    /// 重新導向的標題 主題 目前沒在用
    /// </summary>
    [Browsable(true)]
    public string utm_campaign
    {
        get;
        set;
    }

    /// <summary>
    /// 重新導向的標題 功能 目前沒在用
    /// </summary>
    [Browsable(true)]
    public string utm_medium
    {
        get;
        set;
    }

    [Browsable(true)]
    public bool SmsEnable
    {

        get
        {
            return smsenable;
        }

        set
        {
            smsenable = value;
        }
    }

    [Browsable(true)]
    public bool MailToTest
    {
        get
        {
            return mailtotest;
        }

        set
        {
            mailtotest = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (ButtonColor != null)
        {
            //BtnPostMsg.BackColor = ButtonColor;
            //BtnPostMsg.

        }

        HyperLink hl = (HyperLink)this.FindControl("HLMail");
        if (hl != null)
        {
            hl.NavigateUrl = "mailto:service@gogojp.com.tw?subject=" + MailTitle;
        }

        
    }

    protected void BtnPostMsg_Click(object sender, EventArgs e)
    {
        string name = Txtname.Text;
        string email = Txtemail.Text;
        string tel = Txttel.Text;
        string msg = Txtmsg.Text;

        //判斷資訊是否空白
        if (name.Length == 0) { TxtError.Visible = true; TxtError.Text = "＊請輸入姓名！"; vCheck = "0"; return; } else { vCheck = "1"; }
        if (tel.Length == 0) { TxtError.Visible = true; TxtError.Text = "＊請輸入聯絡電話！"; vCheck = "0"; return; } else { vCheck = "1"; }
        //if(主題.Length == 0) { TxtError.Visible = true; TxtError.Text = "＊請輸入主題！"; vCheck = "0";return; } else { vCheck = "1"; }
        if (msg.Length == 0) { TxtError.Visible = true; TxtError.Text = "＊請輸入留言！"; vCheck = "0"; return; } else { vCheck = "1"; }
        // 暫時不用if (信箱 == "") { TxtError.Visible = true; TxtError.Text = "＊請輸入信箱！"; vCheck = "0";return; } else { vCheck = "1"; }

        if (vCheck == "1")
        {
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
                                            <td align=left valign=top bgcolor=#FFFFFF class=wording13px><br />
                                            您好：<br />
                                            大榮旅遊客服中心已收到您的詢問需求單，我們將會儘速與您聯絡，處理旅遊相關事宜<br />
                                            本確認函僅通知您所需求之內容，不代表您所需求已成立，所有細節確認項目將以客服<br />
                                            人員所回覆為準。若造成您的不便與困擾敬請見諒。<br />
                                            姓名： " + name + @"<br>
                                            信箱： " + email + @"<br>
                                            電話： " + tel + @"<br>
                                            主題： " + MailTitle + @"<br>
                                            留言： " + msg + @"<br>
                                            <br />
                                            如有任何疑問，請來電大榮旅遊台北網路部 02-25223219<br />
                                            大榮旅遊將竭誠為您服務！
                                            <br />
                                            <br />
                                            大榮旅遊 敬祝 鈞安<br />
                                            <br /></td>
                                        </tr>
                                        <tr>
                                            <td><img src=http://www.dtsgroup.com.tw/imgs/footer.jpg /></td>
                                        </tr>
                                    </table>
                                </body>
                                </html>";
            string mailip = WebConfigurationManager.AppSettings["MailServerIP"];
            if (!mailtotest)
            {
                vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", email, mailip);//消費者
                vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "charly@gogojp.com.tw", mailip);//鉅晏
                vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "wuchia@gogojp.com.tw", mailip);//嘉嘉
                vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "service@gogojp.com.tw", mailip);//正式機   
                vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "ck@gogojp.com.tw", mailip);//卡伯
            }
            else
            {
                vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", email, mailip);//消費者
                vEmailMsg.Email("大榮旅遊 - 需求通知信函", "service@gogojp.com.tw", "charly@gogojp.com.tw", mailip);//鉅晏
            }
            Email2DBTTemp(vEmailMsg, 14, 0, 0, name, email, tel, "", "2", 0, MailTitle, MailTitle, "",  @"
                                            姓名： " + name + @"<br>
                                            信箱： " + email + @"<br>
                                            電話： " + tel + @"<br>
                                            主題： " + MailTitle + " " + @"<br>
                                            留言： " + msg + @"<br>", true, 0);
            if (smsenable)
            {

                //寄發簡訊給CK
                //clsSMS.SendSMS("DTS前台", MailTitle + "需求通知信函");
            }
            //Response.Redirect("/event/wintersnow_2015/AD_message.aspx?utm_source=Yahoo&utm_medium=雪祭旅客需求單&utm_campaign=雪祭活動");
            Response.Redirect("/contactform/AD_message.aspx");
        }
        else
        {
            TxtError.Visible = true;
            TxtError.Text = "＊請輸入以上資料 ~ ";
            return;
        }
    }

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
    private void Email2DBTTemp(string DicQuesText,
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
            string bodytitle = "您的" + itemtitle + "訂單如下：";
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

            //switch (MailSample)
            //{
            //    //取得密碼
            //    case 1:
            //        break;
            //    //需求單
            //    case 2:
            //        break;
            //    //訂購單
            //    case 3:
            //        break;
            //    //特殊訂單
            //    case 4:
            //        break;
            //}

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
                DicQuesText = DicQuesText.Trim(), //字元無上限
                DicQuesCoun = "TW",//固定值
                DicQuesNm = DicQuesNm, //無使用
                DicQuesReply = DicQuesReply,//字元 4000內
                DicQuesNotes = DicQuesNotes,//無使用
                DicQuesAD = DateTime.Now,
                DicQuesRD = DateTime.Now,//回復日期
                //DicQuesStatus = 1//DicCatCode	DicCatNm  0 未回覆 1 已完成 2 處理中 3 取消
                DicQuesStatus = 0//DicCatCode	DicCatNm  0 未回覆 1 已完成 2 處理中 3 取消
            };

            if (WriteDB)
            {
                dm.tblDicQues.Add(tdq);
                dm.SaveChanges();
            }

            #endregion
        }

    }
}