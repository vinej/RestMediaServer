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
        public async Task<IEnumerable<Member>> Get()
        {
            return await new MemberService().GetAll();
        }

        // GET api/member/id
        public async Task<Member> Get(string id)
        {
            return long.TryParse(id, out long longId)
                ? await new MemberService().GetById(longId)
                : await new MemberService().GetByAlias(id);
        }            

        // GET api/member/id/type
        public async Task<IEnumerable<Member>> Get(string id, string type)
        {
            switch(type)
            {
                case "alias":
                    // id = alias
                    var list = new List<Member>
                    {
                        await new MemberService().GetByAlias(id)
                    };
                    return list;
                case "isactive":
                    // id = true|false
                    return await new MemberService().GetByIsActive(bool.Parse(id));
                default:
                    return new List<Member>();

            }
        }

        public async Task<long> Post([FromBody]Member member)
        {
            return await new MemberService().Insert(member);
        }

        // PUT api/values/5
        public async Task<long> Put([FromBody]Member member)
        {
            return await new MemberService().Update(member);
        }

        // DELETE api/values/5
        public async Task<long> Delete(long id)
        {
            return await new MemberService().Delete(id);
        }

    }
}
