<%@ Page Language="C#" AutoEventWireup="true"   ViewStateMode="Disabled" %>
<%@ Register Src="~/contactform/control/contactform.ascx" TagPrefix="uc1" TagName="contactform" %>
<!DOCTYPE html>
<html>
<head>
    <!-- Google Tag Manager 分析代碼 下面一行 -->
    <script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src='https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);})(window,document,'script','dataLayer','GTM-NSRSZH9');</script>
    <title>與我聯絡《大榮旅遊》</title>
    <meta name="keywords" content="大榮旅遊,日本,北海道,東京,關西,北陸,東北,九州,琉球,沖繩,自由行" />
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!--手機宣告-->
    <meta name="format-detection" content="telephone=no">
    <!--禁止電話-->
    <meta name="description" content="大榮旅遊!挑戰市場最低價!!團體旅遊、團體自由行、個人自由行、航空公司精緻旅遊、商務機票出遊把握搶低價！" />
    <meta name="copyright" content="大榮旅遊集團版權所有 c2015 DTS Group all rights reserved." />
    <!--facebook star https://developers.facebook.com/tools/debug/-->
    <meta property="og:title" content="與我聯絡《大榮旅遊》"/>
    <!--活動頁標題-->
    <meta property="og:url" content="" />
    <!--網址-->
    <meta property="og:image" content="/event/!templated/images/community/ogImage.jpg" />
    <!--縮圖網址 1200 x 630 小於5MB-->
    <meta property="og:image:width" content="600" />
    <!--縮圖網址 1200 x 630 小於5MB-->
    <meta property="og:image:height" content="315" />
    <!--縮圖網址 1200 x 630 小於5MB-->
    <meta property="og:description" content="大榮旅遊***********!挑戰市場最低價!!團體旅遊、團體自由行、個人自由行、航空公司精緻旅遊、商務機票出遊把握搶低價！" />
    <!--網頁敘述-->
    <meta property="og:site_name" content="大榮旅遊 DTS Group" />
    <!--網站名稱-->
    <meta property="og:locale" content="zh_tw" />
    <!--facebook end-->
    <!--桌面icon star-->
    <meta name="apple-mobile-web-app-title" content="大榮旅遊::與我聯絡" />
    <!--apple-touch-icon title-->
    <link rel="apple-touch-icon" sizes="120x120" href="/event/!templated/images/community/apple-touch-icon.png">
    <!--apple-touch-icon.png-->
    <link rel="apple-touch-icon" sizes="180x180" href="/event/!templated/images/community/touch-icon-iphone6-plus.png">
    <!--apple-touch-icon.png-->
    <!--桌面icon end-->
    <!--CSS 語法-->
    <link rel="stylesheet" href="/event/!templated/css/style.css" />
    <link rel="stylesheet" href="/event/!templated/css/bootstrap.css" />
    <link rel="stylesheet" href="/event/!templated/css/reset.css" />
    <link rel="stylesheet" href="/event/!templated/css/lrtk.css"/><!--gotop-->
    <link rel="stylesheet" href="/event/!templated/css/animate.css"><!--動態-->
    <style>
        body { margin: 0; padding: 0; font-family: "微軟正黑體", sans-serif;}
        .section-heading h2{font-size:35px;font-weight:bold;text-align:center;line-height:80px;}/*聯絡我們*/
        .form-control:focus {border-color: red;} /*聯絡我們*/    
        .btn-theme{color: #ffffff;}/*聯絡我們*/  
        .btn-theme:hover{background: #92bb17;color: #ffffff;}/*聯絡我們*/      
    </style>
    <!--[if lte IE 9]><link rel="stylesheet" href="css/ie/v9.css" /><![endif]-->
    <!--[if lte IE 8]><link rel="stylesheet" href="css/ie/v8.css" /><![endif]-->
    <!--jQuery 函式庫-->
    <!--[if lte IE 8]><script src="css/ie/html5shiv.js"></script><![endif]-->
</head>
<body>
    <!-- Google Tag Manager (noscript) 分析代碼 下面一行-->
    <noscript><iframe src="https://www.googletagmanager.com/ns.html?id=GTM-NSRSZH9" height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
    <!--logo-->
    <div id="header"></div>
    <form id="form1" runat="server">
        <header id="top"></header><!--ID勿刪-->
        <!--連絡表單-->
        <a id="communication"></a>
        <!--電話-->
        <div class="TEL">
        <h1>懶得填資料嗎?試試我!</h1>
        <div class="telbtn" onclick="location.href='tel:(02)-25223219'">手機快速說</div>
        </div>
        <!--表單-->
        <div style="text-align:center;">
            <script type="text/javascript" src="//code.jquery.com/jquery-1.12.3.min.js"></script>
            <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.3/jquery.easing.min.js"></script>
            <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/blueimp-JavaScript-Templates/3.4.0/js/tmpl.min.js"></script>
            <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery.lazyload/1.9.1/jquery.lazyload.min.js"></script>
            <uc1:contactform runat="server" id="contactform" ButtonColor="#84a90e" BackgroundColor="#f5f5f5"  MailTitle="大榮旅遊::行程頁" cssstyle1=""  cssstyle2=""  />
             <%--1.頁面最上面第二行需要加入使用控制項的路徑
                 2.MailTitle 是設定 主題名稱的
                 3.如果需要個別設定聯絡我們的CSS 可以把CSS路徑指向  cssstyle1=路徑1 及 cssstyle2=路徑2 最多可以放兩個CSS檔案 
             --%>
        </div>
<!--連絡表單-->
</form>
<div id="copyright"></div>
<!--footer-->
     <script type="text/javascript" >
         $(document).ready(function () {
             $('#header').load('/dhtml/dw.html');
             $('#copyright').load('/dhtml/copyright.html');
         });
    </script>
<!--footer-->
</body>
</html>

<!--
上線日期:2015/11/2
下線日期:
製作:wichiachia
-->







