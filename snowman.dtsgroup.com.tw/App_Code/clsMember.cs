using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using ExtensionMethods;

/// <summary>
/// clsMember 負責處理所有跟會員有關的函數
/// </summary>
public class clsMember
{
    /// <summary>
    /// 透過Cookie取回會員登入資料；
    /// </summary>
    /// <returns>回傳會員編號 MemberID；錯誤的話回傳0；</returns>
    public static int RetrieveMember()
    {
        string vCookieValue = clsCookie.GetCookie("cMemberData");
        string[] aCookieValue = vCookieValue.Split(',');
        string vMemberID = aCookieValue[0].ToString();
        if ((vMemberID == "0") || (vMemberID == ""))
        {
            return 0;
        }
        else
        {
            SqlConnection vConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
            SqlCommand vCmd = new SqlCommand("SELECT MemberID, MemberCFNm, MemberCLNm, MemberEmail, MemberPWD, MemberTelD, MemberTelM FROM tblMember WHERE MemberID = @MemberID AND MemberType = 4 AND MemberTelM <> '' AND MemberPWD <> '' GROUP BY MemberID, MemberCFNm, MemberCLNm, MemberEmail, MemberPWD, MemberTelD, MemberTelM HAVING COUNT(MemberTelM)<= 10 AND MemberID <> '229658' ", vConn);
            vCmd.CommandType = CommandType.Text;
            vCmd.Parameters.AddWithValue("@MemberID", vMemberID);
            using (vConn)
            {
                vConn.Open();
                SqlDataReader rdr = vCmd.ExecuteReader();
                if (rdr.Read())
                {
                    Int32 rdrMemberID = (Int32)rdr["MemberID"];
                    string rdrMemberPWD = (string)rdr["MemberPWD"];
                    string rdrMemberCFNm = (string)rdr["MemberCFNm"];
                    string rdrMemberCLNm = (string)rdr["MemberCLNm"];
                    string rdrMemberEmail = (string)rdr["MemberEmail"];
                    string rdrMemberTelD = (string)rdr["MemberTelD"];
                    string rdrMemberTelM = (string)rdr["MemberTelM"];

                    clsSession.Current.sMemberID = rdrMemberID.ToString();
                    clsSession.Current.sMemberNm = rdrMemberCFNm + rdrMemberCLNm;
                    clsSession.Current.sMemberTelD = rdrMemberTelD;
                    clsSession.Current.sMemberTelM = rdrMemberTelM;
                    clsSession.Current.sMemberEmail = rdrMemberEmail;

                    clsCookie.SetCookie("cMemberData", rdrMemberID.ToString() + "," + rdrMemberCFNm + rdrMemberCLNm + "," + rdrMemberTelD + "," + rdrMemberTelM + "," + rdrMemberEmail + ",", 1);

                    return rdrMemberID;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
    /// <summary>
    /// 會員登入；
    /// </summary>
    /// <param name="pMemberCFNm">電話</param>
    /// <param name="pMemberCLNm">密碼</param>
    /// <returns>回傳會員編號 MemberID；錯誤的話回傳0；設定Cookie[MemberID], Cookie[MemberCFNm+MemberCLNm], Cookie[MemberTelM]</returns>
    public static int LoginMember(string pMemberPID, string pMemberTelM, string pMemberPWD)
    {
        string vsql = "SELECT MemberID, MemberCFNm, MemberCLNm, MemberEmail, MemberPWD, MemberTelD, MemberTelM, MemberValidate FROM tblMember WHERE  1 = 1 ";
        if (pMemberPID != "") { vsql += " AND MemberPID = @MemberPID "; }
        if (pMemberTelM != "") { vsql += " AND MemberTelM = @MemberTelM "; }
        vsql += " AND MemberPWD = @MemberPWD  AND MemberPWD <> '' AND MemberValidate = 1 ";
        SqlConnection vConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
        SqlCommand vCmd = new SqlCommand(vsql, vConn);
        vCmd.CommandType = CommandType.Text;
        if (pMemberPID != "") { vCmd.Parameters.AddWithValue("@MemberPID", pMemberPID); }
        if (pMemberTelM != "") { vCmd.Parameters.AddWithValue("@MemberTelM", pMemberTelM); }
        vCmd.Parameters.AddWithValue("@MemberPWD", pMemberPWD);
        using (vConn)
        {
            vConn.Open();
            SqlDataReader rdr = vCmd.ExecuteReader();
            if (rdr.Read())
            {
                Int32 rdrMemberID = (Int32)rdr["MemberID"];
                string rdrMemberPWD = (string)rdr["MemberPWD"];
                string rdrMemberCFNm = (string)rdr["MemberCFNm"];
                string rdrMemberCLNm = (string)rdr["MemberCLNm"];
                string rdrMemberEmail = (string)rdr["MemberEmail"];
                string rdrMemberTelD = (string)rdr["MemberTelD"];
                string rdrMemberTelM = (string)rdr["MemberTelM"];
                bool rdrMemberValidate = (bool)rdr["MemberValidate"];

                clsSession.Current.sMemberID = rdrMemberID.ToString();
                clsSession.Current.sMemberNm = rdrMemberCFNm + rdrMemberCLNm;
                clsSession.Current.sMemberTelD = rdrMemberTelD;
                clsSession.Current.sMemberTelM = rdrMemberTelM;
                clsSession.Current.sMemberEmail = rdrMemberEmail;
                clsCookie.SetCookie("cMemberData", rdrMemberID.ToString() + "," + rdrMemberCFNm + rdrMemberCLNm + "," + rdrMemberTelD + "," + rdrMemberTelM + "," + rdrMemberEmail + ",", 1);
                return rdrMemberID;
            }
            else
            {
                return 0;
            }
        }
    }
    /// <summary>
    /// 新增同業會員；
    /// 新增前會檢查MemberTelM這個欄位中是否有重複的資料
    /// 若有重複的資料，則直接回傳會員的編號MemberID
    /// 沒有重複的話，就直接進入新增的程序
    /// </summary>
    /// <param name="pMemberCFNm">中文姓</param>
    /// <param name="pMemberCLNm">中文名</param>
    /// <param name="pMemberFNm">英文姓</param>
    /// <param name="pMemberLNm">英文名</param>
    /// <param name="pMemberSex">性別：M 男, F 女</param>
    /// <param name="pMemberPID">身分證字號</param>
    /// <param name="pMemberEmail">電子郵件</param>
    /// <param name="pMemberTelD">日間聯繫電話</param>
    /// <param name="pMemberTelM">手機號碼</param>
    /// <param name="pMemberTelN">夜間聯繫電話</param>
    /// <param name="pMemberTelF">傳真號碼</param>
    /// <param name="pMemberAddr">地址</param>
    /// <param name="pMemberZip">郵遞區號</param>
    /// <returns>無錯誤的話回傳會員編號 MemberID；錯誤的話回傳0</returns>
    public static int AddMember(string pLoginID, string pMemberCFNm,
                        string pMemberCLNm,
                        string pMemberFNm,
                        string pMemberLNm,
                        string pMemberSex,
                        string pMemberPID,
                        string pMemberEmail,
                        string pMemberTelD,
                        string pMemberTelM,
                        string pMemberTelN,
                        string pMemberTelF,
                        string pMemberAddr,
                        string pMemberZip
                        )
    {
        
        SqlConnection vConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
        SqlCommand vCmd = new SqlCommand("SELECT TOP 1 MemberID FROM tblMember WHERE MemberTelM = @MemberTelM AND MemberID <> 0 AND MemberTelM <> '' GROUP BY MemberID HAVING COUNT(MemberTelM) <= 10", vConn);
        vCmd.CommandType = CommandType.Text;
        vCmd.Parameters.AddWithValue("@MemberTelM", pMemberTelM);
        using (vConn)
        {
            vConn.Open();
            SqlDataReader rdr = vCmd.ExecuteReader();
            if (rdr.Read())
            {
                Int32 vMemberID = (Int32)rdr["MemberID"];
                return vMemberID;
            }
            else
            {
                if(pMemberTelM.Length != 10)
                {
                    return 0;
                }
                else
                {
                    string vSql = @"INSERT INTO tblMember (
                                LoginID,
                                MemberCFNm, 
                                MemberCLNm, 
                                MemberFNm, 
                                MemberLNm, 
                                MemberSex, 
                                MemberCoun, 
                                MemberPID, 
                                MemberEmail, 
                                MemberTelD, 
                                MemberTelM, 
                                MemberTelN, 
                                MemberTelF, 
                                MemberAddr, 
                                MemberZip
                                )  
                                OUTPUT INSERTED.MemberID VALUES
                                (
                                @LoginID,
                                @MemberCFNm,
                                @MemberCLNm,
                                @MemberFNm, 
                                @MemberLNm, 
                                @MemberSex, 
                                'TW', 
                                @MemberPID, 
                                @MemberEmail,  
                                @MemberTelD,  
                                @MemberTelM,  
                                @MemberTelN,  
                                @MemberTelF,  
                                @MemberAddr,  
                                @MemberZip
                                )";
                    SqlConnection vConn1 = new SqlConnection(WebConfigurationManager.ConnectionStrings["TravelDBConnectionString"].ConnectionString);
                    SqlCommand vCmd1 = new SqlCommand(vSql, vConn1);
                    vCmd1.CommandType = CommandType.Text;
                    vCmd1.Parameters.AddWithValue("@LoginID", pLoginID);
                    vCmd1.Parameters.AddWithValue("@MemberCFNm", pMemberCFNm);
                    vCmd1.Parameters.AddWithValue("@MemberCLNm", pMemberCLNm);
                    vCmd1.Parameters.AddWithValue("@MemberFNm", pMemberFNm);
                    vCmd1.Parameters.AddWithValue("@MemberLNm", pMemberLNm);
                    vCmd1.Parameters.AddWithValue("@MemberSex", pMemberSex);
                    vCmd1.Parameters.AddWithValue("@MemberPID", pMemberPID);
                    vCmd1.Parameters.AddWithValue("@MemberEmail", pMemberEmail);
                    vCmd1.Parameters.AddWithValue("@MemberTelD", pMemberTelD);
                    vCmd1.Parameters.AddWithValue("@MemberTelM", pMemberTelM);
                    vCmd1.Parameters.AddWithValue("@MemberTelN", pMemberTelN);
                    vCmd1.Parameters.AddWithValue("@MemberTelF", pMemberTelF);
                    vCmd1.Parameters.AddWithValue("@MemberAddr", pMemberAddr);
                    vCmd1.Parameters.AddWithValue("@MemberZip", pMemberZip);
                    using (vConn1)
                    {
                        vConn1.Open();
                        try
                        {
                            
                            int vMemberID = 0;
                            vMemberID = Convert.ToInt32(vCmd1.ExecuteScalar());
                            return vMemberID;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }
    }
}