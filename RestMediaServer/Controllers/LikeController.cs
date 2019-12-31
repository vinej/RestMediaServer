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
        public IEnumerable<Like> Get()
        {
            return  new LikeService().GetAll();
        }

        // GET api/Like/id
        public Like Get(long id)
        {
            return  new LikeService().GetById(id);
        }            

        public long Post([FromBody]Like Like)
        {
            return  new LikeService().Insert(Like);
        }

        // PUT api/values/5
        public long Put([FromBody]Like Like)
        {
            return  new LikeService().Update(Like);
        }

        // DELETE api/values/5
        public long Delete(long id)
        {
            return  new LikeService().Delete(id);
        }

    }
}
