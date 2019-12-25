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
            //sqlHelper = new SqlHelper(ConfigurationManager.ConnectionStrings["DBConnection"].ToString());
            sqlHelper = new SqlHelper(@"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog = RestMediaServer; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
        }

        public void CloseConnection()
        {
            sqlHelper.CloseConnection(connection);
        }
    }
}
