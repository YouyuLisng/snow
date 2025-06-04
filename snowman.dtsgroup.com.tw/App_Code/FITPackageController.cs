using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

public class FITPackageController : ApiController
{

    // GET api/<controller>
    public List<tblFITPackage> Get(int ItemType)
    {
        using (DataModel dm = new DataModel())
        {
            dm.Configuration.ProxyCreationEnabled = false;
            var fpi = dm.tblFITPackage.Where(a => a.Enable && !a.Del && a.PackageCat == ItemType).ToList();
            return fpi;
        }
        //return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public tblFITPackage Get(int ItemType, int id)
    {
        using (DataModel dm = new DataModel())
        {
            tblFITPackage fpi = dm.tblFITPackage.Where(a => a.PackageID == id && a.Enable && !a.Del && a.PackageCat == ItemType).FirstOrDefault();
            return fpi;
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
