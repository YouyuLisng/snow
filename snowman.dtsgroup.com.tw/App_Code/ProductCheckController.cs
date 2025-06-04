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

public class ProductCheckController : ApiController
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

            return (trmg != null);
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
