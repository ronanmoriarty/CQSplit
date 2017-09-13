CREATE QUEUE EventsToPublishChangeMessages;

CREATE SERVICE EventsToPublishChangeNotifications  
  ON QUEUE EventsToPublishChangeMessages
([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]);