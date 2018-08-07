IF NOT EXISTS(SELECT 1 FROM sys.syslogins WHERE [name] = 'EventProjectingService')
BEGIN
	CREATE LOGIN [EventProjectingService]
		WITH PASSWORD=$(EventProjectingServicePassword),
		DEFAULT_DATABASE=[master],
		DEFAULT_LANGUAGE=[us_english],
		CHECK_EXPIRATION=OFF,
		CHECK_POLICY=OFF
END
GO
