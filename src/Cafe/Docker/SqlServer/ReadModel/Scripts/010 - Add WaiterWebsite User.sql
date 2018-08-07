IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE name = 'WaiterWebsite')
BEGIN
	CREATE USER [WaiterWebsite] FOR LOGIN [WaiterWebsite] WITH DEFAULT_SCHEMA=[dbo]
END

GO