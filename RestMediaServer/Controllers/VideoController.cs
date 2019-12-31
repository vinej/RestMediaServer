using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;

namespace RestMediaServer.Controllers
{
    public class VideoController : ApiController
    {
        // GET api/Video
        public IEnumerable<Video> Get()
        {
            return  new VideoService().GetAll();
        }

        // GET api/Video/id
        public Video Get(long id)
        {
            return  new VideoService().GetById(id);
        }            

        public long Post([FromBody]Video Video)
        {
            return  new VideoService().Insert(Video);
        }

        // PUT api/values/5
        public long Put([FromBody]Video Video)
        {
            return  new VideoService().Update(Video);
        }

        // DELETE api/values/5
        public long Delete(long id)
        {
            return  new VideoService().Delete(id);
        }

    }
}
