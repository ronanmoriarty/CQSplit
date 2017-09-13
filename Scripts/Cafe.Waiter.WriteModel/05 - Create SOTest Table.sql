IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.SOTest'))
BEGIN
	DROP TABLE dbo.SOTest
END
GO

CREATE TABLE dbo.SOTest
(
	Id uniqueidentifier PRIMARY KEY
)