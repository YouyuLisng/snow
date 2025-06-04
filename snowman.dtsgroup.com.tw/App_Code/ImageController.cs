using EntitytoJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

public class ImageController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }
    public string Get(int id)
    {
        return "value";
    }
    // GET api/<controller>/5
    public string Get(string ItemType, string id)
    {
        //return "value";
        var serializer = new JavaScriptSerializer
        {
            MaxJsonLength = 1024000000,
            RecursionLimit = 100
        };

        serializer.RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter() });
        using (DataModel dm = new DataModel())
        {
            var dis = dm.tblDicImage.Where(a => a.DicImageClnm == ItemType && a.DicImageClnmID == 0);

            

            
            List<string> ls = new List<string>();
            //pic.dts.tw/proimages/ProductID/730/<%# Eval("ProductID") %>.jpg
            ls.Add("https://www.dtsgroup.com.tw/proimages/ProductID/730/" + ItemType + ".jpg");
            if (dis.Count() > 0)
            {
                ls.AddRange(dis.Select(a => a.DicImageURL));
            }
            else
            {
                var tpds = dm.tblProductDailyCW.Where(a => a.ProductID == ItemType && a.StyleType != 0);
                if (tpds.Count() > 0)
                {

                    foreach (tblProductDailyCW tpd in tpds)
                    {

                        switch (tpd.StyleType)
                        {
                            case 1:
                                if (tpd.Pic1 != "")
                                {
                                    ls.Add(tpd.Pic1);
                                }
                                break;
                            case 2:
                                if (tpd.Pic1 != "")
                                {
                                    ls.Add(tpd.Pic1);
                                }
                                if (tpd.Pic2 != "")
                                {
                                    ls.Add(tpd.Pic2);
                                }
                                break;
                            case 3:
                                if (tpd.Pic1 != "")
                                {
                                    ls.Add(tpd.Pic1);
                                }
                                if (tpd.Pic2 != "")
                                {
                                    ls.Add(tpd.Pic2);
                                }
                                if (tpd.Pic3 != "")
                                {
                                    ls.Add(tpd.Pic3);
                                }
                                break;
                            case 4:
                                if (tpd.Pic1 != "")
                                {
                                    ls.Add(tpd.Pic1);
                                }
                                if (tpd.Pic2 != "")
                                {
                                    ls.Add(tpd.Pic2);
                                }
                                if (tpd.Pic3 != "")
                                {
                                    ls.Add(tpd.Pic3);
                                }
                                if (tpd.Pic4 != "")
                                {
                                    ls.Add(tpd.Pic4);
                                }
                                break;

                        }

                    }
                }
                int ProductID = 0;
                if (int.TryParse(ItemType, out ProductID))
                {
                    var tpdso = dm.tblProductDaily.Where(a => a.ProductID == ProductID && a.StyleType != 0);
                    foreach (tblProductDaily tpd in tpdso)
                    {

                        switch (tpd.StyleType)
                        {
                            case 1:
                                if (tpd.Pic1 != "")
                                {
                                    ls.Add(tpd.Pic1);
                                }
                                break;
                            case 2:
                                if (tpd.Pic1 != "")
                                {
                                    ls.Add(tpd.Pic1);
                                }
                                if (tpd.Pic2 != "")
                                {
                                    ls.Add(tpd.Pic2);
                                }
                                break;
                            case 3:
                                if (tpd.Pic1 != "")
                                {
                                    ls.Add(tpd.Pic1);
                                }
                                if (tpd.Pic2 != "")
                                {
                                    ls.Add(tpd.Pic2);
                                }
                                if (tpd.Pic3 != "")
                                {
                                    ls.Add(tpd.Pic3);
                                }
                                break;
                            case 4:
                                if (tpd.Pic1 != "")
                                {
                                    ls.Add(tpd.Pic1);
                                }
                                if (tpd.Pic2 != "")
                                {
                                    ls.Add(tpd.Pic2);
                                }
                                if (tpd.Pic3 != "")
                                {
                                    ls.Add(tpd.Pic3);
                                }
                                if (tpd.Pic4 != "")
                                {
                                    ls.Add(tpd.Pic4);
                                }
                                break;

                        }

                    }
                    //RptImg3.DataSource = ls;
                    //var listds = dm.tblProductTI
                    //    //.Where(a=>a.ProductID== ItemType)
                    //    .Join(dm.tblDicImage, p => p.ProductTIID, d => d.DicImageClnmID, (p, d) => new
                    //{
                    //    DicImageID = d.DicImageID,
                    //    ProductTIID = p.ProductTIID,
                    //    ProductID = p.ProductID,
                    //    DicImageClnm = d.DicImageClnm
                    //}).Where(a => a.DicImageClnm == "ProductTIID").Where(a=>a.ProductID== ItemType).AsQueryable();
                }

             
            }
            return serializer.Serialize(ls.Where(a => a != "images/back/plusImg.jpg"));
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
