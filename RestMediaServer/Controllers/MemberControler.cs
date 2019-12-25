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
    public class MemberController : ApiController
    {
        // GET api/member
        public IEnumerable<Member> Get()
        {
            return new MemberDal().GetAll();
        }

        // GET api/member/id
        public Member Get(string id)
        {
            return new MemberDal().GetById(int.Parse(id));
        }

        // GET api/member/id/type
        public IEnumerable<Member> Get(string id, string type)
        {
            switch(type)
            {
                case "alias":
                    // id = alias
                    var list = new List<Member>();
                    list.Add(new MemberDal().GetByAlias(id));
                    return list;
                case "isactive":
                    // id = true|false
                    return new MemberDal().GetByIsActive(bool.Parse(id));
                default:
                    return new List<Member>();

            }
        }
    }
}
