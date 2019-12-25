using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;

namespace SqlDAL.DAL
{
    public class AdvertiseDal : BaseDal
    {
        private Video ReadVideo(IDataReader dataReader)
        {
            var video = new Video();
            var isData = dataReader.Read();
            if (isData)
            {
                video.Id = (int)dataReader["VId"];
                video.Url = dataReader["VUrl"].ToString();
                video.Advertiser = new Advertiser
                {
                    Id = (int)dataReader["VAdvertiserId"]
                };
            }
            return video;
        }


        private IEnumerable<Advertiser> ReadManyFullAdvertiserWithVideo(IDataReader dataReader)
        {
            var advertisers = new List<Advertiser>();
            int lastAdvertiserId = -1;
            while (dataReader.Read())
            {
                Advertiser advertiser = null;
                int id = (int)dataReader["Id"];
                if (id != lastAdvertiserId)
                {
                    if (lastAdvertiserId != -1)
                    {
                        advertisers.Add(advertiser);
                    }
                    lastAdvertiserId = id;
                    advertiser = new Advertiser();
                    ReadBaseAdvertiser(advertiser, dataReader);
                    advertiser.Videos = new List<Video>();
                }
                Video video = ReadVideo(dataReader);
                video.Advertiser.Email = advertiser.Email;
                video.Advertiser.Name = advertiser.Name;
                video.Advertiser.Dob = advertiser.Dob;
                video.Advertiser.IsAccepted = advertiser.IsAccepted;
                ((List<Video>)advertiser.Videos).Add(video);
            }
            return advertisers;
        }

        private IEnumerable<Advertiser> ReadManyAdvertiser(IDataReader dataReader)
        {
            var advertisers = new List<Advertiser>();
            while (dataReader.Read())
            {
                advertisers.Add(ReadAdvertiser(dataReader));
            }
            return advertisers;
        }

        private Advertiser ReadAdvertiser(IDataReader dataReader)
        {
            var advertiser = new Advertiser();
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseAdvertiser(advertiser, dataReader);
            }
            return advertiser;
        }

        private void ReadBaseAdvertiser(Advertiser advertiser, IDataReader dataReader)
        {
            advertiser.Id = (int)dataReader["Id"];
            advertiser.Email = dataReader["Email"].ToString();
            advertiser.Name = dataReader["Name"].ToString();
            advertiser.IsAccepted = (bool)dataReader["IsAccepted"];
            advertiser.Dob = DateTime.Parse(dataReader["Dob"].ToString());
        }

        private void CreateParameter(Advertiser advertiser, List<SqlParameter> parameters)
        {
            parameters.Add(sqlHelper.CreateParameter("@Email", 255, advertiser.Email, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@Name", 255, advertiser.Name, DbType.String));
            parameters.Add(sqlHelper.CreateParameter("@IsAccepted", advertiser.IsAccepted, DbType.Boolean));
            parameters.Add(sqlHelper.CreateParameter("@Dob", advertiser.Dob, DbType.DateTime));
        }

        public int Insert(Advertiser advertiser)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(advertiser, parameters);

            sqlHelper.Insert("DAH_Advertiser_Insert", CommandType.StoredProcedure, parameters.ToArray(), out int lastId);
            advertiser.Id = lastId;
            return lastId;
        }

        public void Update(Advertiser advertiser)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", advertiser.Id, DbType.Int32)
            };
            CreateParameter(advertiser, parameters);
            sqlHelper.Update("DAH_Advertiser_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void Delete(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            sqlHelper.Delete("DAH_Advertiser_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public Advertiser GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int32)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Advertiser_GetById", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadAdvertiser(dataReader);
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

        public Advertiser GetByEmail(string alias)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Email", alias, DbType.String)
            };

            var dataReader = sqlHelper.GetDataReader("DAH_Advertiser_GetByEmail", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            try
            {
                return ReadAdvertiser(dataReader);
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

        public IEnumerable<Advertiser> GetAll()
        {
            var dataReader = sqlHelper.GetDataReader("DAH_Advertiser_GetAll", CommandType.StoredProcedure, null, out connection);

            try
            {
                return ReadManyAdvertiser(dataReader);
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

        public IEnumerable<Advertiser> GetAllWithVideo()
        {
            var dataReader = sqlHelper.GetDataReader("DAH_Advertiser_GetAllWithVideo", CommandType.StoredProcedure, null, out connection);

            try
            {
                return ReadManyFullAdvertiserWithVideo(dataReader);
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

        public IEnumerable<Advertiser> GetByIsAccepted(bool isAccepted)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@IsAccepted", isAccepted, DbType.Boolean)
            };
            var dataReader = sqlHelper.GetDataReader("DAH_Advertiser_GetByIsAccepted", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            try
            {
                return ReadManyAdvertiser(dataReader);
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

        public Advertiser GetByIdWithVideo(int id)
        {
            Advertiser advertiser = GetById(id);
            advertiser.Videos = new VideoDal().GetAllByAdvertiser(advertiser.Id);
            return advertiser;
        }
    }
}
