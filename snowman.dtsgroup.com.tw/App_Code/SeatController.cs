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

public class SeatController : ApiController
{
    //// GET api/<controller>
    public List<seatitem> Get()
    {
        var serializer = new JavaScriptSerializer
        {
            MaxJsonLength = 1024000000,
            RecursionLimit = 100
        };

        //serializer.RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter() });
        //string GRUP_CD = ItemType;

        using (trdatagogojpEntities tdm = new trdatagogojpEntities())
        {
            var trg = tdm.TRGRUP.Select(a => new
            {
                //a.qty
                date = a.LEAV_DT,
                pfProdNo = a.GRUP_CD,
                totQty = a.ESTM_YQT,
                allotQty = (a.GRUP_TP == "2" || a.GRUP_TP == "5" || a.GRUP_TP == "6") ? a.ESTM_YQT - (a.DONE_YQT + a.FOC1_YQT + a.FOC2_YQT + a.KEEP_YQT) : -1,
                ordQty = a.DONE_YQT + a.FOC1_YQT + a.FOC2_YQT + a.KEEP_YQT,
                //reqQty = 0,
            }).GroupJoin(tdm.TRREC.Where(a => a.GRUP_ST == "4"), a => a.pfProdNo, b => b.GRUP_CD, (a, b) => new
            {
                a.date,
                a.pfProdNo,
                a.totQty,
                a.allotQty,
                a.ordQty,
                reqQtys = b.Select(c => c.FQT),
            })

            .ToList().Where(a => Convert.ToDateTime(a.date) >= DateTime.Now).Select(a => new seatitem()
            {

                pfProdNo = a.pfProdNo,
                totQty = a.totQty,
                allotQty = a.allotQty,
                ordQty = a.ordQty,
                reqQty = a.reqQtys.Sum(b => b.GetValueOrDefault(0)),
            }
                ).Select(a => new seatitem()
                {

                    pfProdNo = a.pfProdNo,
                    totQty = a.totQty,
                    allotQty = (a.allotQty - a.reqQty > 0) ? (a.allotQty - a.reqQty) : -1,
                    ordQty = a.ordQty,
                    reqQty = a.reqQty
                })

            ;

            return trg.ToList();
            //if (trg != null)
            //{

            //    return trg.GRUP_TP == "2" ? (trg.ESTM_YQT - trg.KEEP_YQT - trg.DONE_YQT - trg.FOC1_YQT - trg.FOC2_YQT - trg.FOCG_YQT) : 0;
            //}
            //else
            //{
            //    return 0;
            //}

            //TRGRUP trg = tdm.TRGRUP.Where(a => a.GRUP_CD == ItemType).FirstOrDefault();
            //if (trg != null)
            //{

            //    return trg.GRUP_TP == "2" ? (trg.ESTM_YQT - trg.KEEP_YQT - trg.DONE_YQT - trg.FOC1_YQT - trg.FOC2_YQT - trg.FOCG_YQT) : 0;
            //} else {
            //    return  0;
            //}
        }
    }

    // GET api/<controller>/ItemType
    public List<seatitem> Get(string ItemType)
    {
        var serializer = new JavaScriptSerializer
        {
            MaxJsonLength = 1024000000,
            RecursionLimit = 100
        };

        //serializer.RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter() });
        string MGRUP_CD = ItemType;

        using (trdatagogojpEntities tdm = new trdatagogojpEntities())
        {
            var trg = tdm.TRGRUP.Where(a=>a.MGRUP_CD== MGRUP_CD).Select(a => new {
                //a.qty
                date = a.LEAV_DT,
                pfProdNo = a.GRUP_CD,
                totQty = a.ESTM_YQT,
                allotQty = (a.GRUP_TP == "2" || a.GRUP_TP == "5" || a.GRUP_TP == "6") ? (a.ESTM_YQT - (a.DONE_YQT + a.FOC1_YQT + a.FOC2_YQT + a.KEEP_YQT)) : -1,
                ordQty = a.DONE_YQT + a.FOC1_YQT + a.FOC2_YQT + a.KEEP_YQT,
                //reqQty = 0,
            }).GroupJoin(tdm.TRREC.Where(a => a.GRUP_ST == "4"), a => a.pfProdNo, b => b.GRUP_CD, (a, b) => new {
                a.date,
                a.pfProdNo,
                a.totQty,
                a.allotQty,
                a.ordQty,
                reqQty = b.Select(c => c.FQT),
            }).ToList().Where(a => Convert.ToDateTime(a.date) >= DateTime.Now).Select(a =>new seatitem(){

                pfProdNo = a.pfProdNo,
                totQty =a.totQty,
                allotQty =a.allotQty,
                ordQty =a.ordQty,
                reqQty =a.reqQty.Sum(b => b.GetValueOrDefault(0)),
            }

                ).Select(a => new seatitem()
                {

                    pfProdNo = a.pfProdNo,
                    totQty = a.totQty,
                    allotQty = (a.allotQty - a.reqQty > 0) ? (a.allotQty - a.reqQty) : -1,
                    ordQty = a.ordQty,
                    reqQty = a.reqQty
                })


            ;

            return trg.ToList();
            //if (trg != null)
            //{

            //    return trg.GRUP_TP == "2" ? (trg.ESTM_YQT - trg.KEEP_YQT - trg.DONE_YQT - trg.FOC1_YQT - trg.FOC2_YQT - trg.FOCG_YQT) : 0;
            //}
            //else
            //{
            //    return 0;
            //}

            //TRGRUP trg = tdm.TRGRUP.Where(a => a.GRUP_CD == ItemType).FirstOrDefault();
            //if (trg != null)
            //{

            //    return trg.GRUP_TP == "2" ? (trg.ESTM_YQT - trg.KEEP_YQT - trg.DONE_YQT - trg.FOC1_YQT - trg.FOC2_YQT - trg.FOCG_YQT) : 0;
            //} else {
            //    return  0;
            //}
        }
    }

    public class seatitem
    {
        public string pfProdNo { get; set; }
        public short? totQty { get; set; }
        public int? allotQty { get; set; }
        public int? ordQty { get; set; }
        public int? reqQty { get; set; }
    }


    public object Get(string ItemType, string id)
    {
        return 0;

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
