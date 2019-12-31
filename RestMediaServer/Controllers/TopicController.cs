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
        public IEnumerable<Topic> Get()
        {
            return  new TopicService().GetAll();
        }

        // GET api/Topic/id
        public Topic Get(long id)
        {
            return  new TopicService().GetById(id);
        }            

        public long Post([FromBody]Topic Topic)
        {
            return  new TopicService().Insert(Topic);
        }

        // PUT api/values/5
        public long Put([FromBody]Topic Topic)
        {
            return  new TopicService().Update(Topic);
        }

        // DELETE api/values/5
        public long Delete(long id)
        {
            return  new TopicService().Delete(id);
        }

    }
}
