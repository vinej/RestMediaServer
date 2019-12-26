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
            topic.Description = dataReader["Description"].ToString();
            topic.Dob = DateTime.Parse(dataReader["Dob"].ToString());
        }

        private void CreateParameter(Topic topic, List<SqlParameter> parameters)
        {
            parameters.Add(sqlHelper.CreateParameter("@Description", 255, topic.Description, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Dob", topic.Dob, DbType.DateTime));
        }

        public async Task<long> Insert(Topic topic)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(topic, parameters);
            long lastId = await sqlHelper.InsertAsync("DAH_Topic_Insert", CommandType.StoredProcedure, parameters.ToArray());
            topic.Id = lastId;
            return lastId;
        }

        public async Task<long> Update(Topic topic)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", topic.Id, DbType.Int64)
            };
            CreateParameter(topic, parameters);
            return await sqlHelper.UpdateAsync("DAH_Topic_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<long> Delete(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };

            return await sqlHelper.DeleteAsync("DAH_Topic_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<Topic> GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Topic_GetById", parameters, ReadTopic);
        }

        public async Task<IEnumerable<Topic>> GetAll()
        {
            return await ReadManyFunc("DAH_Friend_GetAll", null, ReadManyTopic);
        }
    }
}
