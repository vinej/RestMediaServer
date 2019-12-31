using System.Collections.Generic;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;
using Microsoft.AspNetCore.Authorization;

namespace RestMediaServer.Controllers
{
    public class FriendController : ApiController
    {
        // GET api/friend
        public IEnumerable<MemberFriend> Get()
        {
            return  new FriendService().GetAll();
        }

        // GET api/friend/id
        public MemberFriend Get(long id)
        {
            return  new FriendService().GetById(id);
        }

        // GET api/friend/id/type
        public IEnumerable<MemberFriend> Get(string id, string type)
        {
            switch(type)
            {
                case "topic":
                    // id = memberId|topicId : get all my friends with an opinion on a topic
                    long memberId;
                    long topicId;
                    Tool.GetTwoInteger(id,out memberId, out topicId);
                    return  new FriendService().GetByMemberForTopic(memberId, topicId);
                case "alias":
                    // id = alias
                    return  new FriendService().GetByMemberAlias(id);
                case "id":
                    // id = alias
                    return  new FriendService().GetByMemberId(long.Parse(id));
                default:
                    return new List<MemberFriend>();

            }
        }

    }
}
