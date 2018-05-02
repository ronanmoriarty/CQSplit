function GetMdfFilePath($databaseName)
{
    return "$dbFolder\$databaseName.mdf"
}

function GetLdfFilePath($databaseName)
{
    return "$dbFolder\$($databaseName)_log.ldf"
}

function GetLocalSqlServer()
{
    return new-object Microsoft.SqlServer.Management.Smo.Server -ArgumentList "."
}

function AttachExistingDatabase ($databaseName) {
    Write-Host "Attaching database $databaseName..."
    $dataFiles = New-Object System.Collections.Specialized.StringCollection
    $dataFiles.Add((GetMdfFilePath $databaseName))
    $dataFiles.Add((GetLdfFilePath $databaseName))
    $server = GetLocalSqlServer
    $server.AttachDatabase($databaseName, $dataFiles)
    Write-Host $server.Databases
}