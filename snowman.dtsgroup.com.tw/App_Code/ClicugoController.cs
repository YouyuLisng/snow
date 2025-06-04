using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

public class ClicugoController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public string Get(string ItemType, string id)
    {
        string vURL = "";

        string ProductTourID = id;
        string Type = ItemType;
        switch (Type)
        {
            case "P":
            case "PDEBUG":
                vURL = "http://dtsgroup.clicugo.com/C/tour/webservice/tourPlanService-saveTourProduct";
                //vURL = "https://demo.clicugo.com/C/tour/webservice/tourPlanService-saveProductTest";
                //tourPlanService-saveTourProduct
                break;
            case "D":
            case "DDEBUG":
                vURL = "http://dtsgroup.clicugo.com/C/tour/webservice/turPackageService-saveTurPackage";
                //vURL = "https://demo.clicugo.com/C/tour/webservice/tourPlanService-saveTourProduct";
                ///C/tour/webservice/tourPlanService-saveTourProduct
                break;
        }


        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(vURL);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(vURL);
        //request.Timeout = 30000;
        request.Timeout = -1;
        request.Method = "POST";
        request.KeepAlive = false;
        request.ContentType = "application/json;";
        string sResponse = "";
        using (Stream webStream = request.GetRequestStream())
        using (StreamWriter requestWriter = new StreamWriter(webStream))
        {
            using (trdatagogojpEntities tdm = new trdatagogojpEntities())
            using (DataModel dm = new DataModel())
            {
                string[] NATN_CD = { "JP", "KR", "TW" };

                var dcity = tdm.TRCITY.Where(a => NATN_CD.Contains(a.NATN_CD)).Select(a => new { DicCountryCode = a.NATN_CD, DicCityCode = a.CITY_CD, DicCityNm = a.CITY_CNM }).ToList();
                Dictionary<string, string> dct = new Dictionary<string, string>();
                foreach (var aa in dcity)
                {
                    if (!dct.Keys.Contains(aa.DicCountryCode + aa.DicCityCode))
                    {
                        dct.Add(aa.DicCountryCode + aa.DicCityCode, aa.DicCityNm);
                    }
                }

                //JP0
                //大地區 排除台北營業處
                var TVL_AREA = tdm.TRWORD.Where(a => a.SYS_FG == "Y" && a.VALID_FG == "Y" && a.CLS_CD == "TVL_AREA" && a.DATA_VALUE != "DAREA00003").ToDictionary(a => a.DATA_VALUE, a => a.CHIN_WD);
                var CLS_CD = tdm.TRWORD.Where(a => a.SYS_FG == "Y" && a.VALID_FG == "Y" && TVL_AREA.ContainsKey(a.CLS_CD));
                //var CHIN_WD
                //機場 239 325 桃園國際機場
                var dairport = tdm.TRAIRP.Select(a => new { DicAirPortCode = a.AIRP_CD, DicAirPortNm = a.AIRP_CNM }).ToList();
                Dictionary<string, string> dap = new Dictionary<string, string>();
                foreach (var aa in dairport)
                {
                    if (!dap.Keys.Contains(aa.DicAirPortCode))
                    {
                        dap.Add(aa.DicAirPortCode, aa.DicAirPortNm);
                    }
                }

                //航空公司 QF 重複
                var dairline = tdm.TRCARR.Select(a => new { DicAirlineCode = a.CARR_CD, DicAirlineNm = a.CARR_ANM });
                Dictionary<string, string> dal = new Dictionary<string, string>();
                foreach (var aa in dairline)
                {
                    if (!dal.Keys.Contains(aa.DicAirlineCode))
                    {
                        dal.Add(aa.DicAirlineCode, aa.DicAirlineNm);
                    }
                }

                var serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = 1024000000,
                    RecursionLimit = 100
                };

                //排除local團





                //有上架 直客有價格
                //requestWriter.Write(serializer.Serialize(pl.ToList().First()));
                switch (Type)
                {
                    case "P":
                        #region 產編

                        var productCWBase = tdm.TRMGRUP.Where(a => a.VALID_FG != "0"
                                   && a.TVL_AREA != "SAREA00013"
                                   && a.TVL_AREA != "SAREA00015"
                                   && a.MGRUP_CD == ProductTourID
                                   )
                               .Select(a => new
                               {
                                   a.MGRUP_CD,
                                   a.PORT_CD,
                                   a.GRUP_NM,
                                   a.GRUP_NT,
                                   a.AIRLINE,
                                   a.OBJ_QT,
                                   a.GRUP_SNM,
                                   a.ITN_NATN,
                                   a.ITN_CITY,
                                   a.GRUP_LN,
                                   a.TVL_AREA,
                                   a.ITN_DR,
                                   a.VALID_FG,

                               }).ToList().GroupJoin(
                            tdm.TRMGBOOK
                            , p => p.MGRUP_CD, tbk => tbk.MGRUP_CD
                            , (a, b) => new
                            {
                                a.MGRUP_CD,
                                a.PORT_CD,
                                a.GRUP_NM,
                                a.GRUP_NT,
                                //a.AIRLINE,
                                a.AIRLINE,
                                a.OBJ_QT,
                                a.GRUP_SNM,
                                a.ITN_NATN,
                                a.ITN_CITY,
                                a.GRUP_LN,
                                a.TVL_AREA,
                                //a.ProductNmSuf,
                                a.ITN_DR,
                                a.VALID_FG,
                                transportPlan = b.Select(c => new
                                {
                                    day = c.GBOOK_DY,
                                    tripType = (c.GBOOK_DY == 1) ? "FORWARD" : "BACKWARD",
                                    //transportation = "AIR",
                                    //transportSchedult = c.FLT_NO,
                                    transportSchedule = new
                                    {
                                        //c.FLT_NO
                                        transType = "AIR",
                                        iataCode = c.FLT_NO.Substring(0, 2),
                                        flightCode = c.FLT_NO,
                                        takeOffTime = c.DEP_TM,
                                        arriveTime = c.ARR_TM,
                                        timeOfFlight = c.DEP_DT,
                                        dayChange = c.DEP_DT != c.ARR_DT ? "1" : "0",
                                        flightStart = c.ROUT_ID.Split('/')[0],
                                        middleStop1 = c.ROUT_ID.Split('/')[1],
                                    },
                                    //removed = false,
                                })

                            }
                            )
                            .GroupJoin(
                            tdm.TRMGITN.Select(a => new
                            {
                                a.MGRUP_CD,
                                a.ITN_DY,
                                a.ITN_NM,
                                a.ITN_DR,
                                a.ITNBR_DR,
                                a.ITNLU_DR,
                                a.ITNDI_DR,
                                a.ITN_HTL
                            })
                            , p => p.MGRUP_CD, tbk => tbk.MGRUP_CD
                            , (a, b) => new
                            {
                                a.MGRUP_CD,
                                a.PORT_CD,
                                a.GRUP_NM,
                                a.GRUP_NT,
                                a.AIRLINE,
                                a.OBJ_QT,
                                a.GRUP_SNM,
                                a.ITN_NATN,
                                a.ITN_CITY,
                                a.GRUP_LN,
                                a.TVL_AREA,
                                //a.ProductNmSuf,
                                a.ITN_DR,
                                a.VALID_FG,
                                a.transportPlan,
                                day = b.Where(c => c.ITN_DY < 950).OrderBy(c => c.ITN_DY).Select(c => new
                                {
                                    subject = c.ITN_NM,
                                    itinerary = c.ITN_DR,
                                    breakfast = c.ITNBR_DR,
                                    lunch = c.ITNLU_DR,
                                    dinner = c.ITNDI_DR,
                                    tourHotel = c.ITN_HTL
                                })
                                ,
                                priceInclude = b.Where(c => c.ITN_DY == 954).Select(c => c.ITN_DR).FirstOrDefault() ?? ""

                            })
                            ;

                        var jsonobj = productCWBase.ToList().Select(a => new
                        {
                            tourNumber = a.MGRUP_CD,
                            tourDay = a.GRUP_LN,
                            tourNight = a.GRUP_NT,
                            tourLength = "",
                            tourName = a.GRUP_NM,
                            tourType = "T",////目前只有團體
                            transType = "AIR",
                            departureCity = a.PORT_CD,
                            departureAirport = a.PORT_CD,
                            tourContinentName = "ASIA",
                            tourNationCode = a.ITN_NATN,
                            tourCity = a.ITN_CITY.Split('/')[0],
                            minAmount = a.OBJ_QT,
                            tourHightline = "",
                            tourDescription = a.ITN_DR,
                            tourImage = " ".Split(' ').Select(v => new
                            {
                                imagePath = "http://www.dtsgroup.com.tw/proimages/ProductID/700/" + a.MGRUP_CD + ".jpg",
                                remove = false,
                            }).Take(1),
                            //tourImage[].imagePath
                            //tourImage[].removed

                            onlineDate = DateTime.Now.ToString("yyyy/MM/dd"),
                            openToWeb = a.VALID_FG != "0",
                            offlineDate = DateTime.Now.AddYears(1).ToString("yyyy/MM/dd"),
                            online = true,
                            isDepositPayOnly = true,
                            deposit = 5000,

                            //pricing[]
                            //pricing[].roomType
                            //pricing[].priceOfAdult
                            //pricing[].priceOfKid
                            //pricing[].priceOfKidNB
                            //pricing[].priceOfInfant
                            //pricing[].priceOfOld
                            //pricing[].b2bpriceOfAdult
                            //pricing[].b2bpriceOfKid
                            //pricing[].b2bpriceOfKidNB
                            //pricing[].b2bpriceOfInfant
                            //pricing[].b2bpriceOfOld
                            //pricing[].removed

                            a.priceInclude,
                            priceDoesNotInclude = "",
                            priceDescriptionHighlight = "",
                            priceDescription = "",
                            //            tag[]	
                            //tag[].value	
                            //tag[].removed	刪除 true 新增 false
                            notice = "",
                            departureDateRemark = "",
                            day = a.day.Select(b => new
                            {
                                b.subject,
                                b.itinerary,
                                b.breakfast,
                                b.lunch,
                                b.dinner,
                                tourHotel = b.tourHotel.Split('0').Select(d => new { hotelName = d })
                                //b.tourHotel = c.ITN_HTL


                            }),
                            //day[]
                            //day[].subject
                            //day[].itinerary
                            //day[].breakfast
                            //day[].lunch
                            //day[].dinner
                            //day[].itineraryImage[].imagePath
                            //day[].itineraryImage[].removed
                            //day[].tourHotel[].hotelname
                            //day[].tourHotel[].removed
                            //day[].theSameLevel
                            a.transportPlan,
                            //transportPlan[]
                            //transportPlan[].day
                            //transportPlan[].tripType
                            //transportPlan[].tripsportation
                            //transportPlan[].tripsportSchedult
                            //transportPlan[].removed
                        }
                        )
                        ;
                        #endregion



                        requestWriter.Write(serializer.Serialize(jsonobj.ToList().First()));
                        break;
                    case "D":
                        #region 更新出發日期價格
                        #region 行程
                        var pslist = tdm.TRGRUP.Where(a =>
                            a.WEB_PD == "Y" &&
                            a.GRUP_TP == "2" &&
                            NATN_CD.Contains(a.ITN_NATN) &&
                            a.SALE_CHK1 == "1" && a.SALE_CHK2 == "1" && !a.PACK_FG && a.TKT_FG != "1"
                            && a.MGRUP_CD == ProductTourID
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
                                a.OBJ_QT,
                                a.ORDER_DL,
                                tbd = b.Select(c => new
                                {
                                    c.AGT_AM,
                                    c.BED_TP,
                                    c.CURR_AM,
                                    c.JOIN_TP
                                }),
                                a.WEB_PD,
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
                                a.WEB_PD,
                                a.OBJ_QT,
                                a.ORDER_DL,
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
                                a.WEB_PD,
                                a.OBJ_QT,
                                a.ORDER_DL,
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
                                a.WEB_PD,
                                a.OBJ_QT,
                                a.ORDER_DL,
                                TRGBOOK = b,
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
                            .GroupJoin(tdm.TRGOPITEM.Where(a => a.MUSTBUY == "1").Select(a => new { a.ATT_AM, a.GRUP_CD }),
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
                                    a.WEB_PD,
                                    a.OBJ_QT,
                                    a.ORDER_DL,
                                    a.TRGBOOK,
                                    //TRGOPITEM = 0,
                                    TRGOPITEM = b.Count() > 0 ? b.Sum(c => c.ATT_AM) : 0,
                                }
                                );
                        #endregion
                        var pl = pslist.GroupBy(a => a.MGRUP_CD).ToList().Select(b =>
                               new
                               {
                                   tourNumber = b.Key,
                                   //a.tourName ,
                                   turPackageProduct =
                                  b.Select(c => new
                                  {
                                      groupNumber = c.GRUP_CD,
                                      departureDate = c.LEAV_DT,
                                      groupName = c.GRUP_NM,
                                      openToWeb = c.WEB_PD == "Y",
                                      minAmount = c.OBJ_QT.ToString(),
                                      isDepositPayOnly = true,
                                      deposit = c.DORD_AM.Value.ToString(),
                                      remainingQuantity = (c.ESTM_YQT - c.KEEP_YQT - c.DONE_YQT - c.FOC1_YQT - c.FOC2_YQT - c.FOCG_YQT).ToString(),
                                      totalQuantity = c.ESTM_YQT.ToString(),
                                      offDate = c.ORDER_DL,
                                      pricing = " ".Split(' ').Select(d => new
                                      {
                                          roomType = "Double",
                                          priceOfAdult = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfKid = c.tbd.Where(f => f.BED_TP == "2").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfKidNB = c.tbd.Where(f => f.BED_TP == "3").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfInfant = c.tbd.Where(f => f.BED_TP == "5").OrderBy(f => f.JOIN_TP).First().CURR_AM,
                                          priceOfOld = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          removed = false,

                                      }).Take(1)
                                      ,
                                      b2bPricing = " ".Split(' ').Select(d => new
                                      {
                                          //roomType = "Double",
                                          priceOfAdult = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfKid = c.tbd.Where(f => f.BED_TP == "2").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfKidNB = c.tbd.Where(f => f.BED_TP == "3").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfInfant = c.tbd.Where(f => f.BED_TP == "5").OrderBy(f => f.JOIN_TP).First().AGT_AM,
                                          priceOfOld = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          removed = false,

                                      }).Take(1)
                                      ,
                                      transportPlan = c.TRGBOOK.Select(d => new
                                      {
                                          day = d.GBOOK_DY.ToString(),
                                          tripType = d.GBOOK_DY == 1 ? "FORWARD" : "BACKWARD",
                                          //trapsportation = "AIR",
                                          //transportCompany = d.FLT_NO.Substring(0, 2),
                                          //transportSchedult = d.FLT_NO,
                                          transportSchedule = new
                                          {
                                              //c.FLT_NO
                                              transType = "AIR",
                                              iataCode = d.FLT_NO.Substring(0, 2),
                                              flightCode = d.FLT_NO,
                                              takeOffTime = d.DEP_TM,
                                              arriveTime = d.ARR_TM,
                                              timeOfFlight = d.DEP_DT,
                                              dayChange = d.DEP_DT != d.ARR_DT ? "1" : "0",
                                              flightStart = d.ROUT_ID.Split('/')[0],
                                              middleStop1 = d.ROUT_ID.Split('/')[1],
                                          },
                                          //remove = false,
                                          removed = false,

                                      })
                                  }),




                               }
                            );
                        //var pl = jsonobj.Select(a=>new {
                        //    a.tourNumber,
                        //    //a.tourName,
                        //}).GroupJoin(pslist,
                        //    p => p.tourNumber,
                        //    ps => ps.MGRUP_CD,
                        //    (a, b) => new
                        //    {
                        //        a.tourNumber,
                        //        //a.tourName ,
                        //        turPackageProduct =
                        //        b.Select(c => new
                        //        {
                        //            groupNumber = c.GRUP_CD,
                        //            departureDate = c.LEAV_DT,
                        //            groupName = c.GRUP_NM,
                        //            openToWeb = c.WEB_PD == "Y",
                        //            minAmount = c.OBJ_QT.ToString(),
                        //            isDepositPayOnly = true,
                        //            deposit = c.DORD_AM.Value.ToString(),
                        //            remainingQuantity = (c.ESTM_YQT - c.KEEP_YQT - c.DONE_YQT - c.FOC1_YQT - c.FOC2_YQT - c.FOCG_YQT).ToString(),
                        //            totalQuantity = c.ESTM_YQT.ToString(),
                        //            offDate= c.ORDER_DL,
                        //            pricing = " ".Split(' ').Select(d => new
                        //            {
                        //                roomType = "Double",
                        //                priceOfAdult = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfKid = c.tbd.Where(f => f.BED_TP == "2").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfKidNB = c.tbd.Where(f => f.BED_TP == "3").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfInfant = c.tbd.Where(f => f.BED_TP == "5").OrderBy(f => f.JOIN_TP).First().CURR_AM,
                        //                priceOfOld = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                removed= false,

                        //            }).Take(1)
                        //            ,
                        //            b2bPricing = " ".Split(' ').Select(d => new
                        //            {
                        //                //roomType = "Double",
                        //                priceOfAdult = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfKid = c.tbd.Where(f => f.BED_TP == "2").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfKidNB = c.tbd.Where(f => f.BED_TP == "3").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfInfant = c.tbd.Where(f => f.BED_TP == "5").OrderBy(f => f.JOIN_TP).First().AGT_AM,
                        //                priceOfOld = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                removed = false,

                        //            }).Take(1)
                        //            ,
                        //            transportPlan = c.TRGBOOK.Select(d => new
                        //            {
                        //                day = d.GBOOK_DY.ToString(),
                        //                tripType = d.GBOOK_DY == 1 ? "FORWARD" : "BACKWARD",
                        //                trapsportation = "AIR",
                        //                transportCompany = d.FLT_NO.Substring(0, 2),
                        //                transportSchedult = d.FLT_NO,
                        //                //remove = false,
                        //                removed = false,

                        //            })
                        //        }),




                        //    }
                        //    );
                        #endregion
                        requestWriter.Write(serializer.Serialize(pl.ToList().First()));
                        break;
                    case "PDEBUG":
                        #region 產編

                        var productCWBases = tdm.TRMGRUP.Where(a => a.VALID_FG != "0"
                                   && a.TVL_AREA != "SAREA00013"
                                   && a.TVL_AREA != "SAREA00015"
                                   && a.MGRUP_CD == ProductTourID
                                   )
                               .Select(a => new
                               {
                                   a.MGRUP_CD,
                                   a.PORT_CD,
                                   a.GRUP_NM,
                                   a.GRUP_NT,
                                   a.AIRLINE,
                                   a.OBJ_QT,
                                   a.GRUP_SNM,
                                   a.ITN_NATN,
                                   a.ITN_CITY,
                                   a.GRUP_LN,
                                   a.TVL_AREA,
                                   a.ITN_DR,
                                   a.VALID_FG,

                               }).GroupJoin(
                            tdm.TRMGBOOK
                            , p => p.MGRUP_CD, tbk => tbk.MGRUP_CD
                            , (a, b) => new
                            {
                                a.MGRUP_CD,
                                a.PORT_CD,
                                a.GRUP_NM,
                                a.GRUP_NT,
                                //a.AIRLINE,
                                a.AIRLINE,
                                a.OBJ_QT,
                                a.GRUP_SNM,
                                a.ITN_NATN,
                                a.ITN_CITY,
                                a.GRUP_LN,
                                a.TVL_AREA,
                                //a.ProductNmSuf,
                                a.ITN_DR,
                                a.VALID_FG,
                                transportPlan = b.Select(c => new
                                {
                                    day = c.GBOOK_DY,
                                    tripType = (c.GBOOK_DY == 1) ? "FORWARD" : "BACKWARD",
                                    transportation = "AIR",
                                    transportSchedult = c.FLT_NO,
                                    //removed = false,
                                })

                            }
                            )
                            .GroupJoin(
                            tdm.TRMGITN.Select(a => new
                            {
                                a.MGRUP_CD,
                                a.ITN_DY,
                                a.ITN_NM,
                                a.ITN_DR,
                                a.ITNBR_DR,
                                a.ITNLU_DR,
                                a.ITNDI_DR,
                                a.ITN_HTL
                            })
                            , p => p.MGRUP_CD, tbk => tbk.MGRUP_CD
                            , (a, b) => new
                            {
                                a.MGRUP_CD,
                                a.PORT_CD,
                                a.GRUP_NM,
                                a.GRUP_NT,
                                a.AIRLINE,
                                a.OBJ_QT,
                                a.GRUP_SNM,
                                a.ITN_NATN,
                                a.ITN_CITY,
                                a.GRUP_LN,
                                a.TVL_AREA,
                                //a.ProductNmSuf,
                                a.ITN_DR,
                                a.VALID_FG,
                                a.transportPlan,
                                day = b.Where(c => c.ITN_DY < 950).OrderBy(c => c.ITN_DY).Select(c => new
                                {
                                    subject = c.ITN_NM,
                                    itinerary = c.ITN_DR,
                                    breakfast = c.ITNBR_DR,
                                    lunch = c.ITNLU_DR,
                                    dinner = c.ITNDI_DR,
                                    tourHotel = c.ITN_HTL
                                })
                                ,
                                priceInclude = b.Where(c => c.ITN_DY == 954).Select(c => c.ITN_DR).FirstOrDefault() ?? ""

                            })
                            ;

                        var jsonobjs = productCWBases.ToList().Select(a => new
                        {
                            tourNumber = a.MGRUP_CD,
                            tourDay = a.GRUP_LN,
                            tourNight = a.GRUP_NT,
                            tourLength = "",
                            tourName = a.GRUP_NM,
                            tourType = "T",////目前只有團體
                            transType = "AIR",
                            departureCity = a.PORT_CD,
                            departureAirport = a.PORT_CD,
                            tourContinentName = "ASIA",
                            tourNationCode = a.ITN_NATN,
                            tourCity = a.ITN_CITY.Split('/')[0],
                            minAmount = a.OBJ_QT,
                            tourHightline = "",
                            tourDescription = a.ITN_DR,
                            tourImage = " ".Split(' ').Select(v => new
                            {
                                imagePath = "http://www.dtsgroup.com.tw/proimages/ProductID/700/" + a.MGRUP_CD + ".jpg",
                                remove = false,
                            }).Take(1),
                            //tourImage[].imagePath
                            //tourImage[].removed

                            onlineDate = DateTime.Now.ToString("yyyy/MM/dd"),
                            openToWeb = a.VALID_FG != "0",
                            offlineDate = DateTime.Now.AddYears(1).ToString("yyyy/MM/dd"),
                            online = true,
                            isDepositPayOnly = true,
                            deposit = 5000,

                            //pricing[]
                            //pricing[].roomType
                            //pricing[].priceOfAdult
                            //pricing[].priceOfKid
                            //pricing[].priceOfKidNB
                            //pricing[].priceOfInfant
                            //pricing[].priceOfOld
                            //pricing[].b2bpriceOfAdult
                            //pricing[].b2bpriceOfKid
                            //pricing[].b2bpriceOfKidNB
                            //pricing[].b2bpriceOfInfant
                            //pricing[].b2bpriceOfOld
                            //pricing[].removed

                            a.priceInclude,
                            priceDoesNotInclude = "",
                            priceDescriptionHighlight = "",
                            priceDescription = "",
                            //            tag[]	
                            //tag[].value	
                            //tag[].removed	刪除 true 新增 false
                            notice = "",
                            departureDateRemark = "",
                            day = a.day.Select(b => new
                            {
                                b.subject,
                                b.itinerary,
                                b.breakfast,
                                b.lunch,
                                b.dinner,
                                tourHotel = b.tourHotel.Split('0').Select(d => new { hotelName = d })
                                //b.tourHotel = c.ITN_HTL


                            }),
                            //day[]
                            //day[].subject
                            //day[].itinerary
                            //day[].breakfast
                            //day[].lunch
                            //day[].dinner
                            //day[].itineraryImage[].imagePath
                            //day[].itineraryImage[].removed
                            //day[].tourHotel[].hotelname
                            //day[].tourHotel[].removed
                            //day[].theSameLevel
                            a.transportPlan,
                            //transportPlan[]
                            //transportPlan[].day
                            //transportPlan[].tripType
                            //transportPlan[].tripsportation
                            //transportPlan[].tripsportSchedult
                            //transportPlan[].removed
                        }
                        )
                        ;
                        #endregion


                        sResponse = serializer.Serialize(jsonobjs.ToList().First());
                        break;
                    case "DDEBUG":
                        #region 行程
                        var pslists = tdm.TRGRUP.Where(a =>
                            a.WEB_PD == "Y" &&
                            a.GRUP_TP == "2" &&
                            NATN_CD.Contains(a.ITN_NATN) &&
                            a.SALE_CHK1 == "1" && a.SALE_CHK2 == "1" && !a.PACK_FG && a.TKT_FG != "1"
                            && a.MGRUP_CD == ProductTourID
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
                                a.OBJ_QT,
                                a.ORDER_DL,
                                tbd = b.Select(c => new
                                {
                                    c.AGT_AM,
                                    c.BED_TP,
                                    c.CURR_AM,
                                    c.JOIN_TP
                                }),
                                a.WEB_PD,
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
                                a.WEB_PD,
                                a.OBJ_QT,
                                a.ORDER_DL,
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
                                a.WEB_PD,
                                a.OBJ_QT,
                                a.ORDER_DL,
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
                                a.WEB_PD,
                                a.OBJ_QT,
                                a.ORDER_DL,
                                TRGBOOK = b,
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
                            .GroupJoin(tdm.TRGOPITEM.Where(a => a.MUSTBUY == "1").Select(a => new { a.ATT_AM, a.GRUP_CD }),
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
                                    a.WEB_PD,
                                    a.OBJ_QT,
                                    a.ORDER_DL,
                                    a.TRGBOOK,
                                    //TRGOPITEM = 0,
                                    TRGOPITEM = b.Count() > 0 ? b.Sum(c => c.ATT_AM) : 0,
                                }
                                );
                        #endregion
                        #region 更新出發日期價格
                        var pls = pslists.GroupBy(a => a.MGRUP_CD).ToList().Select(b =>
                               new
                               {
                                   tourNumber = b.Key,
                                   //a.tourName ,
                                   turPackageProduct =
                                  b.Select(c => new
                                  {
                                      groupNumber = c.GRUP_CD,
                                      departureDate = c.LEAV_DT,
                                      groupName = c.GRUP_NM,
                                      openToWeb = c.WEB_PD == "Y",
                                      minAmount = c.OBJ_QT.ToString(),
                                      isDepositPayOnly = true,
                                      deposit = c.DORD_AM.Value.ToString(),
                                      remainingQuantity = (c.ESTM_YQT - c.KEEP_YQT - c.DONE_YQT - c.FOC1_YQT - c.FOC2_YQT - c.FOCG_YQT).ToString(),
                                      totalQuantity = c.ESTM_YQT.ToString(),
                                      offDate = c.ORDER_DL,
                                      pricing = " ".Split(' ').Select(d => new
                                      {
                                          roomType = "Double",
                                          priceOfAdult = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfKid = c.tbd.Where(f => f.BED_TP == "2").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfKidNB = c.tbd.Where(f => f.BED_TP == "3").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfInfant = c.tbd.Where(f => f.BED_TP == "5").OrderBy(f => f.JOIN_TP).First().CURR_AM,
                                          priceOfOld = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          removed = false,

                                      }).Take(1)
                                      ,
                                      b2bPricing = " ".Split(' ').Select(d => new
                                      {
                                          //roomType = "Double",
                                          priceOfAdult = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfKid = c.tbd.Where(f => f.BED_TP == "2").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfKidNB = c.tbd.Where(f => f.BED_TP == "3").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          priceOfInfant = c.tbd.Where(f => f.BED_TP == "5").OrderBy(f => f.JOIN_TP).First().AGT_AM,
                                          priceOfOld = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                                          removed = false,

                                      }).Take(1)
                                      ,
                                      transportPlan = c.TRGBOOK.Select(d => new
                                      {
                                          day = d.GBOOK_DY.ToString(),
                                          tripType = d.GBOOK_DY == 1 ? "FORWARD" : "BACKWARD",
                                          trapsportation = "AIR",
                                          transportCompany = d.FLT_NO.Substring(0, 2),
                                          transportSchedult = d.FLT_NO,
                                          //remove = false,
                                          removed = false,

                                      })
                                  }),




                               }
                            );
                        //var pl = jsonobj.Select(a=>new {
                        //    a.tourNumber,
                        //    //a.tourName,
                        //}).GroupJoin(pslist,
                        //    p => p.tourNumber,
                        //    ps => ps.MGRUP_CD,
                        //    (a, b) => new
                        //    {
                        //        a.tourNumber,
                        //        //a.tourName ,
                        //        turPackageProduct =
                        //        b.Select(c => new
                        //        {
                        //            groupNumber = c.GRUP_CD,
                        //            departureDate = c.LEAV_DT,
                        //            groupName = c.GRUP_NM,
                        //            openToWeb = c.WEB_PD == "Y",
                        //            minAmount = c.OBJ_QT.ToString(),
                        //            isDepositPayOnly = true,
                        //            deposit = c.DORD_AM.Value.ToString(),
                        //            remainingQuantity = (c.ESTM_YQT - c.KEEP_YQT - c.DONE_YQT - c.FOC1_YQT - c.FOC2_YQT - c.FOCG_YQT).ToString(),
                        //            totalQuantity = c.ESTM_YQT.ToString(),
                        //            offDate= c.ORDER_DL,
                        //            pricing = " ".Split(' ').Select(d => new
                        //            {
                        //                roomType = "Double",
                        //                priceOfAdult = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfKid = c.tbd.Where(f => f.BED_TP == "2").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfKidNB = c.tbd.Where(f => f.BED_TP == "3").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfInfant = c.tbd.Where(f => f.BED_TP == "5").OrderBy(f => f.JOIN_TP).First().CURR_AM,
                        //                priceOfOld = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().CURR_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                removed= false,

                        //            }).Take(1)
                        //            ,
                        //            b2bPricing = " ".Split(' ').Select(d => new
                        //            {
                        //                //roomType = "Double",
                        //                priceOfAdult = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfKid = c.tbd.Where(f => f.BED_TP == "2").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfKidNB = c.tbd.Where(f => f.BED_TP == "3").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                priceOfInfant = c.tbd.Where(f => f.BED_TP == "5").OrderBy(f => f.JOIN_TP).First().AGT_AM,
                        //                priceOfOld = c.tbd.Where(f => f.BED_TP == "1").OrderBy(f => f.JOIN_TP).First().AGT_AM + (c.TRGOPITEM.HasValue ? c.TRGOPITEM.Value : 0),
                        //                removed = false,

                        //            }).Take(1)
                        //            ,
                        //            transportPlan = c.TRGBOOK.Select(d => new
                        //            {
                        //                day = d.GBOOK_DY.ToString(),
                        //                tripType = d.GBOOK_DY == 1 ? "FORWARD" : "BACKWARD",
                        //                trapsportation = "AIR",
                        //                transportCompany = d.FLT_NO.Substring(0, 2),
                        //                transportSchedult = d.FLT_NO,
                        //                //remove = false,
                        //                removed = false,

                        //            })
                        //        }),




                        //    }
                        //    );
                        #endregion
                        sResponse = serializer.Serialize(pls.ToList().First());
                        break;
                }
            }

        }

        switch (Type)
        {
            case "P":
            case "D":
                using (var httpResponse = (HttpWebResponse)request.GetResponse())
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    sResponse = streamReader.ReadToEnd();

                }
                break;
        }

        return sResponse;
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
