using System;
using System.Data.SqlClient;
using CQRSTutorial.DAL;
using CQRSTutorial.Publisher;
using log4net;

namespace Cafe.Waiter.Publish.Service
{
    public class PublishService : IDisposable
    {
        private readonly IConnectionStringProviderFactory _connectionStringProviderFactory;
        private readonly IOutboxToMessageQueuePublisher _outboxToMessageQueuePublisher;
        private SqlConnection _connection;
        private SqlDependency _sqlDependency;
        private bool _subscribedToOnChangeEvent;
        private readonly ILog _logger = LogManager.GetLogger(typeof(PublishService));

        public PublishService(IConnectionStringProviderFactory connectionStringProviderFactory,
            IOutboxToMessageQueuePublisher outboxToMessageQueuePublisher)
        {
            _connectionStringProviderFactory = connectionStringProviderFactory;
            _outboxToMessageQueuePublisher = outboxToMessageQueuePublisher;
        }

        public void Start()
        {
            _outboxToMessageQueuePublisher.PublishQueuedMessages(); // OnChange() not always firing. Short term hack! TODO: remove this later
            var connectionString = GetConnectionString();
            _connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT Id, EventType, Data, Created FROM dbo.EventsToPublish", _connection)
            {
                Notification = null
            };
            _sqlDependency = new SqlDependency(command, "SERVICE=EventsToPublishChangeNotifications", int.MaxValue);
            _sqlDependency.OnChange += OnChange;
            _subscribedToOnChangeEvent = true;
            _connection.Open();
            command.ExecuteReader();
        }

        public void Dispose()
        {
            Stop();
        }

        public void Stop()
        {
            if (_subscribedToOnChangeEvent)
            {
                _sqlDependency.OnChange -= OnChange;
                _subscribedToOnChangeEvent = false;
            }
            _connection?.Dispose();
        }

        private string GetConnectionString()
        {
            return _connectionStringProviderFactory.GetConnectionStringProvider().GetConnectionString();
        }

        private void OnChange(object sender, SqlNotificationEventArgs e)
        {
            _logger.Debug($"SqlNotificationInfo: {e.Info}");
            if (e.Info == SqlNotificationInfo.Insert)
            {
                _outboxToMessageQueuePublisher.PublishQueuedMessages();
            }
        }
    }
}