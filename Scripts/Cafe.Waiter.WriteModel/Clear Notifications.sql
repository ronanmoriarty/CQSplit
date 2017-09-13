-- Taken from https://social.msdn.microsoft.com/Forums/vstudio/en-US/79e28ff6-67b3-4cfa-8b91-575c69614311/sqldependency-class-throws-the-given-key-was-not-present-in-the-dictionary-during?forum=sqlservicebroker

DECLARE item_cursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT e.conversation_handle
	FROM sys.conversation_endpoints e
		JOIN dbo.EventsToPublishChangeMessages_PublishServiceTests s -- change this queue as required - can see queue names by running "select * from sys.service_queues"
		ON e.conversation_handle = s.conversation_handle
OPEN item_cursor
DECLARE @conversation UNIQUEIDENTIFIER
FETCH NEXT FROM item_cursor INTO @conversation
WHILE @@FETCH_STATUS = 0 
BEGIN
    END CONVERSATION @conversation WITH CLEANUP
    FETCH NEXT FROM item_cursor INTO @conversation
END