IF NOT EXISTS(SELECT 1 FROM sys.syslogins WHERE [name] = 'CommandService')
BEGIN
	CREATE LOGIN [CommandService]
		WITH PASSWORD=$(CommandServicePassword),
		DEFAULT_DATABASE=[master],
		DEFAULT_LANGUAGE=[us_english],
		CHECK_EXPIRATION=OFF,
		CHECK_POLICY=OFF
END
GO
