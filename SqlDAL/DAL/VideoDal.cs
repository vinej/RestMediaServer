using System;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;

namespace SqlDAL.DAL
{
    public class VideoDal : BaseDal<Video>
    {
        private IEnumerable<Video> ReadManyFullVideo(IDataReader dataReader)
        {
            var Videos = new List<Video>();
            while (dataReader.Read())
            {
                var video = new Video
                {
                    Id = -1
                };
                ReadBaseVideo(video, dataReader);
                ReadBaseFullVideo(video, dataReader);
                Videos.Add(video);
            }
            return Videos;
        }

        private IEnumerable<Video> ReadManyVideo(IDataReader dataReader)
        {
            var videos = new List<Video>();
            while (dataReader.Read())
            {
                var video = new Video
                {
                    Id = -1
                };
                ReadBaseVideo(video, dataReader);
                videos.Add(video);
            }
            return videos;
        }

        private Video ReadVideo(IDataReader dataReader)
        {
            var video = new Video
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseVideo(video, dataReader);
            }
            return video;
        }

        private Video ReadFullVideo(IDataReader dataReader)
        {
            var video = new Video
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseVideo(video, dataReader);
                ReadBaseFullVideo(video, dataReader);
            }
            return video;
        }

        private void ReadBaseVideo(Video video,IDataReader dataReader)
        {
            video.Id = (long)dataReader["Id"];
            video.Advertiser = new Advertiser
            {
                Id = (long)dataReader["AdvertiseId"]
            };
            video.Url = dataReader["Url"].ToString();
            video.Dob = DateTime.Parse(dataReader["Dob"].ToString());
        }

        private void ReadBaseFullVideo(Video video, IDataReader dataReader)
        {
            if (video.Id != -1)
            {
                video.Advertiser.Name = dataReader["AName"].ToString();
                video.Advertiser.Email = dataReader["AEmail"].ToString();
                video.Advertiser.Dob = DateTime.Parse(dataReader["ADob"].ToString());
            }
        }

        private void CreateParameter(Video video, List<SqlParameter> parameters)
        {
            parameters.Add(CreateParameter("@AdvertiseId", video.Advertiser.Id, DbType.String));
            parameters.Add(CreateParameter("@Url", 255, video.Url, DbType.String));
            parameters.Add(CreateParameter("@Dob", video.Dob, DbType.DateTime));
        }

        public async Task<long> Insert(Video video)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(video, parameters);

            var lastId = await InsertAsync("DAH_Video_Insert", CommandType.StoredProcedure, parameters.ToArray());
            video.Id = lastId;
            return lastId;
        }

        public async Task<long> Update(Video video)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", video.Id, DbType.Int64)
            };
            CreateParameter(video, parameters);
            return await UpdateAsync("DAH_Video_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<long> Delete(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };

            return await DeleteAsync("DAH_Video_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<Video> GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Video_GetById", parameters, ReadVideo);
        }

        public async Task<Video> GetByIdWithAdvertiser(int id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Video_GetByIdWithAdvertiser", parameters, ReadFullVideo);
        }

        public async Task<IEnumerable<Video>> GetAll()
        {
            return await ReadManyFunc("DAH_Video_GetAll", null, ReadManyVideo);
        }

        public async Task<IEnumerable<Video>> GetAllByAdvertiser(int id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadManyFunc("DAH_Video_GetAllByAdvertiser", parameters, ReadManyVideo);
        }


        public async Task<IEnumerable<Video>> GetAllWithAdvertiser()
        {
            return await ReadManyFunc("DAH_Video_GetAllWithAdvertiser", null, ReadManyFullVideo);
        }
    }
}
