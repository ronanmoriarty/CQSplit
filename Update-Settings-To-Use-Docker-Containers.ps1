. .\src\CQ\PowerShell\Docker.ps1
. .\src\CQ\PowerShell\FileOperations.ps1

$keyValuePairs = GetKeyValuePairs

$keyValuePairs.Keys | ForEach-Object {
    Write-Output "$($_): $($keyValuePairs[$_])"
}

SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles .\src\Cafe\ appSettings.override.json.example appSettings.override.json $keyValuePairs
