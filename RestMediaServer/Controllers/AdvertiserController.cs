using System.Collections.Generic;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;
using WebApi.Jwt.Filters;

namespace RestMediaServer.Controllers
{
    public class AdvertiserController : ApiController
    {
        // GET api/Advertiser
        [JwtAuthentication]
        public IEnumerable<Advertiser> Get()
        {
            return  new AdvertiserService().GetAll();
        }

        // GET api/Advertiser/id
        [JwtAuthentication]
        public Advertiser Get(long id)
        {
            return  new AdvertiserService().GetById(id);
        }

        [JwtAuthentication]
        public long Post([FromBody]Advertiser Advertiser)
        {
            return  new AdvertiserService().Insert(Advertiser);
        }

        // PUT api/values/5
        [JwtAuthentication]
        public long Put([FromBody]Advertiser Advertiser)
        {
            return  new AdvertiserService().Update(Advertiser);
        }

        // DELETE api/values/5
        [JwtAuthentication]
        public long Delete(long id)
        {
            return  new AdvertiserService().Delete(id);
        }

    }
}
