IF NOT EXISTS(SELECT 1 FROM sys.syslogins WHERE [name] = 'EventProjectingService')
BEGIN
	CREATE LOGIN [EventProjectingService]
		WITH PASSWORD=$(EventProjectingServicePassword),
		DEFAULT_DATABASE=[CQRSTutorial.Cafe.Waiter.ReadModel],
		DEFAULT_LANGUAGE=[us_english],
		CHECK_EXPIRATION=OFF,
		CHECK_POLICY=OFF
END
GO
