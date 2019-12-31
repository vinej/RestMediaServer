using System.Collections.Generic;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;
using WebApi.Jwt.Filters;

namespace RestMediaServer.Controllers
{
    public class VideoController : ApiController
    {
        // GET api/Video
        [JwtAuthentication]
        public IEnumerable<Video> Get()
        {
            return  new VideoService().GetAll();
        }

        // GET api/Video/id
        [JwtAuthentication]
        public Video Get(long id)
        {
            return  new VideoService().GetById(id);
        }

        [JwtAuthentication]
        public long Post([FromBody]Video Video)
        {
            return  new VideoService().Insert(Video);
        }

        // PUT api/values/5
        [JwtAuthentication]
        public long Put([FromBody]Video Video)
        {
            return  new VideoService().Update(Video);
        }

        // DELETE api/values/5
        [JwtAuthentication]
        public long Delete(long id)
        {
            return  new VideoService().Delete(id);
        }

    }
}
