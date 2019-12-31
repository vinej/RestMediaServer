using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;

namespace RestMediaServer.Controllers
{
    public class AdvertiserController : ApiController
    {
        // GET api/Advertiser
        public IEnumerable<Advertiser> Get()
        {
            return  new AdvertiserService().GetAll();
        }

        // GET api/Advertiser/id
        public Advertiser Get(long id)
        {
            return  new AdvertiserService().GetById(id);
        }            

        public long Post([FromBody]Advertiser Advertiser)
        {
            return  new AdvertiserService().Insert(Advertiser);
        }

        // PUT api/values/5
        public long Put([FromBody]Advertiser Advertiser)
        {
            return  new AdvertiserService().Update(Advertiser);
        }

        // DELETE api/values/5
        public long Delete(long id)
        {
            return  new AdvertiserService().Delete(id);
        }

    }
}
