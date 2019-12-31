using System.Collections.Generic;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;
using WebApi.Jwt.Filters;

namespace RestMediaServer.Controllers
{
    public class LikeController : ApiController
    {
        // GET api/Like
        [JwtAuthentication]
        public IEnumerable<Like> Get()
        {
            return  new LikeService().GetAll();
        }

        // GET api/Like/id
        [JwtAuthentication]
        public Like Get(long id)
        {
            return  new LikeService().GetById(id);
        }

        [JwtAuthentication]
        public long Post([FromBody]Like Like)
        {
            return  new LikeService().Insert(Like);
        }

        // PUT api/values/5
        [JwtAuthentication]
        public long Put([FromBody]Like Like)
        {
            return  new LikeService().Update(Like);
        }

        // DELETE api/values/5
        [JwtAuthentication]
        public long Delete(long id)
        {
            return  new LikeService().Delete(id);
        }

    }
}
