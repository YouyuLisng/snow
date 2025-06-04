<%@ Control Language="C#" AutoEventWireup="true" CodeFile="contactform3.ascx.cs" Inherits="control_contactform3" ClientIDMode="Static" %>

<%--<link id="css1" href="/contactform/css/bootstrapc.css" rel="stylesheet" media="screen" runat="server" />
<link id="css2" href="/contactform/css/contactformc.css" rel="stylesheet" media="screen" runat="server" />--%>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.15.0/jquery.validate.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.15.0/additional-methods.min.js"></script>
<script type="text/javascript" src="https://ajax.aspnetcdn.com/ajax/jquery.validate/1.15.0/localization/messages_zh_tw.js"></script>
<script type="text/javascript" src='https://www.google.com/recaptcha/api.js'></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.blockUI/2.70/jquery.blockUI.min.js"></script>
<style>
    /*input, input:focus, input:active {
        user-select: text;
    }

    input, button, select, textarea {
        outline: none;
    }*/

    *:focus {
        outline: none;
    }

    .error {
        color: red;
        font-size: 20px;
    }
    
</style>
<script type="text/javascript">
    $(document).ready(function () {
        //須與form表單ID名稱相同
        // 聯繫電話(手機/電話皆可)驗證 
        jQuery.validator.addMethod("ROC_Celphone",
              function (cellphone, element) {
                  cellphone = cellphone.replace(/\s+/g, "");
                  return (
                      this.optional(element) || /[0][1-9]{3}\-[0-9]{6}/.test(cellphone) || /[0-9]{2}\-[0-9]{7}/.test(cellphone)
                  );
              }, "請輸入XXXX-XXXXXX or 請輸入XX-XXXXXXXX");
        $("#form1").validate({
            rules: {
                <%= this.DDLHiLite.UniqueID %>: {required: true}
            },
            messages:{
                <%= this.DDLHiLite.UniqueID %>: { required: "請選擇服務人員所在地區"}
            }
        });
        $('.btn_send').click(function(){fbq('track', 'Lead');});
    });

    function checkinput() {
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            }
        });
        if (grecaptcha.getResponse() == "") {
            $.unblockUI();
            alert("請通過'我不是機器人'驗證!!!");
          
            return false;
        } else {
            //alert("Thank you");
            if ($("#form1").valid()) {
                return true;
            } else {
                //錯誤訊息
                $.unblockUI();
                alert("尚有資料未正確填寫!!!");
                return false;
            }
        }
    }
</script>
<div id="Form" class="sec" runat="server">
    <div class="contnet">
        <div class="title_page">
            <%--<img src="images/title_2.png" alt="">--%>
            <asp:Image ID="ContactformTitleImg" runat="server" />
            <asp:Literal ID="Lierror" runat="server"></asp:Literal>
        </div>
        
        
        <div class="form_content">
            <asp:PlaceHolder ID="PHProduct" runat="server" Visible="false">行程名稱<asp:Label ID="LabProductNm" runat="server" Text=""></asp:Label><br />
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="PHProductSch" runat="server" Visible="false">團　　號<asp:Label ID="LabProductTourID" runat="server" Text=""></asp:Label><br />
                出發日期<asp:Label ID="LabProductSchSD" runat="server" Text=""></asp:Label><br />
            </asp:PlaceHolder>
            <asp:TextBox ID="Txtname" runat="server" CssClass="form-control required" Placeholder="請輸入您的姓名" onfocus="this.placeholder=''" onblur="this.placeholder='請輸入您的姓名'"></asp:TextBox>
            <asp:TextBox ID="Txtemail" runat="server" CssClass="form-control email" placeholder="請輸入您的電子信箱 / Email" onfocus="this.placeholder=''" onblur="this.placeholder='請輸入您的電子信箱 / Email'"></asp:TextBox>
            <asp:TextBox ID="Txttel" runat="server" CssClass="form-control required" placeholder="聯絡電話 / 例：0912 345 678 / 市話請加區碼" onfocus="this.placeholder=''" onblur="this.placeholder='聯絡電話 / 例：0912 345 678 / 市話請加區碼'"></asp:TextBox>
            <asp:DropDownList ID="DDLHiLite" runat="server">
                <asp:ListItem Text="請選擇所在地區" Value=""></asp:ListItem>
                <asp:ListItem Text="台北" Value="14"></asp:ListItem>
                <asp:ListItem Text="台中" Value="17"></asp:ListItem>
                <asp:ListItem Text="高雄" Value="16"></asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="Txtmsg" runat="server" Rows="8" Columns="80" TextMode="MultiLine" CssClass="form-control" placeholder="留 言" onfocus="this.placeholder=''" onblur="this.placeholder='留 言'"></asp:TextBox>
            <div class="g-recaptcha" data-sitekey="6LefpgkUAAAAAASjQJC30TNMIzALHORXNK5Q5BX6"></div>
            <%--<div id="my-widget"></div>--%>
            <%--<div class="btn_send">確認送出</div>--%>
            <div class="btn_send">
                <asp:Button ID="BtnPostMsg" runat="server" Text="確認送出" OnClick="BtnPostMsg_Click" OnClientClick="if( !checkinput()){return false;}" Width="100%" Style="line-height: 40px;" />
            </div>
            <asp:Label ID="TxtError" runat="server" Visible="True" Width="95%"></asp:Label>
        </div>
    </div>
</div>

