IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.EventsToPublishChangeMessages_PublishServiceTests'))
BEGIN
	CREATE QUEUE EventsToPublishChangeMessages_PublishServiceTests;
END