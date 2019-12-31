using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;

namespace RestMediaServer.Controllers
{
    public class OpinionController : ApiController
    {
        // GET api/Opinion
        public IEnumerable<Opinion> Get()
        {
            return new OpinionService().GetAll();
        }

        // GET api/Opinion/id
        public Opinion Get(long id)
        {
            return new OpinionService().GetById(id);
        }            

        public long Post([FromBody]Opinion Opinion)
        {
            return new OpinionService().Insert(Opinion);
        }

        // PUT api/values/5
        public long Put([FromBody]Opinion Opinion)
        {
            return new OpinionService().Update(Opinion);
        }

        // DELETE api/values/5
        public long Delete(long id)
        {
            return  new OpinionService().Delete(id);
        }

    }
}
