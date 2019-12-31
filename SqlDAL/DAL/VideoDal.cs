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
                Id = (long)dataReader["AdvertiserId"]
            };
            video.Url = dataReader["Url"].ToString();
            video.Dob = DateTime.Parse(dataReader["Dob"].ToString());
        }

        private void ReadBaseFullVideo(Video video, IDataReader dataReader)
        {
            if (video.Id != -1)
            {
                video.Advertiser.Name = (string)dataReader["AName"];
                video.Advertiser.Email = (string)dataReader["AEmail"];
                video.Advertiser.Dob = (DateTime)dataReader["ADob"];
            }
        }

        private void CreateParameter(Video video, List<SqlParameter> parameters)
        {
            parameters.Add(CreateParameter("@AdvertiserId", video.Advertiser.Id, DbType.Int64));
            parameters.Add(CreateParameter("@Url", 255, video.Url, DbType.String));
            parameters.Add(CreateParameter("@Dob", video.Dob, DbType.DateTime));
        }

        public  long Insert(Video video)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(video, parameters);

            var lastId =  Insert("DAH_Video_Insert", CommandType.StoredProcedure, parameters.ToArray());
            video.Id = lastId;
            return lastId;
        }

        public  long Update(Video video)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", video.Id, DbType.Int64)
            };
            CreateParameter(video, parameters);
            return  Update("DAH_Video_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public  long Delete(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };

            return  Delete("DAH_Video_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public Video GetById(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return  ReadSingleFunc("DAH_Video_GetById", parameters, ReadVideo);
        }

        public Video GetByIdWithAdvertiser(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return  ReadSingleFunc("DAH_Video_GetByIdWithAdvertiser", parameters, ReadFullVideo);
        }

        public IEnumerable<Video> GetAll()
        {
            return  ReadManyFunc("DAH_Video_GetAll", null, ReadManyVideo);
        }

        public IEnumerable<Video> GetAllByAdvertiser(int id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return  ReadManyFunc("DAH_Video_GetAllByAdvertiser", parameters, ReadManyVideo);
        }


        public IEnumerable<Video> GetAllWithAdvertiser()
        {
            return  ReadManyFunc("DAH_Video_GetAllWithAdvertiser", null, ReadManyFullVideo);
        }
    }
}
