using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;

// simple service for future use 
namespace SqlDAL.Service
{
    
    public class MemberService
    {
        public int Insert(Member member)
        {
            return new MemberDal().Insert(member);
        }

        public void Update(Member member)
        {
            new MemberDal().Update(member);
        }

        public void Delete(int id)
        {
            new MemberDal().Delete(id);
        }

        public Member GetById(int id)
        {
            return new MemberDal().GetById(id);
        }

        public Member GetByAlias(string alias)
        {
            return new MemberDal().GetByAlias(alias);
        }

        public IEnumerable<Member> GetAll()
        {
            return new MemberDal().GetAll();
        }

        public IEnumerable<Member> GetByIsActive(bool isActive)
        {
            return new MemberDal().GetByIsActive(isActive);
        }

        public int GetScalarValue()
        {
            return new MemberDal().GetScalarValue();
        }
    }
}
