using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using ExtensionMethods;

/// <summary>
/// clsTim 的摘要描述
/// </summary>
public class clsTim
{
	
		//
		// TODO: 回傳939是否有商品
		//
    public static int Return939(string pAreaCode, string philte)
    {
        string vSql = @"SELECT  ProductSchSD, ProductID, ProductSchID, ProductNmPre, ProductNm, ProductNmSuf, ProductSchPA, ProductOptCode, SUBSTRING(DicAreaNm, 0, CHARINDEX('(', DicAreaNm + '(')) AS DicAreaNm FROM v_Repeater2 WHERE 1 = 1 AND (ProductOptCode = '939') ";
        if (pAreaCode != "") { vSql += "AND (DicAreaCode IN (" + pAreaCode + ")) "; }
        if (philte != "") { vSql += "AND (HiLiteID = @HiLiteID) "; }
        SqlConnection vConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
        SqlCommand vCmd = new SqlCommand(vSql, vConn);
        vCmd.CommandType = CommandType.Text;
        vCmd.Parameters.AddWithValue("@HiLiteID", philte);
        using (vConn)
        {
            vConn.Open();
            SqlDataReader rdr = vCmd.ExecuteReader();
            if (rdr.Read())
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    //
    // TODO: 回傳是否有產品
    //
	public static int ReturnProductID(string psql)
    {                
        SqlConnection vConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
        SqlCommand vCmd = new SqlCommand(psql, vConn);
        vCmd.CommandType = CommandType.Text;       
        using (vConn)
        {
            vConn.Open();
            SqlDataReader rdr = vCmd.ExecuteReader();
            if (rdr.Read())
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    //
    // TODO: 回傳中文星期
    //
    public static string ReturnHtlSchWK(string[] pHSW)
    {
        
        string vHtlSchWK = "";
        int a = pHSW.Length;        
        for (int i = 0; i < a; i++)
        {            
            string HtlSchWK = pHSW[i];
            switch (HtlSchWK)
            {
                case "1" :
                    HtlSchWK = "一";
                    break;
                case "2":
                    HtlSchWK = "二";
                    break;
                case "3":
                    HtlSchWK = "三";
                    break;
                case "4":
                    HtlSchWK = "四";
                    break;
                case "5":
                    HtlSchWK = "五";
                    break;
                case "6":
                    HtlSchWK = "六";
                    break;
                case "7":
                    HtlSchWK = "日";
                    break;
            }
            vHtlSchWK += HtlSchWK;
        }
        
        
        return vHtlSchWK;
    }
}