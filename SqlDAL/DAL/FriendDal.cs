using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;

namespace SqlDAL.DAL
{
    public class FriendDal : BaseDal<MemberFriend>
    {
        private IEnumerable<MemberFriend> ReadManyFriend(IDataReader dataReader)
        {
            var friends = new List<MemberFriend>();
            while (dataReader.Read())
            {
                var friend = new MemberFriend();
                ReadBaseFriend(friend, dataReader);
                friends.Add(friend);
            }
            return friends;
        }

        private void ReadBaseFriend(MemberFriend friend, IDataReader dataReader)
        {
            friend.Id = (long)dataReader["Id"];
            friend.MemberId = (long)dataReader["MemberId"];
            friend.Friend = new Member
            {
                Id = (long)dataReader["FriendId"],
                Email = (string)dataReader["FEmail"],
                Alias = (string)dataReader["FAlias"],
                IsActive = (bool)dataReader["FIsActive"],
                Dob = (DateTime)dataReader["FDob"]
            };
            friend.Dob = (DateTime)dataReader["Dob"];
        }

        private MemberFriend ReadFriend(IDataReader dataReader)
        {
            var friend = new MemberFriend
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

        private void CreateParameter(MemberFriend friend, List<SqlParameter> parameters)
        {
            parameters.Add(CreateParameter("@MemberId", friend.MemberId, DbType.Int64));
            parameters.Add(CreateParameter("@FriendId", friend.Friend.Id, DbType.Int64));
            parameters.Add(CreateParameter("@Dob", friend.Dob, DbType.DateTime));
        }

        public async Task<long> Insert(MemberFriend friend)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(friend, parameters);

            long lastId = await InsertAsync("DAH_Friend_Insert", CommandType.StoredProcedure, parameters.ToArray());
            friend.Id = lastId;
            return lastId;
        }

        public async Task<long> Delete(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };

            return await DeleteAsync("DAH_Friend_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<MemberFriend> GetById(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Friend_GetById", parameters, ReadFriend);
        }

        public async Task<IEnumerable<MemberFriend>> GetByMemberAlias(string alias)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Alias", alias, DbType.String)
            };
            return await ReadManyFunc("DAH_Friend_GetByMemberAlias", parameters, ReadManyFriend);
        }

        public async Task<IEnumerable<MemberFriend>> GetByMemberId(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64),

            };
            return await ReadManyFunc("DAH_Friend_GetByMemberId", parameters, ReadManyFriend);
        }

        public async Task<IEnumerable<MemberFriend>> GetByMemberForTopic(long memberId, long topicId)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@MemberId", memberId, DbType.Int64),
                CreateParameter("@TopicId", topicId, DbType.Int64)
            };
            return await ReadManyFunc("DAH_Friend_GetByMemberForTopic", parameters, ReadManyFriend);
        }

        public async Task<IEnumerable<MemberFriend>> GetAll()
        {
            return await ReadManyFunc("DAH_Friend_GetAll", null, ReadManyFriend);
        }
    }
}
