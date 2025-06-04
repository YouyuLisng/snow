using EntitytoJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

public class OptController : ApiController
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
    public object Get(string ItemType, int id)
    {
        

        using (DataModel dm = new DataModel()) {
            var serializer = new JavaScriptSerializer
            {
                MaxJsonLength = 1024000000,
                RecursionLimit = 100
            };

            serializer.RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter() });
            if (ItemType != "0")
            {
                var productopt = dm.tblProductOpt.Where(a => a.ProductOptEnable && a.ProductTourID == ItemType).ToList();

                return serializer.Serialize(productopt);
            }
            if (id != 0)
            {
                var productopt = dm.tblProductOpt.Where(a => a.ProductOptEnable && a.ProductID == id).ToList();
                return serializer.Serialize(productopt);
            }

            return null;
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
