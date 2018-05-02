[reflection.assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")
function GetMdfFilePath($databaseFolder, $databaseName)
{
    return "$databaseFolder\$databaseName.mdf"
}

function GetLdfFilePath($databaseFolder, $databaseName)
{
    return "$databaseFolder\$($databaseName)_log.ldf"
}

function GetLocalSqlServer()
{
    return new-object Microsoft.SqlServer.Management.Smo.Server -ArgumentList "."
}

function AttachExistingDatabase ($databaseFolder, $databaseName) {
    Write-Host "Attaching database $databaseName..."
    $dataFiles = New-Object System.Collections.Specialized.StringCollection
    $dataFiles.Add((GetMdfFilePath $databaseFolder $databaseName))
    $dataFiles.Add((GetLdfFilePath $databaseFolder $databaseName))
    $server = GetLocalSqlServer
    $server.AttachDatabase($databaseName, $dataFiles)
    Write-Host $server.Databases
}