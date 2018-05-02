IF NOT EXISTS(SELECT 1 FROM dbo.Menu WHERE Id = '68FDB1AF-B652-4B37-B274-D1C7F569FFE7')
BEGIN
	INSERT INTO dbo.Menu
	(
		[Id],
		[Data]
	)
	VALUES
	(
		'68FDB1AF-B652-4B37-B274-D1C7F569FFE7',
		'{"Id":"68FDB1AF-B652-4B37-B274-D1C7F569FFE7","Items":[{"Id":123,"Name":"Coca Cola","Price":2.5},{"Id":234,"Name":"Bacon & Cheese Burger","Price":13.0}]}'
	)
END
GO