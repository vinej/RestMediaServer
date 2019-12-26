using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;
using SqlDAL.Core;

namespace SqlDAL.DAL
{
    public class LikeDal : BaseDal<Like>
    {
        private IEnumerable<Like> ReadManyFullLike(IDataReader dataReader)
        {
            var Likes = new List<Like>();
            while (dataReader.Read())
            {
                var Like = new Like
                {
                    Id = -1
                };
                ReadBaseLike(Like, dataReader);
                ReadBaseFullLike(Like, dataReader);
                Likes.Add(Like);
            }
            return Likes;
        }

        private IEnumerable<Like> ReadManyLike(IDataReader dataReader)
        {
            var Likes = new List<Like>();
            while (dataReader.Read())
            {
                var Like = new Like
                {
                    Id = -1
                };
                ReadBaseLike(Like, dataReader);
                Likes.Add(Like);
            }
            return Likes;
        }

        private Like ReadLike(IDataReader dataReader)
        {
            var Like = new Like
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseLike(Like, dataReader);
            }
            return Like;
        }
        private void ReadBaseLike(Like Like,IDataReader dataReader)
        {
            Like.Id = (long)dataReader["Id"];
            Like.Member = new Member
            {
                Id = (long)dataReader["MemberId"]
            };
            Like.Opinion = new Opinion
            {
                Id = (long)dataReader["OpinionId"]
            };
            Like.Dob = DateTime.Parse(dataReader["Dob"].ToString());
        }

        private void ReadBaseFullLike(Like Like, IDataReader dataReader)
        {
            if (Like.Id != -1)
            {
                Like.Member.Id = (long)dataReader["MemberId"];
                Like.Member.Alias = dataReader["MName"].ToString();
                Like.Member.Email = dataReader["MEmail"].ToString();
                Like.Member.Dob = DateTime.Parse(dataReader["MDob"].ToString());
                Like.Opinion.Id = (long)dataReader["OpinionId"];
                Like.Opinion.Comment = dataReader["OComment"].ToString();
                Like.Opinion.Dob = DateTime.Parse(dataReader["ODob"].ToString());
            }
        }

        private void CreateParameter(Like Like, List<SqlParameter> parameters)
        {
            parameters.Add(sqlHelper.CreateParameter("@MemberId", Like.Member.Id, DbType.Int64));
            parameters.Add(sqlHelper.CreateParameter("@TopicId", Like.Opinion.Id, DbType.Int64));
            parameters.Add(sqlHelper.CreateParameter("@Dob", Like.Dob, DbType.DateTime));
        }

        public async Task<long> Insert(Like Like)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(Like, parameters);

            var lastId = await sqlHelper.InsertAsync("DAH_Like_Insert", CommandType.StoredProcedure, parameters.ToArray());
            Like.Id = lastId;
            return lastId;
        }

        public async Task<long> Update(Like Like)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", Like.Id, DbType.Int64)
            };
            CreateParameter(Like, parameters);
            return await sqlHelper.UpdateAsync("DAH_Like_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<long> Delete(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };

            return await sqlHelper.DeleteAsync("DAH_Like_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<Like> GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Like_GetById", parameters, ReadLike);
        }
    }
}
