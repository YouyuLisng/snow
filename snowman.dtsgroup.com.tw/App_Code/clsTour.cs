using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Text;
using ExtensionMethods;

/// <summary>
/// clsTour 主要是更新所有團體的資訊
/// </summary>
public class clsTour
{
    ///<summary>
    /// 計算報名人數與出團日期；
    /// </summary>
    /// <param name="pProductSchID">團體系統編號</param>
    /// <param name="pProductID">產品編號</param>
    /// <returns>無錯誤的話回傳1</returns>
    public static string fncCountTourOrder(int pProductID)
    {
        string vDateStr = "";
        string tmpMonth = "";
        string vSql3 = @"SELECT 
						tblProduct.ProductType, 
						tblProductSch.ProductSchID, 
						tblProductSch.ProductID, 
						tblProductSch.ProductSchSD, 
						tblProductSch.ProductTourID, 
						tblProductSch.ProductSchQA, 
						tblProductSch.ProductSchQB, 
						tblProductSch.ProductSchQC, 
						tblProductSch.ProductSchQD, 
						tblProductSch.ProductSchQG, 
						tblProductSch.ProductSchStatus,
						tblProductSch.ProductSchRG 
						FROM tblProductSch 
						INNER JOIN tblProduct ON tblProduct.ProductID = tblProductSch.ProductID 
						WHERE (1=1) 
						AND tblProduct.ProductType IN (1, 2, 6) 
						AND tblProductSch.ProductID = @ProductID 
						AND tblProductSch.ProductTourID <> '' 
						AND tblProductSch.ProductSchSD > DATEADD (dd  , -7 , GETDATE()  )
						AND tblProductSch.ProductSchStatus NOT IN (0,3)  
						ORDER BY tblProductSch.ProductSchSD";
        SqlConnection vConn3 = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
        SqlCommand vCmd3 = new SqlCommand(vSql3, vConn3);
        vCmd3.CommandType = CommandType.Text;
        vCmd3.Parameters.AddWithValue("@ProductID", pProductID);
        using (vConn3)
        {
            vConn3.Open();
            SqlDataReader rdr = vCmd3.ExecuteReader();
            while (rdr.Read())
            {
                Int32 rdrProductType = rdr.IsDBNull(0) ? 0 : (Int32)rdr["ProductType"];
                Int32 rdrProductSchID = rdr.IsDBNull(1) ? 0 : (Int32)rdr["ProductSchID"];
                Int32 rdrProductID = rdr.IsDBNull(2) ? 0 : (Int32)rdr["ProductID"];
                DateTime rdrProductSchSD = rdr.IsDBNull(3) ? DateTime.Now : (DateTime)rdr["ProductSchSD"];
                string rdrProductTourID = rdr.IsDBNull(4) ? "" : (string)rdr["ProductTourID"];
                Int32 rdrProductSchQA = rdr.IsDBNull(5) ? 0 : (Int32)rdr["ProductSchQA"];
                Int32 rdrProductSchQB = rdr.IsDBNull(6) ? 0 : (Int32)rdr["ProductSchQB"];
                Int32 rdrProductSchQC = rdr.IsDBNull(7) ? 0 : (Int32)rdr["ProductSchQC"];
                Int32 rdrProductSchQD = rdr.IsDBNull(8) ? 0 : (Int32)rdr["ProductSchQD"];
                Int32 rdrProductSchQG = rdr.IsDBNull(9) ? 0 : (Int32)rdr["ProductSchQG"];
                string rdrProductSchStatus = rdr.IsDBNull(10) ? "" : (string)rdr["ProductSchStatus"];
                string rdrProductSchRG = rdr.IsDBNull(11) ? "" : (string)rdr["ProductSchRG"];
                /*更新tblReportTProfit資料表
                string vHttpStatusCode = "";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://admin.dtsgroup.com.tw/dtour/Mng00.aspx?qschid=" + rdrProductSchID);
                webRequest.AllowAutoRedirect = false;
                try
                {
                    HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                    vHttpStatusCode = (string)response.StatusCode.ToString();
                }
                catch (WebException we)
                {
                    vHttpStatusCode = ((HttpWebResponse)we.Response).StatusCode.ToString();
                }
                更新tblReportTProfit資料表*/
                /*更新團體的報名人數與消息人數*******************************************************************/
                string vSql4 = "SELECT (SELECT COUNT(dbo.tblOrderCus.OrderCusID) AS OrderCusCount FROM dbo.tblOrderCus WITH (NOLOCK) INNER JOIN dbo.tblOrder WITH (NOLOCK) ON dbo.tblOrderCus.OrderID = dbo.tblOrder.OrderID INNER JOIN dbo.tblOrderPro ON dbo.tblOrder.OrderID = dbo.tblOrderPro.OrderID WHERE (dbo.tblOrder.OrderStatus = 'ST01') AND (dbo.tblOrderCus.OrderCusTicket NOT IN ('6')) AND (dbo.tblOrderPro.ProductSchID = @ProductSchID)) AS ProductSchQD, (SELECT COUNT(tblOrderCus_1.OrderCusID) AS OrderCusCount FROM dbo.tblOrderCus AS tblOrderCus_1 WITH (NOLOCK) INNER JOIN dbo.tblOrder AS tblOrder_1 WITH (NOLOCK) ON tblOrderCus_1.OrderID = tblOrder_1.OrderID INNER JOIN dbo.tblOrderPro AS tblOrderPro_1 ON tblOrder_1.OrderID = tblOrderPro_1.OrderID WHERE (tblOrder_1.OrderStatus IN ('ST02', 'ST03')) AND (tblOrderCus_1.OrderCusTicket = 1) AND (tblOrderPro_1.ProductSchID = @ProductSchID) AND (tblOrderCus_1.OrderCusType IN (0, 1, 2, 4, 6))) AS ProductSchQB";
                SqlConnection vConn4 = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
                SqlCommand vCmd4 = new SqlCommand(vSql4, vConn4);
                vCmd4.CommandType = CommandType.Text;
                vCmd4.Parameters.AddWithValue("@ProductSchID", rdrProductSchID);
                using (vConn4)
                {
                    vConn4.Open();
                    SqlDataReader rdr4 = vCmd4.ExecuteReader();
                    if (rdr4.Read())
                    {
                        Int32 rdr4ProductSchQD = rdr4.IsDBNull(0) ? 0 : (Int32)rdr4["ProductSchQD"];
                        rdrProductSchQB = rdr4.IsDBNull(1) ? 0 : (Int32)rdr4["ProductSchQB"];
                        if (rdrProductType == 2)
                        {
                            rdrProductSchQD = rdr4ProductSchQD + rdrProductSchQG;
                        }
                        else
                        {
                            rdrProductSchQD = rdrProductSchQG;
                            //這邊是聯營商品的資料，聯營商品不透過系統更新 消息人數
                        }
                        SqlConnection vConn1 = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
                        SqlCommand vCmd1 = new SqlCommand("UPDATE tblProductSch SET ProductSchQB = @ProductSchQB, ProductSchQD = @ProductSchQD WHERE ProductSchID = @ProductSchID ", vConn1);
                        vCmd1.Parameters.AddWithValue("@ProductSchQB", rdrProductSchQB);
                        vCmd1.Parameters.AddWithValue("@ProductSchQD", rdrProductSchQD);
                        vCmd1.Parameters.AddWithValue("@ProductSchID", rdrProductSchID);
                        vCmd1.CommandType = CommandType.Text;
                        using (vConn1)
                        {
                            vConn1.Open();
                            try
                            {
                                vCmd1.ExecuteScalar();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
                /*更新團體的報名人數與消息人數*******************************************************************/
                /*組合出發日期的字串*****************************************************************************/
                if (rdrProductSchSD >= DateTime.Now)
                {
                    string vMonth = rdrProductSchSD.ToString("MM");
                    string vDay = rdrProductSchSD.ToString("dd");
                    if (vMonth == tmpMonth)
                    {
                    }
                    else
                    {
                        vDateStr += "<div style=\"background-image:url(/imgs/dotline_01.gif);\"><img src=/imgs/dotline_01.gif  border=0 /></div>";
                        tmpMonth = vMonth;
                    }
                    if ((rdrProductSchQC <= 0) && (rdrProductSchStatus == "1"))
                    {
                        vDateStr += "<span class=series_status_close> <strike>" + vMonth + "/" + vDay + "</strike></span>,";
                    }
                    else if ((rdrProductSchQC <= rdrProductSchQD) && (rdrProductSchStatus == "1"))
                    {
                        vDateStr += "<a href=\"/tour.asp?qpid=" + rdrProductID.ToString() + "&qschid=" + rdrProductSchID.ToString() + "\" class=\"series_status\"> " + vMonth + "/" + vDay + "</a>,";
                    }
                    else if (rdrProductSchStatus == "5")
                    {
                        vDateStr += "<span class=series_status_close> <strike>" + vMonth + "/" + vDay + "</strike></span>,";
                    }
                    else if (rdrProductSchStatus == "2")
                    {
                        vDateStr += "<span class=series_status_close> <strike>" + vMonth + "/" + vDay + "</strike></span>,";
                    }
                    else
                    {
                        vDateStr += "<a href=\"/tour.asp?qpid=" + rdrProductID.ToString() + "&qschid=" + rdrProductSchID.ToString() + "\" class=\"series_status\"> " + vMonth + "/" + vDay + "</a>,";
                    }
                }
                /*組合出發日期的字串*****************************************************************************/
            }
        }

        if (vDateStr != "")
        {
            SqlConnection vConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
            SqlCommand vCmd = new SqlCommand("UPDATE tblProductDetail SET ProductDetail11 = @ProductDetail11 WHERE ProductID = @ProductID ", vConn);
            vCmd.Parameters.AddWithValue("@ProductID", pProductID);
            vCmd.Parameters.AddWithValue("@ProductDetail11", vDateStr);
            vCmd.CommandType = CommandType.Text;
            using (vConn)
            {
                vConn.Open();
                try
                {
                    vCmd.ExecuteScalar();
                    return "1";
                }
                catch (Exception ex)
                {
                    return "0";
                    //throw ex;
                }
            }
        }
        else
        {
            return "2";
        }
    }
}