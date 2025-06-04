using EntitytoJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

public class ProductOptController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public string Get(string ItemType)
    {
        var serializer = new JavaScriptSerializer
        {
            MaxJsonLength = 1024000000,
            RecursionLimit = 100
        };

        serializer.RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter() });
        using (DataModel dm = new DataModel()) {
            var opt = dm.tblProductOpt.Where(a => a.ProductTourID == ItemType && a.ProductOptEnable ).Select(a=>a.ProductOptCode);
            var optde = dm.tblProductOptDetail.Where(a => a.ProductOptEnable && opt.Contains(a.ProductOptID.ToString())&& a.ProductOptImage.Length>0).ToList();
            return serializer.Serialize(optde);
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
