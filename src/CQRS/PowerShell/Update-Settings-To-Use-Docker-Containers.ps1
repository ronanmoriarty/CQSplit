. .\Docker.ps1
. .\FileOperations.ps1

$keyValuePairs = GetKeyValuePairs

$keyValuePairs.Keys | ForEach-Object {
    Write-Output "$($_): $($keyValuePairs[$_])"
}

$path = GetFullPath ..\
SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles $path appSettings.override.json.example appSettings.override.json $keyValuePairs