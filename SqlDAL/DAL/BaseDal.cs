using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
//using System.Web.Http;
using NLog;

namespace SqlDAL.DAL
{
    public class BaseDal<TType>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public SqlConnection connection = null;
        public string connectionString = null;

        public BaseDal()
        {
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog = RestMediaServer; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
        }

        public SqlConnection OpenConnection()
        {
            logger.Info($"Opening connection by {HttpContext.Current.User.Identity.Name}");
            connection = new SqlConnection(connectionString);
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
            logger.Info($"SQL START: {commandText}");
            connection = OpenConnection();
            var dataReader = await GetDataReaderAsync(commandText, CommandType.StoredProcedure, parameters.ToArray(), connection);
            logger.Info($"SQL END: {commandText}");
            try
            {
                return readItem(dataReader);
            }
            catch (Exception ex)
            {
                logger.Error($"SQL Exception: {ex.Message}");
                SqlException(ex.Message);
                throw;
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
        }

        public async Task<TType> ReadSingleFunc(string commandText, List<SqlParameter> parameters, Func<IDataReader, TType> readItem)
        {
            logger.Info($"SQL START: {commandText}");
            connection = OpenConnection();
            var dataReader = await GetDataReaderAsync(commandText, CommandType.StoredProcedure, parameters.ToArray(), connection);
            logger.Info($"SQL END: {commandText}");
            try
            {
                return readItem(dataReader);
            }
            catch (Exception ex)
            {
                logger.Error($"SQL Exception: {ex.Message}");
                SqlException(ex.Message);
                throw;
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
        }

        public SqlParameter CreateParameter(string name, object value, DbType dbType)
        {
            return CreateParameter(name, 0, value, dbType, ParameterDirection.Input);
        }

        public SqlParameter CreateParameter(string name, int size, object value, DbType dbType)
        {
            return CreateParameter(name, size, value, dbType, ParameterDirection.Input);
        }

        public SqlParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                DbType = dbType,
                ParameterName = name,
                Size = size,
                Direction = direction,
                Value = value
            };
        }

        public async Task<IDataReader> GetDataReaderAsync(string commandText, CommandType commandType, SqlParameter[] parameters, SqlConnection connection)
        {
            IDataReader reader;
            var command = new SqlCommand(commandText, connection)
            {
                CommandType = commandType
            };
            logger.Info($"SQL START: {commandText}");
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    logger.Info($"    Parameter: {parameter.ParameterName} : {parameter.Value} : {parameter.SqlDbType.ToString()}");
                    command.Parameters.Add(parameter);
                }
            }

            logger.Info($"SQL END: {commandText}");
            try
            {
                reader = await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                logger.Error($"SQL Exception: {ex.Message}");
                SqlException(ex.Message);
                throw;
            }

            return reader;
        }

        public async Task<long> CommandAsync(bool isInsert, string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            long newid = -1;
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var transactionScope = connection.BeginTransaction(isolationLevel))
                using (var command = new SqlCommand(commandText, connection, transactionScope))
                {
                    logger.Info($"SQL START: {commandText}");
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            logger.Info($"    Parameter: {parameter.ParameterName} : {parameter.Value} : {parameter.SqlDbType.ToString()}");
                            command.Parameters.Add(parameter);
                        }
                    }
                    logger.Info($"SQL END: {commandText}");

                    try
                    {
                        if (isInsert)
                        {
                            object temp = await command.ExecuteScalarAsync();
                            newid = Convert.ToInt64(temp);
                        }
                        else
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Rollback();
                        logger.Error($"SQL Exception: {ex.Message}");
                        SqlException(ex.Message);
                    }
                    transactionScope.Commit();
                    return newid;
                }
            }
        }

        private string SqlException(string exception)
        {
            var message = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            string error;
            if (exception.Contains("Violation of UNIQUE KEY constraint"))
            {
                error =  "500 : Duplicate key";
            } else
            {
                error = "500 : Unknown error";
            }
            message.Content = new System.Net.Http.StringContent(error);
            throw new System.Web.Http.HttpResponseException(message);
        }

        public async Task<long> DeleteAsync(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            return await CommandAsync(false, commandText, commandType, IsolationLevel.ReadCommitted, parameters);
        }
        public async Task<long> DeleteAsync(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            return await CommandAsync(false, commandText, commandType, isolationLevel, parameters);
        }

        public async Task<long> InsertAsync(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            return await CommandAsync(true, commandText, commandType, IsolationLevel.ReadCommitted, parameters);
        }

        public async Task<long> InsertAsync(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            return await CommandAsync(true, commandText, commandType, isolationLevel, parameters);
        }

        public async Task<long> UpdateAsync(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            return await CommandAsync(false, commandText, commandType, IsolationLevel.ReadCommitted, parameters);
        }

        public async Task<long> UpdateAsync(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            return await CommandAsync(false, commandText, commandType, isolationLevel, parameters);
        }
    }
}
