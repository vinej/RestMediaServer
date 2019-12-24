using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;

namespace SqlDAL.DAL
{
    public class UserDal : BaseDal
    {
        public int Insert(User user)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(sqlHelper.CreateParameter("@Email", 50, user.Email, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Alias", user.Alias, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Dob", user.Dob, DbType.DateTime));
            parameters.Add(sqlHelper.CreateParameter("@IsActive", 50, user.IsActive, DbType.Boolean));

            int lastId = 0;
            sqlHelper.Insert("DAH_User_Insert", CommandType.StoredProcedure, parameters.ToArray(), out lastId);

            return lastId;
        }

        public void Update(User user)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(sqlHelper.CreateParameter("@Id", user.Id, DbType.Int32));
            parameters.Add(sqlHelper.CreateParameter("@Email", 50, user.Email, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Alias", user.Alias, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Dob", user.Dob, DbType.DateTime));

            sqlHelper.Update("DAH_User_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void Delete(int id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(sqlHelper.CreateParameter("@Id", id, DbType.Int32));

            sqlHelper.Delete("DAH_User_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public User GetById(int id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(sqlHelper.CreateParameter("@Id", id, DbType.Int32));

            var dataReader = sqlHelper.GetDataReader("DAH_User_GetById", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            try
            {
                var user = new User();
                while (dataReader.Read())
                {
                    user.Email = dataReader["Email"].ToString();
                    user.Alias = dataReader["Allias"].ToString();
                }

                return user;
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

        public IEnumerable<User> GetAll()
        {
            var parameters = new List<SqlParameter>();
            var dataReader = sqlHelper.GetDataReader("DAH_User_GetAll", CommandType.StoredProcedure, null, out connection);

            try
            {
                var users = new List<User>();
                while (dataReader.Read())
                {
                    var user = new User();
                    user.Email = dataReader["Email"].ToString();
                    user.Alias = dataReader["Alias"].ToString();

                    users.Add(user);
                }

                return users;
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

        public IEnumerable<User> SelectAll()
        {
            var userDataTable = sqlHelper.GetDataTable("DAH_User_GetAll", CommandType.StoredProcedure);
            var users = new List<User>();

            foreach (DataRow row in userDataTable.Rows)
            {
                var user = new User();
                user.Email = row["Email"].ToString();
                user.Alias = row["Alias"].ToString();

                users.Add(user);
            }

            return users;
        }

        public int GetScalarValue()
        {
            object scalarValue = sqlHelper.GetScalarValue("DAH_User_Scalar", CommandType.StoredProcedure);

            return Convert.ToInt32(scalarValue);
        }
    }
}
