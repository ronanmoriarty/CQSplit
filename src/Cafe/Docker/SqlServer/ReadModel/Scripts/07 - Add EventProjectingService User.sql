IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE name = 'EventProjectingService')
BEGIN
	CREATE USER [EventProjectingService] FOR LOGIN [EventProjectingService] WITH DEFAULT_SCHEMA=[dbo]
END

GO