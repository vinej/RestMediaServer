using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            connectionString = @"Data Source=DESKTOP-IPK8MKF\SQLEXPRESS;Initial Catalog=RestMediaServer;Integrated Security=True";
        }

        public long OpenConnection()
        {
            logger.Info($"Opening connection by {System.Security.Principal.WindowsIdentity.GetCurrent().Name}");
            connection = new SqlConnection(connectionString);
             connection.Open();
            return 0;
        }

        public void CloseConnection()
        {
            logger.Info("Closing connection");
            connection.Close();
            connection = null;
        }

        public IEnumerable<TType> ReadManyFunc(string commandText, List<SqlParameter> parameters, Func<IDataReader, IEnumerable<TType>> readItem)
        {
            logger.Info($"SQL START: {commandText}");
            _ =  OpenConnection();
            var dataReader =  GetDataReader(commandText, CommandType.StoredProcedure, parameters?.ToArray(), connection);
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

        public TType ReadSingleFunc(string commandText, List<SqlParameter> parameters, Func<IDataReader, TType> readItem)
        {
            logger.Info($"SQL START: {commandText}");
            _ =  OpenConnection();
            using (var dataReader =  GetDataReader(commandText, CommandType.StoredProcedure, parameters?.ToArray(), connection))
            {
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
                    CloseConnection();
                }
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

        public IDataReader GetDataReader(string commandText, CommandType commandType, SqlParameter[] parameters, SqlConnection connection)
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
                reader =  command.ExecuteReader();
            }
            catch (Exception ex)
            {
                logger.Error($"SQL Exception: {ex.Message}");
                SqlException(ex.Message);
                throw;
            }
            return reader;
        }

        public long Command(bool isInsert, string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            long newid = -1;
            _ =  OpenConnection();
            try {
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
                            object temp = command.ExecuteScalar();
                            newid = Convert.ToInt64(temp);
                        }
                        else
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Rollback();
                        logger.Error($"SQL Exception: {ex.Message}");
                        SqlException(ex.Message);
                    }
                    finally
                    {
                        transactionScope.Commit();
                    }
                    return newid;
                }
            } finally
            {
                CloseConnection();
            }
        }

        private string SqlException(string exception)
        {
            var message = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            string error;
            if (exception.Contains("has too many arguments specified"))
            {
                error = "DAL : Too many parameter";
            }
            else if (exception.Contains("expects parameter"))
            {
                error = "DAL : Missing parameter";
            }
            else if (exception.Contains("Violation of UNIQUE KEY constraint"))
            {
                error = "DAL : Duplicate key";
            }
            else if (exception.Contains("String was not recognized as a valid DateTime"))
            {
                error = "DAL : DateTime invalid format";
            }
            else
            {
                error = "DAL : Unknown error";

            }
            message.Content = new System.Net.Http.StringContent(error);
            throw new System.Web.Http.HttpResponseException(message);
        }

        public  long Delete(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            return  Command(false, commandText, commandType, IsolationLevel.ReadCommitted, parameters);
        }
        public  long Delete(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            return  Command(false, commandText, commandType, isolationLevel, parameters);
        }

        public  long Insert(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            return  Command(true, commandText, commandType, IsolationLevel.ReadCommitted, parameters);
        }

        public  long Insert(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            return  Command(true, commandText, commandType, isolationLevel, parameters);
        }

        public  long Update(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            return  Command(false, commandText, commandType, IsolationLevel.ReadCommitted, parameters);
        }

        public  long Update(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
        {
            return  Command(false, commandText, commandType, isolationLevel, parameters);
        }
    }
}
