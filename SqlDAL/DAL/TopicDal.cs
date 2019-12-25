using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;

namespace SqlDAL.DAL
{
    public class TopicDal : BaseDal
    {
        private IEnumerable<Topic> ReadManyTopic(IDataReader dataReader)
        {
            var topics = new List<Topic>();
            while (dataReader.Read())
            {
                topics.Add(ReadBaseTopic(dataReader));
            }
            return topics;
        }

        private Topic ReadBaseTopic(IDataReader dataReader)
        {
            var topic = new Topic
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                topic.Id = (int)dataReader["Id"];
                topic.Description = dataReader["Description"].ToString();
                topic.Dob = DateTime.Parse(dataReader["Dob"].ToString());
            }
            return topic;
        }

        private void CreateParameter(Topic topic, List<SqlParameter> parameters)
        {
            parameters.Add(sqlHelper.CreateParameter("@Description", 255, topic.Description, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Dob", topic.Dob, DbType.DateTime));
        }

        public int Insert(Topic topic)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(topic, parameters);

            sqlHelper.Insert("DAH_Topic_Insert", CommandType.StoredProcedure, parameters.ToArray(), out int lastId);
            topic.Id = lastId;
            return lastId;
        }

        public void Update(Topic topic)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", topic.Id, DbType.Int32)
            };
            CreateParameter(topic, parameters);
            sqlHelper.Update("DAH_Topic_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void Delete(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            sqlHelper.Delete("DAH_Topic_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public Topic GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Topic_GetById", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadBaseTopic(dataReader);
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

        public IEnumerable<Topic> GetAll()
        {
            var dataReader = sqlHelper.GetDataReader("DAH_Topic_GetAll", CommandType.StoredProcedure, null, out connection);

            try
            {
                return ReadManyTopic(dataReader);
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
            object scalarValue = sqlHelper.GetScalarValue("DAH_Topic_Scalar", CommandType.StoredProcedure);

            return Convert.ToInt32(scalarValue);
        }
    }
}
