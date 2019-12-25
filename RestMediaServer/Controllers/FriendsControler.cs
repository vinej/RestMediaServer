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
        public IEnumerable<Friend> Get()
        {
            return new FriendService().GetAll();
        }

        // GET api/friend/id
        public Friend Get(string id)
        {
            return new FriendService().GetById(int.Parse(id));
        }

        // GET api/friend/id/type
        public async Task<IEnumerable<Friend>> Get(string id, string type)
        {
            switch(type)
            {
                case "alias":
                    // id = alias
                    var _dal = new FriendService();
                    return await _dal.GetByMemberAlias(id);
                case "id":
                    // id = alias
                    return new FriendService().GetByMemberId(Int32.Parse(id));
                default:
                    return new List<Friend>();

            }
        }
    }
}
