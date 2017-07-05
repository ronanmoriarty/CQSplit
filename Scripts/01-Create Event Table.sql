IF EXISTS(SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID(N'dbo.Events'))
BEGIN
	DROP TABLE dbo.Events
END
GO

IF EXISTS(SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID(N'dbo.EventsToPublish'))
BEGIN
	DROP TABLE dbo.EventsToPublish
END
GO

CREATE TABLE dbo.EventsToPublish
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	EventType NVARCHAR(MAX) NOT NULL,
	[Data] NVARCHAR(MAX) NOT NULL
)