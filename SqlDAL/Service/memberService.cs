using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;
using System.Threading.Tasks;

// simple service for future use 
namespace SqlDAL.Service
{

    public class MemberService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Member>> _memberManyCache = new WaitToFinishMemoryCache<IEnumerable<Member>>();
        static readonly WaitToFinishMemoryCache<Member> _memberSingleCache = new WaitToFinishMemoryCache<Member>();

        public async Task<long> Insert(Member member)
        {
            return await new MemberDal().Insert(member);
        }

        public async Task<long> Update(Member member)
        {
            return await new MemberDal().Update(member);
        }

        public async Task<long> Delete(long id)
        {
            return await new MemberDal().Delete(id);
        }

        public async Task<Member> GetById(long id)
        {
            return await _memberSingleCache.GetOrCreate(id, async () => await new MemberDal().GetById(id));
        }

        public async Task<Member> GetByAlias(string alias)
        {
            return await _memberSingleCache.GetOrCreate(alias, async () => await new MemberDal().GetByAlias(alias));
        }

        public async Task<IEnumerable<Member>> GetAll()
        {
            return await _memberManyCache.GetOrCreate("__all__", async () => await new MemberDal().GetAll());
        }

        public async Task<IEnumerable<Member>> GetByIsActive(bool isActive)
        {
            return await _memberManyCache.GetOrCreate(isActive, async () => await new MemberDal().GetByIsActive(isActive));
        }
    }
}
