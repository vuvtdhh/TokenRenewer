using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenRenewer.Helpers
{
    class SqlConnector
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SMSService"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
            }
             catch (SqlException ex)
            {
                //Console.WriteLine("[SQL SERVER DATABASE] Connection create error, " + ex.ToString());
            }
            return conn;
        }
    }
}
