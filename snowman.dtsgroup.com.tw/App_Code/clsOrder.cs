using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using ExtensionMethods;
using System.Collections.Generic;

/// <summary>
/// clsOrder 負責處理所有有關訂單的相關功能
/// </summary>
public class clsOrder
{
    /// <summary>
    /// 產生新的訂單編號EX: G201212310001
    /// </summary>
    public static string GetOrderID()
    {
        return GenerateOrderID();
    }
    /// <summary>
    /// 產生新的訂單編號EX: G201212310001
    /// </summary>
    public static string GenerateOrderID()
    {
        //string[] vArryM = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        //string[] vArryD = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
        string vOrderID = "";
        string F = "G";
        DateTime pDate = DateTime.Now;
        string vYear = pDate.Year.ToString();
        //string vYear = dt.Year.ToString();
        //int vMonth = dt.Month;
        //string vYear = pDate.Year.ToString();
        //int vMonth = pDate.Month;     

        SqlConnection vConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("SELECT TOP 1 *  FROM tblOrderTmp WHERE OrderTmpID like '" + F + pDate.Year + pDate.Month.ToString("00") + "%' order by OrderTmpID desc", vConn);
        cmd.CommandType = CommandType.Text;
        using (vConn)
        {
            vConn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                string vOrderTmpID = (string)rdr["OrderTmpID"];
                string vOrderTmpID1 = vOrderTmpID.Right(5);
                int vOrderTmpNum = vOrderTmpID1.ToInt();
                vOrderTmpNum++;
                vOrderTmpID1 = vOrderTmpNum.ForceLength(5);
                vOrderID = "G" + vYear + pDate.Month + vOrderTmpID1;
            }
            else
            {
                vOrderID = "G" + vYear + pDate.Month + "00001";
            }
        }

        SqlConnection vConn1 = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
        SqlCommand vCmd1 = new SqlCommand("INSERT INTO tblOrderTmp (OrderTmpID) VALUES (@NewOrderID)", vConn1);
        vCmd1.CommandType = CommandType.Text;
        vCmd1.Parameters.AddWithValue("@NewOrderID", vOrderID);
        using (vConn1)
        {
            vConn1.Open();
            try
            {
                vCmd1.ExecuteNonQuery();
                return vOrderID;
            }
            catch
            {
                return "";
            }
        }
    }

    /// <summary>
    /// 新版產生訂單編號
    /// </summary>
    /// <param name="pDate">出發日期</param>
    /// <param name="HiLiteID">公司代碼</param>
    /// <returns></returns>
    public static string GenerateOrderID(DateTime pDate, int HiLiteID)
    {
        if (pDate.Year < 2018)
        {
            return GenerateOrderID();
        }
        else
        {
            string vOrderID = "";
            DateTime dt = DateTime.Now;
            string vYear = pDate.Year.ToString();
            Dictionary<int, string> hilitecode = new Dictionary<int, string>();
            hilitecode.Add(14, "F");
            hilitecode.Add(16, "K");
            hilitecode.Add(17, "X");
            hilitecode.Add(21, "J");

            string F = hilitecode[HiLiteID];
            SqlConnection vConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT count (OrderTmpID)  FROM tblOrderTmp WHERE OrderTmpID like '" + F + pDate.Year + pDate.Month.ToString("00") + "%'", vConn);
            cmd.CommandType = CommandType.Text;
            int count = 0;
            using (vConn)
            {
                vConn.Open();

                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    count = rdr.GetInt32(0);
                    vOrderID = F + vYear + pDate.Month.ToString("00") + (count + 1).ToString("00000");
                }
                rdr.Close();
                bool dup = false;
                do
                {
                    cmd.CommandText = "SELECT Top 1 * From tblOrderTmp Where OrderTmpID = @OrderTmpID";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@OrderTmpID", vOrderID);
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        count++;
                        vOrderID = F + vYear + pDate.Month.ToString("00") + (count + 1).ToString("00000");
                        rdr.Close();
                        dup = true;
                        continue;
                    }
                    else
                    {
                        rdr.Close();
                        dup = false;
                    }
                    //rdr.Close();

                    cmd.CommandText = "SELECT Top 1 * From tblOrder Where OrderID = @OrderID";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@OrderID", vOrderID);
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        rdr.Close();
                        cmd.CommandText = "INSERT INTO tblOrderTmp (OrderTmpID) VALUES (@NewOrderID)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@NewOrderID", vOrderID);
                        cmd.ExecuteNonQuery();
                        rdr.Close();
                        count++;
                        vOrderID = F + vYear + pDate.Month.ToString("00") + (count + 1).ToString("00000");
                        dup = true;
                        continue;
                    }
                    else
                    {
                        rdr.Close();
                        dup = false;
                    }
                    //rdr.Close();


                    if (!dup)
                    {
                        cmd.CommandText = "SELECT Top 1 * FROM tblSalesRec WHERE SalesRecCode = @SalesRecCode";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@SalesRecCode", vOrderID);
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            count++;
                            vOrderID = F + vYear + pDate.Month.ToString("00") + (count + 1).ToString("00000");
                            rdr.Close();
                            dup = true;
                            continue;
                        }
                        else
                        {
                            rdr.Close();
                            dup = false;
                        }
                        //rdr.Close();
                    }
                } while (dup);

                cmd.CommandText = "INSERT INTO tblOrderTmp (OrderTmpID) VALUES (@NewOrderID)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@NewOrderID", vOrderID);
                cmd.ExecuteNonQuery();
                return vOrderID;
            }
        }
    }
    /// <summary>
    /// 產生F單號 2018年出發的團體 回傳訂單編號
    /// </summary>
    /// <param name="pDate">出發日期</param>
    /// <param name="pOrderID">訂單編號</param>
    /// <returns></returns>
    public static string GetSalesRecCode(DateTime pDate, string pOrderID)
    {
        if (pDate.Year > 2017)
        {
            return pOrderID;
        }
        else
        {
            string[] vArryM = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            string[] vArryD = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
            string vYear = pDate.Year.ToString();
            int vMonth = pDate.Month;
            string vTmpNumb = "F" + vYear + vArryM[vMonth].ToString();
            string vHiLiteID = "14";//clsSession.Current.sHiLiteID.ToString();
            if (vHiLiteID == "0") { vHiLiteID = "14"; }

            SqlConnection vConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT SalesRecCode FROM tblSalesRec WHERE SalesRecCode LIKE '" + vTmpNumb + "%' ORDER BY SalesRecCode DESC", vConn);

            cmd.CommandType = CommandType.Text;
            using (vConn)
            {
                vConn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    string vCUST_NUMB = (string)rdr["SalesRecCode"];
                    string vCUST_NUMB1 = vCUST_NUMB.Right(5);
                    int vCUST_NUMBNum = vCUST_NUMB1.ToInt();
                    vCUST_NUMBNum++;
                    vCUST_NUMB1 = vCUST_NUMBNum.ForceLength(5);
                    vTmpNumb = "F" + vYear + vArryM[vMonth] + vCUST_NUMB1;
                }
                else
                {
                    vTmpNumb = "F" + vYear + vArryM[vMonth] + "00001";
                }
            }
            return vTmpNumb;
        }
    }
    /// <summary>
    /// 新增繳款資料
    /// </summary>
    /// <param name="pOrderID">訂單編號</param>
    /// <param name="pOrderPaidNote">備註</param>
    /// <param name="pOrderPaidType">付款類型</param>
    /// <param name="pOrderPaidPrice">付款金額</param>
    /// <param name="pOrderPaidQty">數量</param>
    /// <param name="pOrderPaidCCode">支票代碼</param>
    /// <param name="pOrderPaidCPeriod">支票到期日</param>
    /// <param name="pOrderPaidCurr">幣別</param>
    /// <param name="pOrderPaidRate">匯率</param>
    /// <returns>狀態碼 1:成功 2:失敗</returns>
    public static string fncInsertOrderPaid(
        string pOrderID,
        string pOrderPaidNote,
        string pOrderPaidType,
        Int32 pOrderPaidPrice,
        Int32 pOrderPaidQty,
        string pOrderPaidCCode,
        string pOrderPaidCPeriod,
        string pOrderPaidCurr,
        string pOrderPaidRate
        )
    {
        return fncInsertOrderPaid(pOrderID, pOrderID,
        pOrderPaidNote,
        pOrderPaidType,
        pOrderPaidPrice,
        pOrderPaidQty,
        pOrderPaidCCode,
        pOrderPaidCPeriod,
        pOrderPaidCurr,
        pOrderPaidRate);


    }

    /// <summary>
    /// 新增繳款資料
    /// </summary>
    /// <param name="pOrderID">訂單編號</param>
    /// <param name="pOrderIDs">繳款編號</param>
    /// <param name="pOrderPaidNote">備註</param>
    /// <param name="pOrderPaidType">付款類型</param>
    /// <param name="pOrderPaidPrice">付款金額</param>
    /// <param name="pOrderPaidQty">數量</param>
    /// <param name="pOrderPaidCCode">支票代碼</param>
    /// <param name="pOrderPaidCPeriod">支票到期日</param>
    /// <param name="pOrderPaidCurr">幣別</param>
    /// <param name="pOrderPaidRate">匯率</param>
    /// <returns>狀態碼 1:成功 2:失敗</returns>
    public static string fncInsertOrderPaid(
        string pOrderID,
        string pOrderIDs,
        string pOrderPaidNote,
        string pOrderPaidType,
        Int32 pOrderPaidPrice,
        Int32 pOrderPaidQty,
        string pOrderPaidCCode,
        string pOrderPaidCPeriod,
        string pOrderPaidCurr,
        string pOrderPaidRate
        )
    {
        if (pOrderPaidCPeriod == "") { pOrderPaidCPeriod = "2000-01-01"; }

        double vOrderPaidRate = Convert.ToDouble(pOrderPaidRate);//匯率
        int vOrderPaidAmount = Convert.ToInt32(Math.Round((pOrderPaidPrice * pOrderPaidQty * vOrderPaidRate), 0, MidpointRounding.AwayFromZero));//台幣總價
        int vOrderPaidPrice = Convert.ToInt32(Math.Round((pOrderPaidPrice * vOrderPaidRate), 0, MidpointRounding.AwayFromZero));//台幣單價

        string vSql = @"SELECT * FROM tblSalesRecL WHERE OrderID = @OrderID;";
        SqlConnection vConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
        SqlCommand vCmd = new SqlCommand(vSql, vConn);
        vCmd.CommandType = CommandType.Text;
        vCmd.Parameters.AddWithValue("@OrderID", pOrderID);
        using (vConn)
        {
            vConn.Open();
            SqlDataReader rdr = vCmd.ExecuteReader();
            if (rdr.Read())
            {
                //如果有找到資料代表示鎖定的狀態
                return "2";//訂單鎖定
            }
            else
            {
                //System.Web.HttpContext.Current.Response.Write(pOrderCashCPrice);
                //System.Web.HttpContext.Current.Response.End();
                string vSql3 = @"INSERT INTO tblOrderPaid (
                                    OrderIDs,
                                    OrderID,
                                    OrderPaidNote,
                                    OrderPaidStatus,
                                    OrderPaidCat,
                                    OrderPaidType,
                                    OrderPaidDate,
                                    OrderPaidPrice,
                                    OrderPaidQty,
                                    OrderPaidAmount,
                                    OrderPaidCCode,
                                    OrderPaidCPeriod,
                                    OrderPaidTransDate,
                                    OrderPaidTransTime,
                                    OrderPaidCurr,
                                    OrderPaidRate,
                                    OrderPaidCPrice
                                )VALUES(
                                    @OrderIDs,
                                    @OrderID,
                                    @OrderPaidNote,
                                    '審核中',
                                    '2',
                                    @OrderPaidType,
                                    GETDATE(), 
                                    @OrderPaidPrice,
                                    @OrderPaidQty,
                                    @OrderPaidAmount,
                                    @OrderPaidCCode,
                                    @OrderPaidCPeriod,
                                    CONVERT(VARCHAR(10), GETDATE(), 20), 
                                    '000000', 
                                    @OrderPaidCurr, 
                                    @OrderPaidRate,
                                    @OrderPaidCPrice 
                                )";
                SqlConnection vConn3 = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
                SqlCommand vCmd3 = new SqlCommand(vSql3, vConn3);
                vCmd3.CommandType = CommandType.Text;
                vCmd3.Parameters.AddWithValue("@OrderIDs", pOrderIDs);
                vCmd3.Parameters.AddWithValue("@OrderID", pOrderID);
                vCmd3.Parameters.AddWithValue("@OrderPaidNote", pOrderPaidNote);
                vCmd3.Parameters.AddWithValue("@OrderPaidType", pOrderPaidType);
                vCmd3.Parameters.AddWithValue("@OrderPaidPrice", vOrderPaidPrice);
                vCmd3.Parameters.AddWithValue("@OrderPaidQty", pOrderPaidQty);
                vCmd3.Parameters.AddWithValue("@OrderPaidAmount", vOrderPaidAmount);
                vCmd3.Parameters.AddWithValue("@OrderPaidCCode", pOrderPaidCCode);
                vCmd3.Parameters.AddWithValue("@OrderPaidCPeriod", pOrderPaidCPeriod);
                vCmd3.Parameters.AddWithValue("@OrderPaidCurr", pOrderPaidCurr);
                vCmd3.Parameters.AddWithValue("@OrderPaidRate", pOrderPaidRate);
                vCmd3.Parameters.AddWithValue("@OrderPaidCPrice", pOrderPaidPrice);
                using (vConn3)
                {
                    vConn3.Open();
                    try
                    {
                        vCmd3.ExecuteScalar();
                        //clsMsg.Record("tblOrder", "OrderID", pOrderID, "新增繳款資料", "訂單管理 clsOrder.fncInsertOrderPaid");

                        return "1";//更新成功
                    }
                    catch (Exception ex)
                    {
                        return "0";//更新失敗
                        //System.Web.HttpContext.Current.Response.Write(ex);
                        //System.Web.HttpContext.Current.Response.End();
                        throw ex;
                    }
                }
            }
        }

    }
}