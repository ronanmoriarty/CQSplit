IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE name = 'CommandService')
BEGIN
	CREATE USER [CommandService] FOR LOGIN [CommandService] WITH DEFAULT_SCHEMA=[dbo]
END

GO