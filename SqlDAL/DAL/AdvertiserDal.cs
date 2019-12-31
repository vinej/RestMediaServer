using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;

namespace SqlDAL.DAL
{
    public class AdvertiserDal : BaseDal<Advertiser>
    {
        private IEnumerable<Advertiser> ReadManyAdvertiser(IDataReader dataReader)
        {
            var advertisers = new List<Advertiser>();
            while (dataReader.Read())
            {
                var advertiser = new Advertiser
                {
                    Id = -1
                };
                ReadBaseAdvertiser(advertiser, dataReader);
                advertisers.Add(advertiser);
            }
            return advertisers;
        }

        private Advertiser ReadAdvertiser(IDataReader dataReader)
        {
            var advertiser = new Advertiser
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseAdvertiser(advertiser, dataReader);
            }
            return advertiser;
        }

        private void ReadBaseAdvertiser(Advertiser advertiser, IDataReader dataReader)
        {
            advertiser.Id = (long)dataReader["Id"];
            advertiser.Email = (string)dataReader["Email"];
            advertiser.Name = (string)dataReader["Name"];
            advertiser.IsAccepted = (bool)dataReader["IsAccepted"];
            advertiser.Dob = (DateTime)dataReader["Dob"];
        }

        private void CreateParameter(Advertiser advertiser, List<SqlParameter> parameters)
        {
            parameters.Add(CreateParameter("@Email", 255, advertiser.Email, DbType.String));
            parameters.Add(CreateParameter("@Name", 255, advertiser.Name, DbType.String));
            parameters.Add(CreateParameter("@IsAccepted", advertiser.IsAccepted, DbType.Boolean));
            parameters.Add(CreateParameter("@Dob", advertiser.Dob, DbType.DateTime));
        }

        public  long Insert(Advertiser advertiser)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(advertiser, parameters);

            var lastId =  Insert("DAH_Advertiser_Insert", CommandType.StoredProcedure, parameters.ToArray());
            advertiser.Id = lastId;
            return lastId;
        }

        public  long Update(Advertiser advertiser)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", advertiser.Id, DbType.Int64)
            };
            CreateParameter(advertiser, parameters);
            return  Update("DAH_Advertiser_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public  long Delete(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };

            return  Delete("DAH_Advertiser_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public Advertiser GetById(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return  ReadSingleFunc("DAH_Advertiser_GetById", parameters, ReadAdvertiser);
        }

        public Advertiser GetByEmail(string email)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Email", email, DbType.String)
            };
            return ReadSingleFunc("DAH_Advertiser_GetByEmail", parameters, ReadAdvertiser);
        }

        public IEnumerable<Advertiser> GetAll()
        {
            return  ReadManyFunc("DAH_Advertiser_GetAll", null, ReadManyAdvertiser);
        }

        public IEnumerable<Advertiser> GetByIsAccepted(bool isAccepted)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@IsAccepted", isAccepted, DbType.Boolean)
            };
            return  ReadManyFunc("DAH_Advertiser_GetByIsAccepted", parameters, ReadManyAdvertiser);
        }
    }
}
