using System;
using System.Data.SqlClient;
using CQRSTutorial.DAL;

namespace CQRSTutorial.Publisher
{
    public class EventToPublishNotifier : IDisposable
    {
        private readonly IConnectionStringProviderFactory _connectionStringProviderFactory;
        private readonly IOutboxToMessageQueuePublisherConfiguration _outboxToMessageQueuePublisherConfiguration;
        private readonly Action _onNewEventQueuedForPublishing;
        private SqlConnection _connection;
        private SqlDependency _sqlDependency;
        private bool _subscribedToOnChangeEvent;

        public EventToPublishNotifier(IConnectionStringProviderFactory connectionStringProviderFactory, 
            IOutboxToMessageQueuePublisherConfiguration outboxToMessageQueuePublisherConfiguration, 
            Action onNewEventQueuedForPublishing)
        {
            _connectionStringProviderFactory = connectionStringProviderFactory;
            _outboxToMessageQueuePublisherConfiguration = outboxToMessageQueuePublisherConfiguration;
            _onNewEventQueuedForPublishing = onNewEventQueuedForPublishing;
        }

        public void Start()
        {
            var connectionString = GetConnectionString();
            _connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT Id, EventType, Data, Created FROM dbo.EventsToPublish", _connection);
            command.Notification = null;
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
            var connectionString = GetConnectionString();
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
            _onNewEventQueuedForPublishing();
        }
    }
}