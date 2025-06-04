<%@ Control Language="C#" AutoEventWireup="true" CodeFile="contactformClubMed.ascx.cs" Inherits="control_contactformClubMed" %>

<link id="css1" href="/contactform/css/bootstrapc.css" rel="stylesheet" media="screen" runat="server" />
<link id="css2" href="/contactform/css/contactformc.css" rel="stylesheet" media="screen" runat="server" />
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.15.0/jquery.validate.min.js"></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.15.0/additional-methods.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.15.0/localization/messages_zh.min.js"></script>
<script type="text/javascript" src='https://www.google.com/recaptcha/api.js'></script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.blockUI/2.70/jquery.blockUI.min.js"></script>
<style>
    input, input:focus, input:active {
        user-select: text;
    }

    input, button, select, textarea {
        outline: none;
    }

    *:focus {
        outline: none;
    }

    .error {
        color: red;
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
        $('.btn').click(function(){fbq('track', 'Lead');});
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
<section id="contact" class="home-section bg-white" runat="server">
    <div id="FormContainer" class="container" runat="server">
        <div class="row">
            <div class="col-md-offset-2 col-md-8">
                <div class="section-heading">
                    <h2>專人為我服務</h2>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-offset-1 col-md-10">
                <asp:PlaceHolder ID="PHProduct" runat="server" Visible="false">
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-8 text-left">
                            <label for="name">行程名稱　</label>
                            <asp:Label ID="LabProductNm" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="PHProductSch" runat="server" Visible="false">
                    <asp:PlaceHolder ID="PHFullMSG" runat="server" Visible="false">
                     <div class="form-group">
                        <div class="col-md-offset-2 col-md-8 text-left">
                            <label for="name">
                                您所報名的團體，目前暫時報名額滿。請於下方表單中留下您的聯繫資料，我們業務人員會主動聯繫您，為您爭取報名出團的機會。
                            </label>
                        </div>
                    </div>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-8 text-left">
                            <label for="name">團 號　</label>
                            <asp:Label ID="LabProductTourID" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-8 text-left">
                            <label for="name">出發日期　</label>
                            <asp:Label ID="LabProductSchSD" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left" style="margin-bottom: 10px;">
                        ★<label for="name">姓 名</label>
                        <asp:TextBox ID="Txtname" runat="server" CssClass="form-control required" Placeholder="姓名"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left" style="margin-bottom: 10px;">
                        <label for="inputEmail">信 箱</label>
                        <asp:TextBox ID="Txtemail" runat="server" CssClass="form-control email" Placeholder="Email"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left" style="margin-bottom: 10px;">
                        ★<label for="inputTel">電 話</label>
                        <asp:TextBox ID="Txttel" runat="server" CssClass="form-control required" Placeholder="手機 或 區碼加市話"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left" style="margin-bottom: 10px;">
                        <label for="inputTel">地 區</label>
                        <asp:DropDownList ID="DDLHiLite" runat="server">
                            <asp:ListItem Text="台北" Value="14" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left" style="margin-bottom: 10px;">
                        <label for="DDLHotel">飯 店</label>
                        <asp:DropDownList ID="DDLHotel" runat="server" Visible="true">
                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                            <asp:ListItem Text="sahoro 北海道" Value="sahoro 北海道"></asp:ListItem>
                            <asp:ListItem Text="Tomamu 北海道" Value="Tomamu 北海道"></asp:ListItem>
                            <asp:ListItem Text="北海道 kiroro peak 行館" Value="北海道 kiroro peak 行館"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left">
                        <label for="inputSubject" hidden="hidden">主 題</label>
                        <asp:TextBox ID="Txttitle" runat="server" CssClass="form-control" Visible="False"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left">
                        <label for="inputMessage">留 言</label>
                        <asp:TextBox ID="Txtmsg" runat="server" Rows="3" TextMode="MultiLine" CssClass="form-control" placeholder="留言"></asp:TextBox>
                        <br />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-8">
                        <div class="g-recaptcha" data-sitekey="6LefpgkUAAAAAASjQJC30TNMIzALHORXNK5Q5BX6"></div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-2 col-md-8" style="margin-top: 15px;">
                        <asp:Button ID="BtnPostMsg" runat="server" Text="送出訊息" CssClass="btn btn-theme btn-lg btn-block" OnClick="BtnPostMsg_Click" OnClientClick="if( !checkinput()){return false;}" />
                    </div>
                </div>
                <asp:Label ID="TxtError" runat="server" Visible="True" Width="95%"></asp:Label>
                <br />
            </div>
        </div>
    </div>
</section>
