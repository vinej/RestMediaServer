using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;

namespace SqlDAL.DAL
{
    public class FriendDal : BaseDal<Friend>
    {
        private IEnumerable<Friend> ReadManyFriend(IDataReader dataReader)
        {
            var friends = new List<Friend>();
            while (dataReader.Read())
            {
                var friend = new Friend();
                ReadBaseFriend(friend, dataReader);
                friends.Add(friend);
            }
            return friends;
        }

        private void ReadBaseFriend(Friend friend, IDataReader dataReader)
        {
            friend.Id = (long)dataReader["Id"];
            friend.MemberId = (long)dataReader["MemberId"];
            friend.Dob = (DateTime)dataReader["FDob"];
        }

        private Friend ReadFriend(IDataReader dataReader)
        {
            var friend = new Friend
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseFriend(friend, dataReader);
            }
            return friend;
        }

        private void CreateParameter(Friend friend, List<SqlParameter> parameters)
        {
            parameters.Add(sqlHelper.CreateParameter("@MemberId", friend.MemberId, DbType.Int64));
            parameters.Add(sqlHelper.CreateParameter("@FriendId", friend.TFriend.Id, DbType.Int64));
            parameters.Add(sqlHelper.CreateParameter("@Dob", friend.Dob, DbType.DateTime));
        }

        public async Task<long> Insert(Friend friend)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(friend, parameters);

            long lastId = await sqlHelper.InsertAsync("DAH_Friend_Insert", CommandType.StoredProcedure, parameters.ToArray());
            friend.Id = lastId;
            return lastId;
        }

        public async Task<long> Delete(long id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };

            return await sqlHelper.DeleteAsync("DAH_Friend_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<Friend> GetById(long id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Friend_GetById", parameters, ReadFriend);
        }

        public async Task<IEnumerable<Friend>> GetByMemberAlias(string alias)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Alias", alias, DbType.String)
            };
            return await ReadManyFunc("DAH_Friend_GetByMemberAlias", parameters, ReadManyFriend);
        }

        public async Task<IEnumerable<Friend>> GetByMemberId(long id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadManyFunc("DAH_Friend_GetByMemberId", parameters, ReadManyFriend);
        }

        public async Task<IEnumerable<Friend>> GetAll()
        {
            return await ReadManyFunc("DAH_Friend_GetAll", null, ReadManyFriend);
        }
    }
}
