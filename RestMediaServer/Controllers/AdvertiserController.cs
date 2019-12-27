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
        public async Task<IEnumerable<Advertiser>> Get()
        {
            return await new AdvertiserService().GetAll();
        }

        // GET api/Advertiser/id
        public async Task<Advertiser> Get(long id)
        {
            return await new AdvertiserService().GetById(id);
        }            

        public async Task<long> Post([FromBody]Advertiser Advertiser)
        {
            return await new AdvertiserService().Insert(Advertiser);
        }

        // PUT api/values/5
        public async Task<long> Put([FromBody]Advertiser Advertiser)
        {
            return await new AdvertiserService().Update(Advertiser);
        }

        // DELETE api/values/5
        public async Task<long> Delete(long id)
        {
            return await new AdvertiserService().Delete(id);
        }

    }
}
