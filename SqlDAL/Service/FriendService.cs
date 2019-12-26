using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;
using System.Threading.Tasks;

namespace SqlDAL.Service
{
    public class FriendService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<Friend>> _friendManyCache = new WaitToFinishMemoryCache<IEnumerable<Friend>>();
        static readonly WaitToFinishMemoryCache<Friend> _friendSingleCache = new WaitToFinishMemoryCache<Friend>();

        public async Task<long> Insert(Friend friend)
        {
            return await new FriendDal().Insert(friend);
        }

        public async Task<long> Delete(long id)
        {
            return await new FriendDal().Delete(id);
        }

        public async Task<Friend> GetById(long id)
        {
            return await _friendSingleCache.GetOrCreate(id, async () => await new FriendDal().GetById(id));
        }

        public async Task<IEnumerable<Friend>> GetByMemberAlias(string alias)
        {
            return await _friendManyCache.GetOrCreate(alias, async () => await new FriendDal().GetByMemberAlias(alias));
        }

        public async Task<IEnumerable<Friend>> GetByMemberId(long id)
        {
            return await _friendManyCache.GetOrCreate(id, async () => await new FriendDal().GetByMemberId(id));
        }

        public async Task<IEnumerable<Friend>> GetAll()
        {
            return await _friendManyCache.GetOrCreate("__all__", async () => await new FriendDal().GetAll());
        }
    }
}
