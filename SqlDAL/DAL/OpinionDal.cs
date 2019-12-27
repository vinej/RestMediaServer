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
            opinion.Id = (long)dataReader["Id"];
            opinion.Member = new Member
            {
                Id = (long)dataReader["MemberId"]
            };
            opinion.Topic = new Topic
            {
                Id = (long)dataReader["TopicId"]
            };
            opinion.Comment = (string)dataReader["Comment"];
            opinion.Dob = (DateTime)dataReader["Dob"];
        }

        private void ReadBaseFullOpinion(Opinion opinion, IDataReader dataReader)
        {
            if (opinion.Id != -1)
            {
                opinion.Member.Id = (long)dataReader["MemberId"];
                opinion.Member.Alias = (string)dataReader["MName"];
                opinion.Member.Email = (string)dataReader["MEmail"];
                opinion.Member.Dob = (DateTime)dataReader["MDob"];
                opinion.Topic.Id = (long)dataReader["TopicId"];
                opinion.Topic.Description = (string)dataReader["TDescription"];
                opinion.Topic.Dob = (DateTime)dataReader["TDob"];
            }
        }

        private void CreateParameter(Opinion Opinion, List<SqlParameter> parameters)
        {
            parameters.Add(CreateParameter("@MemberId", Opinion.Member.Id, DbType.Int64));
            parameters.Add(CreateParameter("@TopicId", Opinion.Topic.Id, DbType.Int64));
            parameters.Add(CreateParameter("@Dob", Opinion.Dob, DbType.DateTime));
        }

        public async Task<long> Insert(Opinion Opinion)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(Opinion, parameters);

            var lastId = await InsertAsync("DAH_Opinion_Insert", CommandType.StoredProcedure, parameters.ToArray());
            Opinion.Id = lastId;
            return lastId;
        }

        public async Task<long> Update(Opinion Opinion)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", Opinion.Id, DbType.Int64)
            };
            CreateParameter(Opinion, parameters);
            return await UpdateAsync("DAH_Opinion_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<long> Delete(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };

            return await DeleteAsync("DAH_Opinion_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<Opinion> GetById(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Opinion_GetById", parameters, ReadOpinion);
        }

        public async Task<IEnumerable<Opinion>> GetByMember(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@MemberId", id, DbType.Int64)
            };
            return await ReadManyFunc("DAH_Opinion_GetByMember", parameters, ReadManyOpinion);
        }

        public async Task<IEnumerable<Opinion>> GetByFullMember(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@MemberId", id, DbType.String)
            };
            return await ReadManyFunc("DAH_Opinion_GetByFullMember", parameters, ReadManyFullOpinion);
        }

        public async Task<IEnumerable<Opinion>> GetByTopic(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@TopicId", id, DbType.Int64)
            };
            return await ReadManyFunc("DAH_Opinion_GetByTopic", parameters, ReadManyOpinion);
        }

        public async Task<IEnumerable<Opinion>> GetByFullTopic(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@TopicId", id, DbType.Int64)
            };
            return await ReadManyFunc("DAH_Opinion_GetByFullTopic", parameters, ReadManyFullOpinion);
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
