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
        public async Task<IEnumerable<MemberFriend>> Get()
        {
            return await new FriendService().GetAll();
        }

        // GET api/friend/id
        public async Task<MemberFriend> Get(long id)
        {
            return await new FriendService().GetById(id);
        }

        // GET api/friend/id/type
        public async Task<IEnumerable<MemberFriend>> Get(string id, string type)
        {
            switch(type)
            {
                case "topic":
                    // id = memberId|topicId : get all my friends with an opinion on a topic
                    long memberId;
                    long topicId;
                    Tool.GetTwoInteger(id,out memberId, out topicId);
                    return await new FriendService().GetByMemberForTopic(memberId, topicId);
                case "alias":
                    // id = alias
                    return await new FriendService().GetByMemberAlias(id);
                case "id":
                    // id = alias
                    return await new FriendService().GetByMemberId(long.Parse(id));
                default:
                    return new List<MemberFriend>();

            }
        }
    }
}
