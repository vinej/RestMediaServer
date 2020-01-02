using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;

// simple service for future use 
namespace SqlDAL.Service
{

    public class MemberService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Member>> _memberManyCache = new WaitToFinishMemoryCache<IEnumerable<Member>>();
        static readonly WaitToFinishMemoryCache<Member> _memberSingleCache = new WaitToFinishMemoryCache<Member>();

        public  long Insert(Member member)
        {
            return  new MemberDal().Insert(member);
        }

        public  long Update(Member member)
        {
            return  new MemberDal().Update(member);
        }

        public  long Delete(long id)
        {
            return  new MemberDal().Delete(id);
        }

        public  Member GetById(long id)
        {
            return  _memberSingleCache.GetOrCreate(id,  () =>  new MemberDal().GetById(id));
        }

        public Member GetByAlias(string alias)
        {
            return  _memberSingleCache.GetOrCreate(alias,  () =>  new MemberDal().GetByAlias(alias));
        }

        public Member GetByEmail(string email)
        {
            return _memberSingleCache.GetOrCreate(email, () => new MemberDal().GetByEmail(email));
        }

        public IEnumerable<Member> GetAll()
        {
            return  _memberManyCache.GetOrCreate("__all__",  () =>  new MemberDal().GetAll());
        }

        public IEnumerable<Member> GetByIsActive(bool isActive)
        {
            return  _memberManyCache.GetOrCreate(isActive,  () =>  new MemberDal().GetByIsActive(isActive));
        }
    }
}
