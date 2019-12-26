using NLog;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;

namespace SqlDAL.Core
{
    public class SqlHelper
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public string ConnectionString { get; set; }

        public SqlHelper(string connectionString)
        {
            ConnectionString = connectionString;
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
            reader = await command.ExecuteReaderAsync();

            return reader;
        }

        public async Task<long> CommandAsync(bool isInsert, string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            long newid = -1;
            using (var connection = new SqlConnection(ConnectionString))
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
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                        throw;
                    }
                    transactionScope.Commit();
                    return newid;
                }
            }
        }

        public async Task<long> DeleteAsync(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            return await CommandAsync(false, commandText, commandType, IsolationLevel.ReadCommitted, parameters);
        }
        public async Task<long> DeleteAsync(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            return await CommandAsync(false,commandText, commandType, isolationLevel, parameters);
        }

        public async Task<long> InsertAsync(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            return await CommandAsync(true,commandText, commandType, IsolationLevel.ReadCommitted, parameters);
        }

        public async Task<long> InsertAsync(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            return await CommandAsync(true,commandText, commandType, isolationLevel, parameters);
        }

        public async Task<long> UpdateAsync(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            return await CommandAsync(false,commandText, commandType, IsolationLevel.ReadCommitted, parameters);
        }

        public async Task<long> UpdateAsync(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            return await CommandAsync(false,commandText, commandType, isolationLevel, parameters);
        }
    }
}
