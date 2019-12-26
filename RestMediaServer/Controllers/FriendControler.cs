using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;

namespace RestMediaServer.Controllers
{
    public class FriendController : ApiController
    {
        // GET api/friend
        public async Task<IEnumerable<Friend>> Get()
        {
            return await new FriendService().GetAll();
        }

        // GET api/friend/id
        public async Task<Friend> Get(string id)
        {
            return await new FriendService().GetById(long.Parse(id));
        }

        // GET api/friend/id/type
        public async Task<IEnumerable<Friend>> Get(string id, string type)
        {
            switch(type)
            {
                case "alias":
                    // id = alias
                    return await new FriendService().GetByMemberAlias(id);
                case "id":
                    // id = alias
                    return await new FriendService().GetByMemberId(Int64.Parse(id));
                default:
                    return new List<Friend>();

            }
        }
    }
}
