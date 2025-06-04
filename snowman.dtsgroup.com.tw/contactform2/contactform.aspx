<%@ Page Language="C#" AutoEventWireup="true" CodeFile="contactform.aspx.cs" Inherits="_contactform_index" %>

<html>
  <head>
    <title></title>
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- css -->
    <link href="css/bootstrap.css" rel="stylesheet" media="screen">
    <link href="css/contactform.css" rel="stylesheet" media="screen">
  </head>
  <body>
     <!-- Contact -->
      <section id="contact" class="home-section bg-white">
          <div class="container">
              <div class="row">
                  <div class="col-md-offset-2 col-md-8">
                    <div class="section-heading">
                     <h2>聯絡我們</h2>
                    </div>
                  </div>
              </div>
              <div class="row">
                  <div class="col-md-offset-1 col-md-10">
                <form id="form1" runat="server" CssClass="form-horizontal">
                  <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left">
                    <label for="name">姓名</label>
                      <asp:TextBox ID="Txt姓名" runat="server" CssClass="form-control" Placeholder="Name"></asp:TextBox>
                    </div>
                  </div>
                  <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left">
                    <label for="inputEmail">信箱</label>
                      <asp:TextBox ID="Txt信箱" runat="server" CssClass="form-control" Placeholder="Email"></asp:TextBox>
                    </div>
                  </div>
                  <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left">
                    <label for="inputTel">電話</label>
                      <asp:TextBox ID="Txt電話" runat="server" CssClass="form-control" Placeholder="Phone-Number"></asp:TextBox>
                    </div>
                  </div>
                  <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left">
                    <label for="inputSubject" hidden="hidden">主題</label>
                      <asp:TextBox ID="Txt主題" runat="server" CssClass="form-control" Visible="False"></asp:TextBox>
                    </div>
                  </div>
                  <div class="form-group">
                    <div class="col-md-offset-2 col-md-8 text-left">
                    <label for="inputMessage">留言</label>
                      <asp:TextBox ID="Txt留言" runat="server" Rows="3"  TextMode="MultiLine" CssClass="form-control" placeholder="Messsage"></asp:TextBox>
                   <br />
                   </div>
                  <div class="form-group">
                    <div class="col-md-offset-2 col-md-8">
                     <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="送出訊息" CssClass="btn btn-theme btn-lg btn-block" />
                    </div>
                  </div>
                  <asp:Label ID="TxtError" runat="server" Visible="True" Width="95%"></asp:Label>
                </form>
                  </div>
</html>