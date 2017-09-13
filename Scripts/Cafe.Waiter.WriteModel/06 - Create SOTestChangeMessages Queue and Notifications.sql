IF NOT EXISTS(select * from sys.objects where object_id = object_id(N'SOTestChangeMessages'))
BEGIN
	CREATE QUEUE SOTestChangeMessages;

	CREATE SERVICE SOTestChangeNotifications
	  ON QUEUE SOTestChangeMessages
	([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]);
END