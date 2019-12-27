using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;

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
            topic.Dob = (DateTime)dataReader["Dob"];
        }

        private void CreateParameter(Topic topic, List<SqlParameter> parameters)
        {
            parameters.Add(CreateParameter("@Description", 255, topic.Description, DbType.String));
            parameters.Add(CreateParameter("@Dob", topic.Dob, DbType.DateTime));
        }

        public async Task<long> Insert(Topic topic)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(topic, parameters);
            long lastId = await InsertAsync("DAH_Topic_Insert", CommandType.StoredProcedure, parameters.ToArray());
            topic.Id = lastId;
            return lastId;
        }

        public async Task<long> Update(Topic topic)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", topic.Id, DbType.Int64)
            };
            CreateParameter(topic, parameters);
            return await UpdateAsync("DAH_Topic_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<long> Delete(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };

            return await DeleteAsync("DAH_Topic_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<Topic> GetById(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Topic_GetById", parameters, ReadTopic);
        }

        public async Task<IEnumerable<Topic>> GetAll()
        {
            return await ReadManyFunc("DAH_Topic_GetAll", null, ReadManyTopic);
        }
    }
}
