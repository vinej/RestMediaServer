using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;
using System.Threading.Tasks;

namespace SqlDAL.Service
{
    public class FriendService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<MemberFriend>> _friendManyCache = new WaitToFinishMemoryCache<IEnumerable<MemberFriend>>();
        static readonly WaitToFinishMemoryCache<MemberFriend> _friendSingleCache = new WaitToFinishMemoryCache<MemberFriend>();

        public async Task<long> Insert(MemberFriend friend)
        {
            return await new FriendDal().Insert(friend);
        }

        public async Task<long> Delete(long id)
        {
            return await new FriendDal().Delete(id);
        }

        public async Task<MemberFriend> GetById(long id)
        {
            return await _friendSingleCache.GetOrCreate(id, async () => await new FriendDal().GetById(id));
        }

        public async Task<IEnumerable<MemberFriend>> GetByMemberAlias(string alias)
        {
            return await _friendManyCache.GetOrCreate(alias, async () => await new FriendDal().GetByMemberAlias(alias));
        }

        public async Task<IEnumerable<MemberFriend>> GetByMemberForTopic(long memberId, long topicId)
        {
            return await _friendManyCache.GetOrCreate($"{memberId}:{topicId}", async () => await new FriendDal().GetByMemberForTopic(memberId, topicId));
        }

        public async Task<IEnumerable<MemberFriend>> GetByMemberId(long id)
        {
            return await _friendManyCache.GetOrCreate(id, async () => await new FriendDal().GetByMemberId(id));
        }

        public async Task<IEnumerable<MemberFriend>> GetAll()
        {
            return await _friendManyCache.GetOrCreate("__all__", async () => await new FriendDal().GetAll());
        }
    }
}
