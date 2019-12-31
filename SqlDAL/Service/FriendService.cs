using System.Collections.Generic;
using SqlDAL.Domain;
using SqlDAL.DAL;

namespace SqlDAL.Service
{
    public class FriendService
    {
        static readonly WaitToFinishMemoryCache<IEnumerable<MemberFriend>> _friendManyCache = new WaitToFinishMemoryCache<IEnumerable<MemberFriend>>();
        static readonly WaitToFinishMemoryCache<MemberFriend> _friendSingleCache = new WaitToFinishMemoryCache<MemberFriend>();

        public  long Insert(MemberFriend friend)
        {
            return  new FriendDal().Insert(friend);
        }

        public  long Delete(long id)
        {
            return  new FriendDal().Delete(id);
        }

        public MemberFriend GetById(long id)
        {
            return  _friendSingleCache.GetOrCreate(id,  () =>  new FriendDal().GetById(id));
        }

        public IEnumerable<MemberFriend> GetByMemberAlias(string alias)
        {
            return  _friendManyCache.GetOrCreate(alias,  () =>  new FriendDal().GetByMemberAlias(alias));
        }

        public IEnumerable<MemberFriend> GetByMemberForTopic(long memberId, long topicId)
        {
            return  _friendManyCache.GetOrCreate($"{memberId}:{topicId}",  () =>  new FriendDal().GetByMemberForTopic(memberId, topicId));
        }

        public IEnumerable<MemberFriend> GetByMemberId(long id)
        {
            return  _friendManyCache.GetOrCreate(id,  () =>  new FriendDal().GetByMemberId(id));
        }

        public IEnumerable<MemberFriend> GetAll()
        {
            return  _friendManyCache.GetOrCreate("__all__",  () =>  new FriendDal().GetAll());
        }
    }
}
