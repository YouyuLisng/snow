using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

public class DayTourController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public object Get(string ItemType)
    {
        using (trdatagogojpEntities tdm = new trdatagogojpEntities())
        using (DataModel dm = new DataModel())
        {
            //var itn = fdm.TRGITN.Where(a => a.GRUP_CD == ItemType && a.ITN_DY < 950).OrderBy(a => a.ITN_DY);
            TRGRUP tg = tdm.TRGRUP.Where(a => a.GRUP_CD == ItemType || a.MGRUP_CD == ItemType).FirstOrDefault();
            string MGRUP_CD = "";
            if (tg != null)
            {
                MGRUP_CD = tg.MGRUP_CD;
            }

            if (MGRUP_CD != ItemType)
            {

                var itn = tdm.TRGITN.Where(a => a.GRUP_CD == ItemType && a.ITN_DY < 950).OrderBy(a => a.ITN_DY);
                if (itn.Count() > 0 && MGRUP_CD.Length > 0)
                {
                    if (dm.tblProductDailyCW.Where(b => b.ProductID == MGRUP_CD).Count() == tg.GRUP_LN)
                    {

                        var ps = itn.ToList().Select(a => new
                        {
                            ProductTIS = a.ITN_DY,
                            ProductTIA = HtmlHelper.StripHTML(a.ITN_NM),
                            //ProductTIB = 
                            //ProductTIC ="注意事項不在這個資料表",
                            ProductTIC = a.ITN_MEAL,
                            ProductTID = HtmlHelper.StripHTML(a.ITNBR_DR),
                            ProductTIE = HtmlHelper.StripHTML(a.ITNLU_DR),
                            ProductTIF = HtmlHelper.StripHTML(a.ITNDI_DR),
                            ProductTIG = HtmlHelper.StripHTML(a.ITN_HTL),

                            tblProductDaily = dm.tblProductDailyCW.Where(b => b.ItemDay == a.ITN_DY && b.ProductID == MGRUP_CD).Select(b => new
                            {
                                b.ItemDescript
                                                ,
                                b.ItemSight
                                                ,
                                b.Pic1
                                                ,
                                b.Title1
                                                ,
                                b.Tag1
                                                ,
                                b.Content1
                                                ,
                                b.WorldFlag1
                                                ,
                                b.IncludeFlag1
                                                ,
                                b.OnlyFlag1
                                                ,
                                b.Pic2
                                                ,
                                b.Title2
                                                ,
                                b.Tag2
                                                ,
                                b.Content2
                                                ,
                                b.WorldFlag2
                                                ,
                                b.IncludeFlag2
                                                ,
                                b.OnlyFlag2
                                                ,
                                b.Pic3
                                                ,
                                b.Title3
                                                ,
                                b.Tag3
                                                ,
                                b.Content3
                                                ,
                                b.WorldFlag3
                                                ,
                                b.IncludeFlag3
                                                ,
                                b.OnlyFlag3
                                                ,
                                b.Pic4
                                                ,
                                b.Title4
                                                ,
                                b.Tag4
                                                ,
                                b.Content4
                                                ,
                                b.WorldFlag4
                                                ,
                                b.IncludeFlag4
                                                ,
                                b.OnlyFlag4
                                                ,
                                b.Pic5
                                                ,
                                b.Title5
                                                ,
                                b.Tag5
                                                ,
                                b.Content5
                                                ,
                                b.WorldFlag5
                                                ,
                                b.IncludeFlag5
                                                ,
                                b.OnlyFlag5
                                                ,
                                b.AlertFlag
                                                ,
                                b.AlertTitle
                                                ,
                                b.AlertContent
                                                ,
                                b.NoteTitle
                                                ,
                                b.ModifyUser
                                                ,
                                b.ModifyDate
                                                ,
                                b.CheckSelect
                            }).First(),
                            tblProductDailySight = dm.tblProductDailySightCW.Select(b=>new {
                                b.ItemDay,
                                b.OrderByValue,
                                b.ProductID,
                                b.Title,
                                b.SightContent,
                                b.SightNotice,
                            }).Where(b => b.ItemDay == a.ITN_DY && b.ProductID == MGRUP_CD).OrderBy(b=>b.OrderByValue),
                        });
                        return JsonConvert.SerializeObject(ps);

                    }
                    else
                    {
                        var ps = itn.ToList().Select(a => new
                        {
                            ProductTIS = a.ITN_DY,
                            ProductTIA = HtmlHelper.StripHTML(a.ITN_NM),
                            //ProductTIB = 
                            //ProductTIC ="注意事項不在這個資料表",
                            ProductTIC = a.ITN_MEAL,
                            ProductTID = HtmlHelper.StripHTML(a.ITNBR_DR),
                            ProductTIE = HtmlHelper.StripHTML(a.ITNLU_DR),
                            ProductTIF = HtmlHelper.StripHTML(a.ITNDI_DR),
                            ProductTIG = HtmlHelper.StripHTML(a.ITN_HTL),
                            tblProductDaily = new
                            {
                                ItemDescript =
                                a.ITN_DR.Length > 0 ?
                                a.ITN_DR
                                :
                                (
                                tdm.TRMGITN.Where(b => b.MGRUP_CD == MGRUP_CD && b.ITN_DY == a.ITN_DY).FirstOrDefault() != null ?
                                tdm.TRMGITN.Where(b => b.MGRUP_CD == MGRUP_CD && b.ITN_DY == a.ITN_DY).First().ITN_DR : "")
                            },
                            tblProductDailySight = 
                            dm.tblProductDailySightCW.Where(b => b.ItemDay == a.ITN_DY && b.ProductID == MGRUP_CD).OrderBy(b => b.OrderByValue).Select(b=>new {
                                b.ItemDay,
                                b.ModifyDate,
                                b.ModifyUser,
                                b.OrderByValue,
                                b.ProductID,
                                b.SightContent,
                                b.SightNotice,
                                b.Title

                            })
                        });

                        return JsonConvert.SerializeObject(ps);
                    }
                }


            }
            else
            {
                var mitn = tdm.TRMGITN.Where(a => a.MGRUP_CD == MGRUP_CD && a.ITN_DY < 950).OrderBy(a => a.ITN_DY);
                if (mitn.Count() > 0 && MGRUP_CD.Length > 0)
                {
                    //天數可能不一樣長!!!
                    if (dm.tblProductDailyCW.Where(b => b.ProductID == MGRUP_CD).Count() == tg.GRUP_LN)
                    {

                        var ps = mitn.ToList().Select(a => new
                        {
                            ProductTIS = a.ITN_DY,
                            ProductTIA = HtmlHelper.StripHTML(a.ITN_NM),
                            //ProductTIB = 
                            //ProductTIC ="注意事項不在這個資料表",
                            ProductTIC = a.ITN_MEAL,
                            ProductTID = HtmlHelper.StripHTML(a.ITNBR_DR),
                            ProductTIE = HtmlHelper.StripHTML(a.ITNLU_DR),
                            ProductTIF = HtmlHelper.StripHTML(a.ITNDI_DR),
                            ProductTIG = HtmlHelper.StripHTML(a.ITN_HTL),

                            tblProductDaily = dm.tblProductDailyCW.Where(b => b.ItemDay == a.ITN_DY && b.ProductID == MGRUP_CD).Select(b => new
                            {
                                b.ItemDescript
                                                ,
                                b.ItemSight
                                                ,
                                b.Pic1
                                                ,
                                b.Title1
                                                ,
                                b.Tag1
                                                ,
                                b.Content1
                                                ,
                                b.WorldFlag1
                                                ,
                                b.IncludeFlag1
                                                ,
                                b.OnlyFlag1
                                                ,
                                b.Pic2
                                                ,
                                b.Title2
                                                ,
                                b.Tag2
                                                ,
                                b.Content2
                                                ,
                                b.WorldFlag2
                                                ,
                                b.IncludeFlag2
                                                ,
                                b.OnlyFlag2
                                                ,
                                b.Pic3
                                                ,
                                b.Title3
                                                ,
                                b.Tag3
                                                ,
                                b.Content3
                                                ,
                                b.WorldFlag3
                                                ,
                                b.IncludeFlag3
                                                ,
                                b.OnlyFlag3
                                                ,
                                b.Pic4
                                                ,
                                b.Title4
                                                ,
                                b.Tag4
                                                ,
                                b.Content4
                                                ,
                                b.WorldFlag4
                                                ,
                                b.IncludeFlag4
                                                ,
                                b.OnlyFlag4
                                                ,
                                b.Pic5
                                                ,
                                b.Title5
                                                ,
                                b.Tag5
                                                ,
                                b.Content5
                                                ,
                                b.WorldFlag5
                                                ,
                                b.IncludeFlag5
                                                ,
                                b.OnlyFlag5
                                                ,
                                b.AlertFlag
                                                ,
                                b.AlertTitle
                                                ,
                                b.AlertContent
                                                ,
                                b.NoteTitle
                                                ,
                                b.ModifyUser
                                                ,
                                b.ModifyDate
                                                ,
                                b.CheckSelect
                            }).First(),
                            tblProductDailySight = 
                            dm.tblProductDailySightCW.Where(b => b.ItemDay == a.ITN_DY && b.ProductID == MGRUP_CD).OrderBy(b => b.OrderByValue).Select(b => new {
                                b.ItemDay,
                                b.ModifyDate,
                                b.ModifyUser,
                                b.OrderByValue,
                                b.ProductID,
                                b.SightContent,
                                b.SightNotice,
                                b.Title

                            })
                        });
                        return JsonConvert.SerializeObject(ps);

                    }
                    else
                    {
                        var ps = mitn.ToList().Select(a => new
                        {
                            ProductTIS = a.ITN_DY,
                            ProductTIA = HtmlHelper.StripHTML(a.ITN_NM),
                            //ProductTIB = 
                            //ProductTIC ="注意事項不在這個資料表",
                            ProductTIC = a.ITN_MEAL,
                            ProductTID = HtmlHelper.StripHTML(a.ITNBR_DR),
                            ProductTIE = HtmlHelper.StripHTML(a.ITNLU_DR),
                            ProductTIF = HtmlHelper.StripHTML(a.ITNDI_DR),
                            ProductTIG = HtmlHelper.StripHTML(a.ITN_HTL),
                            tblProductDaily = new
                            {
                                ItemDescript = a.ITN_DR,
                                
                            }
                            ,
                            tblProductDailySight = dm.tblProductDailySightCW.Where(b => b.ItemDay == a.ITN_DY && b.ProductID == MGRUP_CD).OrderBy(b => b.OrderByValue).Select(b => new {
                                b.ItemDay,
                                b.ModifyDate,
                                b.ModifyUser,
                                b.OrderByValue,
                                b.ProductID,
                                b.SightContent,
                                b.SightNotice,
                                b.Title

                            })
                        });

                        return JsonConvert.SerializeObject(ps);
                    }
                }
            }
        }
        using (DataModel dm = new DataModel())
        {
            int ProductSchID = 0;
            if (int.TryParse(ItemType, out ProductSchID))
            {
                tblProductSch ps = dm.tblProductSch.Where(a => a.ProductSchID == ProductSchID || a.ProductID == ProductSchID).FirstOrDefault();
                if (ps != null)
                {
                    var ti = dm.tblProductTI.Where(a => a.ProductID == ps.ProductID && a.ProductTIS <= a.tblProduct.ProductDD).Select(a => new
                    {
                        a.ProductTIS,
                        a.ProductTIA,
                        a.ProductTIB,
                        a.ProductTIC,
                        a.ProductTID,
                        a.ProductTIE,
                        a.ProductTIF,
                        a.ProductTIG,
                        tblProductDaily = a.tblProduct.tblProductDaily.Where(b => b.ItemDay == a.ProductTIS).Select(b => new
                        {
                            b.ProductID
                                          ,
                            b.ItemDay
                                          ,
                            b.StyleType
                                          ,
                            b.ItemDescript
                                          ,
                            b.ItemSight
                                          ,
                            b.Pic1
                                          ,
                            b.Title1
                                          ,
                            b.Tag1
                                          ,
                            b.Content1
                                          ,
                            b.WorldFlag1
                                          ,
                            b.IncludeFlag1
                                          ,
                            b.OnlyFlag1
                                          ,
                            b.Pic2
                                          ,
                            b.Title2
                                          ,
                            b.Tag2
                                          ,
                            b.Content2
                                          ,
                            b.WorldFlag2
                                          ,
                            b.IncludeFlag2
                                          ,
                            b.OnlyFlag2
                                          ,
                            b.Pic3
                                          ,
                            b.Title3
                                          ,
                            b.Tag3
                                          ,
                            b.Content3
                                          ,
                            b.WorldFlag3
                                          ,
                            b.IncludeFlag3
                                          ,
                            b.OnlyFlag3
                                          ,
                            b.Pic4
                                          ,
                            b.Title4
                                          ,
                            b.Tag4
                                          ,
                            b.Content4
                                          ,
                            b.WorldFlag4
                                          ,
                            b.IncludeFlag4
                                          ,
                            b.OnlyFlag4
                                          ,
                            b.Pic5
                                          ,
                            b.Title5
                                          ,
                            b.Tag5
                                          ,
                            b.Content5
                                          ,
                            b.WorldFlag5
                                          ,
                            b.IncludeFlag5
                                          ,
                            b.OnlyFlag5
                                          ,
                            b.AlertFlag
                                          ,
                            b.AlertTitle
                                          ,
                            b.AlertContent
                                          ,
                            b.NoteTitle
                                          ,
                            b.ModifyUser
                                          ,
                            b.ModifyDate
                                          ,
                            b.CheckSelect


                        }),

                    }).OrderBy(a => a.ProductTIS);
                    return JsonConvert.SerializeObject(ti);
                }
                else
                {
                    return ps;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public object Get(string ItemType, string id)
    {
        using (trdatagogojpEntities fdm = new trdatagogojpEntities())
        using (DataModel dm = new DataModel())
        {
            string MGRUP_CD = id;
            var trm = fdm.TRMGRUP.Where(a => a.MGRUP_CD == MGRUP_CD).FirstOrDefault();
            var mitn = fdm.TRMGITN.Where(a => a.MGRUP_CD == MGRUP_CD && a.ITN_DY < 950).OrderBy(a => a.ITN_DY);
            if (mitn.Count() > 0 && MGRUP_CD.Length > 0 && trm!=null)
            {
                //天數可能不一樣長!!!
                if (dm.tblProductDailyCW.Where(b => b.ProductID == MGRUP_CD).Count() == trm.GRUP_LN)
                {

                    var ps = mitn.ToList().Select(a => new
                    {
                        ProductTIS = a.ITN_DY,
                        ProductTIA = HtmlHelper.StripHTML(a.ITN_NM),
                        //ProductTIB = 
                        //ProductTIC ="注意事項不在這個資料表",
                        ProductTIC = a.ITN_MEAL,
                        ProductTID = HtmlHelper.StripHTML(a.ITNBR_DR),
                        ProductTIE = HtmlHelper.StripHTML(a.ITNLU_DR),
                        ProductTIF = HtmlHelper.StripHTML(a.ITNDI_DR),
                        ProductTIG = HtmlHelper.StripHTML(a.ITN_HTL),

                        tblProductDaily = dm.tblProductDailyCW.Where(b => b.ItemDay == a.ITN_DY && b.ProductID == MGRUP_CD).Select(b => new
                        {
                            b.ItemDescript
                                            ,
                            b.ItemSight
                                            ,
                            b.Pic1
                                            ,
                            b.Title1
                                            ,
                            b.Tag1
                                            ,
                            b.Content1
                                            ,
                            b.WorldFlag1
                                            ,
                            b.IncludeFlag1
                                            ,
                            b.OnlyFlag1
                                            ,
                            b.Pic2
                                            ,
                            b.Title2
                                            ,
                            b.Tag2
                                            ,
                            b.Content2
                                            ,
                            b.WorldFlag2
                                            ,
                            b.IncludeFlag2
                                            ,
                            b.OnlyFlag2
                                            ,
                            b.Pic3
                                            ,
                            b.Title3
                                            ,
                            b.Tag3
                                            ,
                            b.Content3
                                            ,
                            b.WorldFlag3
                                            ,
                            b.IncludeFlag3
                                            ,
                            b.OnlyFlag3
                                            ,
                            b.Pic4
                                            ,
                            b.Title4
                                            ,
                            b.Tag4
                                            ,
                            b.Content4
                                            ,
                            b.WorldFlag4
                                            ,
                            b.IncludeFlag4
                                            ,
                            b.OnlyFlag4
                                            ,
                            b.Pic5
                                            ,
                            b.Title5
                                            ,
                            b.Tag5
                                            ,
                            b.Content5
                                            ,
                            b.WorldFlag5
                                            ,
                            b.IncludeFlag5
                                            ,
                            b.OnlyFlag5
                                            ,
                            b.AlertFlag
                                            ,
                            b.AlertTitle
                                            ,
                            b.AlertContent
                                            ,
                            b.NoteTitle
                                            ,
                            b.ModifyUser
                                            ,
                            b.ModifyDate
                                            ,
                            b.CheckSelect
                        }).First(),
                        tblProductDailySight = 
                        dm.tblProductDailySightCW.Where(b => b.ItemDay == a.ITN_DY && b.ProductID == MGRUP_CD).OrderBy(b => b.OrderByValue).Select(b => new {
                            b.ItemDay,
                            b.ModifyDate,
                            b.ModifyUser,
                            b.OrderByValue,
                            b.ProductID,
                            b.SightContent,
                            b.SightNotice,
                            b.Title

                        })
                    });
                    return JsonConvert.SerializeObject(ps);

                }
                else
                {
                    var ps = mitn.ToList().Select(a => new
                    {
                        ProductTIS = a.ITN_DY,
                        ProductTIA = HtmlHelper.StripHTML(a.ITN_NM),
                        //ProductTIB = 
                        //ProductTIC ="注意事項不在這個資料表",
                        ProductTIC = a.ITN_MEAL,
                        ProductTID = HtmlHelper.StripHTML(a.ITNBR_DR),
                        ProductTIE = HtmlHelper.StripHTML(a.ITNLU_DR),
                        ProductTIF = HtmlHelper.StripHTML(a.ITNDI_DR),
                        ProductTIG = HtmlHelper.StripHTML(a.ITN_HTL),
                        tblProductDaily = new
                        {
                            ItemDescript = a.ITN_DR,
                            
                        }
                        ,
                        tblProductDailySight = dm.tblProductDailySightCW.Where(b => b.ItemDay == a.ITN_DY && b.ProductID == MGRUP_CD).OrderBy(b => b.OrderByValue).Select(b => new {
                            b.ItemDay,
                            b.ModifyDate,
                            b.ModifyUser,
                            b.OrderByValue,
                            b.ProductID,
                            b.SightContent,
                            b.SightNotice,
                            b.Title

                        })
                    });

                    return JsonConvert.SerializeObject(ps);
                }
            }
            else {
                int ProductID = 0;
                if (int.TryParse(id, out ProductID))
                {
                    var ti = dm.tblProductTI.Where(a => a.ProductID == ProductID && a.ProductTIS <= a.tblProduct.ProductDD).Select(a => new
                    {
                        a.ProductTIS,
                        a.ProductTIA,
                        a.ProductTIB,
                        a.ProductTIC,
                        a.ProductTID,
                        a.ProductTIE,
                        a.ProductTIF,
                        a.ProductTIG,
                        tblProductDaily = a.tblProduct.tblProductDaily.Where(b => b.ItemDay == a.ProductTIS).Select(b => new
                        {
                            b.ProductID
                                          ,
                            b.ItemDay
                                          ,
                            b.StyleType
                                          ,
                            b.ItemDescript
                                          ,
                            b.ItemSight
                                          ,
                            b.Pic1
                                          ,
                            b.Title1
                                          ,
                            b.Tag1
                                          ,
                            b.Content1
                                          ,
                            b.WorldFlag1
                                          ,
                            b.IncludeFlag1
                                          ,
                            b.OnlyFlag1
                                          ,
                            b.Pic2
                                          ,
                            b.Title2
                                          ,
                            b.Tag2
                                          ,
                            b.Content2
                                          ,
                            b.WorldFlag2
                                          ,
                            b.IncludeFlag2
                                          ,
                            b.OnlyFlag2
                                          ,
                            b.Pic3
                                          ,
                            b.Title3
                                          ,
                            b.Tag3
                                          ,
                            b.Content3
                                          ,
                            b.WorldFlag3
                                          ,
                            b.IncludeFlag3
                                          ,
                            b.OnlyFlag3
                                          ,
                            b.Pic4
                                          ,
                            b.Title4
                                          ,
                            b.Tag4
                                          ,
                            b.Content4
                                          ,
                            b.WorldFlag4
                                          ,
                            b.IncludeFlag4
                                          ,
                            b.OnlyFlag4
                                          ,
                            b.Pic5
                                          ,
                            b.Title5
                                          ,
                            b.Tag5
                                          ,
                            b.Content5
                                          ,
                            b.WorldFlag5
                                          ,
                            b.IncludeFlag5
                                          ,
                            b.OnlyFlag5
                                          ,
                            b.AlertFlag
                                          ,
                            b.AlertTitle
                                          ,
                            b.AlertContent
                                          ,
                            b.NoteTitle
                                          ,
                            b.ModifyUser
                                          ,
                            b.ModifyDate
                                          ,
                            b.CheckSelect


                        }),
                        tblProductDailySight = dm.tblProductDailySightCW.Where(b => b.ItemDay == a.ProductTIS && b.ProductID == MGRUP_CD).OrderBy(b => b.OrderByValue)
                        .Select(b => new {
                            b.ItemDay,
                            b.ModifyDate,
                            b.ModifyUser,
                            b.OrderByValue,
                            b.ProductID,
                            b.SightContent,
                            b.SightNotice,
                            b.Title

                        })
                    });
                    return JsonConvert.SerializeObject(ti);
                }
                else {
                    return JsonConvert.SerializeObject(null);
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
