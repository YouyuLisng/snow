using EntitytoJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

public class ProductAirController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public string Get(int id)
    {
        return "value";
    }

    // GET api/<controller>/5
    public object Get(string ItemType, string id)
    {
        var serializer = new JavaScriptSerializer
        {
            MaxJsonLength = 1024000000,
            RecursionLimit = 100
        };

        serializer.RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter() });
        using (trdatagogojpEntities fdm = new trdatagogojpEntities())
        using (DataModel dm = new DataModel())
        {
            //TRGRUP trg = fdm.TRGRUP.Where(a => a.GRUP_CD == ItemType).FirstOrDefault();
            ////TRSUBD trg = fdm.trsubd
            //if (trg != null)
            //{//ps.ProductSchAirGo
            //機場 239 325 桃園國際機場
            var dairport = dm.tblDicAirPort.Where(a => a.DicAirPortEnable && !a.DicAirPortDel).Select(a => new { a.DicAirPortCode, a.DicAirPortNm }).ToList();
            Dictionary<string, string> dap = new Dictionary<string, string>();
            foreach (var aa in dairport)
            {
                //context.Response.Write(aa.DicCountryCode + aa.DicCityCode + "<br />");
                if (!dap.Keys.Contains(aa.DicAirPortCode))
                {
                    dap.Add(aa.DicAirPortCode, aa.DicAirPortNm);
                }

            }
            //航空公司 QF 重複

            var dairline = dm.tblDicAirline.Where(a => a.DicAirlineEnable && !a.DicAirlineDel).Select(a => new { a.DicAirlineCode, a.DicAirlineNm });
            Dictionary<string, string> dal = new Dictionary<string, string>();
            foreach (var aa in dairline)
            {
                if (!dal.Keys.Contains(aa.DicAirlineCode))
                {
                    dal.Add(aa.DicAirlineCode, aa.DicAirlineNm);
                }

            }
            var trgb = fdm.TRGBOOK.Where(a => a.GRUP_CD == ItemType && !a.EWDISAB_FG)
                .OrderBy(a => a.GBOOK_DY).ThenBy(a => a.GBOOK_SQ).ThenBy(a => a.EGBOOK_SQ).ToList();
            if (trgb.Count() > 0)
            {
                var go = trgb.FirstOrDefault();
                var ba = trgb.LastOrDefault();

                var psa = new
                {
                    //郵輪會爆炸!!!
                    AirlineCodeGo = go.FLT_NO.Substring(0, 2),
                    AirlineNmGo = dal.ContainsKey(go.FLT_NO.Substring(0, 2)) ? dal[go.FLT_NO.Substring(0, 2)] : go.FLT_NO.Substring(0, 2),
                    AirPortStartGo = go.ROUT_ID.Split('/')[0],
                    AirPortStartNmGo = //go.ROUT_ID.Split('/')[0],
                    dap.ContainsKey(
                        go.ROUT_ID.Split('/')[0])
                        ?
                        dap[go.ROUT_ID.Split('/')[0]]
                        :
                        go.ROUT_ID.Split('/')[0],
                    AirPortEndGo = go.ROUT_ID.Split('/')[1],
                    AirPortEndNmGo = //go.ROUT_ID.Split('/')[1],
                    dap.ContainsKey(
                        go.ROUT_ID.Split('/')[1])
                        ?
                        dap[go.ROUT_ID.Split('/')[1]]
                        :
                        go.ROUT_ID.Split('/')[1],
                    AirPlaneCodeGo = go.FLT_NO.Substring(2),
                    ProductSchSDGo = Convert.ToDateTime(go.DEP_DT) ,
                    AirPlaneSDGo = Convert.ToDateTime( go.DEP_DT),
                    AirPlaneEDGo = Convert.ToDateTime(go.ARR_DT),
                    AirPlaneSDGoS = go.DEP_TM,
                    AirPlaneEDGoS = go.ARR_TM,

                    AirlineCodeBa = ba.FLT_NO.Substring(0, 2),
                    AirlineNmBa = dal.ContainsKey(ba.FLT_NO.Substring(0, 2)) ? dal[ba.FLT_NO.Substring(0, 2)] : ba.FLT_NO.Substring(0, 2),
                    AirPortStartBa= ba.ROUT_ID.Split('/')[0],
                    AirPortStartNmBa= //ba.ROUT_ID.Split('/')[0],
                    dap.ContainsKey(
                        ba.ROUT_ID.Split('/')[0])
                        ?
                        dap[ba.ROUT_ID.Split('/')[0]]
                        :
                        ba.ROUT_ID.Split('/')[0],
                    AirPortEndBa = ba.ROUT_ID.Split('/')[1],
                    AirPortEndNmBa = //ba.ROUT_ID.Split('/')[1],
                     dap.ContainsKey(
                        ba.ROUT_ID.Split('/')[1])
                        ?
                        dap[ba.ROUT_ID.Split('/')[1]]
                        :
                        ba.ROUT_ID.Split('/')[1],
                    AirPlaneCodeBa = ba.FLT_NO.Substring(2),
                    ProductSchSDBa = Convert.ToDateTime(ba.DEP_DT),
                    AirPlaneSDBa = Convert.ToDateTime(ba.DEP_DT),
                    AirPlaneEDBa = Convert.ToDateTime(ba.ARR_DT),
                    AirPlaneSDBaS = ba.DEP_TM,
                    AirPlaneEDBaS = ba.ARR_TM,

                };

            return serializer.Serialize(psa);
            //}
        }
            int ProductSchID = 0;
            if (int.TryParse(ItemType, out ProductSchID))
            {
                tblProductSch ps = dm.tblProductSch.Where(a => a.ProductSchID == ProductSchID).FirstOrDefault();
                if (ps != null)
                {
                    DateTime GoAirPlaneSD = ps.ProductSchSD;
                    DateTime GoAirPlaneED = ps.ProductSchSD;
                    DateTime BaAirPlaneSD = ps.ProductSchSD.AddDays(ps.tblProduct.ProductDD - 1);
                    DateTime BAAirPlaneED = ps.ProductSchSD.AddDays(ps.tblProduct.ProductDD - 1);


                    char[] spch = new char[] { ' ' };
                    tblProductAir go = dm.tblProductAir.Where(a => a.ProductAirID == ps.ProductSchAirGo).FirstOrDefault();
                    tblProductAir ba = dm.tblProductAir.Where(a => a.ProductAirID == ps.ProductSchAirBa).FirstOrDefault();
                    if (go != null && ba != null)
                    {
                        string AirlineCodeGo = IsNumeric(go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2, 1)) ? go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 2) : go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 3);
                        string AirlineNmGo = dal.ContainsKey(IsNumeric(go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2, 1)) ? go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 2) : go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 3)) ? dal[IsNumeric(go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2, 1)) ? go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 2) : go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 3)] : IsNumeric(go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2, 1)) ? go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 2) : go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 3);
                        string AirPortStartGo = go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[0];
                        string AirPortStartNmGo = dap.ContainsKey(go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[0]) ? dap[go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[0]] : go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[0]; // 
                        string AirPortEndGo = go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[1];
                        string AirPortEndNmGo = dap.ContainsKey(go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[1]) ? dap[go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[1]] : go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[1]; // 
                                                                                                                                                                                                                                                                                                                                                            //AirPlaneCode = pago[a.ProductSchAirGo].Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2), //數字
                        string AirPlaneCodeGo = IsNumeric(go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2, 1)) ? go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2) : ""; //數字
                        string ProductSchSDGo = ps.ProductSchSD.ToString("yyyy/MM/dd");
                        DateTime AirPlaneSDGo = getSDTime(go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[2].Split('/')[0].Replace(":", ""), ps.ProductSchSD);
                        DateTime AirPlaneEDGo = getEDTime(go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[2].Split('/')[0].Replace(":", ""), go.ProductAirGo.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[2].Split('/')[1].Replace("+0", "").Replace(";", "").Replace(":", ""), ps.ProductSchSD);


                        string AirlineCodeBa = IsNumeric(ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2, 1)) ? ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 2) : ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 3);
                        string AirlineNmBa = dal.ContainsKey(IsNumeric(ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2, 1)) ? ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 2) : ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 3)) ? dal[IsNumeric(ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2, 1)) ? ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 2) : ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 3)] : IsNumeric(ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2, 1)) ? ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 2) : ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 3);
                        string AirPortStartBa = ba.ProductAirBa.Split(spch, StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[0];
                        string AirPortStartNmBa = dap.ContainsKey(ba.ProductAirBa.Split(spch, StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[0]) ? dap[ba.ProductAirBa.Split(spch, StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[0]] : ba.ProductAirBa.Split(spch, StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[0];
                        string AirPortEndBa = ba.ProductAirBa.Split(spch, StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[1];
                        string AirPortEndNmBa = dap.ContainsKey(ba.ProductAirBa.Split(spch, StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[1]) ? dap[ba.ProductAirBa.Split(spch, StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[1]] : ba.ProductAirBa.Split(spch, StringSplitOptions.RemoveEmptyEntries)[0].Split('/')[1];
                        string AirPlaneCodeBa = IsNumeric(ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[1].Substring(2, 1)) ? ba.ProductAirBa.Split(spch, StringSplitOptions.RemoveEmptyEntries)[1].Substring(2) : ""; //數字
                        string ProductSchSDBa = ps.ProductSchSD.AddDays(ps.tblProduct.ProductDD - 1).ToString("yyyy/MM/dd");
                        DateTime AirPlaneSDBa = getSDTime(ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[2].Split('/')[0].Replace(":", ""), ps.ProductSchSD.AddDays(ps.tblProduct.ProductDD - 1));
                        DateTime AirPlaneEDBa = getEDTime(ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[2].Split('/')[0].Replace(":", ""), ba.ProductAirBa.Split(spch, System.StringSplitOptions.RemoveEmptyEntries)[2].Split('/')[1].Replace("+0", "").Replace(";", "").Replace(":", ""), ps.ProductSchSD.AddDays(ps.tblProduct.ProductDD - 1));

                        var air = new
                        {
                            AirlineCodeGo,
                            AirlineNmGo,
                            AirPortStartGo,
                            AirPortStartNmGo,
                            AirPortEndGo,
                            AirPortEndNmGo,
                            AirPlaneCodeGo,
                            ProductSchSDGo,
                            AirPlaneSDGo,
                            AirPlaneEDGo,
                            AirPlaneSDGoS = AirPlaneSDGo.ToString("HH:mm"),
                            AirPlaneEDGoS = AirPlaneEDGo.ToString("HH:mm"),

                            AirlineCodeBa,
                            AirlineNmBa,
                            AirPortStartBa,
                            AirPortStartNmBa,
                            AirPortEndBa,
                            AirPortEndNmBa,
                            AirPlaneCodeBa,
                            ProductSchSDBa,
                            AirPlaneSDBa,
                            AirPlaneEDBa,
                            AirPlaneSDBaS = AirPlaneSDBa.ToString("HH:mm"),
                            AirPlaneEDBaS = AirPlaneEDBa.ToString("HH:mm"),
                        };
                        return serializer.Serialize(air);
                    }
                    else
                    {
                        return null;
                    }



                    //if (pa != null) {
                    //    switch (id) {
                    //        //去程
                    //        case 1:
                    //            //pa.ProductAirGo
                    //            break;
                    //        //回程
                    //        case 2:
                    //            //pa.ProductAirBa
                    //            break;
                    //    }

                    //}

                }
                else
                {
                    return null;
                }
            }
            else {
                return null;
            }
        }
            
    }
    public DateTime getSDTime(string planesdtime, DateTime sd)
    {
        int sdhour = int.Parse(planesdtime.Substring(0, 2));
        int sdminute = int.Parse(planesdtime.Substring(2, 2));
        DateTime sdt = sd.AddHours(sdhour).AddMinutes(sdminute);

        return sdt;

    }
    public DateTime getEDTime(string planesdtime, string planeedtime, DateTime sd)
    {
        int sdhour = int.Parse(planesdtime.Substring(0, 2));
        int sdminute = int.Parse(planesdtime.Substring(2, 2));
        int edhour = int.Parse(planeedtime.Substring(0, 2));
        int edminute = int.Parse(planeedtime.Substring(2, 2));
        DateTime sdt = sd.AddHours(sdhour).AddMinutes(sdminute);
        DateTime edt = sd.AddHours(edhour).AddMinutes(edminute);

        if (sdt > edt)
        {
            return edt.AddDays(1);
        }
        else
        {
            return edt;
        }
    }
    // IsNumeric Function
    // 資料來源：http://support.microsoft.com/kb/329488/zh-tw
    static bool IsNumeric(object Expression)
    {
        // Variable to collect the Return value of the TryParse method.
        bool isNum;

        // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
        double retNum;

        // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
        // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
        isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);


        return isNum;
    }
    // POST api/<controller>
    public void Post([FromBody]string value)
    {
    }

    // PUT api/<controller>/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/<controller>/5
    public void Delete(int id)
    {
    }
}
