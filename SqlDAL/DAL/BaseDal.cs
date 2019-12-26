using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using NLog;
using SqlDAL.Core;

namespace SqlDAL.DAL
{
    public class BaseDal<TType>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public SqlHelper sqlHelper = null;
        public SqlConnection connection = null;

        public BaseDal()
        {
            //sqlHelper = new SqlHelper(ConfigurationManager.ConnectionStrings["DBConnection"].ToString());
            sqlHelper = new SqlHelper(@"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog = RestMediaServer; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
        }

        public SqlConnection OpenConnection()
        {
            logger.Info($"Opening connection by {HttpContext.Current.User.Identity.Name}");
            connection = new SqlConnection(sqlHelper.ConnectionString);
            connection.Open();
            return connection;
        }

        public void CloseConnection()
        {
            logger.Info("Closing connection");
            connection.Close();
        }

        public async Task<IEnumerable<TType>> ReadManyFunc(string commandText, List<SqlParameter> parameters, Func<IDataReader, IEnumerable<TType>> readItem)
        {
            connection = OpenConnection();
            var dataReader = await sqlHelper.GetDataReaderAsync(commandText, CommandType.StoredProcedure, parameters.ToArray(), connection);
            try
            {
                return readItem(dataReader);
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

        public async Task<TType> ReadSingleFunc(string commandText, List<SqlParameter> parameters, Func<IDataReader, TType> readItem)
        {
            connection = OpenConnection();
            var dataReader = await sqlHelper.GetDataReaderAsync(commandText, CommandType.StoredProcedure, parameters.ToArray(), connection);
            try
            {
                return readItem(dataReader);
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

    }
}
