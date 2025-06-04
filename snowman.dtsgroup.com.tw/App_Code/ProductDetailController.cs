using EntitytoJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

public class ProductDetailController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public object Get(string ItemType)
    {
        var serializer = new JavaScriptSerializer
        {
            MaxJsonLength = 1024000000,
            RecursionLimit = 100
        };

        serializer.RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter() });

        using (trdatagogojpEntities fdm = new trdatagogojpEntities())
        {
            //TRGRUP trg = fdm.TRGRUP.Where(a => a.GRUP_CD == ItemType).FirstOrDefault();
            ////TRSUBD trg = fdm.trsubd
            //if (trg != null)
            //{
                TRMGRUP trmg = fdm.TRMGRUP.Where(a => a.MGRUP_CD == ItemType).FirstOrDefault();
            if (trmg != null)
            {
                
                var tgits = fdm.TRMGITN.Where(a => a.MGRUP_CD == ItemType && a.ITN_DY>950 ).ToList();


                //string ProductContain = "";
                //string ProductNoContain = "";
                //行程特色 951
                //特別安排 952
                //團費說明 953
                //團票說明 954
                string TourFeatures = tgits.Where(a => a.ITN_DY == 951).FirstOrDefault()!=null? tgits.Where(a => a.ITN_DY == 951).FirstOrDefault().ITN_DR:"";
                string SPArrangement = tgits.Where(a => a.ITN_DY == 952).FirstOrDefault() != null ? tgits.Where(a => a.ITN_DY == 952).FirstOrDefault().ITN_DR : ""; ;
                string TourFee = tgits.Where(a => a.ITN_DY == 953).FirstOrDefault() != null ? tgits.Where(a => a.ITN_DY == 953).FirstOrDefault().ITN_DR : ""; ;
                string GroupTicket = tgits.Where(a => a.ITN_DY == 954).FirstOrDefault() != null ? tgits.Where(a => a.ITN_DY == 954).FirstOrDefault().ITN_DR : ""; ;
                string GRUP_RK = trmg.GRUP_RK;

                var ps = new
                {
                    ProductDetailH = "",
                    ProductDetailE = "",
                    ProductDetail2 = trmg.ITN_DR,
                    ProductDetail3 = "",
                    ProductDetail4 = "",
                    ProductDetail5 = "",
                    ProductDetail6 = "",
                    TourFeatures,
                    SPArrangement,
                    TourFee,
                    GroupTicket,
                    GRUP_RK = GRUP_RK,
                };

                return serializer.Serialize(ps);
                //}
            }

            using (DataModel dm = new DataModel())
            {
                //dm.Configuration.ProxyCreationEnabled = false;

                //tblProductSch ps = dm.tblProductSch.Where(a => a.ProductSchID == ItemType &&
                //a.tblProduct.ProductTerm.Length > 0
                //&& a.tblProduct.ProductCity.Length > 0
                //&& a.tblProduct.ProductFlight.Length > 0
                //&& a.tblProduct.ProductCity.Length > 0
                //&& a.tblProduct.ProductCoun.Length > 0
                //&& a.tblProduct.ProductCont.Length > 0
                //&& a.tblProduct.ProductEnable && !a.tblProduct.ProductDel
                //    //1	販售  //0	取消 //2 結團  //3	刪除 //4	候補  //5	停售
                //    //&& (a.ProductSchStatus == "1" || a.ProductSchStatus == "4" || a.ProductSchStatus == "5") && (a.ProductSchQA - a.ProductSchQB - a.ProductSchQD) > 0
                //    && (a.ProductSchStatus == "1" || a.ProductSchStatus == "2" || a.ProductSchStatus == "4" || a.ProductSchStatus == "5")
                //    && a.ProductSchSD >= DateTime.Now
                //    && a.tblProduct.ProductBS <= DateTime.Now && a.tblProduct.ProductBE >= DateTime.Now
                //    && a.tblProduct.ProductSite != "2" //排除同業專賣
                //    && ((a.ProductSchDC.Year == 2000 && a.ProductSchDC.Month == 1 && a.ProductSchDC.Day == 1) || a.ProductSchDC >= DateTime.Now)
                //    //強制價格大於0 才顯示
                //    && a.ProductSchPA > 0
                //).FirstOrDefault();
                int ProductID = 0;
                if (int.TryParse(ItemType, out ProductID))
                {
                    tblProductDetail ps = dm.tblProductDetail.Where(a => a.ProductID == ProductID).ToList().FirstOrDefault();
                    if (ps != null)
                    {
                        //tblProduct p = dm.tblProduct.Where(a => a.ProductID == ps.ProductID).Select(a=>new { });
                        return serializer.Serialize(ps);
                        //return ps;
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
