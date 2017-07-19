IF EXISTS(SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID(N'dbo.Events'))
BEGIN
	DROP TABLE dbo.Events
END
GO

CREATE TABLE dbo.Events
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	AggregateId INT NOT NULL,
	EventType NVARCHAR(25) NOT NULL,
	[Data] NVARCHAR(MAX) NOT NULL,
	Created DATETIME NOT NULL
)