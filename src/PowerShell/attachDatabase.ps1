function AttachExistingDatabase ($databaseName) {
    Write-Host "Attaching database $databaseName..."
    $dataFiles = New-Object System.Collections.Specialized.StringCollection
    $dataFiles.Add((GetMdfFilePath $databaseName))
    $dataFiles.Add((GetLdfFilePath $databaseName))
    $server = GetLocalSqlServer
    $server.AttachDatabase($databaseName, $dataFiles)
    Write-Host $server.Databases
}