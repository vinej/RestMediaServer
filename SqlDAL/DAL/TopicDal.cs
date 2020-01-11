using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;

namespace SqlDAL.DAL
{
    public class TopicDal : BaseDal<Topic>
    {
        private IEnumerable<Topic> ReadManyTopic(IDataReader dataReader)
        {
            var topics = new List<Topic>();
            while (dataReader.Read())
            {
                var topic = new Topic
                {
                    Id = -1
                };
                ReadBaseTopic(topic, dataReader);
                topics.Add(topic);
            }
            return topics;
        }
        private Topic ReadTopic(IDataReader dataReader)
        {
            var topic = new Topic
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseTopic(topic, dataReader);
            }
            return topic;
        }

        private void ReadBaseTopic(Topic topic,IDataReader dataReader)
        {
            topic.Id = (long)dataReader["Id"];
            topic.Description = (string)dataReader["Description"];
            topic.StartDate = (DateTime)dataReader["StartDate"];
            topic.EndDate = (DateTime)dataReader["EndDate"];
            topic.IsActivated = (bool)dataReader["IsActivated"];
            topic.Dob = (DateTime)dataReader["Dob"];
        }

        private void CreateParameter(Topic topic, List<SqlParameter> parameters)
        {
            parameters.Add(CreateParameter("@Description", 255, topic.Description, DbType.String));
            parameters.Add(CreateParameter("@StartDate", topic.StartDate, DbType.DateTime));
            parameters.Add(CreateParameter("@EndDate", topic.EndDate, DbType.DateTime));
            parameters.Add(CreateParameter("@IsActivated", topic.IsActivated, DbType.Boolean));
            parameters.Add(CreateParameter("@Dob", topic.Dob, DbType.DateTime));
        }

        public  long Insert(Topic topic)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(topic, parameters);
            long lastId =  Insert("DAH_Topic_Insert", CommandType.StoredProcedure, parameters.ToArray());
            topic.Id = lastId;
            return lastId;
        }

        public  long Update(Topic topic)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", topic.Id, DbType.Int64)
            };
            CreateParameter(topic, parameters);
            return  Update("DAH_Topic_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public  long Delete(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };

            return  Delete("DAH_Topic_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public Topic GetById(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return  ReadSingleFunc("DAH_Topic_GetById", parameters, ReadTopic);
        }

        public IEnumerable<Topic> GetAll()
        {
            return  ReadManyFunc("DAH_Topic_GetAll", null, ReadManyTopic);
        }

        public Topic GetCurrent()
        {
            return ReadSingleFunc("DAH_Topic_GetCurrent", null, ReadTopic);
        }

    }
}
