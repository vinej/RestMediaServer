using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;

namespace SqlDAL.DAL
{
    public class AdvertiseDal : BaseDal<Advertiser>
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

        public async Task<long> Insert(Advertiser advertiser)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(advertiser, parameters);

            var lastId = await sqlHelper.InsertAsync("DAH_Advertiser_Insert", CommandType.StoredProcedure, parameters.ToArray());
            advertiser.Id = lastId;
            return lastId;
        }

        public async Task<long> Update(Advertiser advertiser)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", advertiser.Id, DbType.Int64)
            };
            CreateParameter(advertiser, parameters);
            return await sqlHelper.UpdateAsync("DAH_Advertiser_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<long> Delete(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };

            return await sqlHelper.DeleteAsync("DAH_Advertiser_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<Advertiser> GetById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Advertiser_GetById", parameters, ReadAdvertiser);
        }

        public async Task<Advertiser> GetByEmail(string email)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@Email", email, DbType.String)
            };
            return await ReadSingleFunc("DAH_Advertiser_GetByEmail", parameters, ReadAdvertiser);
        }

        public async Task<IEnumerable<Advertiser>> GetAll()
        {
            return await ReadManyFunc("DAH_Advertiser_GetByEmail", null, ReadManyAdvertiser);
        }

        public async Task<IEnumerable<Advertiser>> GetByIsAccepted(bool isAccepted)
        {
            var parameters = new List<SqlParameter>
            {
                sqlHelper.CreateParameter("@IsAccepted", isAccepted, DbType.Boolean)
            };
            return await ReadManyFunc("DAH_Advertiser_GetByIsAccepted", parameters, ReadManyAdvertiser);
        }
    }
}
