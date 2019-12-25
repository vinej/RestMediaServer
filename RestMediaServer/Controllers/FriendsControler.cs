using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.DAL;

namespace RestMediaServer.Controllers
{
    public class FriendController : ApiController
    {
        // GET api/friend
        public IEnumerable<Friend> Get()
        {
            return new FriendDal().GetAll();
        }

        // GET api/friend/id
        public Friend Get(string id)
        {
            return new FriendDal().GetById(int.Parse(id));
        }

        // GET api/friend/id/type
        public IEnumerable<Friend> Get(string id, string type)
        {
            switch(type)
            {
                case "alias":
                    // id = alias
                    return new FriendDal().GetByMemberAlias(id);
                case "id":
                    // id = alias
                    return new FriendDal().GetByMemberId(Int32.Parse(id));
                default:
                    return new List<Friend>();

            }
        }
    }
}
