using CQRSTutorial.DAL;

namespace CQRSTutorial.Publisher.Tests
{
    public class WriteModelConnectionStringProviderFactory
    {
        public static ConnectionStringProviderFactory Instance => new ConnectionStringProviderFactory("CQRSTutorial.Cafe.Waiter", "CQRSTUTORIAL_CAFE_WAITER_WRITEMODEL_CONNECTIONSTRING_OVERRIDE");
    }
}