using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;

namespace SqlDAL.DAL
{
    public class OpinionDal : BaseDal
    {
        private IEnumerable<Opinion> ReadManyFullOpinion(IDataReader dataReader)
        {
            var opinions = new List<Opinion>();
            while (dataReader.Read())
            {
                opinions.Add(ReadFullOpinion(dataReader));
            }
            return opinions;
        }

        private IEnumerable<Opinion> ReadManyOpinion(IDataReader dataReader)
        {
            var opinions = new List<Opinion>();
            while (dataReader.Read())
            {
                opinions.Add(ReadBaseOpinion(dataReader));
            }
            return opinions;
        }

        private Opinion ReadBaseOpinion(IDataReader dataReader)
        {
            var opinion = new Opinion
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                opinion.Id = (int)dataReader["Id"];
                opinion.Member = new Member
                {
                    Id = (int)dataReader["MemberId"]
                };
                opinion.Topic = new Topic
                {
                    Id = (int)dataReader["TopicId"]
                };
                opinion.Comment = dataReader["Comment"].ToString();
                opinion.Dob = DateTime.Parse(dataReader["Dob"].ToString());
            }
            return opinion;
        }

        private Opinion ReadFullOpinion(IDataReader dataReader)
        {
            var opinion = ReadBaseOpinion(dataReader);
            if (opinion.Id != -1)
            {
                opinion.Member.Id = (int)dataReader["MemberId"];
                opinion.Member.Alias = dataReader["MName"].ToString();
                opinion.Member.Email = dataReader["MEmail"].ToString();
                opinion.Member.Dob = DateTime.Parse(dataReader["MDob"].ToString());
                opinion.Topic.Id = (int)dataReader["TopicId"];
                opinion.Topic.Description = dataReader["TDescription"].ToString();
                opinion.Topic.Dob = DateTime.Parse(dataReader["TDob"].ToString());
            }
            return opinion;
        }

        private void CreateParameter(Opinion Opinion, List<SqlParameter> parameters)
        {
            parameters.Add(sqlHelper.CreateParameter("@MemberId", Opinion.Member.Id, DbType.Int32));
            parameters.Add(sqlHelper.CreateParameter("@TopicId", Opinion.Topic.Id, DbType.Int32));
            parameters.Add(sqlHelper.CreateParameter("@Dob", Opinion.Dob, DbType.DateTime));
        }

        public int Insert(Opinion Opinion)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(Opinion, parameters);

            sqlHelper.Insert("DAH_Opinion_Insert", CommandType.StoredProcedure, parameters.ToArray(), out int lastId);
            Opinion.Id = lastId;
            return lastId;
        }

        public void Update(Opinion Opinion)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", Opinion.Id, DbType.Int32)
            };
            CreateParameter(Opinion, parameters);
            sqlHelper.Update("DAH_Opinion_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void Delete(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            sqlHelper.Delete("DAH_Opinion_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public Opinion GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Opinion_GetById", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadBaseOpinion(dataReader);
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

        public IEnumerable<Opinion> GetByMember(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@MemberId", id, DbType.String)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Opinion_GetByMember", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadManyOpinion(dataReader);
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

        public IEnumerable<Opinion> GetByFullMember(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@MemberId", id, DbType.String)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Opinion_GetByFullMember", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadManyFullOpinion(dataReader);
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

        public IEnumerable<Opinion> GetByTopic(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@TopicId", id, DbType.String)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Opinion_GetByTopic", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadManyOpinion(dataReader);
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

        public IEnumerable<Opinion> GetByFullTopic(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@TopicId", id, DbType.String)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Opinion_GetByFullTopic", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadManyFullOpinion(dataReader);
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

        public IEnumerable<Opinion> GetAll()
        {
            var dataReader = sqlHelper.GetDataReader("DAH_Opinion_GetAll", CommandType.StoredProcedure, null, out connection);

            try
            {
                return ReadManyOpinion(dataReader);
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


        public IEnumerable<Opinion> GetAllFull()
        {
            var dataReader = sqlHelper.GetDataReader("DAH_Opinion_GetAllFull", CommandType.StoredProcedure, null, out connection);

            try
            {
                return ReadManyFullOpinion(dataReader);
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
            object scalarValue = sqlHelper.GetScalarValue("DAH_Opinion_Scalar", CommandType.StoredProcedure);

            return Convert.ToInt32(scalarValue);
        }
    }
}
