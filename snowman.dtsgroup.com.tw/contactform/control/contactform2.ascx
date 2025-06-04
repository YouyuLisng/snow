<%@ Control Language="C#" AutoEventWireup="true" CodeFile="contactform2.ascx.cs" Inherits="contactform_control_contactform2" %>
<link id="css1" href="" rel="stylesheet" media="screen" runat="server" />
<link id="css2" href="" rel="stylesheet" media="screen" runat="server" />

<div class="block-2-container section-container contact-container liaison02-container">
            <div class="container">
	            <div class="row">
	                <div class="col-sm-12 block-2 section-description wow fadeIn">
	                	<h2>聯絡我們</h2>
	                	<div class="divider-1 wow fadeInUp"><span></span></div>
	                </div>
	            </div>
	            <div class="row">
	            	<div class="col-sm-4 block-2-box block-2-left contact-form wow fadeInLeft">
	                    
                            <div class="form-group">
                            </div>
                            <div class="form-group">                            
                             <label class="sr-only" for="contact-name">姓名</label>                 
	                        	
                                <asp:TextBox ID="Txtname" runat="server" CssClass="contact-name form-control required" Placeholder="姓名"></asp:TextBox>
	                        </div>
	                    	<div class="form-group">                            
                             <label class="sr-only" for="contact-email">Email信箱</label>                 
	                        	<asp:TextBox ID="Txtemail" runat="server" CssClass="contact-email form-control email" Placeholder="Email信箱"></asp:TextBox>
	                        </div>
                            <div class="form-group">
                              <label class="sr-only" for="contact-tel">聯絡方式</label>
                                <asp:TextBox ID="Txttel" runat="server" CssClass="contact-tel form-control ROC_Celphone required" Placeholder="電話格式 0912-3456789或02-12345678"></asp:TextBox>
                            </div>
                            <div class="form-group">
                               
                            </div>
	                        <div class="form-group">
	                        	<label class="sr-only" for="contact-subject">Subject</label>
	                        	<asp:TextBox ID="Txttitle" runat="server" CssClass="contact-subject form-control" placeholder="主題" ></asp:TextBox>
	                        </div>
                            <div class="form-group">
                               
                            </div>
	                        <div class="form-group">
	                        	<label class="sr-only" for="contact-message">留言</label>
	                        	<asp:TextBox ID="Txtmsg" runat="server" Rows="3"  TextMode="MultiLine" CssClass="contact-message form-control required" placeholder="留言"></asp:TextBox>
	                        </div>
                        <button id="BtnPostMsg" Class="btn" onclick="if(!$('#form1').validate().form()){return false;}"  runat="server" onserverclick="BtnPostMsg_Click" >
                        送出
	                    </button>
                        <br/>
                        <asp:Label ID="TxtError" runat="server" Visible="True" Width="95%"></asp:Label>
	                    
	            	</div>
	            	<div class="col-sm-4 block-2-box block-2-right contact-address wow fadeInUp">
	            		<h3>連絡資訊</h3>
	                    <p><span aria-hidden="true" class="icon_pin"></span>週一~五 09:00-20:00</p>
	                    <p><span aria-hidden="true" class="icon_phone"></span>Phone: 02-25223219</p>
	                    <p><span aria-hidden="true" class="icon_mail"></span>Email: <asp:HyperLink ID="HLMail" runat="server" NavigateUrl="mailto:service@gogojp.com.tw?subject=關於藏王樹冰">service@gogojp.com.tw</asp:HyperLink></p>
	            	</div>
	            </div><a href="">
	            <div class="contact-icon-container">
            		<span aria-hidden="true" class="icon_mail"></span>
            	</div>
	        </a></div>
        </div>

<script type="text/javascript">    
    function importJS(src, look_for, onload) {
        var s = document.createElement('script');
        s.setAttribute('type', 'text/javascript');
        s.setAttribute('src', src);
        if (onload) wait_for_script_load(look_for, onload);
        if (eval("typeof " + look_for) == 'undefined') {
            var head = document.getElementsByTagName('head')[0];
            if (head) head.appendChild(s);
            else document.body.appendChild(s);
        }
    }
    function wait_for_script_load(look_for, callback) {
        var interval = setInterval(function () {
            if (eval("typeof " + look_for) != 'undefined') {
                clearInterval(interval);
                callback();
            }
        }, 50);
    }
    
    var jQueryScriptOutputted = false;
    function initJQuery() {

        //if the jQuery object isn't available
        if (typeof (jQuery) == 'undefined') {


            if (!jQueryScriptOutputted) {
                //jQueryScriptOutputted 這個變數的目的就只是紀錄著JQuery是不是已經被呼叫過了
                jQueryScriptOutputted = true;

                //output the script (load it from google api)
                document.write("<scr" + "ipt type=\"text/javascript\" src=\"http://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js\"></scr" + "ipt>");
            }
            setTimeout("initJQuery()", 50);
        } else {

            //JQuery相關的程式碼就寫在這邊,當然也可以另外寫在一個function 在這邊呼叫他
            $(document).ready(function () {
                jQuery.validator.addMethod("ROC_Celphone",
                       function (cellphone, element) {
                           cellphone = cellphone.replace(/\s+/g, "");
                           return (
                         this.optional(element) || /[0][1-9]{3}\-[0-9]{6}/.test(cellphone) || /[0-9]{2}\-[0-9]{7}/.test(cellphone)
                     );
                       }, "手機 0900-000000 或 區碼-00000000");
                var validator = $("#form1").validate();
                //$("#form1").validate();
            });

        }

    }
    initJQuery();
</script>

