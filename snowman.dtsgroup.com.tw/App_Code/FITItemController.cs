using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
public class FITItemController : ApiController
{
    
    // GET api/<controller>
    public List<tblFITPackageItem> Get( int ItemType)
    {
        using (DataModel dm = new DataModel())
        {
            dm.Configuration.ProxyCreationEnabled = false;
            //var fpi = dm.tblFITPackageItem.Where(a => a.Enable && !a.Del && a.ItemType == ItemType).ToList();
            var fpi = dm.tblFITPackageItem.Where(a => a.Enable && !a.Del && a.PackageID == ItemType).OrderBy(a => a.Sort).ToList();
            return fpi;
        }
        //return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public tblFITPackageItem Get(int ItemType, int id)
    {
        using (DataModel dm = new DataModel())
        {
            dm.Configuration.ProxyCreationEnabled = false;
            tblFITPackageItem fpi = dm.tblFITPackageItem.Where(a => a.ItemID == id && a.Enable && !a.Del).FirstOrDefault();
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
