using System.Configuration;
using System.Data.SqlClient;
using SqlDAL.Core;

namespace SqlDAL.DAL
{
    public class BaseDal
    {
        public SqlHelper sqlHelper = null;
        public SqlConnection connection = null;

        public BaseDal()
        {
            sqlHelper = new SqlHelper(ConfigurationManager.ConnectionStrings["DBConnection"].ToString());
        }

        public void CloseConnection()
        {
            sqlHelper.CloseConnection(connection);
        }
    }
}
