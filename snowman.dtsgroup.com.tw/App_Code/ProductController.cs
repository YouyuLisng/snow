using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Http;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Data.Entity;
using EntitytoJson;

public class ProductController : ApiController
{
    //// GET api/<controller>
    //public IEnumerable<string> Get()
    //{
    //    return new string[] { "value1", "value2" };
    //}

    // GET api/<controller>/ItemType
    public object Get(string ItemType)
    {
        var serializer = new JavaScriptSerializer
        {
            MaxJsonLength = 1024000000,
            RecursionLimit = 100
        };

        serializer.RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter() });
        string MGRUP_CD = ItemType;

        using (trdatagogojpEntities tdm = new trdatagogojpEntities())
        {
            TRMGRUP trmg = tdm.TRMGRUP.Where(a => a.MGRUP_CD == ItemType).FirstOrDefault();
            if (trmg != null)
            {
                var trg = tdm.TRGRUP.Where(a => a.MGRUP_CD == ItemType && a.WEB_PD != "N").ToList()
                    .Where(a => DateTime.Parse(a.LEAV_DT) > DateTime.Now);
                List<string> GRUP_CDL = trg.Select(a => a.GRUP_CD).ToList();

                var trsbd = tdm.TRSUBD.Where(a => GRUP_CDL.Contains(a.GRUP_CD));
                if (trg.Count() > 0)
                {
                    //a.VALID_FG啟用
                    //TRMGRUP trmg = fdm.TRMGRUP.Where(a => a.MGRUP_CD == trg.MGRUP_CD && a.VALID_FG.Length == 0).FirstOrDefault();
                    double ProductSchPA = 0;
                    double ProductSchPE = 0;
                    double ProductSchPC = 0;
                    double ProductSchPG = 0;

                    if (trsbd.Where(a => a.CURR_AM > 0 && a.BED_TP == "1" && a.JOIN_TP == "1").Count() > 0)
                    {
                        ProductSchPA = trsbd.Where(a => a.CURR_AM > 0 && a.BED_TP == "1" && a.JOIN_TP == "1").Min(a => a.CURR_AM).GetValueOrDefault(0);
                        ProductSchPE = trsbd.Where(a => a.CURR_AM > 0 && a.BED_TP == "2" && a.JOIN_TP == "1").Min(a => a.CURR_AM).GetValueOrDefault(0);
                        ProductSchPC = trsbd.Where(a => a.CURR_AM > 0 && a.BED_TP == "3" && a.JOIN_TP == "1").Min(a => a.CURR_AM).GetValueOrDefault(0);
                        //加床
                        //ProductSchPG = trsbd.Where(a => a.CURR_AM > 0 && a.BED_TP == "4" && a.JOIN_TP == "1").Min(a => a.CURR_AM).Value;
                        ProductSchPG = trsbd.Where(a => a.CURR_AM > 0 && a.BED_TP == "5" && a.JOIN_TP == "1").Min(a => a.CURR_AM).GetValueOrDefault(0);
                    }
                    else {
                        ProductSchPA = trsbd.Where(a => a.CURR_AM > 0 && a.BED_TP == "1" && a.JOIN_TP == "3").Min(a => a.CURR_AM).GetValueOrDefault(0);
                        ProductSchPE = trsbd.Where(a => a.CURR_AM > 0 && a.BED_TP == "2" && a.JOIN_TP == "3").Min(a => a.CURR_AM).GetValueOrDefault(0);
                        ProductSchPC = trsbd.Where(a => a.CURR_AM > 0 && a.BED_TP == "3" && a.JOIN_TP == "3").Min(a => a.CURR_AM).GetValueOrDefault(0);
                        ProductSchPG = trsbd.Where(a => a.CURR_AM > 0 && a.BED_TP == "5" && a.JOIN_TP == "3").Min(a => a.CURR_AM).GetValueOrDefault(0);

                    }
                    //dk3
                    TRSUBD ts = trsbd.Where(a => a.BED_TP == "1" && a.CURR_AM == ProductSchPA).FirstOrDefault();
                    if (ts != null)
                    {
                        TRGRUP tr = trg.Where(a => a.GRUP_CD == ts.GRUP_CD).FirstOrDefault();

                        if (tr != null)
                        {
                            var ps = new
                            {

                                ProductID = trmg.MGRUP_CD,
                                ProductSchPA,
                                ProductSchPE,
                                ProductSchPC,
                                ProductSchPG,
                                ProductSchSD = DateTime.Parse(tr.LEAV_DT),
                                ProductSchDP = tr.DORD_AM,
                                //有問題
                                ProductSchTax =
                                tdm.TRGOPITEM.Where(b => b.GRUP_CD == tr.GRUP_CD && b.MUSTBUY == "1").Count() > 0
                                ? tdm.TRGOPITEM.Where(b => b.GRUP_CD == tr.GRUP_CD && b.MUSTBUY == "1").First().ATT_AM.ToString() : "單價包含稅險",
                                tblProduct = new
                                {
                                    ProductNmPre = "",
                                    ProductNm = trmg.GRUP_NM,
                                    ProductID = trmg.MGRUP_CD
                                }

                            };
                            return serializer.Serialize(ps);
                        }
                      
                    }
                    
                    var nogroup = new
                    {

                        ProductID = trmg.MGRUP_CD,
                        ProductSchPA = "目前無可販售團體",
                        ProductSchPE = "目前無可販售團體",
                        ProductSchPC = "目前無可販售團體",
                        ProductSchPG = "目前無可販售團體",
                        ProductSchSD = "目前無可販售團體",
                        ProductSchDP = "目前無可販售團體",
                        //有問題
                        ProductSchTax = "目前無可販售團體",
                        tblProduct = new
                        {
                            ProductNmPre = "",
                            ProductNm = trmg.GRUP_NM,
                            ProductID = trmg.MGRUP_CD
                        }

                    };
                    return serializer.Serialize(nogroup);
                }
                else
                {
                    var nogroup = new
                    {

                        ProductID = trmg.MGRUP_CD,
                        ProductSchPA = "目前無可販售團體",
                        ProductSchPE = "目前無可販售團體",
                        ProductSchPC = "目前無可販售團體",
                        ProductSchPG = "目前無可販售團體",
                        ProductSchSD = "目前無可販售團體",
                        ProductSchDP = "目前無可販售團體",
                        //有問題
                        ProductSchTax = "目前無可販售團體",
                        tblProduct = new
                        {
                            ProductNmPre = "",
                            ProductNm = trmg.GRUP_NM,
                            ProductID = trmg.MGRUP_CD
                        }

                    };
                    return serializer.Serialize(nogroup);
                }
            }
        }
        using (DataModel dm = new DataModel())
        {
            int ProductID = 0;
            //dm.Configuration.ProxyCreationEnabled = false;
            if (int.TryParse(ItemType, out ProductID))
            {
                tblProduct p = dm.tblProduct.Where(a => a.ProductID == ProductID
                && a.ProductTerm.Length > 0
                && a.ProductCity.Length > 0
                && a.ProductFlight.Length > 0
                && a.ProductCity.Length > 0
                && a.ProductCoun.Length > 0
                && a.ProductCont.Length > 0).FirstOrDefault();
                if (p != null)
                {
                    var ps = new
                    {

                        p.ProductID,
                        ProductSchPA = p.ProductPA,
                        ProductSchPE = p.ProductPE,
                        ProductSchPC = p.ProductPC,
                        ProductSchPG = p.ProductPG,
                        //ProductSchSD = DateTime.Parse(tr.LEAV_DT),
                        ProductSchSD = "",
                        ProductSchDP = "依各團規定",
                        //有問題
                        ProductSchTax = p.ProductTax != 0 ? p.ProductTax.ToString() : "單價包含稅險",
                        tblProduct = new
                        {
                            ProductNmPre = "",
                            ProductNm = p.ProductNm,
                            ProductID = p.ProductID
                        }

                    };

                    return serializer.Serialize(ps);
                }
            }
            return serializer.Serialize(null);

        }
    }

    // GET api/<controller>/ItemType/id
    //public object Get(string ItemType, int id)
    //{
    //    using (trdatagogojpEntities fdm = new trdatagogojpEntities())
    //    using (DataModel dm = new DataModel())
    //    {


    //        var serializer = new JavaScriptSerializer
    //        {
    //            MaxJsonLength = 1024000000,
    //            RecursionLimit = 100
    //        };

    //        serializer.RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter() });

    //        TRGRUP tr = fdm.TRGRUP.Where(a => a.GRUP_CD == ItemType).FirstOrDefault();
    //        if (tr != null)
    //        {
    //            TRMGRUP trm = fdm.TRMGRUP.Where(a => a.MGRUP_CD == tr.MGRUP_CD).FirstOrDefault();
    //            if (trm != null)
    //            {
    //                var p = new {
    //                    ProductID = trm.MGRUP_CD,
    //                    ProductNmPre = trm.GRUP_IOTM,
    //                    ProductNm = trm.GRUP_NM,
    //                    ProductTourID = tr.GRUP_CD ,

    //                    //GRUP_NM

    //                };
    //                return serializer.Serialize(trm);
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //        }
    //        else {
    //            return null;
    //        }

    //        //tblProductSch ps = dm.tblProductSch.Where(a => a.ProductSchID == ItemType

    //        //).ToList().FirstOrDefault();
    //        //if (ps != null)
    //        //{
    //        //    //tblProduct p = dm.tblProduct.Where(a => a.ProductID == ps.ProductID).Select(a=>new { });
    //        //    return serializer.Serialize( ps);
    //        //    //return ps;
    //        //}
    //        //else {
    //        //    return null;
    //        //}

    //    }

    //}
    public object Get(string ItemType, string id)
    {
        var serializer = new JavaScriptSerializer
        {
            MaxJsonLength = 1024000000,
            RecursionLimit = 100
        };

        serializer.RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter() });

        using (trdatagogojpEntities tdm = new trdatagogojpEntities())
        {
            TRGRUP trg = tdm.TRGRUP.Where(a => a.GRUP_CD == ItemType ).FirstOrDefault();
            

            var trsbd = tdm.TRSUBD.Where(a => a.GRUP_CD == ItemType && a.JOIN_TP=="1");
            if (trg != null)
            {
                //a.VALID_FG啟用
                //TRMGRUP trmg = fdm.TRMGRUP.Where(a => a.MGRUP_CD == trg.MGRUP_CD && a.VALID_FG.Length == 0).FirstOrDefault();
                double ProductSchPA = 0;
                double ProductSchPE = 0;
                double ProductSchPC = 0;
                double ProductSchPG = 0;

                foreach (var p in trsbd) {
                    switch (p.BED_TP) {
                        case "1":
                            //大人
                            ProductSchPA = p.CURR_AM.Value;
                            break;
                        case "2":
                            //小孩佔床
                            ProductSchPE = p.CURR_AM.Value;
                            break;
                        case "3":
                            //小孩不佔床
                            ProductSchPC = p.CURR_AM.Value;
                            break;
                        case "4":
                            //加床
                            break;
                        case "5":
                            //嬰兒
                            ProductSchPG = p.CURR_AM.Value;
                            break;
                        
                        case "6":
                            //老人
                            break;
                    }
                }

                var ps = new
                {
                   
                    ProductID = trg.MGRUP_CD,
                    ProductSchPA,
                    ProductSchPE,
                    ProductSchPC,
                    ProductSchPG,
                    ProductSchSD = DateTime.Parse( trg.LEAV_DT),
                    ProductSchDP = trg.DORD_AM,
                    //有問題
                    ProductSchTax = 
                    tdm.TRGOPITEM.Where(a=>a.GRUP_CD==trg.GRUP_CD && a.MUSTBUY=="1").Sum(a=>a.ATT_AM)!=null?
                    tdm.TRGOPITEM.Where(a => a.GRUP_CD == trg.GRUP_CD && a.MUSTBUY == "1").Sum(a => a.ATT_AM)
                    :0
                    ,
                    tblProduct = new
                    {
                        ProductNmPre = "",
                        ProductNm = trg.GRUP_NM,
                        ProductID = trg.MGRUP_CD
                    }

                };
                return serializer.Serialize(ps);

                //}
            }
        }

        using (DataModel dm = new DataModel())
        {
            int ProductSchID = 0;
            if (int.TryParse(ItemType, out ProductSchID))
            {
                tblProductSch ps = dm.tblProductSch.Where(a => a.ProductSchID == ProductSchID).ToList().FirstOrDefault();


                if (ps != null)
                {
                    return serializer.Serialize(ps);
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
