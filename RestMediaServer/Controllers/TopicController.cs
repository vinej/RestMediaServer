using System.Collections.Generic;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;
using WebApi.Jwt.Filters;

namespace RestMediaServer.Controllers
{
    public class TopicController : ApiController
    {
        // GET api/Topic
        [JwtAuthentication]
        public IEnumerable<Topic> Get()
        {
            return  new TopicService().GetAll();
        }

        // GET api/Topic/id
        [JwtAuthentication]
        public Topic Get(long id)
        {
            return  new TopicService().GetById(id);
        }

        // GET api/Topic/id
        [JwtAuthentication]
        public Topic Get(long id, string action)
        {
            if (action == "current")
            {
                return new TopicService().GetCurrent();
            } else
            {
                return null;
            }
        }

        [JwtAuthentication]
        public long Post([FromBody]Topic Topic)
        {
            return  new TopicService().Insert(Topic);
        }

        // PUT api/values/5
        [JwtAuthentication]
        public long Put([FromBody]Topic Topic)
        {
            return  new TopicService().Update(Topic);
        }

        // DELETE api/values/5
        [JwtAuthentication]
        public long Delete(long id)
        {
            return  new TopicService().Delete(id);
        }

    }
}
