IF NOT EXISTS(SELECT 1 FROM sys.syslogins WHERE [name] = 'WaiterWebsite')
BEGIN
	CREATE LOGIN [WaiterWebsite]
		WITH PASSWORD=$(WaiterWebsitePassword),
		DEFAULT_DATABASE=[master],
		DEFAULT_LANGUAGE=[us_english],
		CHECK_EXPIRATION=OFF,
		CHECK_POLICY=OFF
END
GO
