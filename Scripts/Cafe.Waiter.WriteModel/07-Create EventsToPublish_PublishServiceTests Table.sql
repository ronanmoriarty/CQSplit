IF EXISTS(SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID(N'dbo.EventsToPublish_PublishServiceTests'))
BEGIN
	DROP TABLE dbo.EventsToPublish_PublishServiceTests
END
GO

CREATE TABLE dbo.EventsToPublish_PublishServiceTests
(
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	EventType NVARCHAR(25) NOT NULL,
	[Data] NVARCHAR(MAX) NOT NULL,
	Created DATETIME NOT NULL
)