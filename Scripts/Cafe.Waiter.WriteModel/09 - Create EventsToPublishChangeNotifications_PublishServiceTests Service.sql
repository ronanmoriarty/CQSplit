IF NOT EXISTS(SELECT 1 FROM sys.services WHERE name = 'EventsToPublishChangeNotifications_PublishServiceTests')
BEGIN
	CREATE SERVICE EventsToPublishChangeNotifications_PublishServiceTests
	  ON QUEUE EventsToPublishChangeMessages_PublishServiceTests
	([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]);
END