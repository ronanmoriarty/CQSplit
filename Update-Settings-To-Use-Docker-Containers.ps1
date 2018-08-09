. .\src\CQ\PowerShell\Docker.ps1
. .\src\CQ\PowerShell\FileOperations.ps1

$keyValuePairs = GetKeyValuePairs

$keyValuePairs.Keys | ForEach-Object {
    Write-Output "$($_): $($keyValuePairs[$_])"
}

SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles .\src\Cafe\ appSettings.json.example appSettings.json $keyValuePairs
