using System.Collections.Generic;
using System.Web.Http;
using SqlDAL.Domain;
using SqlDAL.Service;

namespace RestMediaServer.Controllers
{
    public class MemberController : ApiController
    {
        // GET api/member
        public IEnumerable<Member> Get()
        {
            return new MemberService().GetAll();
        }

        // GET api/member/id
        public Member Get(string id)
        {
            return new MemberService().GetById(int.Parse(id));
        }

        // GET api/member/id/type
        public IEnumerable<Member> Get(string id, string type)
        {
            switch(type)
            {
                case "alias":
                    // id = alias
                    var list = new List<Member>();
                    list.Add(new MemberService().GetByAlias(id));
                    return list;
                case "isactive":
                    // id = true|false
                    return new MemberService().GetByIsActive(bool.Parse(id));
                default:
                    return new List<Member>();

            }
        }
    }
}
