using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;

namespace SqlDAL.DAL
{
    public class MemberDal : BaseDal
    {
        private IEnumerable<Member> ReadManyMember(IDataReader dataReader)
        {
            var members = new List<Member>();
            while (dataReader.Read())
            {
                members.Add(ReadMember(dataReader));
            }
            return members;
        }

        private Member ReadMember(IDataReader dataReader)
        {
            var member = new Member();
            var isData = dataReader.Read();
            if (isData)
            {
                member.Id = (int)dataReader["Id"];
                member.Email = dataReader["Email"].ToString();
                member.Alias = dataReader["Alias"].ToString();
                member.IsActive = (bool)dataReader["IsActive"];
                member.Dob = DateTime.Parse(dataReader["Dob"].ToString());
            }
            return member;
        }

        private void CreateParameter(Member member, List<SqlParameter> parameters)
        {
            parameters.Add(sqlHelper.CreateParameter("@Email", 255, member.Email, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Alias", 255, member.Alias, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@IsActive", member.IsActive, DbType.Boolean));
            parameters.Add(sqlHelper.CreateParameter("@Dob", member.Dob, DbType.DateTime));
        }

        public int Insert(Member member)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(member, parameters);

            sqlHelper.Insert("DAH_Member_Insert", CommandType.StoredProcedure, parameters.ToArray(), out int lastId);
            member.Id = lastId;
            return lastId;
        }

        public void Update(Member member)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", member.Id, DbType.Int32)
            };
            CreateParameter(member, parameters);
            sqlHelper.Update("DAH_Member_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void Delete(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            sqlHelper.Delete("DAH_Member_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public Member GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Member_GetById", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadMember(dataReader);
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

        public Member GetByAlias(string alias)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Alias", alias, DbType.String)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Member_GetByAlias", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadMember(dataReader);
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

        public IEnumerable<Member> GetAll()
        {
            var dataReader = sqlHelper.GetDataReader("DAH_Member_GetAll", CommandType.StoredProcedure, null, out connection);

            try
            {
                return ReadManyMember(dataReader);
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

        public IEnumerable<Member> GetByIsActive(bool isActive)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@IsActriveAlias", isActive, DbType.Boolean)
            };
            var dataReader = sqlHelper.GetDataReader("DAH_Member_GetByIsActive", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            try
            {
                return ReadManyMember(dataReader);
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

        public Member GetMemberWithFriends(string alias)
        {
            var member = GetByAlias(alias);
            member.Friends = new FriendDal().GetByMember(alias);
            return member;
        }

        public int GetScalarValue()
        {
            object scalarValue = sqlHelper.GetScalarValue("DAH_Member_Scalar", CommandType.StoredProcedure);

            return Convert.ToInt32(scalarValue);
        }
    }
}
