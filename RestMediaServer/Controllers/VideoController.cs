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
        public async Task<IEnumerable<Video>> Get()
        {
            return await new VideoService().GetAll();
        }

        // GET api/Video/id
        public async Task<Video> Get(long id)
        {
            return await new VideoService().GetById(id);
        }            

        public async Task<long> Post([FromBody]Video Video)
        {
            return await new VideoService().Insert(Video);
        }

        // PUT api/values/5
        public async Task<long> Put([FromBody]Video Video)
        {
            return await new VideoService().Update(Video);
        }

        // DELETE api/values/5
        public async Task<long> Delete(long id)
        {
            return await new VideoService().Delete(id);
        }

    }
}
