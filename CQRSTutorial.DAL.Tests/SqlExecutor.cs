using System.Configuration;
using System.Data.SqlClient;

namespace CQRSTutorial.DAL.Tests
{
    public class SqlExecutor
    {
        public int ExecuteScalar(string commandText)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CQRSTutorial"].ConnectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = commandText;
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public void ExecuteNonQuery(string commandText)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CQRSTutorial"].ConnectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}