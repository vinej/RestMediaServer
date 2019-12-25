using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;

namespace SqlDAL.DAL
{
    public class FriendDal : BaseDal
    {
        private IEnumerable<Friend> ReadManyFriend(IDataReader dataReader)
        {
            var friends = new List<Friend>();
            while (dataReader.Read())
            {
                friends.Add(ReadFriend(dataReader));
            }
            return friends;
        }

        private IEnumerable<Friend> ReadManyFullFriend(IDataReader dataReader)
        {
            var friends = new List<Friend>();
            while (dataReader.Read())
            {
                friends.Add(ReadFullFriend(dataReader));
            }
            return friends;
        }
        private void ReadBaseFriend(Friend friend, IDataReader dataReader)
        {
            friend.Id = (int)dataReader["Id"];
            friend.Member = new Member()
            {
                Id = (int)dataReader["MemberId"]

            };

            friend.TFriend = new Member()
            {
                Id = (int)dataReader["FriendId"]
            };

            friend.Dob = DateTime.Parse(dataReader["FDob"].ToString());
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

        private Friend ReadFullFriend(IDataReader dataReader)
        {
            var friend = new Friend();
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseFriend(friend, dataReader);
                friend.TFriend.Email = dataReader["FEmail"].ToString();
                friend.TFriend.Alias = dataReader["FAlias"].ToString();
                friend.TFriend.Dob = DateTime.Parse(dataReader["FDob"].ToString());
                friend.Member.Email = dataReader["MEmail"].ToString();
                friend.Member.Alias = dataReader["MAlias"].ToString();
                friend.Member.Dob = DateTime.Parse(dataReader["MDob"].ToString());
            }
            return friend;
        }

        private void CreateParameter(Friend friend, List<SqlParameter> parameters)
        {
            parameters.Add(sqlHelper.CreateParameter("@MemberId", friend.Member.Id, DbType.Int32));
            parameters.Add(sqlHelper.CreateParameter("@FriendId", friend.TFriend.Id, DbType.Int32));
            parameters.Add(sqlHelper.CreateParameter("@Dob", friend.Dob, DbType.DateTime));
        }

        public int Insert(Friend friend)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(friend, parameters);

            sqlHelper.Insert("DAH_Friend_Insert", CommandType.StoredProcedure, parameters.ToArray(), out int lastId);
            friend.Id = lastId;
            return lastId;
        }

        public void Update(Friend friend)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", friend.Id, DbType.Int32)
            };
            CreateParameter(friend, parameters);
            sqlHelper.Update("DAH_Friend_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void Delete(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            sqlHelper.Delete("DAH_Friend_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public Friend GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Friend_GetById", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadFriend(dataReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
        }

        public IEnumerable<Friend> GetByMember(string alias)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@MemberId", alias, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Friend_GetByMember", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadManyFullFriend(dataReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
        }

        public IEnumerable<Friend> GetByMemberForTopic(string alias, int topicId)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@MemberId", alias, DbType.Int32),
                sqlHelper.CreateParameter("@TopicId", topicId, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Friend_GetByMemberForTopic", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadManyFullFriend(dataReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
        }

        public IEnumerable<Friend> GetAll()
        {
            var dataReader = sqlHelper.GetDataReader("DAH_Friend_GetAll", CommandType.StoredProcedure, null, out connection);

            try
            {
                return ReadManyFriend(dataReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
        }

        public int GetScalarValue()
        {
            object scalarValue = sqlHelper.GetScalarValue("DAH_Friend_Scalar", CommandType.StoredProcedure);

            return Convert.ToInt32(scalarValue);
        }
    }
}
