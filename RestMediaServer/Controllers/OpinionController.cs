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
        public async Task<IEnumerable<Opinion>> Get()
        {
            return await new OpinionService().GetAll();
        }

        // GET api/Opinion/id
        public async Task<Opinion> Get(long id)
        {
            return await new OpinionService().GetById(id);
        }            

        public async Task<long> Post([FromBody]Opinion Opinion)
        {
            return await new OpinionService().Insert(Opinion);
        }

        // PUT api/values/5
        public async Task<long> Put([FromBody]Opinion Opinion)
        {
            return await new OpinionService().Update(Opinion);
        }

        // DELETE api/values/5
        public async Task<long> Delete(long id)
        {
            return await new OpinionService().Delete(id);
        }

    }
}
