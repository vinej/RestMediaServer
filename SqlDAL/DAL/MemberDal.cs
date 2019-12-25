using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;

namespace SqlDAL.DAL
{
    public class MemberDal : BaseDal
    {
        private void ReadMember(Member member, IDataReader dataReader)
        {
            while (dataReader.Read())
            {
                member.Id = (int)dataReader["Id"];
                member.Email = dataReader["Email"].ToString();
                member.Alias = dataReader["Alias"].ToString();
                member.IsActive = (bool)dataReader["IsActive"];
                member.Dob = DateTime.Parse(dataReader["Dob"].ToString());
            }
        }
               
        public int Insert(Member member)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(sqlHelper.CreateParameter("@Email", 255, member.Email, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Alias", 255, member.Alias, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@IsActive", member.IsActive, DbType.Boolean));
            parameters.Add(sqlHelper.CreateParameter("@Dob", member.Dob, DbType.DateTime));

            int lastId = 0;
            sqlHelper.Insert("DAH_Member_Insert", CommandType.StoredProcedure, parameters.ToArray(), out lastId);
            member.Id = lastId;
            return lastId;
        }

        public void Update(Member member)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(sqlHelper.CreateParameter("@Id", member.Id, DbType.Int32));
            parameters.Add(sqlHelper.CreateParameter("@Email", 50, member.Email, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Alias", member.Alias, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@IsActive", member.IsActive, DbType.Boolean));
            parameters.Add(sqlHelper.CreateParameter("@Dob", member.Dob, DbType.DateTime));

            sqlHelper.Update("DAH_Member_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void Delete(int id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(sqlHelper.CreateParameter("@Id", id, DbType.Int32));

            sqlHelper.Delete("DAH_Member_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public Member GetById(int id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(sqlHelper.CreateParameter("@Id", id, DbType.Int32));

            var dataReader = sqlHelper.GetDataReader("DAH_Member_GetById", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                var member = new Member();
                ReadMember(member, dataReader);
                if (member.Id == 0) return null;
                return member;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
        }

        public Member GetByAlias(string alias)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(sqlHelper.CreateParameter("@Alias", alias, DbType.String));

            var dataReader = sqlHelper.GetDataReader("DAH_Member_GetByAlias", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                var member = new Member();
                ReadMember(member, dataReader);
                return member;
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
            var parameters = new List<SqlParameter>();
            var dataReader = sqlHelper.GetDataReader("DAH_Member_GetAll", CommandType.StoredProcedure, null, out connection);

            try
            {
                var members = new List<Member>();
                while (dataReader.Read())
                {
                    var member = new Member();
                    ReadMember(member, dataReader);
                    members.Add(member);
                }

                return members;
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
            object scalarValue = sqlHelper.GetScalarValue("DAH_Member_Scalar", CommandType.StoredProcedure);

            return Convert.ToInt32(scalarValue);
        }
    }
}
