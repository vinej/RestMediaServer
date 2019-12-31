using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
            return  new MemberService().GetAll();
        }

        // GET api/member/id
        public Member Get(string id)
        {
            return long.TryParse(id, out long longId)
                ?  new MemberService().GetById(longId)
                :  new MemberService().GetByAlias(id);
        }            

        // GET api/member/id/type
        public IEnumerable<Member> Get(string id, string type)
        {
            switch(type)
            {
                case "alias":
                    // id = alias
                    var list = new List<Member>
                    {
                         new MemberService().GetByAlias(id)
                    };
                    return list;
                case "isactive":
                    // id = true|false
                    return  new MemberService().GetByIsActive(bool.Parse(id));
                default:
                    return new List<Member>();

            }
        }

        public static long Post([FromBody]Member member)
        {
            //  /api/member
            // need to register the member and generate a token
            // hash the password before inserting it into the database
            member.HashPassword = SecurePasswordHasher.Hash(member.HashPassword);
            return  new MemberService().Insert(member);
        }

        // PUT api/values/5
        public static long Put([FromBody]Member member)
        {
            return  new MemberService().Update(member);
        }

        // DELETE api/values/5
        public static long Delete(long id)
        {
            return  new MemberService().Delete(id);
        }

    }
}
