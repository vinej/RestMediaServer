using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;

namespace RestMediaServer.Controllers
{
    public class LikeController : ApiController
    {
        // GET api/Like
        public async Task<IEnumerable<Like>> Get()
        {
            return await new LikeService().GetAll();
        }

        // GET api/Like/id
        public async Task<Like> Get(long id)
        {
            return await new LikeService().GetById(id);
        }            

        public async Task<long> Post([FromBody]Like Like)
        {
            return await new LikeService().Insert(Like);
        }

        // PUT api/values/5
        public async Task<long> Put([FromBody]Like Like)
        {
            return await new LikeService().Update(Like);
        }

        // DELETE api/values/5
        public async Task<long> Delete(long id)
        {
            return await new LikeService().Delete(id);
        }

    }
}
