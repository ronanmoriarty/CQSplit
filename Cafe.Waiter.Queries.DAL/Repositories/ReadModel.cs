using System.Configuration;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class ReadModel
    {
        public static string GetEntityFrameworkConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Cafe.Waiter.EF"].ConnectionString;
        }
    }
}