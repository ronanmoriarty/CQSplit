using System;
using System.Data.SqlClient;
using System.Security.Permissions;
using CQRSTutorial.DAL;
using CQRSTutorial.Publisher;
using log4net;

namespace Cafe.Waiter.Publish.Service
{
    public class PublishService : IDisposable
    {
        private readonly IConnectionStringProviderFactory _connectionStringProviderFactory;
        private readonly IOutboxToMessageQueuePublisher _outboxToMessageQueuePublisher;
        private readonly IOutboxToMessageQueuePublisherConfiguration _outboxToMessageQueuePublisherConfiguration;
        private SqlConnection _connection;
        private SqlDependency _sqlDependency;
        private bool _subscribedToOnChangeEvent;
        private readonly ILog _logger = LogManager.GetLogger(typeof(PublishService));

        public PublishService(IConnectionStringProviderFactory connectionStringProviderFactory,
            IOutboxToMessageQueuePublisher outboxToMessageQueuePublisher,
            IOutboxToMessageQueuePublisherConfiguration outboxToMessageQueuePublisherConfiguration)
        {
            _connectionStringProviderFactory = connectionStringProviderFactory;
            _outboxToMessageQueuePublisher = outboxToMessageQueuePublisher;
            _outboxToMessageQueuePublisherConfiguration = outboxToMessageQueuePublisherConfiguration;
        }

        public void Start()
        {
            try
            {
                EnsureCanRequestNotifications();
                var connectionString = GetConnectionString();
                SqlDependency.Start(connectionString, _outboxToMessageQueuePublisherConfiguration.QueueName);
                _connection = new SqlConnection(connectionString);
                using (var command = new SqlCommand(_outboxToMessageQueuePublisherConfiguration.QueryToWatch, _connection))
                {
                    _sqlDependency = new SqlDependency(command);
                    _sqlDependency.OnChange += OnChange;
                    _subscribedToOnChangeEvent = true;
                    _connection.Open();
                    command.ExecuteReader();
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Error starting Publish Service", exception);
                throw;
            }
        }

        private void EnsureCanRequestNotifications()
        {
            if (!CanRequestNotifications())
            {
                throw new Exception("Current user does not have permission to query notifications");
            }

            _logger.Info("User has permission to query notifications");
        }

        public void Dispose()
        {
            Stop();
        }

        public void Stop()
        {
            try
            {
                if (_subscribedToOnChangeEvent)
                {
                    _sqlDependency.OnChange -= OnChange;
                    _subscribedToOnChangeEvent = false;
                }
                _connection?.Dispose();
            }
            catch(Exception exception)
            {
                _logger.Error("Error stopping Publish Service", exception);
            }
            finally
            {
                SqlDependency.Stop(GetConnectionString(), _outboxToMessageQueuePublisherConfiguration.QueueName);
            }
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

        private bool CanRequestNotifications()
        {
            var permission = new SqlClientPermission(PermissionState.Unrestricted);
            try
            {
                permission.Demand();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}