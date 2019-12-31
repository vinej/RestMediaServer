using System.Collections.Generic;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;
using WebApi.Jwt.Filters;

namespace RestMediaServer.Controllers
{
    public class OpinionController : ApiController
    {
        // GET api/Opinion
        [JwtAuthentication]
        public IEnumerable<Opinion> Get()
        {
            return new OpinionService().GetAll();
        }

        // GET api/Opinion/id
        [JwtAuthentication]
        public Opinion Get(long id)
        {
            return new OpinionService().GetById(id);
        }

        [JwtAuthentication]
        public long Post([FromBody]Opinion Opinion)
        {
            return new OpinionService().Insert(Opinion);
        }

        // PUT api/values/5
        [JwtAuthentication]
        public long Put([FromBody]Opinion Opinion)
        {
            return new OpinionService().Update(Opinion);
        }

        // DELETE api/values/5
        [JwtAuthentication]
        public long Delete(long id)
        {
            return  new OpinionService().Delete(id);
        }

    }
}
