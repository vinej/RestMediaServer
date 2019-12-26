using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;

namespace SqlDAL.DAL
{
    public class OpinionDal : BaseDal<Opinion>
    {
        private IEnumerable<Opinion> ReadManyFullOpinion(IDataReader dataReader)
        {
            var opinions = new List<Opinion>();
            while (dataReader.Read())
            {
                var opinion = new Opinion
                {
                    Id = -1
                };
                ReadBaseOpinion(opinion, dataReader);
                ReadBaseFullOpinion(opinion, dataReader);
                opinions.Add(opinion);
            }
            return opinions;
        }

        private IEnumerable<Opinion> ReadManyOpinion(IDataReader dataReader)
        {
            var opinions = new List<Opinion>();
            while (dataReader.Read())
            {
                var opinion = new Opinion
                {
                    Id = -1
                };
                ReadBaseOpinion(opinion, dataReader);
                opinions.Add(opinion);
            }
            return opinions;
        }

        private Opinion ReadOpinion(IDataReader dataReader)
        {
            var opinion = new Opinion
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseOpinion(opinion, dataReader);
            }
            return opinion;
        }
        private void ReadBaseOpinion(Opinion opinion,IDataReader dataReader)
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

        private void ReadBaseFullOpinion(Opinion opinion, IDataReader dataReader)
        {
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
        }

        private void CreateParameter(Opinion Opinion, List<SqlParameter> parameters)
        {
            parameters.Add(sqlHelper.CreateParameter("@MemberId", Opinion.Member.Id, DbType.Int64));
            parameters.Add(sqlHelper.CreateParameter("@TopicId", Opinion.Topic.Id, DbType.Int64));
            parameters.Add(sqlHelper.CreateParameter("@Dob", Opinion.Dob, DbType.DateTime));
        }

        public async Task<long> Insert(Opinion Opinion)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(Opinion, parameters);

            var lastId = await sqlHelper.InsertAsync("DAH_Opinion_Insert", CommandType.StoredProcedure, parameters.ToArray());
            Opinion.Id = lastId;
            return lastId;
        }

        public async Task<long> Update(Opinion Opinion)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", Opinion.Id, DbType.Int64)
            };
            CreateParameter(Opinion, parameters);
            return await sqlHelper.UpdateAsync("DAH_Opinion_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<long> Delete(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };

            return await sqlHelper.DeleteAsync("DAH_Opinion_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<Opinion> GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Opinion_GetById", parameters, ReadOpinion);
        }

        public async Task<IEnumerable<Opinion>> GetByMember(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@MemberId", id, DbType.String)
            };
            return await ReadManyFunc("DAH_Opinion_GetByMember", parameters, ReadManyOpinion);
        }

        public async Task<IEnumerable<Opinion>> GetByFullMember(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@MemberId", id, DbType.String)
            };
            return await ReadManyFunc("DAH_Opinion_GetByMember", parameters, ReadManyFullOpinion);
        }

        public async Task<IEnumerable<Opinion>> GetByTopic(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@TopicId", id, DbType.String)
            };
            return await ReadManyFunc("DAH_Opinion_GetByTopic", parameters, ReadManyOpinion);
        }

        public async Task<IEnumerable<Opinion>> GetByFullTopic(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@TopicId", id, DbType.String)
            };
            return await ReadManyFunc("DAH_Opinion_GetByTopic", parameters, ReadManyFullOpinion);
        }

        public async Task<IEnumerable<Opinion>> GetAll()
        {
            return await ReadManyFunc("DAH_Opinion_GetAll", null, ReadManyOpinion);
        }


        public async Task<IEnumerable<Opinion>> GetAllFull()
        {
            return await ReadManyFunc("DAH_Opinion_GetAllFull", null, ReadManyFullOpinion);
        }

    }
}
