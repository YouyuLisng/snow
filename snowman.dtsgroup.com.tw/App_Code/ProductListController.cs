using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

public class ProductListController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public IEnumerable<string> Get(string ItemType)
    {

        using (trdatagogojpEntities tdm = new trdatagogojpEntities())
        using (DataModel dm = new DataModel())
        {

            var dairport = tdm.TRAIRP.Select(a => new { DicAirPortCode = a.AIRP_CD, DicAirPortNm = a.AIRP_CNM }).ToList();
            Dictionary<string, string> dap = new Dictionary<string, string>();
            foreach (var aa in dairport)
            {
                if (!dap.Keys.Contains(aa.DicAirPortCode))
                {
                    dap.Add(aa.DicAirPortCode, aa.DicAirPortNm);
                }
            }
            string[] NATN_CD = { "JP", "TW" };

            var pslist = tdm.TRGRUP.Where(a =>
        a.WEB_PD == "Y" &&
        a.GRUP_TP == "2" &&
        NATN_CD.Contains(a.ITN_NATN) &&
        a.SALE_CHK1 == "1" && a.SALE_CHK2 == "1" && !a.PACK_FG && a.TKT_FG != "1"
        ).GroupJoin(tdm.TRSUBD.Where(b => b.AGT_AM <= b.CURR_AM),
        a => a.GRUP_CD,
        b => b.GRUP_CD,
        (a, b) => new
        {
            a.GRUP_TP,
            a.GRUP_CD,
            a.MGRUP_CD,
            a.LEAV_DT,
            a.GRUP_NM,
            a.GRUP_LN,
            a.GRUP_NT,
            a.ESTM_YQT,
            a.KEEP_YQT,
            a.FOC1_YQT,
            a.FOC2_YQT,
            a.FOCG_YQT,
            a.DONE_YQT,
            a.DORD_AM,
            a.GRUP_RK,
            tbd = b.Select(c => new
            {
                c.AGT_AM,
                c.BED_TP,
                c.CURR_AM,
                c.JOIN_TP
            }),
        }
        ).Join(tdm.TRMGRUP.Where(a => a.VALID_FG != "N" && a.PORT_CD == "TPE"
        && a.TVL_AREA != "SAREA00013"
        && a.TVL_AREA != "SAREA00015"
        ),
        a => a.MGRUP_CD,
        b => b.MGRUP_CD,
        (a, b) => new
        {
            a.GRUP_TP,
            a.GRUP_CD,
            a.MGRUP_CD,
            a.LEAV_DT,
            a.GRUP_NM,
            a.GRUP_LN,
            a.GRUP_NT,
            a.ESTM_YQT,
            a.KEEP_YQT,
            a.FOC1_YQT,
            a.FOC2_YQT,
            a.FOCG_YQT,
            a.DONE_YQT,
            a.DORD_AM,
            a.GRUP_RK,
            a.tbd,
            b.PORT_CD,
            b.ITN_DR,
        }
        ).GroupJoin(
            tdm.TRMGITN,
         a => a.MGRUP_CD,
        b => b.MGRUP_CD,
        (a, b) => new
        {
            a.GRUP_TP,
            a.GRUP_CD,
            a.MGRUP_CD,
            a.LEAV_DT,
            a.GRUP_NM,
            a.GRUP_LN,
            a.GRUP_NT,
            a.ESTM_YQT,
            a.KEEP_YQT,
            a.FOC1_YQT,
            a.FOC2_YQT,
            a.FOCG_YQT,
            a.DONE_YQT,
            a.DORD_AM,
            a.GRUP_RK,
            a.tbd,
            a.PORT_CD,
            a.ITN_DR,
            TRMGITN = b.Select(
                c => new
                {
                    c.ITN_DY,
                    c.ITN_NM,
                    c.ITN_DR,
                    c.ITNBR_DR,
                    c.ITN_LU,
                    c.ITN_DI,
                    c.ITN_HTL,
                    c.ITNLU_DR,
                    c.ITNDI_DR,
                }
                ).Where(c => c.ITN_DY <= a.GRUP_LN),
        }
        ).Where(a => a.TRMGITN.Count() > 0)
        .GroupJoin(
            tdm.TRGBOOK,
         a => a.GRUP_CD,
        b => b.GRUP_CD,
        (a, b) => new
        {
            a.GRUP_TP,
            a.GRUP_CD,
            a.MGRUP_CD,
            a.LEAV_DT,
            a.GRUP_NM,
            a.GRUP_LN,
            a.GRUP_NT,
            a.ESTM_YQT,
            a.KEEP_YQT,
            a.FOC1_YQT,
            a.FOC2_YQT,
            a.FOCG_YQT,
            a.DONE_YQT,
            a.DORD_AM,
            a.GRUP_RK,
            a.tbd,
            a.PORT_CD,
            a.ITN_DR,
            a.TRMGITN,
            tbkgo = b.Where(c => c.GBOOK_DY == 1).Select(c => new
            {
                c.ARR_DT,
                c.ARR_TM,
                c.BOOK_SQ,
                c.GBOOK_DY,
                c.GRUP_CD,
                c.ROUT_ID,
                c.DEP_DT,
                c.DEP_TM,
                c.BOOK_QT,
                c.FLT_NO,
                c.UPD_DTM,
            }),
            tbkba = b.Where(c => c.GBOOK_DY > 1).Select(c => new
            {
                c.ARR_DT,
                c.ARR_TM,
                c.BOOK_SQ,
                c.GBOOK_DY,
                c.GRUP_CD,
                c.ROUT_ID,
                c.DEP_DT,
                c.DEP_TM,
                c.BOOK_QT,
                c.FLT_NO,
                c.UPD_DTM,

            }),
        }
        )
        .GroupJoin(
            tdm.TRGOPITEM.Where(a => a.MUSTBUY == "1").Select(a => new { a.ATT_AM, a.GRUP_CD }),
         a => a.GRUP_CD,
        b => b.GRUP_CD,
        (a, b) => new
        {
            a.GRUP_TP,
            a.GRUP_CD,
            a.MGRUP_CD,
            a.LEAV_DT,
            a.GRUP_NM,
            a.GRUP_LN,
            a.GRUP_NT,
            a.ESTM_YQT,
            a.KEEP_YQT,
            a.FOC1_YQT,
            a.FOC2_YQT,
            a.FOCG_YQT,
            a.DONE_YQT,
            a.DORD_AM,
            a.GRUP_RK,
            a.tbd,
            a.PORT_CD,
            a.ITN_DR,
            a.TRMGITN,
            a.tbkgo,
            a.tbkba,
            //TRGOPITEM = 0,
            TRGOPITEM = b.Count() > 0 ? b.Sum(c => c.ATT_AM) : 0,
        }
        )
        .Select(a => a.MGRUP_CD).Distinct().ToList()
        ;






            return pslist;
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
