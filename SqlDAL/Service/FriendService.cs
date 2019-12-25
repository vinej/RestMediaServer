using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using SqlDAL.DAL;
using System.Threading.Tasks;

namespace SqlDAL.Service
{
    public class FriendService
    {
        static WaitToFinishMemoryCache<IEnumerable<Friend>> _friendCache = new WaitToFinishMemoryCache<IEnumerable<Friend>>();

        public int Insert(Friend friend)
        {
            return new FriendDal().Insert(friend);
        }

        public void Delete(int id)
        {
            new FriendDal().Delete(id);
        }

        public Friend GetById(int id)
        {
            return new FriendDal().GetById(id);
        }

        public async Task<IEnumerable<Friend>> GetByMemberAlias(string alias)
        {
            return await _friendCache.GetOrCreate(alias, async () => await new FriendDal().GetByMemberAlias(alias));
        }

        public IEnumerable<Friend> GetByMemberId(int id)
        {
            return new FriendDal().GetByMemberId(id);
        }

        public IEnumerable<Friend> GetAll()
        {
            return new FriendDal().GetAll();
        }

        public int GetScalarValue()
        {
            return new FriendDal().GetScalarValue();
        }
    }
}
