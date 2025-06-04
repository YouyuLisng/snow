using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Runtime.Caching;
/// <summary>
/// 2019-11-19 [CK] 將取得科威團體列表的功能 建構成一個大類別
/// </summary>
public class CowellAPI
{
    public string vleavDt1 = "2010/01/01";
    public string vleavDt2 = "2099/12/31";
    public string vregmCd = "";
    public string vregsCd = "";
    public string vportCd = "";
    public string vorderCd = "";
    public string vGrupCd = "";
    public string vClear = "";

    public string GetListJson()
    {
        #region 準備網址
        // string vURL = "http://erp.gogojp.com.tw:8000/WMnet/API/V5/GetProdList.ashx";
        string vURL = "http://ssl.dtsgroup.com.tw/WMnet/API/V5/GetProdList.ashx";
        //&leavDt1=2019/10/30
        //&leavDt2=2020/10/30
        //&amrnk=1
        //&refAmrnk=1
        //&pageShow=999
        //&pageAll=1
        //&pageGO=1
        //&pagePGO=1
        //&srcCls=D
        //&allowjoin=true
        //&allowwait=true
        //&displayType=M&
        //&regmCd=0003
        //&regsCd=0003-0001
        //&orderCd=4

        string urlParameters = "?IWEB_ID=dtsgroup&amrnk=1&refAmrnk=1&orderCd=4&pageShow=999&pageAll=1&pageGO=1&pagePGO=1&srcCls=D&allowjoin=true&allowwait=true&displayType=M";
        string jsonStr = @"{""SiteTitle"": ""大榮旅遊B2B同業網"", ""All"": [], ""Go"": [], ""Pgo"": [], ""SearchCondition"": """", ""AmrnkNm"": """", ""MetaInfos"": [], ""Verify"": false, ""ErrMsg"": {0}}";
        urlParameters += "&leavDt1=" + vleavDt1 + "&leavDt2=" + vleavDt2 + "&regmCd=" + vregmCd + "&regsCd=" + vregsCd + "&portCd=" + vportCd + "" + "&vorderCd=" + vorderCd + "";

        #endregion
        #region 準備cache資料
        ObjectCache cache = MemoryCache.Default;
        bool vBoolCacheClear = false;
        var vPolicy = new CacheItemPolicy();//設定回收時間
                                            //多久時間清除快取項目
                                            //policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(15.0);
                                            //policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15.0);
        vPolicy.SlidingExpiration = TimeSpan.FromMinutes(5);
        string vCacheName = "ListJson";
        vCacheName += vleavDt1 + "_";
        vCacheName += vleavDt2 + "_";
        vCacheName += vregmCd + "_";
        vCacheName += vregsCd + "_";
        vCacheName += vportCd + "_";
        vCacheName += vorderCd + "_";
        vCacheName += vGrupCd + "_";
        string vCache = cache[vCacheName] as string;
        if (vClear == "Y")
        {
            vBoolCacheClear = true;
        }
        #endregion
        #region 取得資料
        if (vCache == null || vBoolCacheClear)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(vURL + urlParameters) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 30000;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            jsonStr = sr.ReadToEnd();
                            cache.Set(vCacheName, jsonStr, vPolicy);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    jsonStr += String.Format(jsonStr, reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                jsonStr += String.Format(jsonStr, "");
            }
        }
        else
        {
            jsonStr = vCache;
        }
        //jsonStr = @"{""SiteTitle"":""大榮旅遊B2B同業網"",""All"":[{""MgrupSnm"":""動感韓國～五星乙晚、購物兩站、冰雪樂園、HEYRI藝術村四日 \u003c不上攝影、無自理餐、百年土種人蔘雞+荒謬的五花肉吃到飽\u003e"",""LeavDtAll"":[{""LeavDt"":""2019/11/13(三)"",""GrupCd"":""DTS19B13ICKD"",""GrupCnt"":2},{""LeavDt"":""2019/11/18(一)"",""GrupCd"":""DTS19B18ICKD"",""GrupCnt"":1},{""LeavDt"":""2019/11/20(三)"",""GrupCd"":""DTS19B20ICKD"",""GrupCnt"":1},{""LeavDt"":""2019/11/25(一)"",""GrupCd"":""DTS19B25ICKD"",""GrupCnt"":1},{""LeavDt"":""2019/11/27(三)"",""GrupCd"":""DTS19B27ICKD"",""GrupCnt"":1}],""RecCnt"":224,""GoCnt"":216,""PgoCnt"":8,""RowId"":1,""SacctNo"":"""",""MgrupCd"":""64712"",""SaleAm"":5900,""AgtAm"":5900,""GrupLn"":4,""SubCd"":""GO"",""SubCdAnm"":""團"",""SortSq"":7,""Url"":""/EW/GO/MGroupDetail.asp?prodCd=64712"",""ShareUrl"":""http://erp.gogojp.com.tw/EW/GO/MGroupDetail.asp?prodCd=64712"",""SrcCls"":0,""ImgUrl"":""/eWeb_gogojp/IMGDB/000019/000502/00011826.jpg""},{""MgrupSnm"":""網友推到爆～超值沖繩四日自由行(稅外)"",""LeavDtAll"":[{""LeavDt"":""2019/11/06(三)"",""GrupCd"":""DTS19B06B4IT"",""GrupCnt"":1},{""LeavDt"":""2019/11/11(一)"",""GrupCd"":""DTS19B11B4IT"",""GrupCnt"":1},{""LeavDt"":""2019/11/14(四)"",""GrupCd"":""DTS19B14C4IT"",""GrupCnt"":1},{""LeavDt"":""2019/11/18(一)"",""GrupCd"":""DTS19B18B4IT"",""GrupCnt"":1},{""LeavDt"":""2019/11/20(三)"",""GrupCd"":""DTS19B20B4IT"",""GrupCnt"":1}],""RecCnt"":224,""GoCnt"":216,""PgoCnt"":8,""RowId"":2,""SacctNo"":"""",""MgrupCd"":""DTS18-56580"",""SaleAm"":6900,""AgtAm"":6900,""GrupLn"":4,""SubCd"":""PGO"",""SubCdAnm"":""自"",""SortSq"":7,""Url"":""/eweb/PG/V_P_1.asp?PACKAGE_CD=DTS18-56580\u0026SUB_CD=PGO"",""ShareUrl"":""http://erp.gogojp.com.tw/eweb/PG/V_P_1.asp?PACKAGE_CD=DTS18-56580\u0026SUB_CD=PGO"",""SrcCls"":0,""ImgUrl"":""/eWeb_gogojp/IMGDB/000006/000310/00003864.jpg""}],""Go"":[{""MgrupSnm"":""動感韓國～五星乙晚、購物兩站、冰雪樂園、HEYRI藝術村四日 \u003c不上攝影、無自理餐、百年土種人蔘雞+荒謬的五花肉吃到飽\u003e"",""LeavDtAll"":[{""LeavDt"":""2019/11/13(三)"",""GrupCd"":""DTS19B13ICKD"",""GrupCnt"":2},{""LeavDt"":""2019/11/18(一)"",""GrupCd"":""DTS19B18ICKD"",""GrupCnt"":1},{""LeavDt"":""2019/11/20(三)"",""GrupCd"":""DTS19B20ICKD"",""GrupCnt"":1},{""LeavDt"":""2019/11/25(一)"",""GrupCd"":""DTS19B25ICKD"",""GrupCnt"":1},{""LeavDt"":""2019/11/27(三)"",""GrupCd"":""DTS19B27ICKD"",""GrupCnt"":1}],""RecCnt"":216,""GoCnt"":216,""PgoCnt"":0,""RowId"":1,""SacctNo"":"""",""MgrupCd"":""64712"",""SaleAm"":5900,""AgtAm"":5900,""GrupLn"":4,""SubCd"":""GO"",""SubCdAnm"":""團"",""SortSq"":7,""Url"":""/EW/GO/MGroupDetail.asp?prodCd=64712"",""ShareUrl"":""http://erp.gogojp.com.tw/EW/GO/MGroupDetail.asp?prodCd=64712"",""SrcCls"":0,""ImgUrl"":""/eWeb_gogojp/IMGDB/000019/000502/00011826.jpg""},{""MgrupSnm"":""閃亮韓國～樂天世界、兩水頭、懷舊車站、宇宙星光村、塗鴉秀五日 \u003c不上攝影、不走土產店、市區住一晚、善良的豬烤肉吃到飽\u003e"",""LeavDtAll"":[{""LeavDt"":""2019/11/14(四)"",""GrupCd"":""DTS19B14ICKE"",""GrupCnt"":1},{""LeavDt"":""2019/11/16(六)"",""GrupCd"":""DTS19B16ICKE"",""GrupCnt"":1},{""LeavDt"":""2019/11/21(四)"",""GrupCd"":""DTS19B21ICKE"",""GrupCnt"":1},{""LeavDt"":""2019/11/23(六)"",""GrupCd"":""DTS19B23ICKE"",""GrupCnt"":1},{""LeavDt"":""2019/11/28(四)"",""GrupCd"":""DTS19B28ICKE"",""GrupCnt"":1}],""RecCnt"":216,""GoCnt"":216,""PgoCnt"":0,""RowId"":2,""SacctNo"":"""",""MgrupCd"":""DTS19-ICNK11"",""SaleAm"":7900,""AgtAm"":7900,""GrupLn"":5,""SubCd"":""GO"",""SubCdAnm"":""團"",""SortSq"":7,""Url"":""/EW/GO/MGroupDetail.asp?prodCd=DTS19-ICNK11"",""ShareUrl"":""http://erp.gogojp.com.tw/EW/GO/MGroupDetail.asp?prodCd=DTS19-ICNK11"",""SrcCls"":0,""ImgUrl"":""/eWeb_gogojp/IMGDB/000019/000609/00012135.jpg""}],""Pgo"":[{""MgrupSnm"":""網友推到爆～超值沖繩四日自由行(稅外)"",""LeavDtAll"":[{""LeavDt"":""2019/11/06(三)"",""GrupCd"":""DTS19B06B4IT"",""GrupCnt"":1},{""LeavDt"":""2019/11/11(一)"",""GrupCd"":""DTS19B11B4IT"",""GrupCnt"":1},{""LeavDt"":""2019/11/14(四)"",""GrupCd"":""DTS19B14C4IT"",""GrupCnt"":1},{""LeavDt"":""2019/11/18(一)"",""GrupCd"":""DTS19B18B4IT"",""GrupCnt"":1},{""LeavDt"":""2019/11/20(三)"",""GrupCd"":""DTS19B20B4IT"",""GrupCnt"":1}],""RecCnt"":8,""GoCnt"":0,""PgoCnt"":8,""RowId"":1,""SacctNo"":"""",""MgrupCd"":""DTS18-56580"",""SaleAm"":6900,""AgtAm"":6900,""GrupLn"":4,""SubCd"":""PGO"",""SubCdAnm"":""自"",""SortSq"":7,""Url"":""/eweb/PG/V_P_1.asp?PACKAGE_CD=DTS18-56580\u0026SUB_CD=PGO"",""ShareUrl"":""http://erp.gogojp.com.tw/eweb/PG/V_P_1.asp?PACKAGE_CD=DTS18-56580\u0026SUB_CD=PGO"",""SrcCls"":0,""ImgUrl"":""/eWeb_gogojp/IMGDB/000006/000310/00003864.jpg""},{""MgrupSnm"":""韓國首爾自由行五日-入住明洞TMARK HOTEL(兩晚)"",""LeavDtAll"":[{""LeavDt"":""2019/11/20(三)"",""GrupCd"":""DTS19B20ICN3"",""GrupCnt"":1},{""LeavDt"":""2019/11/27(三)"",""GrupCd"":""DTS19B27ICN3"",""GrupCnt"":1},{""LeavDt"":""2019/12/11(三)"",""GrupCd"":""DTS19C11ICN3"",""GrupCnt"":1}],""RecCnt"":8,""GoCnt"":0,""PgoCnt"":8,""RowId"":2,""SacctNo"":"""",""MgrupCd"":""DTS19-ICN007"",""SaleAm"":9900,""AgtAm"":9900,""GrupLn"":5,""SubCd"":""PGO"",""SubCdAnm"":""自"",""SortSq"":7,""Url"":""/eweb/PG/V_P_1.asp?PACKAGE_CD=DTS19-ICN007\u0026SUB_CD=PGO"",""ShareUrl"":""http://erp.gogojp.com.tw/eweb/PG/V_P_1.asp?PACKAGE_CD=DTS19-ICN007\u0026SUB_CD=PGO"",""SrcCls"":0,""ImgUrl"":""/eWeb_gogojp/IMGDB/000019/000555/00012068.jpg""}],""SearchCondition"":""\u003cli\u003e搜尋條件：\u003c/li\u003e"",""AmrnkNm"":""銷售價"",""MetaInfos"":[{""Name"":""keywords"",""Content"":""""},{""Name"":""description"",""Content"":""""}],""Verify"":false,""ErrMsg"":null}";
        #endregion
        return jsonStr;
    }
    public string GetGODetail()
    {
        #region 準備網址
        // string vURL = "http://erp.gogojp.com.tw:8000/WMnet/API/V5/GetProductInfo.ashx";
        string vURL = "https://ssl.dtsgroup.com.tw/WMnet/API/V5/GetProductInfo.ashx";
        
        //&prodCd=DTS19B27C4CI
        //&amrnk=1
        //&refAmrnk=1

        string urlParameters = "?IWEB_ID=dtsgroup&amrnk=1&refAmrnk=1";
        string jsonStr = @"{""ProductInfo"": { }, ""Verify"": true, ""ErrMsg"": ""}";
        urlParameters += "&prodCd=" + vGrupCd + "";

        #endregion
        #region 準備cache資料
        ObjectCache cache = MemoryCache.Default;
        bool vBoolCacheClear = false;
        var vPolicy = new CacheItemPolicy();//設定回收時間
                                            //多久時間清除快取項目
                                            //policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(15.0);
                                            //policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15.0);
        vPolicy.SlidingExpiration = TimeSpan.FromMinutes(5);
        string vCacheName = "GODetail";
        vCacheName += vGrupCd + "_";
        string vCache = cache[vCacheName] as string;
        if (vClear == "Y")
        {
            vBoolCacheClear = true;
        }
        #endregion
        #region 取得資料
        if (vCache == null || vBoolCacheClear)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(vURL + urlParameters) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 30000;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            jsonStr = sr.ReadToEnd();
                            cache.Set(vCacheName, jsonStr, vPolicy);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    jsonStr += String.Format(jsonStr, reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                jsonStr += String.Format(jsonStr, "");
            }
        }
        else
        {
            jsonStr = vCache;
        }
        //jsonStr = @"{ ""ProductInfo"": { ""SiteTitle"": ""大榮旅遊B2B同業網"", ""VerifyMsg"": null, ""AgreeMent"": ""/eweb_gogojp/public/GO_Agreement.txt"", ""RqPriceFg"": true, ""GrupDyc"": { ""GrupCd"": ""DTS19B27C4CI"", ""SacctNo"": """", ""MgrupCd"": ""DTS19-OKA006"", ""GrupSnm"": ""新版沖繩醉療癒～ORION啤酒‧琉球雪景‧幸福果報崖輕遊四日"", ""GrupLn"": 4, ""GrupNt"": 3, ""OrderDl"": ""2019/11/21"", ""LeavDt"": ""2019/11/27"", ""RetnDt"": ""2019/11/30"", ""LowSaleAm"": 0, ""LowRefAm"": 0, ""IsHaveProduct"": false, ""Promote"": false, ""Guarantee"": false, ""PortCd"": ""TPE"", ""SaleQt"": 19, ""FullSts"": null, ""ItnDy958"": """", ""SignUpLink"": null, ""RqstYqt"": 0 }, ""GrupStc"": { ""DordFg"": true, ""DordAm"": 5000, ""DordRate"": 0, ""DordDl"": ""2019/11/21"", ""DordDl2"": 2, ""GcwktmFg"": false, ""ItnNatn"": ""日本"", ""ItnNatnCd"": ""JP"", ""ItnCity"": ""沖繩"", ""ItnCityCd"": ""OKA"", ""ImgUrl"": ""/eWeb_gogojp/IMGDB/000006/000298/00003847.jpg"", ""ItnRk2"": """", ""ItnDrD1"": """", ""ItnDy950"": """", ""ItnDy951"": """", ""ItnDy952"": """", ""ItnDy953"": """", ""ItnDy954"": ""『團費』說明"", ""ItnDy955"": """", ""ItnDy959"": """", ""ItnDy960"": """", ""ItnDy990"": """", ""VisaRk"": [  ], ""MainFeature"": [  ], ""NatnRk"": """", ""AsmbPl"": """", ""AsmbPl2"": """", ""AsmbTm"": ""2019/11/27 15:10"", ""AsmbTm2"": """" }, ""GrupBook"": [ { ""bookSq"": ""01"", ""gbookDy"": 1, ""fltNo"": ""CI122"", ""depDt"": ""2019/11/27"", ""depTm"": ""17:10"", ""arrDt"": ""2019/11/27"", ""arrTm"": ""19:45"", ""routId"": ""TPE/OKA"", ""depPtCd"": ""TPE"", ""arrPtCd"": ""OKA"", ""DepCityNm"": ""台北市"", ""ArrCityNm"": ""沖繩"", ""DepAirpNm"": ""台北-桃園機場"", ""ArrAirpNm"": ""琉球-那霸機場"", ""carrNm"": ""中華航空"" }, { ""bookSq"": ""01"", ""gbookDy"": 4, ""fltNo"": ""CI123"", ""depDt"": ""2019/11/30"", ""depTm"": ""20:50"", ""arrDt"": ""2019/11/30"", ""arrTm"": ""21:15"", ""routId"": ""OKA/TPE"", ""depPtCd"": ""OKA"", ""arrPtCd"": ""TPE"", ""DepCityNm"": ""沖繩"", ""ArrCityNm"": ""台北市"", ""DepAirpNm"": ""琉球-那霸機場"", ""ArrAirpNm"": ""台北-桃園機場"", ""carrNm"": ""中華航空"" } ], ""GrupGitn"": [ { ""ItnDy"": 1, ""ItnNm"": ""桃園國際機場／那霸空港→住宿飯店"", ""ItnDr"": """", ""ItnHtl"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": false, ""ItnLu"": false, ""ItnDi"": true, ""ItnBrDr"": """", ""ItnLuDr"": """", ""ItnDiDr"": ""機上輕食"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] }, { ""ItnDy"": 2, ""ItnNm"": ""ORION啤酒名護工廠→沖繩海洋博公園～海豚表演秀～珊瑚七色海～沖繩美之海水族館→流傳至今的沖繩昔有風景~備瀨福木林道→北谷美國村"", ""ItnDr"": """", ""ItnHtl"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": true, ""ItnLu"": true, ""ItnDi"": false, ""ItnBrDr"": ""飯店內用"", ""ItnLuDr"": ""山原御殿燒烤(或)水果樂園燒烤自助餐"", ""ItnDiDr"": ""為方便逛街～敬請自理"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] }, { ""ItnDy"": 3, ""ItnNm"": ""腳踏車漫遊～系満美美沖縄海岸→絕美海中道路~海の駅～ぬちマース鹽場見學~世界獨一無二的鹽雪景~幸福的絕壁～果報崖→國際通大道:元氣的洗禮"", ""ItnDr"": """", ""ItnHtl"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": true, ""ItnLu"": true, ""ItnDi"": false, ""ItnBrDr"": ""飯店內用"", ""ItnLuDr"": ""沖繩風味手打麵(或)風味自助餐"", ""ItnDiDr"": ""為方便逛街～敬請自理"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] }, { ""ItnDy"": 4, ""ItnNm"": ""免稅店→??妝店→玉泉洞～王國村～琉球大鼓表演→OUTLET→那霸空港／桃園國際機場"", ""ItnDr"": """", ""ItnHtl"": ""溫暖的家"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": true, ""ItnLu"": true, ""ItnDi"": false, ""ItnBrDr"": ""飯店內用"", ""ItnLuDr"": ""８０種料理自助餐 (或)海景餐廳自助餐"", ""ItnDiDr"": ""為方便逛街～敬請自理"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""溫暖的家"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] } ], ""GrupPrice"": [ { ""BedTp"": ""1"", ""BedTpNm"": ""大人"", ""SaleAm"": 14900, ""AgtAm"": 14900 }, { ""BedTp"": ""2"", ""BedTpNm"": ""小孩佔床"", ""SaleAm"": 14900, ""AgtAm"": 14900 }, { ""BedTp"": ""3"", ""BedTpNm"": ""小孩不佔床"", ""SaleAm"": 12900, ""AgtAm"": 12900 }, { ""BedTp"": ""4"", ""BedTpNm"": ""加床"", ""SaleAm"": 14900, ""AgtAm"": 14900 }, { ""BedTp"": ""5"", ""BedTpNm"": ""嬰兒"", ""SaleAm"": 2000, ""AgtAm"": 2000 } ], ""GrupGopItem"": [  ], ""GrupGchGitn"": [  ], ""GrupRegmRegses"": [ { ""RegmCd"": ""0005"", ""RegmNm"": ""沖繩(琉球)"", ""RegsCd"": ""0001"", ""RegsNm"": ""沖繩(琉球)"" } ], ""BookingClasses"": [ { ""Code"": ""Y"", ""Name"": ""經濟艙"", ""IncreasePrice"": 0, ""SaleQt"": 19, ""SubCd"": ""M"" }, { ""Code"": ""C"", ""Name"": ""商務艙"", ""IncreasePrice"": 0, ""SaleQt"": 0, ""SubCd"": ""M"" }, { ""Code"": ""F"", ""Name"": ""頭等艙"", ""IncreasePrice"": 0, ""SaleQt"": 0, ""SubCd"": ""M"" }, { ""Code"": ""ED"", ""Name"": ""長榮豪華艙"", ""IncreasePrice"": 0, ""SaleQt"": 0, ""SubCd"": ""M"" } ], ""GrupInfo"": { ""OrderLink"": { ""Status"": 3, ""Name"": ""報名"", ""Url"": ""/EW/GO/GroupOrder.asp?prodCd=DTS19B27C4CI"" }, ""GrupTag"": { ""GuaranteeFg"": false, ""PromoteFg"": false, ""HotTp"": """", ""HotTpNm"": """", ""IsShowGuarantee"": false, ""IsShowPromote"": false, ""IsShowHotTp"": false }, ""OtherGrupLink"": ""/EW/GO/GroupList.asp?mGrupCd=DTS19-OKA006\u0026beginDt=2019/11/25"" }, ""MetaInfos"": [ { ""Name"": ""keywords"", ""Content"": """" }, { ""Name"": ""description"", ""Content"": """" } ], ""OtherInfos"": [ { ""Title"": ""小費"", ""Context"": ""日本：團體出發小費一天300元。"" }, { ""Title"": ""出團備註"", ""Context"": """" }, { ""Title"": ""注意事項"", ""Context"": """" }, { ""Title"": ""電壓"", ""Context"": """" }, { ""Title"": ""時差"", ""Context"": """" }, { ""Title"": ""電話通訊"", ""Context"": """" }, { ""Title"": ""簽證護照"", ""Context"": """" }, { ""Title"": ""幣值"", ""Context"": """" }, { ""Title"": ""天氣"", ""Context"": ""請點選天氣參考網址"" }, { ""Title"": ""其他"", ""Context"": """" } ], ""Verify"": false, ""ErrMsg"": null }, ""Verify"": true, ""ErrMsg"": """"}}";
        #endregion
        return jsonStr;
    }
    public string GetPGODetail()
    {
        #region 準備網址
        // string vURL = "http://erp.gogojp.com.tw:8000/WMnet/API/product/PG/PgoInfo.ashx?prodType=G";
        string vURL = "https://ssl.dtsgroup.com.tw/WMnet/API/product/PG/PgoInfo.ashx?prodType=G";
        //&prodCd=DTS19B27C4CI
        //&amrnk=1
        //&refAmrnk=1
        //&prodType=G

        string urlParameters = "?IWEB_ID=dtsgroup&amrnk=1&refAmrnk=1";
        string jsonStr = @"{""ProductInfo"": { }, ""Verify"": true, ""ErrMsg"": ""}";
        urlParameters += "&prodCd=" + vGrupCd + "";

        #endregion
        #region 準備cache資料
        ObjectCache cache = MemoryCache.Default;
        bool vBoolCacheClear = false;
        var vPolicy = new CacheItemPolicy();//設定回收時間
                                            //多久時間清除快取項目
                                            //policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(15.0);
                                            //policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15.0);
        vPolicy.SlidingExpiration = TimeSpan.FromMinutes(5);
        string vCacheName = "PGODetail";
        vCacheName += vGrupCd + "_";
        string vCache = cache[vCacheName] as string;
        if (vClear == "Y")
        {
            vBoolCacheClear = true;
        }
        #endregion
        #region 取得資料
        if (vCache == null || vBoolCacheClear)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(vURL + urlParameters) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 30000;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            jsonStr = sr.ReadToEnd();
                            cache.Set(vCacheName, jsonStr, vPolicy);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    jsonStr += String.Format(jsonStr, reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                jsonStr += String.Format(jsonStr, "");
            }
        }
        else
        {
            jsonStr = vCache;
        }
        //jsonStr = @"{ ""ProductInfo"": { ""SiteTitle"": ""大榮旅遊B2B同業網"", ""VerifyMsg"": null, ""AgreeMent"": ""/eweb_gogojp/public/GO_Agreement.txt"", ""RqPriceFg"": true, ""GrupDyc"": { ""GrupCd"": ""DTS19B27C4CI"", ""SacctNo"": """", ""MgrupCd"": ""DTS19-OKA006"", ""GrupSnm"": ""新版沖繩醉療癒～ORION啤酒‧琉球雪景‧幸福果報崖輕遊四日"", ""GrupLn"": 4, ""GrupNt"": 3, ""OrderDl"": ""2019/11/21"", ""LeavDt"": ""2019/11/27"", ""RetnDt"": ""2019/11/30"", ""LowSaleAm"": 0, ""LowRefAm"": 0, ""IsHaveProduct"": false, ""Promote"": false, ""Guarantee"": false, ""PortCd"": ""TPE"", ""SaleQt"": 19, ""FullSts"": null, ""ItnDy958"": """", ""SignUpLink"": null, ""RqstYqt"": 0 }, ""GrupStc"": { ""DordFg"": true, ""DordAm"": 5000, ""DordRate"": 0, ""DordDl"": ""2019/11/21"", ""DordDl2"": 2, ""GcwktmFg"": false, ""ItnNatn"": ""日本"", ""ItnNatnCd"": ""JP"", ""ItnCity"": ""沖繩"", ""ItnCityCd"": ""OKA"", ""ImgUrl"": ""/eWeb_gogojp/IMGDB/000006/000298/00003847.jpg"", ""ItnRk2"": """", ""ItnDrD1"": """", ""ItnDy950"": """", ""ItnDy951"": """", ""ItnDy952"": """", ""ItnDy953"": """", ""ItnDy954"": ""『團費』說明"", ""ItnDy955"": """", ""ItnDy959"": """", ""ItnDy960"": """", ""ItnDy990"": """", ""VisaRk"": [  ], ""MainFeature"": [  ], ""NatnRk"": """", ""AsmbPl"": """", ""AsmbPl2"": """", ""AsmbTm"": ""2019/11/27 15:10"", ""AsmbTm2"": """" }, ""GrupBook"": [ { ""bookSq"": ""01"", ""gbookDy"": 1, ""fltNo"": ""CI122"", ""depDt"": ""2019/11/27"", ""depTm"": ""17:10"", ""arrDt"": ""2019/11/27"", ""arrTm"": ""19:45"", ""routId"": ""TPE/OKA"", ""depPtCd"": ""TPE"", ""arrPtCd"": ""OKA"", ""DepCityNm"": ""台北市"", ""ArrCityNm"": ""沖繩"", ""DepAirpNm"": ""台北-桃園機場"", ""ArrAirpNm"": ""琉球-那霸機場"", ""carrNm"": ""中華航空"" }, { ""bookSq"": ""01"", ""gbookDy"": 4, ""fltNo"": ""CI123"", ""depDt"": ""2019/11/30"", ""depTm"": ""20:50"", ""arrDt"": ""2019/11/30"", ""arrTm"": ""21:15"", ""routId"": ""OKA/TPE"", ""depPtCd"": ""OKA"", ""arrPtCd"": ""TPE"", ""DepCityNm"": ""沖繩"", ""ArrCityNm"": ""台北市"", ""DepAirpNm"": ""琉球-那霸機場"", ""ArrAirpNm"": ""台北-桃園機場"", ""carrNm"": ""中華航空"" } ], ""GrupGitn"": [ { ""ItnDy"": 1, ""ItnNm"": ""桃園國際機場／那霸空港→住宿飯店"", ""ItnDr"": """", ""ItnHtl"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": false, ""ItnLu"": false, ""ItnDi"": true, ""ItnBrDr"": """", ""ItnLuDr"": """", ""ItnDiDr"": ""機上輕食"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] }, { ""ItnDy"": 2, ""ItnNm"": ""ORION啤酒名護工廠→沖繩海洋博公園～海豚表演秀～珊瑚七色海～沖繩美之海水族館→流傳至今的沖繩昔有風景~備瀨福木林道→北谷美國村"", ""ItnDr"": """", ""ItnHtl"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": true, ""ItnLu"": true, ""ItnDi"": false, ""ItnBrDr"": ""飯店內用"", ""ItnLuDr"": ""山原御殿燒烤(或)水果樂園燒烤自助餐"", ""ItnDiDr"": ""為方便逛街～敬請自理"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] }, { ""ItnDy"": 3, ""ItnNm"": ""腳踏車漫遊～系満美美沖縄海岸→絕美海中道路~海の駅～ぬちマース鹽場見學~世界獨一無二的鹽雪景~幸福的絕壁～果報崖→國際通大道:元氣的洗禮"", ""ItnDr"": """", ""ItnHtl"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": true, ""ItnLu"": true, ""ItnDi"": false, ""ItnBrDr"": ""飯店內用"", ""ItnLuDr"": ""沖繩風味手打麵(或)風味自助餐"", ""ItnDiDr"": ""為方便逛街～敬請自理"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] }, { ""ItnDy"": 4, ""ItnNm"": ""免稅店→??妝店→玉泉洞～王國村～琉球大鼓表演→OUTLET→那霸空港／桃園國際機場"", ""ItnDr"": """", ""ItnHtl"": ""溫暖的家"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": true, ""ItnLu"": true, ""ItnDi"": false, ""ItnBrDr"": ""飯店內用"", ""ItnLuDr"": ""８０種料理自助餐 (或)海景餐廳自助餐"", ""ItnDiDr"": ""為方便逛街～敬請自理"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""溫暖的家"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] } ], ""GrupPrice"": [ { ""BedTp"": ""1"", ""BedTpNm"": ""大人"", ""SaleAm"": 14900, ""AgtAm"": 14900 }, { ""BedTp"": ""2"", ""BedTpNm"": ""小孩佔床"", ""SaleAm"": 14900, ""AgtAm"": 14900 }, { ""BedTp"": ""3"", ""BedTpNm"": ""小孩不佔床"", ""SaleAm"": 12900, ""AgtAm"": 12900 }, { ""BedTp"": ""4"", ""BedTpNm"": ""加床"", ""SaleAm"": 14900, ""AgtAm"": 14900 }, { ""BedTp"": ""5"", ""BedTpNm"": ""嬰兒"", ""SaleAm"": 2000, ""AgtAm"": 2000 } ], ""GrupGopItem"": [  ], ""GrupGchGitn"": [  ], ""GrupRegmRegses"": [ { ""RegmCd"": ""0005"", ""RegmNm"": ""沖繩(琉球)"", ""RegsCd"": ""0001"", ""RegsNm"": ""沖繩(琉球)"" } ], ""BookingClasses"": [ { ""Code"": ""Y"", ""Name"": ""經濟艙"", ""IncreasePrice"": 0, ""SaleQt"": 19, ""SubCd"": ""M"" }, { ""Code"": ""C"", ""Name"": ""商務艙"", ""IncreasePrice"": 0, ""SaleQt"": 0, ""SubCd"": ""M"" }, { ""Code"": ""F"", ""Name"": ""頭等艙"", ""IncreasePrice"": 0, ""SaleQt"": 0, ""SubCd"": ""M"" }, { ""Code"": ""ED"", ""Name"": ""長榮豪華艙"", ""IncreasePrice"": 0, ""SaleQt"": 0, ""SubCd"": ""M"" } ], ""GrupInfo"": { ""OrderLink"": { ""Status"": 3, ""Name"": ""報名"", ""Url"": ""/EW/GO/GroupOrder.asp?prodCd=DTS19B27C4CI"" }, ""GrupTag"": { ""GuaranteeFg"": false, ""PromoteFg"": false, ""HotTp"": """", ""HotTpNm"": """", ""IsShowGuarantee"": false, ""IsShowPromote"": false, ""IsShowHotTp"": false }, ""OtherGrupLink"": ""/EW/GO/GroupList.asp?mGrupCd=DTS19-OKA006\u0026beginDt=2019/11/25"" }, ""MetaInfos"": [ { ""Name"": ""keywords"", ""Content"": """" }, { ""Name"": ""description"", ""Content"": """" } ], ""OtherInfos"": [ { ""Title"": ""小費"", ""Context"": ""日本：團體出發小費一天300元。"" }, { ""Title"": ""出團備註"", ""Context"": """" }, { ""Title"": ""注意事項"", ""Context"": """" }, { ""Title"": ""電壓"", ""Context"": """" }, { ""Title"": ""時差"", ""Context"": """" }, { ""Title"": ""電話通訊"", ""Context"": """" }, { ""Title"": ""簽證護照"", ""Context"": """" }, { ""Title"": ""幣值"", ""Context"": """" }, { ""Title"": ""天氣"", ""Context"": ""請點選天氣參考網址"" }, { ""Title"": ""其他"", ""Context"": """" } ], ""Verify"": false, ""ErrMsg"": null }, ""Verify"": true, ""ErrMsg"": """"}}";
        #endregion
        return jsonStr;
    }
    public string GetPGOOInfo()
    {
        #region 準備網址
        // string vURL = "http://erp.gogojp.com.tw:8000/WMnet/API/Product/PG/PgoOrderInfo.ashx?";
        string vURL = "https://ssl.dtsgroup.com.tw/WMnet/API/Product/PG/PgoOrderInfo.ashx?";
        //&prodCd=DTS19C11ICN3
        //&amrnk=1
        //&refAmrnk=1
        //&prodType=G

        string urlParameters = "?IWEB_ID=dtsgroup&amrnk=1&refAmrnk=1";
        string jsonStr = @"{""ProductInfo"": {0}, ""Verify"": true, ""ErrMsg"": ""}";
        urlParameters += "&prodCd=" + vGrupCd + "";

        #endregion
        #region 準備cache資料
        ObjectCache cache = MemoryCache.Default;
        bool vBoolCacheClear = false;
        var vPolicy = new CacheItemPolicy();//設定回收時間
                                            //多久時間清除快取項目
                                            //policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(15.0);
                                            //policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15.0);
        vPolicy.SlidingExpiration = TimeSpan.FromMinutes(5);
        string vCacheName = "PGOOInfo";
        vCacheName += vGrupCd + "_";
        string vCache = cache[vCacheName] as string;
        if (vClear == "Y")
        {
            vBoolCacheClear = true;
        }
        #endregion
        #region 取得資料
        if (vCache == null || vBoolCacheClear)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(vURL + urlParameters) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 30000;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            jsonStr = sr.ReadToEnd();
                            cache.Set(vCacheName, jsonStr, vPolicy);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    string vTmp = reader.ReadToEnd().ToString();
                    jsonStr += String.Format(jsonStr, vTmp);
                }
            }
            catch (Exception ex)
            {
                jsonStr += String.Format(jsonStr, "");
            }
        }
        else
        {
            jsonStr = vCache;
        }
        //jsonStr = @"{ ""ProductInfo"": { ""SiteTitle"": ""大榮旅遊B2B同業網"", ""VerifyMsg"": null, ""AgreeMent"": ""/eweb_gogojp/public/GO_Agreement.txt"", ""RqPriceFg"": true, ""GrupDyc"": { ""GrupCd"": ""DTS19B27C4CI"", ""SacctNo"": """", ""MgrupCd"": ""DTS19-OKA006"", ""GrupSnm"": ""新版沖繩醉療癒～ORION啤酒‧琉球雪景‧幸福果報崖輕遊四日"", ""GrupLn"": 4, ""GrupNt"": 3, ""OrderDl"": ""2019/11/21"", ""LeavDt"": ""2019/11/27"", ""RetnDt"": ""2019/11/30"", ""LowSaleAm"": 0, ""LowRefAm"": 0, ""IsHaveProduct"": false, ""Promote"": false, ""Guarantee"": false, ""PortCd"": ""TPE"", ""SaleQt"": 19, ""FullSts"": null, ""ItnDy958"": """", ""SignUpLink"": null, ""RqstYqt"": 0 }, ""GrupStc"": { ""DordFg"": true, ""DordAm"": 5000, ""DordRate"": 0, ""DordDl"": ""2019/11/21"", ""DordDl2"": 2, ""GcwktmFg"": false, ""ItnNatn"": ""日本"", ""ItnNatnCd"": ""JP"", ""ItnCity"": ""沖繩"", ""ItnCityCd"": ""OKA"", ""ImgUrl"": ""/eWeb_gogojp/IMGDB/000006/000298/00003847.jpg"", ""ItnRk2"": """", ""ItnDrD1"": """", ""ItnDy950"": """", ""ItnDy951"": """", ""ItnDy952"": """", ""ItnDy953"": """", ""ItnDy954"": ""『團費』說明"", ""ItnDy955"": """", ""ItnDy959"": """", ""ItnDy960"": """", ""ItnDy990"": """", ""VisaRk"": [  ], ""MainFeature"": [  ], ""NatnRk"": """", ""AsmbPl"": """", ""AsmbPl2"": """", ""AsmbTm"": ""2019/11/27 15:10"", ""AsmbTm2"": """" }, ""GrupBook"": [ { ""bookSq"": ""01"", ""gbookDy"": 1, ""fltNo"": ""CI122"", ""depDt"": ""2019/11/27"", ""depTm"": ""17:10"", ""arrDt"": ""2019/11/27"", ""arrTm"": ""19:45"", ""routId"": ""TPE/OKA"", ""depPtCd"": ""TPE"", ""arrPtCd"": ""OKA"", ""DepCityNm"": ""台北市"", ""ArrCityNm"": ""沖繩"", ""DepAirpNm"": ""台北-桃園機場"", ""ArrAirpNm"": ""琉球-那霸機場"", ""carrNm"": ""中華航空"" }, { ""bookSq"": ""01"", ""gbookDy"": 4, ""fltNo"": ""CI123"", ""depDt"": ""2019/11/30"", ""depTm"": ""20:50"", ""arrDt"": ""2019/11/30"", ""arrTm"": ""21:15"", ""routId"": ""OKA/TPE"", ""depPtCd"": ""OKA"", ""arrPtCd"": ""TPE"", ""DepCityNm"": ""沖繩"", ""ArrCityNm"": ""台北市"", ""DepAirpNm"": ""琉球-那霸機場"", ""ArrAirpNm"": ""台北-桃園機場"", ""carrNm"": ""中華航空"" } ], ""GrupGitn"": [ { ""ItnDy"": 1, ""ItnNm"": ""桃園國際機場／那霸空港→住宿飯店"", ""ItnDr"": """", ""ItnHtl"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": false, ""ItnLu"": false, ""ItnDi"": true, ""ItnBrDr"": """", ""ItnLuDr"": """", ""ItnDiDr"": ""機上輕食"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] }, { ""ItnDy"": 2, ""ItnNm"": ""ORION啤酒名護工廠→沖繩海洋博公園～海豚表演秀～珊瑚七色海～沖繩美之海水族館→流傳至今的沖繩昔有風景~備瀨福木林道→北谷美國村"", ""ItnDr"": """", ""ItnHtl"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": true, ""ItnLu"": true, ""ItnDi"": false, ""ItnBrDr"": ""飯店內用"", ""ItnLuDr"": ""山原御殿燒烤(或)水果樂園燒烤自助餐"", ""ItnDiDr"": ""為方便逛街～敬請自理"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] }, { ""ItnDy"": 3, ""ItnNm"": ""腳踏車漫遊～系満美美沖縄海岸→絕美海中道路~海の駅～ぬちマース鹽場見學~世界獨一無二的鹽雪景~幸福的絕壁～果報崖→國際通大道:元氣的洗禮"", ""ItnDr"": """", ""ItnHtl"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": true, ""ItnLu"": true, ""ItnDi"": false, ""ItnBrDr"": ""飯店內用"", ""ItnLuDr"": ""沖繩風味手打麵(或)風味自助餐"", ""ItnDiDr"": ""為方便逛街～敬請自理"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] }, { ""ItnDy"": 4, ""ItnNm"": ""免稅店→??妝店→玉泉洞～王國村～琉球大鼓表演→OUTLET→那霸空港／桃園國際機場"", ""ItnDr"": """", ""ItnHtl"": ""溫暖的家"", ""HtlCd"": """", ""SameHtlFg"": ""0"", ""IsSameHtl"": false, ""ItnBr"": true, ""ItnLu"": true, ""ItnDi"": false, ""ItnBrDr"": ""飯店內用"", ""ItnLuDr"": ""８０種料理自助餐 (或)海景餐廳自助餐"", ""ItnDiDr"": ""為方便逛街～敬請自理"", ""ItnHtls"": [  {  ""HtlCd"": """",  ""HtlCName"": ""溫暖的家"",  ""HtlAName"": """",  ""HtlEName"": """",  ""WebUrl"": null  } ] } ], ""GrupPrice"": [ { ""BedTp"": ""1"", ""BedTpNm"": ""大人"", ""SaleAm"": 14900, ""AgtAm"": 14900 }, { ""BedTp"": ""2"", ""BedTpNm"": ""小孩佔床"", ""SaleAm"": 14900, ""AgtAm"": 14900 }, { ""BedTp"": ""3"", ""BedTpNm"": ""小孩不佔床"", ""SaleAm"": 12900, ""AgtAm"": 12900 }, { ""BedTp"": ""4"", ""BedTpNm"": ""加床"", ""SaleAm"": 14900, ""AgtAm"": 14900 }, { ""BedTp"": ""5"", ""BedTpNm"": ""嬰兒"", ""SaleAm"": 2000, ""AgtAm"": 2000 } ], ""GrupGopItem"": [  ], ""GrupGchGitn"": [  ], ""GrupRegmRegses"": [ { ""RegmCd"": ""0005"", ""RegmNm"": ""沖繩(琉球)"", ""RegsCd"": ""0001"", ""RegsNm"": ""沖繩(琉球)"" } ], ""BookingClasses"": [ { ""Code"": ""Y"", ""Name"": ""經濟艙"", ""IncreasePrice"": 0, ""SaleQt"": 19, ""SubCd"": ""M"" }, { ""Code"": ""C"", ""Name"": ""商務艙"", ""IncreasePrice"": 0, ""SaleQt"": 0, ""SubCd"": ""M"" }, { ""Code"": ""F"", ""Name"": ""頭等艙"", ""IncreasePrice"": 0, ""SaleQt"": 0, ""SubCd"": ""M"" }, { ""Code"": ""ED"", ""Name"": ""長榮豪華艙"", ""IncreasePrice"": 0, ""SaleQt"": 0, ""SubCd"": ""M"" } ], ""GrupInfo"": { ""OrderLink"": { ""Status"": 3, ""Name"": ""報名"", ""Url"": ""/EW/GO/GroupOrder.asp?prodCd=DTS19B27C4CI"" }, ""GrupTag"": { ""GuaranteeFg"": false, ""PromoteFg"": false, ""HotTp"": """", ""HotTpNm"": """", ""IsShowGuarantee"": false, ""IsShowPromote"": false, ""IsShowHotTp"": false }, ""OtherGrupLink"": ""/EW/GO/GroupList.asp?mGrupCd=DTS19-OKA006\u0026beginDt=2019/11/25"" }, ""MetaInfos"": [ { ""Name"": ""keywords"", ""Content"": """" }, { ""Name"": ""description"", ""Content"": """" } ], ""OtherInfos"": [ { ""Title"": ""小費"", ""Context"": ""日本：團體出發小費一天300元。"" }, { ""Title"": ""出團備註"", ""Context"": """" }, { ""Title"": ""注意事項"", ""Context"": """" }, { ""Title"": ""電壓"", ""Context"": """" }, { ""Title"": ""時差"", ""Context"": """" }, { ""Title"": ""電話通訊"", ""Context"": """" }, { ""Title"": ""簽證護照"", ""Context"": """" }, { ""Title"": ""幣值"", ""Context"": """" }, { ""Title"": ""天氣"", ""Context"": ""請點選天氣參考網址"" }, { ""Title"": ""其他"", ""Context"": """" } ], ""Verify"": false, ""ErrMsg"": null }, ""Verify"": true, ""ErrMsg"": """"}}";
        #endregion
        return jsonStr;
    }
}
/*
 * 取得團型列表
 * http://erp.gogojp.com.tw:8000/WMnet/API/V5/GetProdList.ashx?IWEB_ID=dtsgroup&leavDt1=2019/11/20&leavDt2=2020/11/20&amrnk=1&refAmrnk=1&orderCd=4&pageShow=20&pageAll=1&pageGO=1&pagePGO=1&srcCls=D&allowjoin=true&allowwait=true&displayType=M&
*/
/*
 {
  "SiteTitle": "大榮旅遊B2B同業網",
  "All": [
    {
      "MgrupSnm": "網友推到爆～超值沖繩四日自由行(稅外)",
      "LeavDtAll": [
        {
          "LeavDt": "2019/11/20(三)",
          "GrupCd": "DTS19B20B4IT",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/11/28(四)",
          "GrupCd": "DTS19B28B4IT",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/12/01(日)",
          "GrupCd": "DTS19C01B4IT",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/12/02(一)",
          "GrupCd": "DTS19C02B4IT",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/12/04(三)",
          "GrupCd": "DTS19C04B4IT",
          "GrupCnt": 1
        }
      ],
      "RecCnt": 220,
      "GoCnt": 213,
      "PgoCnt": 7,
      "RowId": 1,
      "SacctNo": "",
      "MgrupCd": "DTS18-56580",
      "SaleAm": 6900,
      "AgtAm": 6900,
      "GrupLn": 4,
      "SubCd": "PGO",
      "SubCdAnm": "自",
      "SortSq": 7,
      "Url": "/eweb/PG/V_P_1.asp?PACKAGE_CD=DTS18-56580\u0026SUB_CD=PGO",
      "ShareUrl": "http://erp.gogojp.com.tw/eweb/PG/V_P_1.asp?PACKAGE_CD=DTS18-56580\u0026SUB_CD=PGO",
      "SrcCls": 0,
      "ImgUrl": "/eWeb_gogojp/IMGDB/000006/000310/00003864.jpg"
    }
  ],
  "Go": [
    {
      "MgrupSnm": "動感韓國～五星乙晚、冰雪樂園、普羅旺斯、HEYRI藝術村四日 \u003c不上攝影、無自理餐、百年土種人蔘雞+荒謬的五花肉吃到飽\u003e",
      "LeavDtAll": [
        {
          "LeavDt": "2019/11/25(一)",
          "GrupCd": "DTS19B25ICKD",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/12/02(一)",
          "GrupCd": "DTS19C02ICKD",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/12/09(一)",
          "GrupCd": "DTS19C09ICKD",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/12/16(一)",
          "GrupCd": "DTS19C16ICKD",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/12/23(一)",
          "GrupCd": "DTS19C23ICKD",
          "GrupCnt": 1
        }
      ],
      "RecCnt": 213,
      "GoCnt": 213,
      "PgoCnt": 0,
      "RowId": 1,
      "SacctNo": "",
      "MgrupCd": "64712",
      "SaleAm": 7388,
      "AgtAm": 7388,
      "GrupLn": 4,
      "SubCd": "GO",
      "SubCdAnm": "團",
      "SortSq": 7,
      "Url": "/EW/GO/MGroupDetail.asp?prodCd=64712",
      "ShareUrl": "http://erp.gogojp.com.tw/EW/GO/MGroupDetail.asp?prodCd=64712",
      "SrcCls": 0,
      "ImgUrl": "/eWeb_gogojp/IMGDB/000019/000502/00011826.jpg"
    }
  ],
  "Pgo": [
    {
      "MgrupSnm": "網友推到爆～超值沖繩四日自由行(稅外)",
      "LeavDtAll": [
        {
          "LeavDt": "2019/11/20(三)",
          "GrupCd": "DTS19B20B4IT",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/11/28(四)",
          "GrupCd": "DTS19B28B4IT",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/12/01(日)",
          "GrupCd": "DTS19C01B4IT",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/12/02(一)",
          "GrupCd": "DTS19C02B4IT",
          "GrupCnt": 1
        },
        {
          "LeavDt": "2019/12/04(三)",
          "GrupCd": "DTS19C04B4IT",
          "GrupCnt": 1
        }
      ],
      "RecCnt": 7,
      "GoCnt": 0,
      "PgoCnt": 7,
      "RowId": 1,
      "SacctNo": "",
      "MgrupCd": "DTS18-56580",
      "SaleAm": 6900,
      "AgtAm": 6900,
      "GrupLn": 4,
      "SubCd": "PGO",
      "SubCdAnm": "自",
      "SortSq": 7,
      "Url": "/eweb/PG/V_P_1.asp?PACKAGE_CD=DTS18-56580\u0026SUB_CD=PGO",
      "ShareUrl": "http://erp.gogojp.com.tw/eweb/PG/V_P_1.asp?PACKAGE_CD=DTS18-56580\u0026SUB_CD=PGO",
      "SrcCls": 0,
      "ImgUrl": "/eWeb_gogojp/IMGDB/000006/000310/00003864.jpg"
    }
  ],
  "SearchCondition": "\u003cli\u003e搜尋條件：\u003c/li\u003e",
  "AmrnkNm": "銷售價",
  "MetaInfos": [
    {
      "Name": "keywords",
      "Content": ""
    },
    {
      "Name": "description",
      "Content": ""
    }
  ],
  "Verify": false,
  "ErrMsg": null
}
*/
public class DataContainer
{
    public string SiteTitle { get; set; }
    public List<RootObject> All { get; set; }
    public List<RootObject> Go { get; set; }
    public List<RootObject> Pgo { get; set; }
    public string SearchCondition { get; set; }
    public string AmrnkNm { get; set; }
    public List<MetaInfos> MetaInfos { get; set; }
    public string Verify { get; set; }
    public string ErrMsg { get; set; }
}
public class RootObject
{
    public string MgrupSnm { get; set; }
    public List<LeavAllDate> LeavDtAll { get; set; }
    public string RecCnt { get; set; }
    public string GoCnt { get; set; }
    public string PgoCnt { get; set; }
    public string RowId { get; set; }
    public string SacctNo { get; set; }
    public string MgrupCd { get; set; }
    public string SaleAm { get; set; }
    public string AgtAm { get; set; }
    public string GrupLn { get; set; }
    public string SubCd { get; set; }
    public string SubCdAnm { get; set; }
    public string SortSq { get; set; }
    public string Url { get; set; }
    public string ShareUrl { get; set; }
    public string SrcCls { get; set; }
    public string ImgUrl { get; set; }
}
public class MetaInfos
{
    public string Name { get; set; }
    public string Content { get; set; }
}
public class LeavAllDate
{
    public string LeavDt { get; set; }
    public string GrupCd { get; set; }
    public string GrupCnt { get; set; }
}
/*
 * 取得團體詳細資訊
 * http://erp.gogojp.com.tw:8000/WMnet/API/V5/GetProductInfo.ashx?prodCd=DTS19B27C4CI&amrnk=1&refAmrnk=1
*/
/*
 {
  "ProductInfo": {
    "SiteTitle": "大榮旅遊B2B同業網",
    "VerifyMsg": null,
    "AgreeMent": "/eweb_gogojp/public/GO_Agreement.txt",
    "RqPriceFg": true,
    "GrupDyc": {
      "GrupCd": "DTS19B27C4CI",
      "SacctNo": "",
      "MgrupCd": "DTS19-OKA006",
      "GrupSnm": "新版沖繩醉療癒～ORION啤酒‧琉球雪景‧幸福果報崖輕遊四日",
      "GrupLn": 4,
      "GrupNt": 3,
      "OrderDl": "2019/11/21",
      "LeavDt": "2019/11/27",
      "RetnDt": "2019/11/30",
      "LowSaleAm": 0,
      "LowRefAm": 0,
      "IsHaveProduct": false,
      "Promote": false,
      "Guarantee": false,
      "PortCd": "TPE",
      "SaleQt": 19,
      "FullSts": null,
      "ItnDy958": "",
      "SignUpLink": null,
      "RqstYqt": 0
    },
    "GrupStc": {
      "DordFg": true,
      "DordAm": 5000,
      "DordRate": 0,
      "DordDl": "2019/11/21",
      "DordDl2": 2,
      "GcwktmFg": false,
      "ItnNatn": "日本",
      "ItnNatnCd": "JP",
      "ItnCity": "沖繩",
      "ItnCityCd": "OKA",
      "ImgUrl": "/eWeb_gogojp/IMGDB/000006/000298/00003847.jpg",
      "ItnRk2": "",
      "ItnDrD1": "\u003c!-- ORION --\u003e\r\n\u003cdiv class=\"slider\"\u003e\r\n\u003cimg height=\"auto\" src=\"http://www.dtsgroup.com.tw/japan/jp_spots/oki_76.jpg\" width=\"100%\"\u003e\r\n\u003cdiv class=\"sliderpp\"\u003e在沖繩提到啤酒就非「ORION啤酒」莫屬。其中名護工廠因為開放參觀而成為極受歡迎的景點。啤酒誕生前一項一項的製成參觀，了解如何製作新鮮且品質優良的啤酒。參觀工廠後還可試喝新鮮冷藏的啤酒唷!\u003c/div\u003e\r\n\u003c/div\u003e\r\n\u003c!-- ORION --\u003e\u003c!--ぬちマース鹽場見學--\u003e\r\n\u003cdiv class=\"slider\"\u003e\r\n\u003cimg height=\"auto\" src=\"http://www.dtsgroup.com.tw/japan/jp_spots/oki_36.jpg\" width=\"100%\"\u003e\r\n\u003cdiv class=\"sliderpp\"\u003e大海是一切生物的源泉，包含了生物必要的礦物質，大海之於生物就如同羊水之於新生命的誕生，ぬちマース工場使用世界第一的「常温瞬間空中結晶製塩法」將海水製成結晶有如雪結晶，在工場進行導覽的同時仿佛置身於雪國。\u003c/div\u003e\r\n\u003c/div\u003e\r\n\u003c!-- ぬちマース鹽場見學 --\u003e\u003c!-- 備瀨福木林道  --\u003e\r\n\u003cdiv class=\"slider\"\u003e\r\n\u003cimg height=\"auto\" src=\"http://www.dtsgroup.com.tw/japan/jp_spots/oki_70.jpg\" width=\"100%\"\u003e\r\n\u003cdiv class=\"sliderpp\"\u003e種植來抵抗颱風的福木森林，沿著海岸成了三百年樹齡的綠蔭大道；光影扶疏，海風輕拂，寧靜悠閒地通往迷人湛藍邊岸。\u003c/div\u003e\r\n\u003c/div\u003e\r\n\u003c!-- 備瀨福木林道--\u003e\u003c!--【海洋博公園】--\u003e\r\n\u003cdiv class=\"slider\"\u003e\r\n\u003cimg src=\"http://www.dtsgroup.com.tw/japan/jp_spots/oki_47.jpg\" width=\"100%\" height=\"auto\" /\u003e\r\n\u003cdiv class=\"sliderpp\"\u003e\r\n以「陽光、花卉、海洋」為主題的沖繩海洋博公園，佔地約77公頃的園區中，分為海洋、花卉綠地及歷史文化三大區，於1976年正式對外開放，至今已成為沖繩主要的觀光景點。園區內的海豚表演秀，可愛的瓶鼻海豚、擬虎鯨和太平洋白側海豚做出的騰空高跳、滑稽舞蹈、唱歌和驚人特技等表演，總是激起現場一陣陣驚呼。\r\n\u003c/div\u003e\u003c/div\u003e\r\n\u003c!--【海洋博公園】--\u003e\u003c!-- 玉泉洞王國村 --\u003e\r\n\u003cdiv class=\"slider\"\u003e\r\n\u003cimg height=\"auto\" src=\"http://www.dtsgroup.com.tw/japan/jp_spots/oki_44.jpg\" width=\"100%\"\u003e\r\n\u003cdiv class=\"sliderpp\"\u003e1967年發現的巨大鐘乳洞，深入地底全長5公里，名列日本三大鐘乳洞。以玉泉洞為中心規劃出10個主題專區，統稱王國村，展示如陶瓷、手製樂器、藍染等傳統工藝，特別安排傳統太鼓表演，雄壯的魂祭舞，傳達出沖繩人的蓬勃熱情。\u003c/div\u003e\r\n\u003c/div\u003e\r\n\u003c!-- 玉泉洞王國村 --\u003e\u003c!-- 北谷町美國村--\u003e\r\n\u003cdiv class=\"slider\"\u003e\r\n\u003cimg height=\"auto\" src=\"http://www.dtsgroup.com.tw/japan/jp_spots/oki_43.jpg\" width=\"100%\"\u003e\r\n\u003cdiv class=\"sliderpp\"\u003e\r\n原為美軍基地範圍，重新規劃後有著特別的美系風味小村，在這裡有沿續著美國的小酒館風情，美麗的摩天輪是這裡最顯著的地標，如果你為美軍配備著迷，那麼就必來這裡挖寶唷!\r\n\u003c/div\u003e\r\n\u003c/div\u003e\r\n\u003c!-- 北谷町美國村 --\u003e\u003c!-- Ashibinaa Outlet--\u003e\r\n\u003cdiv class=\"slider\"\u003e\r\n\u003cimg height=\"auto\" src=\"http://www.dtsgroup.com.tw/japan/jp_spots/oki_40.jpg\" width=\"100%\"\u003e\r\n\u003cdiv class=\"sliderpp\"\u003e\r\n仿古希臘建築打造度假氛圍的免稅購物園區，聚集世界各精品名牌還有當地美食、家電、藥妝通通找得到，穿插於消費空間的休閒娛樂設施適合旅客前來在瘋購物之餘稍作舒緩，休息後再戰! \r\n\u003c/div\u003e\r\n\u003c/div\u003e\r\n\u003c!-- Ashibinaa Outlet --\u003e\u003c!-- 國際通大道 --\u003e\r\n\u003cdiv class=\"slider\"\u003e\r\n\u003cimg height=\"auto\" src=\"http://www.dtsgroup.com.tw/japan/jp_spots/oki_48.jpg\" width=\"100%\"\u003e\r\n\u003cdiv class=\"sliderpp\"\u003e絕對讓你失控的景點，國際通上聚集各類店家，必買的伴手禮店、藥妝店、百貨通通都有！短短1.6公里有著奇蹟大街的美稱，就算一開始錯過沒買到，沒關係!繼續往前走還是可買齊想要的東西。\u003c/div\u003e\r\n\u003c/div\u003e\r\n\u003c!-- 國際通大道 --\u003e",
      "ItnDy950": "",
      "ItnDy951": "",
      "ItnDy952": "",
      "ItnDy953": "",
      "ItnDy954": "\u003cp style=\"text-align: justify;\"\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e\u003cstrong\u003e『團費』說明\u003c/strong\u003e：\u003cbr /\u003e\u003cstrong\u003e報價包含:\u003c/strong\u003e\u003cbr /\u003e1.團體來回機票。\u003cbr /\u003e2.行程表所註明之餐食、景點門票、住宿飯店及當地交通接送費用。\u003cbr /\u003e3.五百萬旅責險及二十萬意外醫療險，0-14歲及70歲以上則為二百萬旅責險及二十萬意外醫療險 。\u003cbr /\u003e4.兩地離境機場稅及燃油附加費。\u003cbr /\u003e5.團費已含行李服務包含：\u003cbr /\u003e華航－每人手提行李7公斤+托運行李30公斤。\u003cspan style=\"font-size:14px;\"\u003e\u003cspan style=\"color:#FF0000;\"\u003e(航空公司保有異動之權利)\u003c/span\u003e\u003c/span\u003e\u003cbr /\u003e虎航－團費已含行李服務包含：手提行李1件（三邊尺寸小於54cm x 38cm x 23cm ,上述尺寸包含輪子及手把等外部突出物）及１件個人隨身物品(手提包、電腦或免稅商品等)共計２件總重量不得超過10公斤+托運行李(去程20公斤／回程20公斤)。\u003c/span\u003e\u003c/span\u003e\u003c/p\u003e\u003cp style=\"text-align: justify;\"\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e\u003cstrong\u003e報價不包含:\u003c/strong\u003e\u003cbr /\u003e1.導遊、司機服務費用；每人每日NT$300元。\u003cbr /\u003e2.北高接駁交通費用：不提供服務。\u003cbr /\u003e3.行程表未註明之各項開支，如自選建議行程之交通費用及應付費用。\u003cbr /\u003e4.純私人之消費，如行李超重、飲料酒類、洗衣、電話、電報、飯店內個人消費行為及私人交通費。\u003cbr /\u003e5.虎航託運行李總重超過團體票價所含20公斤者，以每5公斤為單位計費，開票前確認加購；如欲指定機上座位，也請事先加價需求。(加購行李公斤數與指定座位，請洽業務單位)\u003c/span\u003e\u003c/span\u003e\u003c/p\u003e\u003cp style=\"text-align: justify;\"\u003e\u003cstrong\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e『團體機票』使用限制：\u003c/span\u003e\u003c/span\u003e\u003c/strong\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e\u003cspan style=\"color:#FF0000;\"\u003e※ (為免爭議，敬請於購買前詳細閱讀)\u003c/span\u003e\u003c/span\u003e\u003cbr /\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e1.團體行程使用團體來回經濟艙機票，需「團進團出」，不可延回、不得退票及延期使用。\u003cbr /\u003e2.團體行程使用之票種為團體機票，需達16人以上方可成團開票，且機票無法累積航空公司哩程數、不可事先指定座位或劃位。(航空公司依照乘客之英文姓名之字母先後順序作訂位紀錄以及機上座位排序。)\u003cbr /\u003e3.為響應環保及考量作業之便捷，本行程將以電子機票取代紙本票根列印。如欲取得搭機證明，請於搭機後盡速向業務提出申請；另需加收手續費100元(單程)。\u003cbr /\u003e4. 嬰兒搭機的相關規定\u003cbr /\u003e華航-嬰兒(0-2歲)費用含10KG托運行李、無座位；若要需求嬰兒搖籃，嬰兒的身高不能超過 71 CM 體重不能超過 11KG，數量有限；機上嬰兒的安全設施數量有限，基於安全考量，沖繩航線限制機上僅接受最多25位嬰兒搭機，因此有帶嬰兒一起參團的貴賓請於繳付訂金後盡快提供大人及嬰兒的名單，以便將嬰兒名單提供給航空公司統計人數。\u003cbr /\u003e虎航-嬰兒(0-2歲)費用不含行李及無座位，班機亦沒有嬰兒搖籃提供需全程父母抱著,造成不便，敬請見諒。機上嬰兒的安全設施數量有限，基於安全考量，沖繩航線限制機上僅接受最多10位嬰兒搭機，因此有帶嬰兒一起參團的貴賓請於繳付訂金後盡快提供大人及嬰兒的名單，以便將嬰兒名單提供給航空公司統計人數。\u003cbr /\u003e5.機上輕食說明\u003cbr /\u003e華航-因航程時間較短之故，機上餐皆以冷食為主。特殊餐僅有東方素(VOML)、水果餐(FPML)、糖尿病餐(DBML)、嬰兒餐(BBML)(2017/7/1起華航將不再提供兒童餐)。最終回覆仍需由航空公司為主，且保有變更的權益！敬請見諒！\u003cbr /\u003e虎航-短程航線機上餐點僅提供三明治輕食及短程素食特餐，不提供兒童餐、嬰兒餐等特殊餐。\u003cbr /\u003e6.如航班為(加)包機，航班時間為暫定時間，正確起降時刻請以說明會資料為準；將依兩國政府及航空公司公佈為準。若有調整，本公司將保有行程前後順序調整之權。亦盡全力維持原行程之規劃，調度考量恕無法退費及取消。\u003cbr /\u003e7.台灣虎航實施無退票政策，如果乘客由於個人情況變動，包括決定不再需要飛行或無法參與本次旅行，但不限於醫療原因， 因此虎航恕不予以退票（機票價格及費用）。所有乘客為預防此類取消狀況，建議可投保個人旅行保險。\u003c/span\u003e\u003c/span\u003e\u003c/p\u003e\u003cp style=\"text-align: justify;\"\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e\u003cstrong\u003e『出團』相 關 注 意 事 項：\u003c/strong\u003e\u003cbr /\u003e1.持外國護照大人、小孩之旅客或於外站行程中途脫隊者，一律追加團費每人NT2000元。\u003cbr /\u003e2.小孩佔床同大人價，若2歲以上未滿12歲不佔床，請依當團售價所示為主！\u003cbr /\u003e3.人數需達16人以上方可成行。\u003cbr /\u003e4.機位保留每位訂金5,000元；報名確認後3天內繳交。尾款繳交期限：出發前5天（不含例假日）。\u003cbr /\u003e5.飯店團體如遇單男或單女時會以三人房作業，若單人報名，不能湊成雙人房時，需補房間差額。\u003cbr /\u003e6.日本飯店房間坪數較小，多為16～21㎡不等，故少有3人房型，如需加床團體三人房可能會是：一大床+一行軍床或沙發床 / 二小床+一行軍床或沙發床 /一大床+一小床，以當天入住飯店情形為主！若無需求到三人房請分出一人與他人同住，敬請見諒 ！\u003cbr /\u003e7.日本飯店房型除和式房外，皆為兩張小床，如需一張大床之房型請於報名時告知服務人員，我們將為您向飯店提出需求。但大床房數有限，是否住得到大床房需以當天入住情形為主，且若遇飯店指定房型需額外加價，敬請諒解。\u003cbr /\u003e8.旺季期間無法指定飯店，飯店安排順序及出發班機請以行前確定說明資料為準。\u003cbr /\u003e9.日本飯店床頭無須放小費，除茶包免費外，客房內或冰箱放置的零食、酒或飲料等，使用後請自行登記並至櫃台結帳。\u003cbr /\u003e10.如需加訂特殊餐食，(例如:不吃牛、素食、食物過敏等)請於出發前7天前，通知您的業務專員需求。\u003cbr /\u003e11.素食者貼心小叮嚀：因國家風俗民情不同，日本的素食者大多可食用蔥、薑、韭、蒜、辣椒、蛋、奶等食材，與國人之素食習慣有所不同。日本團體用餐除了華僑開的中華料理餐廳外，多數皆以蔬菜、豆腐、等食材料理火鍋為主。若為飯店內用自助餐或在外一般餐廳用餐，料理變化較少多數以生菜、漬物、水果等佐以白飯或麵類主食。敬告素食旅客前往日本旅遊，可自行準備素食。\u003cbr /\u003e12.請確認護照有效期限，需於本行程回國日起算6個月以上。\u003cbr /\u003e13.辦理新護照需7工作天，簽證工作天數依各地區而定；役男護照需自行加蓋出境章。\u003cbr /\u003e14.報名繳費後，觀光局國外旅遊定型化契約書即生效力，變更或取消行程依契約內容辦理。\u003cbr /\u003e15.網路行程僅供參考，出團之景點順序、出團名稱、航班、住宿及內容以說明會資料為準，一律以郵寄方式送達。\u003cbr /\u003e16.如遇颱風、交通擁塞、觀光點休假、住宿飯店調整或其他不可抗拒現象，則行程安排以當地導遊調整為主。\u003cbr /\u003e17. 飯店目前大都實行禁菸，客房基本上都是禁菸房。如有吸煙的客人可事先需求吸菸房，但不是每間飯店都有吸菸房。請務必告知客人注意飯店內的禁菸和吸菸標識，遵守酒店的規定。若於禁菸房內(包含浴室)都無法抽菸，違反者飯店會收取2萬日幣-6萬日幣不等的罰款。\u003c/span\u003e\u003c/span\u003e\u003c/p\u003e",
      "ItnDy955": "",
      "ItnDy959": "",
      "ItnDy960": "",
      "ItnDy990": "",
      "VisaRk": [
        
      ],
      "MainFeature": [
        
      ],
      "NatnRk": "",
      "AsmbPl": "",
      "AsmbPl2": "",
      "AsmbTm": "2019/11/27 15:10",
      "AsmbTm2": ""
    },
    "GrupBook": [
      {
        "bookSq": "01",
        "gbookDy": 1,
        "fltNo": "CI122",
        "depDt": "2019/11/27",
        "depTm": "17:10",
        "arrDt": "2019/11/27",
        "arrTm": "19:45",
        "routId": "TPE/OKA",
        "depPtCd": "TPE",
        "arrPtCd": "OKA",
        "DepCityNm": "台北市",
        "ArrCityNm": "沖繩",
        "DepAirpNm": "台北-桃園機場",
        "ArrAirpNm": "琉球-那霸機場",
        "carrNm": "中華航空"
      },
      {
        "bookSq": "01",
        "gbookDy": 4,
        "fltNo": "CI123",
        "depDt": "2019/11/30",
        "depTm": "20:50",
        "arrDt": "2019/11/30",
        "arrTm": "21:15",
        "routId": "OKA/TPE",
        "depPtCd": "OKA",
        "arrPtCd": "TPE",
        "DepCityNm": "沖繩",
        "ArrCityNm": "台北市",
        "DepAirpNm": "琉球-那霸機場",
        "ArrAirpNm": "台北-桃園機場",
        "carrNm": "中華航空"
      }
    ],
    "GrupGitn": [
      {
        "ItnDy": 1,
        "ItnNm": "桃園國際機場／那霸空港→住宿飯店",
        "ItnDr": "\u003cp\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e搭乘舒適班機抵達日本最南端之島嶼～有東方夏威夷之稱，浪漫多情的～琉球群島【沖繩】。抵達後由專業親切的導遊帶領各位貴賓開始四天愉快的旅程。\u003c/span\u003e\u003c/span\u003e\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#FF0000;\"\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e小叮嚀:\u003cbr /\u003e1.\u003cspan style=\"white-space:pre\"\u003e \u003c/span\u003e若為早班機出發，第一天與第四天行程對調。\u003cbr /\u003e2.\u003cspan style=\"white-space:pre\"\u003e \u003c/span\u003e若為早去晚回，最後一天為自由活動，午餐逛街自理。\u003c/span\u003e\u003c/span\u003e\u003c/span\u003e\u003c/p\u003e",
        "ItnHtl": "SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級",
        "HtlCd": "",
        "SameHtlFg": "0",
        "IsSameHtl": false,
        "ItnBr": false,
        "ItnLu": false,
        "ItnDi": true,
        "ItnBrDr": "",
        "ItnLuDr": "",
        "ItnDiDr": "機上輕食",
        "ItnHtls": [
          {
            "HtlCd": "",
            "HtlCName": "SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級",
            "HtlAName": "",
            "HtlEName": "",
            "WebUrl": null
          }
        ]
      },
      {
        "ItnDy": 2,
        "ItnNm": "ORION啤酒名護工廠→沖繩海洋博公園～海豚表演秀～珊瑚七色海～沖繩美之海水族館→流傳至今的沖繩昔有風景~備瀨福木林道→北谷美國村",
        "ItnDr": "\u003cp style=\"text-align: justify;\"\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e\u003cspan style=\"color:#0000FF;\"\u003e【名護Orion啤酒工廠】\u003c/span\u003e在沖繩提到啤酒就非ORION啤酒莫屬。它是以三星並列做為標誌的地方啤酒。其中ORION啤酒名護工廠因為開放工廠參觀而成為極受歡迎的景點。啤酒誕生前一項一項的製成參觀，了解如何製作新鮮且品質優良的啤酒。參觀工廠後還可試喝新鮮冷藏的啤酒唷!!\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【海洋博公園】\u003c/span\u003e紀念1975年在沖繩舉行的國際海洋博覽會，在會址上規劃設置的國營公園，占地70萬平方公尺；除了國際知名的「美麗海水族館」作為賞遊核心，園內還有「熱帶、亞熱帶都市綠化植物園」、「沖繩鄉土村‧歌謠植物園」等其他主題設施，都是為了讓來客能趁遊興更認識沖繩的海島生態、人文風情以及歷史沿革；四季安排不同節目，在原先便具散策價值的廣大園區綴出豐富觀光亮點。\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【美の海水族館】\u003c/span\u003e以「與沖繩海洋有約」為規劃精神，三層次主題展區：「珊瑚礁之旅」、「黑潮之旅」及「深海之旅」可以讓訪客將沖繩海域的豐富生態系盡覽無遺。其中東亞第一大水族箱，主缸「黑潮之海」，可觀察三隻8.6公尺長的巨型「鯨鯊」直立進食的特殊畫面，以及世界最大軟骨魚，且為培植重點的「鬼蝠魟」在面前翩翩迴游。由淺而深的海層介紹順序，利用上而下的參觀動線引領旅客從近海礁湖生態一路潛進深海之中，在開放式觸摸池拾海星、海參玩賞，在「珊瑚之海」見識人工養殖出的繽紛多彩，在「深層之海」依賴微光感受深海寂靜的異趣。從水族館頂樓的「漁夫之門」往海上望穿，可見翡翠海色與「伊江島」影，沖繩的自然風情伴隨旅程無所不在。\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【備瀨福木林道\u003c/span\u003e】福木在沖繩以前就時常被用來對付颱風的防風林，因此人民會模木種植在房屋的周圍。 備瀨地區的防風林就是種植大量的。\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【北谷美國村】\u003c/span\u003e位於那霸市東北的北谷町，模仿美國西岸風格，對美軍駐日基地遺址和緊鄰的海岸作綜合開發。美國村內擁有以3D映像為主體的娛樂設施，8個屏幕的電影館、購物中心、保齡球場、美國餐廳、錄音室、娛樂中心、進口雜貨店等，另備有運動的複合設施；巨大摩天輪是此地標誌(需自費)，白天乘坐可一覽北谷地區、眺望東海甚至觀賞夕日；晚上則閃爍霓虹，綴成本地浪漫燈飾。\u003c/span\u003e\u003c/span\u003e\u003c/p\u003e",
        "ItnHtl": "SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級",
        "HtlCd": "",
        "SameHtlFg": "0",
        "IsSameHtl": false,
        "ItnBr": true,
        "ItnLu": true,
        "ItnDi": false,
        "ItnBrDr": "飯店內用",
        "ItnLuDr": "山原御殿燒烤(或)水果樂園燒烤自助餐",
        "ItnDiDr": "為方便逛街～敬請自理",
        "ItnHtls": [
          {
            "HtlCd": "",
            "HtlCName": "SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級",
            "HtlAName": "",
            "HtlEName": "",
            "WebUrl": null
          }
        ]
      },
      {
        "ItnDy": 3,
        "ItnNm": "腳踏車漫遊～系満美美沖縄海岸→絕美海中道路~海の駅～ぬちマース鹽場見學~世界獨一無二的鹽雪景~幸福的絕壁～果報崖→國際通大道:元氣的洗禮",
        "ItnDr": "\u003cp style=\"text-align: justify;\"\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e\u003cspan style=\"color:#0000FF;\"\u003e【腳踏車漫遊】\u003c/span\u003e系滿是沖繩最南端的城市，也是最靠近台灣的城市，糸滿美美海灘是2006年完工的人造海灘，位於沖繩本島南部糸滿市的填海區域，糸滿漁港交流公園內。美美海灘擁有的獨特自然景觀，一片雪白細柔沙灘，加上藍色寬廣無邊果凍色漸層海洋，您可騎乘著腳踏車與三五好友迎向清爽海風。\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【海之駅】\u003c/span\u003e沖繩中部的「海中道路」是連接本島和「平安座島」的跨海大橋，除了能通往濱比嘉島、宮城島及伊計島的美麗海灘，還可在橋上欣賞沖繩海景、購買土產和用餐，是自駕旅客特別喜愛的景點。\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【鹽場見學】\u003c/span\u003e大海是一切生物的源泉，包含了生物必要的礦物質，大海之於生物就如同羊水之於新生命的誕生，ぬちマース工場使用世界第一的「常温瞬間空中結晶製塩法」將海水製成結晶有如雪結晶，在工場進行導覽的同時仿佛置身於雪國。\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【果報崖】\u003c/span\u003e意稱幸福的絕壁，果報在宮城的方言也是幸福的意思，是宮城島的代表性景點。眺望半月ンダチカナ浜沙灘，海底佈滿珊瑚礁，充滿了層次感。\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【國際通大道】\u003c/span\u003e二戰時嚴重毀損，但因地理位置通商便利，戰後又急速復興發展，被稱做「奇跡的1英哩」。全長1.6公里的大道兩側，百貨大樓、餐廳、軍品雜貨、特產、藥妝、工藝與時裝店面櫛比鱗次，旁支街道如睦橋通、平和通與市場本通等亦保有在地魅力與風情，許多特色店家隱身於此；終日遊客不斷，日本國內外旅人皆慕名來訪，朝氣滿滿是此地特徵。\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【國際通屋台村】\u003c/span\u003e2015年新開幕的觀光景點，位於國際通正中央。除了介紹離島資訊的離島市集之外，還有提供沖繩食材料理的20間屋台。想吃盡各種沖繩料理，無論是中式、西式、日式、酒還是家常菜都可以在這裡找到唷。***屋台村官網: http://www.okinawa-yatai.jp/\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#FF0000;\"\u003e小叮嚀:\u003cbr /\u003e☆腳踏車體驗，如遇天候不佳，無法騎乘，或因個人因素放棄，恕不退費。\u003c/span\u003e\u003c/span\u003e\u003c/span\u003e\u003c/p\u003e",
        "ItnHtl": "SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級",
        "HtlCd": "",
        "SameHtlFg": "0",
        "IsSameHtl": false,
        "ItnBr": true,
        "ItnLu": true,
        "ItnDi": false,
        "ItnBrDr": "飯店內用",
        "ItnLuDr": "沖繩風味手打麵(或)風味自助餐",
        "ItnDiDr": "為方便逛街～敬請自理",
        "ItnHtls": [
          {
            "HtlCd": "",
            "HtlCName": "SUN PLAZA HOTEL (或)GRANTIA NAHA. (或)CHURA RYUKYU(或)NEW CENTURY(或) OROX HOTEL (或)山之內HOTEL(或)AZAT HOTEL(或)同級",
            "HtlAName": "",
            "HtlEName": "",
            "WebUrl": null
          }
        ]
      },
      {
        "ItnDy": 4,
        "ItnNm": "免稅店→藥妝店→玉泉洞～王國村～琉球大鼓表演→OUTLET→那霸空港／桃園國際機場",
        "ItnDr": "\u003cp\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"color:#0000FF;\"\u003e【免稅店】\u003c/span\u003e選購健康食品與日式精品。\u003c/span\u003e\u003c/p\u003e\u003cp\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"color:#0000FF;\"\u003e【藥妝店】\u003c/span\u003e在此您可盡情採購日本純正的自然食品、美妝用品，以及各式各樣的新奇有趣的百貨雜物。\u003c/span\u003e\u003c/p\u003e\u003cp style=\"text-align: justify;\"\u003e\u003cspan style=\"font-size:16px;\"\u003e\u003cspan style=\"font-family:微軟正黑體;\"\u003e\u003cspan style=\"color:#0000FF;\"\u003e【王國村】\u003c/span\u003e全稱「沖繩世界文化王國」，為沖繩最大體驗琉球文化的主題園區；感受玉泉洞的鬼斧神工之後，人文的豐厚也邀您前來體味。在製陶、造酒、黑糖、玻璃、機織、藍染、竹材手造與皮件手造等工房見習，遊客皆有自費體驗留念的機會；而「傳統太鼓隊」的「EISA」傳統藝能太鼓秀，以及驅邪鎮煞「獅子舞」，壓倒性地魅力十足。\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【玉泉洞】\u003c/span\u003e總長5公里，於展區內開放1公里地底的驚心動魄；鐘乳石洞內各式石鐘乳、石筍、石柱足見造物奇巧，是大自然給人的震撼； 30萬年間積累出不同型態的結晶，日人極富聯想力地命名，如「昇龍之鐘」、「龍神之池」、「銀滝池」等等，各異其趣。\u003cbr /\u003e\u003cbr /\u003e\u003cspan style=\"color:#0000FF;\"\u003e【Outlet暢貨中心】\u003c/span\u003e仿古希臘建築打造的購物園區，聚集世界各地名牌，穿插於消費空間的休閒娛樂設施適合各種旅客組合前來在瘋購物之餘稍作舒緩，休息後再戰!\u003cbr /\u003e\u003cbr /\u003e前往機場搭乘豪華客機回到台灣，揮別團員回到可愛的家，結束這多彩多姿的四日之旅。\u003c/span\u003e\u003c/span\u003e\u003c/p\u003e",
        "ItnHtl": "溫暖的家",
        "HtlCd": "",
        "SameHtlFg": "0",
        "IsSameHtl": false,
        "ItnBr": true,
        "ItnLu": true,
        "ItnDi": false,
        "ItnBrDr": "飯店內用",
        "ItnLuDr": "８０種料理自助餐 (或)海景餐廳自助餐",
        "ItnDiDr": "為方便逛街～敬請自理",
        "ItnHtls": [
          {
            "HtlCd": "",
            "HtlCName": "溫暖的家",
            "HtlAName": "",
            "HtlEName": "",
            "WebUrl": null
          }
        ]
      }
    ],
    "GrupPrice": [
      {
        "BedTp": "1",
        "BedTpNm": "大人",
        "SaleAm": 14900,
        "AgtAm": 14900
      },
      {
        "BedTp": "2",
        "BedTpNm": "小孩佔床",
        "SaleAm": 14900,
        "AgtAm": 14900
      },
      {
        "BedTp": "3",
        "BedTpNm": "小孩不佔床",
        "SaleAm": 12900,
        "AgtAm": 12900
      },
      {
        "BedTp": "4",
        "BedTpNm": "加床",
        "SaleAm": 14900,
        "AgtAm": 14900
      },
      {
        "BedTp": "5",
        "BedTpNm": "嬰兒",
        "SaleAm": 2000,
        "AgtAm": 2000
      }
    ],
    "GrupGopItem": [
      
    ],
    "GrupGchGitn": [
      
    ],
    "GrupRegmRegses": [
      {
        "RegmCd": "0005",
        "RegmNm": "沖繩(琉球)",
        "RegsCd": "0001",
        "RegsNm": "沖繩(琉球)"
      }
    ],
    "BookingClasses": [
      {
        "Code": "Y",
        "Name": "經濟艙",
        "IncreasePrice": 0,
        "SaleQt": 19,
        "SubCd": "M"
      },
      {
        "Code": "C",
        "Name": "商務艙",
        "IncreasePrice": 0,
        "SaleQt": 0,
        "SubCd": "M"
      },
      {
        "Code": "F",
        "Name": "頭等艙",
        "IncreasePrice": 0,
        "SaleQt": 0,
        "SubCd": "M"
      },
      {
        "Code": "ED",
        "Name": "長榮豪華艙",
        "IncreasePrice": 0,
        "SaleQt": 0,
        "SubCd": "M"
      }
    ],
    "GrupInfo": {
      "OrderLink": {
        "Status": 3,
        "Name": "報名",
        "Url": "/EW/GO/GroupOrder.asp?prodCd=DTS19B27C4CI"
      },
      "GrupTag": {
        "GuaranteeFg": false,
        "PromoteFg": false,
        "HotTp": "",
        "HotTpNm": "",
        "IsShowGuarantee": false,
        "IsShowPromote": false,
        "IsShowHotTp": false
      },
      "OtherGrupLink": "/EW/GO/GroupList.asp?mGrupCd=DTS19-OKA006\u0026beginDt=2019/11/25"
    },
    "MetaInfos": [
      {
        "Name": "keywords",
        "Content": ""
      },
      {
        "Name": "description",
        "Content": ""
      }
    ],
    "OtherInfos": [
      {
        "Title": "小費",
        "Context": "日本：團體出發小費一天300元。\u0026#x0D;\n計劃票及自由行不需支付。\u0026lt;br\u0026gt;"
      },
      {
        "Title": "出團備註",
        "Context": ""
      },
      {
        "Title": "注意事項",
        "Context": ""
      },
      {
        "Title": "電壓",
        "Context": ""
      },
      {
        "Title": "時差",
        "Context": ""
      },
      {
        "Title": "電話通訊",
        "Context": ""
      },
      {
        "Title": "簽證護照",
        "Context": ""
      },
      {
        "Title": "幣值",
        "Context": ""
      },
      {
        "Title": "天氣",
        "Context": "請點選\u003ca href=\u0027http://www.cwb.gov.tw/V7/forecast/world/world_aa.htm\u0027 target=\u0027_blank\u0027\u003e天氣參考網址\u003c/a\u003e"
      },
      {
        "Title": "其他",
        "Context": ""
      }
    ],
    "Verify": false,
    "ErrMsg": null
  },
  "Verify": true,
  "ErrMsg": ""
}
 */
public class DataContainerGO
{
    public RootObjectGO ProductInfo { get; set; }
    public string Verify { get; set; }
    public string ErrMsg { get; set; }
}
public class RootObjectGO
{
    public string SiteTitle { get; set; }
    public string VerifyMsg { get; set; }
    public string AgreeMent { get; set; }
    public string RqPriceFg { get; set; }
    public GrupDycList GrupDyc { get; set; }
    public GrupStcList GrupStc { get; set; }
    public List<GrupBookList> GrupBook { get; set; }
    public List<GrupGitnList> GrupGitn { get; set; }
    public List<GrupPriceList> GrupPrice { get; set; }
    public List<GrupGopItemList> GrupGopItem { get; set; }
    public List<GrupGchGitnList> GrupGchGitn { get; set; }
    public List<GrupRegmRegsesList> GrupRegmRegses { get; set; }
    public List<BookingClassesList> BookingClasses { get; set; }
    public GrupInfoList GrupInfo { get; set; }
    public List<OtherInfos> OtherInfos { get; set; }
}
public class GrupDycList //定義 RootObjectGO 中的物件
{
    public string GrupCd { get; set; }
    public string SacctNo { get; set; }
    public string MgrupCd { get; set; }
    public string GrupSnm { get; set; }
    public string GrupLn { get; set; }
    public string GrupNt { get; set; }
    public string OrderDl { get; set; }
    public string LeavDt { get; set; }
    public string RetnDt { get; set; }
    public string LowSaleAm { get; set; }
    public string LowRefAm { get; set; }
    public string IsHaveProduct { get; set; }
    public string Promote { get; set; }
    public string Guarantee { get; set; }
    public string PortCd { get; set; }
    public string SaleQt { get; set; }
    public string FullSts { get; set; }
    public string ItnDy958 { get; set; }
    public string SignUpLink { get; set; }
    public string RqstYqt { get; set; }
}
public class GrupStcList //定義 RootObjectGO 中的物件
{
    public string DordFg { get; set; } //true
    public string DordAm { get; set; } //5000
    public string DordRate { get; set; } //0
    public string DordDl { get; set; } //2019/11/21
    public string DordDl2 { get; set; } //2
    public string GcwktmFg { get; set; } //false
    public string ItnNatn { get; set; } //日本
    public string ItnNatnCd { get; set; } //JP
    public string ItnCity { get; set; } //沖繩
    public string ItnCityCd { get; set; } //OKA
    public string ImgUrl { get; set; } ///eWeb_gogojp/IMGDB/000006/000298/00003847.jpg
    public string ItnRk2 { get; set; } //
    public string ItnDrD1 { get; set; } //
    public string ItnDy950 { get; set; } //
    public string ItnDy951 { get; set; } //
    public string ItnDy952 { get; set; } //
    public string ItnDy953 { get; set; } //
    public string ItnDy954 { get; set; } //
    public string ItnDy955 { get; set; } //
    public string ItnDy959 { get; set; } //
    public string ItnDy960 { get; set; } //
    public string ItnDy990 { get; set; } //
    public List<VisaRk> VisaRk { get; set; }
    public List<MainFeature> MainFeature { get; set; }
    public string NatnRk { get; set; } //
    public string AsmbPl { get; set; } //
    public string AsmbPl2 { get; set; } //
    public string AsmbTm { get; set; } //2019/11/27 15:10
    public string AsmbTm2 { get; set; } //
}
public class VisaRk //定義 RootObjectGO -> GrupStcList 中的物件
{
}
public class MainFeature //定義 RootObjectGO -> GrupStcList 中的物件
{
}
public class GrupBookList //定義 RootObjectGO 中的物件
{
    public string bookSq { get; set; } //01
    public string gbookDy { get; set; } //1
    public string fltNo { get; set; } //CI122
    public string depDt { get; set; } //2019/11/27
    public string depTm { get; set; } //17:10
    public string arrDt { get; set; } //2019/11/27
    public string arrTm { get; set; } //19:45
    public string routId { get; set; } //TPE/OKA
    public string depPtCd { get; set; } //TPE
    public string arrPtCd { get; set; } //OKA
    public string DepCityNm { get; set; } //台北市
    public string ArrCityNm { get; set; } //沖繩
    public string DepAirpNm { get; set; } //台北-桃園機場
    public string ArrAirpNm { get; set; } //琉球-那霸機場
    public string carrNm { get; set; } //中華航空
}
public class GrupGitnList //定義 RootObjectGO 中的物件
{
    public string ItnDy { get; set; } //1
    public string ItnNm { get; set; } //桃園國際機場／那霸空港→住宿飯店
    public string ItnDr { get; set; } //
    public string ItnHtl { get; set; } //SUN PLAZA HOTEL (或)
    public string HtlCd { get; set; } //
    public string SameHtlFg { get; set; } //0
    public string IsSameHtl { get; set; } //false
    public string ItnBr { get; set; } //false
    public string ItnLu { get; set; } //false
    public string ItnDi { get; set; } //true
    public string ItnBrDr { get; set; } //
    public string ItnLuDr { get; set; } //
    public string ItnDiDr { get; set; } //機上輕食
    public List<ItnHtls> ItnHtls { get; set; } //
}
public class ItnHtls //定義 RootObjectGO -> GrupGitnList 中的物件
{
    public string HtlCd { get; set; } //
    public string HtlCName { get; set; } //SUN PLAZA HOTEL (或)GRANTIA NAHA.
    public string HtlAName { get; set; } //
    public string HtlEName { get; set; } //
    public string WebUrl { get; set; } //null
}
public class GrupPriceList //定義 RootObjectGO 中的物件
{
    public string BedTp { get; set; } //1
    public string BedTpNm { get; set; } //大人
    public string SaleAm { get; set; } //14900,
    public string AgtAm { get; set; } //14900
}
public class GrupGopItemList //定義 RootObjectGO 中的物件
{
}
public class GrupGchGitnList //定義 RootObjectGO 中的物件
{
}
public class GrupRegmRegsesList //定義 RootObjectGO 中的物件
{
    public string RegmCd { get; set; } //"0005"　　　大分類
    public string RegmNm { get; set; } //"沖繩(琉球)"
    public string RegsCd { get; set; } //"0001"　　　小分類
    public string RegsNm { get; set; } //"沖繩(琉球)"
}
public class BookingClassesList //定義 RootObjectGO 中的物件
{
    public string Code { get; set; } //Y
    public string Name { get; set; } //經濟艙
    public string IncreasePrice { get; set; } //0
    public string SaleQt { get; set; } //19
    public string SubCd { get; set; } //M
}
public class GrupInfoList //定義 RootObjectGO 中的物件
{
    public string OrderLink { get; set; } //
    public string GrupTag { get; set; } //
    public string OtherGrupLink { get; set; } ///EW/GO/GroupList.asp?mGrupCd=DTS19-OKA006\u0026beginDt=2019/11/25"
}
public class OtherInfos //定義 RootObjectGO 中的物件
{
    public string Title { get; set; }
    public string Content { get; set; }
}
/*
 * 取得團體自由行詳細資訊
 * http://erp.gogojp.com.tw:8000/WMnet/API/product/PG/PgoInfo.ashx?prodCd=DTS19C11ICN3&prodType=G
*/
/*
{
  "Result": {
    "ProductCode": "DTS19C11ICN3",
    "BaseInfo": {
      "Name": "韓國首爾自由行五日-入住明洞TMARK HOTEL(兩晚)",
      "MGrupCd": "DTS19-ICN007",
      "Image": "erp.gogojp.com.tw/eWeb_gogojp/IMGDB/000019/000555/00012068.jpg",
      "MetaInfos": [
        {
          "Name": "keywords",
          "Content": ""
        },
        {
          "Name": "description",
          "Content": ""
        }
      ],
      "SaleTopic": null,
      "Include": "",
      "Exclude": "",
      "IsGuarantee": false,
      "IsPromote": false
    },
    "StcInfo": [
      {
        "No": 0,
        "Name": "行程特色",
        "Memo": ""
      },
      {
        "No": 0,
        "Name": "建議行程",
        "Memo": ""
      },
      {
        "No": 0,
        "Name": "商品備註",
        "Memo": "【報價包含】.台北/仁川/台北 酷航經濟艙團體來回機票。酷航如加購餐食後，如放棄機位使用權益餐食亦無法退費，敬請注意，且酷航保有變更之權利。"
      },
      {
        "No": 970,
        "Name": "同業備註",
        "Memo": ""
      },
      {
        "No": 971,
        "Name": "員工備註",
        "Memo": "實批1000"
      },
      {
        "No": 972,
        "Name": "優惠方案",
        "Memo": ""
      },
      {
        "No": 973,
        "Name": "自由行-優惠簡述16字",
        "Memo": ""
      },
      {
        "No": 975,
        "Name": "團體自由行-自動上架文字",
        "Memo": ""
      }
    ],
    "SeatInfo": [
      {
        "Class": "Y",
        "Total": 4,
        "Leader": 0,
        "Guide": 0,
        "Keep": 0,
        "Sold": 0,
        "SelfTrip": 0,
        "Available": 4
      },
      {
        "Class": "ED",
        "Total": 0,
        "Leader": 0,
        "Guide": 0,
        "Keep": 0,
        "Sold": 0,
        "SelfTrip": 0,
        "Available": 0
      },
      {
        "Class": "C",
        "Total": 0,
        "Leader": 0,
        "Guide": 0,
        "Keep": 0,
        "Sold": 0,
        "SelfTrip": 0,
        "Available": 0
      },
      {
        "Class": "F",
        "Total": 0,
        "Leader": 0,
        "Guide": 0,
        "Keep": 0,
        "Sold": 0,
        "SelfTrip": 0,
        "Available": 0
      }
    ],
    "FlightInfo": [
      {
        "Pnr": "",
        "No": "01",
        "Day": 1,
        "BookQt": 4,
        "FlightNo": "TR896",
        "DepartureDate": "2019/12/11",
        "DepartureTime": "18:00",
        "ArrivalDate": "2019/12/11",
        "ArrivalTime": "21:35",
        "SegmentRoute": "TPE/ICN",
        "Class": "Y",
        "BookingStatus": "HK",
        "TicketDeadLine": "",
        "BookingNote": "",
        "IsFrontShow": false,
        "AirCarrier": "酷航",
        "DepAirpNm": "台北-桃園機場",
        "ArrAirpNm": "仁川機場"
      },
      {
        "Pnr": "",
        "No": "01",
        "Day": 5,
        "BookQt": 4,
        "FlightNo": "TR897",
        "DepartureDate": "2019/12/15",
        "DepartureTime": "22:55",
        "ArrivalDate": "2019/12/16",
        "ArrivalTime": "00:30",
        "SegmentRoute": "ICN/TPE",
        "Class": "Y",
        "BookingStatus": "HK",
        "TicketDeadLine": "",
        "BookingNote": "",
        "IsFrontShow": false,
        "AirCarrier": "酷航",
        "DepAirpNm": "仁川機場",
        "ArrAirpNm": "台北-桃園機場"
      }
    ],
    "DailyGitnInfo": [
      {
        "Day": 1,
        "Title": "桃園國際機場／仁川國際機場→飯店",
        "City": [
          
        ],
        "Memo": "今日集合於桃園國際機場辦理出境手續後，飛往韓國仁川國際機場，精彩行程即將展開。",
        "Image": "http://erp.gogojp.com.tw:8000/WMnet/api/common/showImage.ashx?prodType=G\u0026prodCd=DTS19C11ICN3\u0026itnDy=1",
        "HotelInfo": {
          "IsSameLevel": false,
          "Detail": [
            {
              "Code": "",
              "Name": "明洞TMARK HOTEL (或) 同級",
              "EngName": ""
            }
          ]
        },
        "FoodInfo": {
          "Memo": "",
          "Detail": [
            {
              "Name": "早餐",
              "Memo": "X",
              "IsOffer": false
            },
            {
              "Name": "午餐",
              "Memo": "X",
              "IsOffer": false
            },
            {
              "Name": "晚餐",
              "Memo": "X",
              "IsOffer": false
            }
          ]
        },
        "ScensInfo": [
          
        ]
      },
      {
        "Day": 2,
        "Title": "首爾-自由活動(建議行程)",
        "City": [
          
        ],
        "Memo": "自選加購建議行程",
        "Image": "http://erp.gogojp.com.tw:8000/WMnet/api/common/showImage.ashx?prodType=G\u0026prodCd=DTS19C11ICN3\u0026itnDy=2",
        "HotelInfo": {
          "IsSameLevel": false,
          "Detail": [
            {
              "Code": "",
              "Name": "明洞TMARK HOTEL (或) 同級",
              "EngName": ""
            }
          ]
        },
        "FoodInfo": {
          "Memo": "",
          "Detail": [
            {
              "Name": "早餐",
              "Memo": "X",
              "IsOffer": false
            },
            {
              "Name": "午餐",
              "Memo": "X",
              "IsOffer": false
            },
            {
              "Name": "晚餐",
              "Memo": "X",
              "IsOffer": false
            }
          ]
        },
        "ScensInfo": [
          
        ]
      },
      {
        "Day": 3,
        "Title": "首爾-自由活動(建議行程)",
        "City": [
          
        ],
        "Memo": "可自行前往各大知名景點",
        "Image": "http://erp.gogojp.com.tw:8000/WMnet/api/common/showImage.ashx?prodType=G\u0026prodCd=DTS19C11ICN3\u0026itnDy=3",
        "HotelInfo": {
          "IsSameLevel": false,
          "Detail": [
            {
              "Code": "",
              "Name": "可加購 明洞TMARK HOTEL (或) 東大門 D7 SUITES HOTEL (或) IBC HOTEL (或)同級",
              "EngName": ""
            }
          ]
        },
        "FoodInfo": {
          "Memo": "",
          "Detail": [
            {
              "Name": "早餐",
              "Memo": "X",
              "IsOffer": false
            },
            {
              "Name": "午餐",
              "Memo": "X",
              "IsOffer": false
            },
            {
              "Name": "晚餐",
              "Memo": "X",
              "IsOffer": false
            }
          ]
        },
        "ScensInfo": [
          
        ]
      },
      {
        "Day": 4,
        "Title": "首爾-自由活動(建議行程)",
        "City": [
          
        ],
        "Memo": "自選加購建議行程",
        "Image": "http://erp.gogojp.com.tw:8000/WMnet/api/common/showImage.ashx?prodType=G\u0026prodCd=DTS19C11ICN3\u0026itnDy=4",
        "HotelInfo": {
          "IsSameLevel": false,
          "Detail": [
            {
              "Code": "",
              "Name": "可加購 明洞TMARK HOTEL (或) 東大門 D7 SUITES HOTEL (或) IBC HOTEL (或)同級",
              "EngName": ""
            }
          ]
        },
        "FoodInfo": {
          "Memo": "",
          "Detail": [
            {
              "Name": "早餐",
              "Memo": "X",
              "IsOffer": false
            },
            {
              "Name": "午餐",
              "Memo": "X",
              "IsOffer": false
            },
            {
              "Name": "晚餐",
              "Memo": "X",
              "IsOffer": false
            }
          ]
        },
        "ScensInfo": [
          
        ]
      },
      {
        "Day": 5,
        "Title": "首爾-自由活動(建議行程)",
        "City": [
          
        ],
        "Memo": "自選加購建議行程",
        "Image": "http://erp.gogojp.com.tw:8000/WMnet/api/common/showImage.ashx?prodType=G\u0026prodCd=DTS19C11ICN3\u0026itnDy=5",
        "HotelInfo": {
          "IsSameLevel": false,
          "Detail": [
            {
              "Code": "",
              "Name": "溫暖的家",
              "EngName": ""
            }
          ]
        },
        "FoodInfo": {
          "Memo": "",
          "Detail": [
            {
              "Name": "早餐",
              "Memo": "X",
              "IsOffer": false
            },
            {
              "Name": "午餐",
              "Memo": "X",
              "IsOffer": false
            },
            {
              "Name": "晚餐",
              "Memo": "X",
              "IsOffer": false
            }
          ]
        },
        "ScensInfo": [
          
        ]
      }
    ],
    "HotelInfo": [
      {
        "PhoneNumber": "",
        "Code": "SEL001",
        "Name": "帝馬克酒店明洞",
        "EngName": "TMARK HOTEL MYEONGDONG",
        "Class": "",
        "ClassDr": "",
        "CityCd": "SEL",
        "CityName": "首爾",
        "Synopsis": "帝瑪克酒店明洞”在文化，購物，以及古現代和諧的中心首爾明洞忠武路，交通便捷。",
        "Url": "tmarkhotel.com/accommodation/",
        "MapUrl": ""
      }
    ],
    "VisaInfo": [
      {
        "Appldy": "1",
        "VldeDt": 0,
        "BeDate": "0",
        "VldeDay": 0,
        "VldeMonth": 0,
        "VldeYear": 0,
        "Memo": "",
        "Describe": "免簽證\r\n",
        "Code": "SEL0",
        "Name": "韓國簽證(免簽證)"
      }
    ],
    "OtherInfo": [
      {
        "Title": "小費",
        "Context": "韓國：領隊及國外導遊司機服務小費不包括在團費內，依當地行情每位旅客每日新台幣 200 元；"
      },
      {
        "Title": "電壓",
        "Context": ""
      },
      {
        "Title": "時差",
        "Context": ""
      },
      {
        "Title": "電話通訊",
        "Context": ""
      },
      {
        "Title": "簽證護照",
        "Context": ""
      },
      {
        "Title": "幣值",
        "Context": ""
      },
      {
        "Title": "天氣",
        "Context": "請點選"
      },
      {
        "Title": "其他",
        "Context": ""
      }
    ]
  },
  "Verify": true,
  "ErrMsg": ""
}
*/
public class DataContainerPGO
{
    public RootObjectPGO Result { get; set; }
    public string Verify { get; set; }
    public string ErrMsg { get; set; }
}
public class RootObjectPGO
{
    public string ProductCode { get; set; }
    public BaseInfo BaseInfo { get; set; }
    public List<StcInfo> StcInfo { get; set; }
    public List<SeatInfo> SeatInfo { get; set; }
    public List<FlightInfo> FlightInfo { get; set; }
    public List<DailyGitnInfo> DailyGitnInfo { get; set; }
    public List<HotelInfo> HotelInfo { get; set; }
    public List<VisaInfo> VisaInfo { get; set; }
    public List<OtherInfos> OtherInfos { get; set; }
}
public class BaseInfo //定義 RootObjectPGO 中的物件
{
    public string Name { get; set; }
    public string MGrupCd { get; set; }
    public string Image { get; set; }
    public List<MetaInfos> MetaInfos { get; set; }
    public string SaleTopic { get; set; }
    public string Include { get; set; }
    public string Exclude { get; set; }
    public string IsGuarantee { get; set; }
    public string IsPromote { get; set; }
}
public class StcInfo //定義 RootObjectPGO 中的物件
{
    public string No { get; set; } //0 0 0 970
    public string Name { get; set; } //行程特色 建議行程 商品備註 同業備註
    public string Memo { get; set; } //【報價包含】.台北/仁川/台北 酷航經濟艙團體來回機票。酷航如加購餐食後，如放棄機位使用權益餐食亦無法退費，敬請注意，且酷航保有變更之權利。
}
public class SeatInfo //定義 RootObjectPGO 中的物件
{
    public string Class { get; set; }//Y
    public string Total { get; set; }//4
    public string Leader { get; set; }//0
    public string Guide { get; set; }//0
    public string Keep { get; set; }//0
    public string Sold { get; set; }//0
    public string SelfTrip { get; set; }//0
    public string Available { get; set; }//4
}
public class FlightInfo //定義 RootObjectPGO 中的物件
{
    public string Pnr { get; set; }//
    public string No { get; set; }//01
    public string Day { get; set; }//1
    public string BookQt { get; set; }//4
    public string FlightNo { get; set; }//TR896
    public string DepartureDate { get; set; }//2019/12/11
    public string DepartureTime { get; set; }//18:00
    public string ArrivalDate { get; set; }//2019/12/11
    public string ArrivalTime { get; set; }//21:35
    public string SegmentRoute { get; set; }//TPE/ICN
    public string Class { get; set; }//Y
    public string BookingStatus { get; set; }//HK
    public string TicketDeadLine { get; set; }//
    public string BookingNote { get; set; }//
    public string IsFrontShow { get; set; }//false
    public string AirCarrier { get; set; }//酷航
    public string DepAirpNm { get; set; }//台北-桃園機場
    public string ArrAirpNm { get; set; }//仁川機場
}
public class DailyGitnInfo //定義 RootObjectPGO 中的物件
{
    public string Day { get; set; }
    public string Title { get; set; }
    public List<DailyGitnInfoCity> City { get; set; }
    public string Memo { get; set; }
    public string Image { get; set; }
    public DailyGitnInfoHotelInfo HotelInfo { get; set; }
    public FoodInfo FoodInfo { get; set; }
    public List<DailyGitnInfoScensInfo> ScensInfo { get; set; }
    public string IsGuarantee { get; set; }
    public string IsPromote { get; set; }
}
public class DailyGitnInfoCity //定義 RootObjectPGO -> DailyGitnInfo 中的物件
{
}
public class DailyGitnInfoHotelInfo //定義 RootObjectPGO -> DailyGitnInfo 中的物件
{
    public string IsSameLevel { get; set; }//false
    public List<HotelInfoDetail> Detail { get; set; }
}
public class HotelInfoDetail //定義 RootObjectPGO -> DailyGitnInfo -> HotelInfo 中的物件
{
    public string Code { get; set; }//
    public string Name { get; set; }//明洞TMARK HOTEL (或) 同級
    public string EngName { get; set; }//
}
public class FoodInfo //定義 RootObjectPGO -> DailyGitnInfo 中的物件
{
    public string Memo { get; set; }//
    public List<FoodInfoDetail> Detail { get; set; }
}
public class FoodInfoDetail //定義 RootObjectPGO -> DailyGitnInfo -> FoodInfo 中的物件
{
    public string Name { get; set; }//早餐
    public string Memo { get; set; }//X
    public string IsOffer { get; set; }//false
}
public class DailyGitnInfoScensInfo //定義 RootObjectPGO -> DailyGitnInfo 中的物件
{
}
public class HotelInfo //定義 RootObjectPGO 中的物件
{
    public string PhoneNumber { get; set; }//
    public string Code { get; set; }//SEL001
    public string Name { get; set; }//帝馬克酒店明洞
    public string EngName { get; set; }//TMARK HOTEL MYEONGDONG
    public string Class { get; set; }//
    public string ClassDr { get; set; }//
    public string CityCd { get; set; }//SEL
    public string CityName { get; set; }//首爾
    public string Synopsis { get; set; }//帝瑪克酒店明洞”在文化，購物，以及古現代和諧的中心首爾明洞忠武路，交通便捷。
    public string Url { get; set; }//tmarkhotel.com/accommodation/
    public string MapUrl { get; set; }//
}
public class VisaInfo //定義 RootObjectPGO 中的物件
{
    public string Appldy { get; set; }//1
    public string VldeDt { get; set; }//0
    public string BeDate { get; set; }//0
    public string VldeDay { get; set; }//0
    public string VldeMonth { get; set; }//0
    public string VldeYear { get; set; }//0
    public string Memo { get; set; }//
    public string Describe { get; set; }//免簽證
    public string Code { get; set; }//SEL0
    public string Name { get; set; }//韓國簽證(免簽證)
}
/*
 * 取得團體自由行訂購時所需要的詳細資訊
 * http://erp.gogojp.com.tw:8000/WMnet/API/Product/PG/PgoOrderInfo.ashx?prodCd=DTS19C11ICN3&amrnk=1&refAmrnk=1
*/
/*
{
  "Result": {
    "ProductCode": "DTS19C11ICN3",
    "MProductCode": "DTS19-ICN007",
    "AgreeMent": "/eweb_gogojp/public/PGO_Agreement.txt",
    "Category": [
      {
        "RegmCode": "0003",
        "RegmName": "韓國",
        "RegsCode": "0001",
        "RegsName": "首爾",
        "SortSq": 0
      }
    ],
    "MarketInfo": {
      "Status": "開團",
      "IsSelling": true,
      "LeaveDate": "2019/12/11",
      "RetnDate": "2019/12/15",
      "ClosingDate": "2019/11/25",
      "DordDate": "2019/11/25",
      "MinNumberTrip": 16,
      "IsDord": true,
      "DordAm": 0,
      "DordNote": "報名後2天內 訂金:NT$5,000",
      "SalesStatus": "一般",
      "SalesTitle": "韓國首爾自由行五日-入住明洞TMARK HOTEL(兩晚)",
      "Dord": {
        "DordDl": "2019/11/25",
        "DordAm": 5000,
        "DordRate": 0,
        "DordFg": true,
        "DordDl2": 2
      }
    },
    "OrderLink": {
      "Status": 3,
      "Name": "報名",
      "Url": "/EW/PG/PgOrder.asp?prodCd=DTS19C11ICN3"
    },
    "SeatInfo": [
      {
        "Class": "Y",
        "Total": 4,
        "Leader": 0,
        "Guide": 0,
        "Keep": 0,
        "Sold": 0,
        "SelfTrip": 0,
        "Available": 4
      },
      {
        "Class": "ED",
        "Total": 0,
        "Leader": 0,
        "Guide": 0,
        "Keep": 0,
        "Sold": 0,
        "SelfTrip": 0,
        "Available": 0
      },
      {
        "Class": "C",
        "Total": 0,
        "Leader": 0,
        "Guide": 0,
        "Keep": 0,
        "Sold": 0,
        "SelfTrip": 0,
        "Available": 0
      },
      {
        "Class": "F",
        "Total": 0,
        "Leader": 0,
        "Guide": 0,
        "Keep": 0,
        "Sold": 0,
        "SelfTrip": 0,
        "Available": 0
      }
    ],
    "PriceInfo": [
      {
        "Code": "001",
        "Memo": "明洞TMARK HOTEL MYEONGDONG(2晚)",
        "Price": [
          {
            "BedType": "1",
            "BedName": "大人",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 10900,
            "ReferenceAm": 10900
          },
          {
            "BedType": "2",
            "BedName": "小孩佔床",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 10900,
            "ReferenceAm": 10900
          },
          {
            "BedType": "3",
            "BedName": "小孩不佔床",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 7900,
            "ReferenceAm": 7900
          },
          {
            "BedType": "5",
            "BedName": "嬰兒",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 4000,
            "ReferenceAm": 4000
          }
        ],
        "Hotel": [
          {
            "ItnDy": 1,
            "ItnEdy": 2,
            "PgrpDr": "明洞TMARK HOTEL MYEONGDONG(2晚)",
            "SameHtlFg": true,
            "Hotel": [
              {
                "Code": "SEL001",
                "Name": "帝馬克酒店明洞",
                "EngName": "TMARK HOTEL MYEONGDONG"
              }
            ],
            "Food": {
              "Memo": "",
              "Detail": [
                {
                  "Name": "早餐",
                  "Memo": "",
                  "IsOffer": false
                },
                {
                  "Name": "午餐",
                  "Memo": "",
                  "IsOffer": false
                },
                {
                  "Name": "晚餐",
                  "Memo": "",
                  "IsOffer": false
                }
              ]
            }
          }
        ]
      },
      {
        "Code": "002",
        "Memo": "TMARK HOTEL明洞(2晚)+D7 SUITES東大門(2晚)",
        "Price": [
          {
            "BedType": "1",
            "BedName": "大人",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 13900,
            "ReferenceAm": 13900
          },
          {
            "BedType": "2",
            "BedName": "小孩佔床",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 13900,
            "ReferenceAm": 13900
          },
          {
            "BedType": "3",
            "BedName": "小孩不佔床",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 7900,
            "ReferenceAm": 7900
          },
          {
            "BedType": "5",
            "BedName": "嬰兒",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 4000,
            "ReferenceAm": 4000
          }
        ],
        "Hotel": [
          {
            "ItnDy": 1,
            "ItnEdy": 2,
            "PgrpDr": "TMARK HOTEL明洞(2晚)+D7 SUITES東大門(2晚)",
            "SameHtlFg": true,
            "Hotel": [
              {
                "Code": "SEL001",
                "Name": "帝馬克酒店明洞",
                "EngName": "TMARK HOTEL MYEONGDONG"
              }
            ],
            "Food": {
              "Memo": "",
              "Detail": [
                {
                  "Name": "早餐",
                  "Memo": "",
                  "IsOffer": false
                },
                {
                  "Name": "午餐",
                  "Memo": "",
                  "IsOffer": false
                },
                {
                  "Name": "晚餐",
                  "Memo": "",
                  "IsOffer": false
                }
              ]
            }
          },
          {
            "ItnDy": 3,
            "ItnEdy": 4,
            "PgrpDr": "TMARK HOTEL明洞(2晚)+D7 SUITES東大門(2晚)",
            "SameHtlFg": true,
            "Hotel": [
              {
                "Code": "",
                "Name": "D7 SUITES DONGDAEMUN",
                "EngName": ""
              }
            ],
            "Food": {
              "Memo": "",
              "Detail": [
                {
                  "Name": "早餐",
                  "Memo": "",
                  "IsOffer": false
                },
                {
                  "Name": "午餐",
                  "Memo": "",
                  "IsOffer": false
                },
                {
                  "Name": "晚餐",
                  "Memo": "",
                  "IsOffer": false
                }
              ]
            }
          }
        ]
      },
      {
        "Code": "003",
        "Memo": "明洞TMARK HOTEL MYEONGDONG(4晚)",
        "Price": [
          {
            "BedType": "1",
            "BedName": "大人",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 14900,
            "ReferenceAm": 14900
          },
          {
            "BedType": "2",
            "BedName": "小孩佔床",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 14900,
            "ReferenceAm": 14900
          },
          {
            "BedType": "3",
            "BedName": "小孩不佔床",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 7900,
            "ReferenceAm": 7900
          },
          {
            "BedType": "5",
            "BedName": "嬰兒",
            "RoomType": "2",
            "RoomName": "雙人房",
            "RoomMaxPeople": "2",
            "SaleAm": 4000,
            "ReferenceAm": 4000
          }
        ],
        "Hotel": [
          {
            "ItnDy": 1,
            "ItnEdy": 4,
            "PgrpDr": "明洞TMARK HOTEL MYEONGDONG(4晚)",
            "SameHtlFg": true,
            "Hotel": [
              {
                "Code": "SEL001",
                "Name": "帝馬克酒店明洞",
                "EngName": "TMARK HOTEL MYEONGDONG"
              }
            ],
            "Food": {
              "Memo": "",
              "Detail": [
                {
                  "Name": "早餐",
                  "Memo": "",
                  "IsOffer": false
                },
                {
                  "Name": "午餐",
                  "Memo": "",
                  "IsOffer": false
                },
                {
                  "Name": "晚餐",
                  "Memo": "",
                  "IsOffer": false
                }
              ]
            }
          }
        ]
      }
    ],
    "OptionsGitnInfo": [
      
    ],
    "ExtraGitnInfo": [
      
    ],
    "OtherPgoInfo": [
      {
        "LeavDt": "2019/12/04",
        "GroupList": "DTS19C04ICN4"
      },
      {
        "LeavDt": "2019/12/11",
        "GroupList": "DTS19C11ICN3"
      }
    ]
  },
  "Verify": true,
  "ErrMsg": null
}
*/
public class DataContainerPGOOrder
{
    public RootObjectPGOOrder Result { get; set; }
    public string Verify { get; set; }
    public string ErrMsg { get; set; }
}
public class RootObjectPGOOrder
{
    public string ProductCode { get; set; }//DTS19C11ICN3
    public string MProductCode { get; set; }//DTS19-ICN007
    public string AgreeMent { get; set; }///eweb_gogojp/public/PGO_Agreement.txt
    public List<Category> Category { get; set; }
    public MarketInfo MarketInfo { get; set; }
    public OrderLink OrderLink { get; set; }
    public List<SeatInfo> SeatInfo { get; set; }
    public List<PriceInfo> PriceInfo { get; set; }
    public List<OptionsGitnInfo> OptionsGitnInfo { get; set; }
    public List<ExtraGitnInfo> ExtraGitnInfo { get; set; }
    public List<OtherPgoInfo> OtherPgoInfo { get; set; }
}
public class Category //定義 RootObjectPGOOrder 中的物件
{
    public string RegmCode { get; set; }//0003
    public string RegmName { get; set; }//韓國
    public string RegsCode { get; set; }//0001
    public string RegsName { get; set; }//首爾
    public string SortSq { get; set; }//0
}
public class MarketInfo //定義 RootObjectPGOOrder 中的物件
{
    public string Status { get; set; }//開團
    public string IsSelling { get; set; }//true
    public string LeaveDate { get; set; }//2019/12/11
    public string RetnDate { get; set; }//2019/12/15
    public string ClosingDate { get; set; }//2019/11/25
    public string DordDate { get; set; }//2019/11/25
    public string MinNumberTrip { get; set; }//16
    public string IsDord { get; set; }//true
    public string DordAm { get; set; }//0
    public string DordNote { get; set; }//報名後2天內 訂金:NT$5,000
    public string SalesStatus { get; set; }//一般
    public string SalesTitle { get; set; }//韓國首爾自由行五日-入住明洞TMARK HOTEL(兩晚)
    public MarketInfoDord Dord { get; set; }//
}
public class MarketInfoDord //定義 RootObjectPGOOrder 中的物件
{
    public string DordDl { get; set; }//2019/11/25
    public string DordAm { get; set; }//5000
    public string DordRate { get; set; }//0
    public string DordFg { get; set; }//true
    public string DordDl2 { get; set; }//2
}
public class OrderLink //定義 RootObjectPGOOrder 中的物件
{
    public string Status { get; set; }//3
    public string Name { get; set; }//韓國
    public string Url { get; set; }///EW/PG/PgOrder.asp?prodCd=DTS19C11ICN3
}
public class PriceInfo //定義 RootObjectPGOOrder 中的物件
{
    public string Code { get; set; }//001
    public string Memo { get; set; }//明洞TMARK HOTEL MYEONGDONG(2晚)
    public List<PriceInfoPrice> Price { get; set; }
    public List<PriceInfoHotel> Hotel { get; set; }
}
public class PriceInfoPrice //定義 RootObjectPGOOrder PriceInfo 中的物件
{
    public string BedType { get; set; }//1
    public string BedName { get; set; }//大人
    public string RoomType { get; set; }//2
    public string RoomName { get; set; }//雙人房
    public string RoomMaxPeople { get; set; }//2
    public string SaleAm { get; set; }//10900
    public string ReferenceAm { get; set; }//10900
}
public class PriceInfoHotel //定義 RootObjectPGOOrder PriceInfo 中的物件
{
    public string ItnDy { get; set; }//1
    public string ItnEdy { get; set; }//2
    public string PgrpDr { get; set; }//明洞TMARK HOTEL MYEONGDONG(2晚)
    public string SameHtlFg { get; set; }//true
    public List<PriceInfoHotelHotel> Hotel { get; set; }
    public PriceInfoHotelFood Food { get; set; }//10900
    public string ReferenceAm { get; set; }//10900
}
public class PriceInfoHotelHotel //定義 RootObjectPGOOrder -> PriceInfo -> PriceInfoHotel 中的物件
{
    public string Code { get; set; }//SEL001
    public string Name { get; set; }//帝馬克酒店明洞
    public string EngName { get; set; }//TMARK HOTEL MYEONGDONG
}
public class PriceInfoHotelFood //定義 RootObjectPGOOrder -> PriceInfo -> PriceInfoHotel 中的物件
{
    public string Memo { get; set; }//
    public List<PriceInfoHotelFoodDetail> Detail { get; set; }//
}
public class PriceInfoHotelFoodDetail //定義 RootObjectPGOOrder -> PriceInfo -> PriceInfoHotel -> PriceInfoHotelFood中的物件
{
    public string Name { get; set; }//早餐
    public string Memo { get; set; }
    public string ReIsOffergsCode { get; set; }//false
}
public class OptionsGitnInfo //定義 RootObjectPGOOrder 中的物件
{
}
public class ExtraGitnInfo //定義 RootObjectPGOOrder 中的物件
{
}
public class OtherPgoInfo //定義 RootObjectPGOOrder 中的物件
{
    public string LeavDt { get; set; }//2019/12/04
    public string GroupList { get; set; }//DTS19C04ICN4
}
