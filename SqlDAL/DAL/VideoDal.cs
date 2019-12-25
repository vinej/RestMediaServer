using System;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;

namespace SqlDAL.DAL
{
    public class VideoDal : BaseDal
    {
        private IEnumerable<Video> ReadManyFullVideo(IDataReader dataReader)
        {
            var Videos = new List<Video>();
            while (dataReader.Read())
            {
                Videos.Add(ReadFullVideo(dataReader));
            }
            return Videos;
        }

        private IEnumerable<Video> ReadManyVideo(IDataReader dataReader)
        {
            var videos = new List<Video>();
            while (dataReader.Read())
            {
                videos.Add(ReadBaseVideo(dataReader));
            }
            return videos;
        }

        private Video ReadBaseVideo(IDataReader dataReader)
        {
            var video = new Video
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                video.Id = (int)dataReader["Id"];
                video.Advertiser = new Advertiser
                {
                    Id = (int)dataReader["AdvertiseId"]
                };
                video.Url = dataReader["Url"].ToString();
                video.Dob = DateTime.Parse(dataReader["Dob"].ToString());
            }
            return video;
        }

        private Video ReadFullVideo(IDataReader dataReader)
        {
            var video = ReadBaseVideo(dataReader);
            if (video.Id != -1)
            {
                video.Advertiser.Name = dataReader["Name"].ToString();
                video.Advertiser.Email = dataReader["Email"].ToString();
                video.Advertiser.Dob = DateTime.Parse(dataReader["ADob"].ToString());
            }
            return video;
        }

        private void CreateParameter(Video video, List<SqlParameter> parameters)
        {
            parameters.Add(sqlHelper.CreateParameter("@AdvertiseId", video.Advertiser.Id, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Url", 255, video.Url, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Dob", video.Dob, DbType.DateTime));
        }

        public int Insert(Video video)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(video, parameters);

            sqlHelper.Insert("DAH_Video_Insert", CommandType.StoredProcedure, parameters.ToArray(), out int lastId);
            video.Id = lastId;
            return lastId;
        }

        public void Update(Video video)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", video.Id, DbType.Int32)
            };
            CreateParameter(video, parameters);
            sqlHelper.Update("DAH_Video_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void Delete(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            sqlHelper.Delete("DAH_Video_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public Video GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Video_GetById", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadBaseVideo(dataReader);
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

        public Video GetByEmail(string email)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Email", email, DbType.String)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Video_GetByEmail", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadBaseVideo(dataReader);
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

        public Video GetByEmailWithAdvertiser(string email)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Email", email, DbType.String)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Video_GetByEmailWithAdvertiser", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadFullVideo(dataReader);
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

        public Video GetByIdWithAdvertiser(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Video_GetByIdWithAdvertiser", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadFullVideo(dataReader);
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

        public IEnumerable<Video> GetAll()
        {
            var dataReader = sqlHelper.GetDataReader("DAH_Video_GetAll", CommandType.StoredProcedure, null, out connection);

            try
            {
                return ReadManyVideo(dataReader);
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

        public IEnumerable<Video> GetAllByAdvertiser(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Video_GetAllByAdvertiser", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            try
            {
                return ReadManyVideo(dataReader);
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


        public IEnumerable<Video> GetAllWithAdvertiser()
        {
            var dataReader = sqlHelper.GetDataReader("DAH_Video_GetAllWithAdvertiser", CommandType.StoredProcedure, null, out connection);

            try
            {
                return ReadManyFullVideo(dataReader);
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
            object scalarValue = sqlHelper.GetScalarValue("DAH_Video_Scalar", CommandType.StoredProcedure);

            return Convert.ToInt32(scalarValue);
        }
    }
}
