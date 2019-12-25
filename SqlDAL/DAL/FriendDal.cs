using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;

namespace SqlDAL.DAL
{
    public class FriendDal : BaseDal
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
            friend.Id = (int)dataReader["Id"];
            friend.MemberId = (int)dataReader["MemberId"];

            friend.TFriend = new Member()
            {
                Id = (int)dataReader["FriendId"],
                Email = (string)dataReader["FEmail"],
                Alias = (string)dataReader["FAlias"],
                IsActive = (bool)dataReader["FIsActive"],
                Dob = (DateTime)dataReader["FDob"]
            };

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
            parameters.Add(sqlHelper.CreateParameter("@MemberId", friend.MemberId, DbType.Int32));
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

        public async Task<IEnumerable<Friend>> GetByMemberAlias(string alias)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Alias", alias, DbType.String)
            };
            connection = new SqlConnection(sqlHelper.ConnectionString);
            connection.Open();

            var dataReader = await sqlHelper.GetDataReaderAsync("DAH_Friend_GetByMemberAlias", CommandType.StoredProcedure, parameters.ToArray(), connection);
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

        public IEnumerable<Friend> GetByMemberId(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Friend_GetByMemberId", CommandType.StoredProcedure, parameters.ToArray(), out connection);
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
