using System.Data.SqlClient;

namespace CQRSTutorial.DAL
{
    public interface ISqlConnectionFactory
    {
        SqlConnection Create();
    }
}