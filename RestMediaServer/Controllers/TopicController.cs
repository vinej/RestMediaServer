using System.Collections.Generic;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;
using WebApi.Jwt.Filters;

namespace RestMediaServer.Controllers
{
    public class TopicController : ApiControllerWithHub<NotificationHub>
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
        public Topic Get(string id, string type)
        {
            if (type == "current")
            {
                Hub.Clients.All.Notification("update:current");
                return new TopicService().GetCurrent();
            } else
            {
                return null;
            }
        }

        [JwtAuthentication]
        public long Post([FromBody]Topic topic)
        {
            return  new TopicService().Insert(topic);
        }

        // PUT api/values/5
        [JwtAuthentication]
        public long Put([FromBody]Topic topic)
        {
            // Notify the connected clients
            long id = new TopicService().Update(topic);
            Hub.Clients.All.Say("update");
            return id;
        }

        // DELETE api/values/5
        [JwtAuthentication]
        public long Delete(long id)
        {
            return  new TopicService().Delete(id);
        }

    }
}
