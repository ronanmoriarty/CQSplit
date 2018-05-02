function AttachExistingDatabase () {
    Write-Host "Attaching database $dbName..."
    $dataFiles = New-Object System.Collections.Specialized.StringCollection
    $dataFiles.Add((GetMdfFilePath $dbName))
    $dataFiles.Add((GetLdfFilePath $dbName))
    $server = GetLocalSqlServer
    $server.AttachDatabase($dbName, $dataFiles)
    Write-Host $server.Databases
}