IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID(N'dbo.Events'))
BEGIN
	CREATE TABLE dbo.Events
	(
		Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
		AggregateId UNIQUEIDENTIFIER NOT NULL,
		CommandId UNIQUEIDENTIFIER NOT NULL,
		EventType NVARCHAR(25) NOT NULL,
		[Data] NVARCHAR(MAX) NOT NULL,
		Created DATETIME NOT NULL
	)
END
GO