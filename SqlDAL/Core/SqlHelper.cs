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
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public string ConnectionString { get; set; }

        public SqlHelper(string connectionString)
        {
            logger.Info($"Opening connection by {HttpContext.Current.User.Identity.Name}");
            ConnectionString = connectionString;
        }

        public void CloseConnection(SqlConnection connection)
        {
            logger.Info("Closing connection");
            connection.Close();
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

        public IDataReader GetDataReader(string commandText, CommandType commandType, SqlParameter[] parameters, out SqlConnection connection)
        {
            IDataReader reader;
            connection = new SqlConnection(ConnectionString);
            connection.Open();

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
            reader = command.ExecuteReader();

            return reader;
        }

        public void Delete(string commandText, CommandType commandType, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(commandText, connection))
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
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Insert(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(commandText, connection))
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
                    command.ExecuteNonQuery();
                }
            }
        }

        public int Insert(string commandText, CommandType commandType, SqlParameter[] parameters, out int lastId)
        {
            lastId = 0;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(commandText, connection))
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

                    object newId = command.ExecuteScalar();
                    lastId = Convert.ToInt32(newId);
                }
            }

            return lastId;
        }

        public long Insert(string commandText, CommandType commandType, SqlParameter[] parameters, out long lastId)
        {
            lastId = 0;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(commandText, connection))
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

                    object newId = command.ExecuteScalar();
                    lastId = Convert.ToInt64(newId);
                }
            }

            return lastId;
        }

        public void InsertWithTransaction(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            SqlTransaction transactionScope;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();

                using (var command = new SqlCommand(commandText, connection))
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
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            SqlTransaction transactionScope;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);

                using (var command = new SqlCommand(commandText, connection))
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
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void Update(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(commandText, connection))
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

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateWithTransaction(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            SqlTransaction transactionScope;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction();

                using (var command = new SqlCommand(commandText, connection))
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
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            SqlTransaction transactionScope;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                transactionScope = connection.BeginTransaction(isolationLevel);

                using (var command = new SqlCommand(commandText, connection))
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
                        command.ExecuteNonQuery();
                        transactionScope.Commit();
                    }
                    catch (Exception)
                    {
                        transactionScope.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public object GetScalarValue(string commandText, CommandType commandType, SqlParameter[] parameters= null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(commandText, connection))
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

                    return command.ExecuteScalar();
                }
            }
        }
    }
}
