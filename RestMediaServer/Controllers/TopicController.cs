using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;

namespace RestMediaServer.Controllers
{
    public class TopicController : ApiController
    {
        // GET api/Topic
        public async Task<IEnumerable<Topic>> Get()
        {
            return await new TopicService().GetAll();
        }

        // GET api/Topic/id
        public async Task<Topic> Get(long id)
        {
            return await new TopicService().GetById(id);
        }            

        public async Task<long> Post([FromBody]Topic Topic)
        {
            return await new TopicService().Insert(Topic);
        }

        // PUT api/values/5
        public async Task<long> Put([FromBody]Topic Topic)
        {
            return await new TopicService().Update(Topic);
        }

        // DELETE api/values/5
        public async Task<long> Delete(long id)
        {
            return await new TopicService().Delete(id);
        }

    }
}
