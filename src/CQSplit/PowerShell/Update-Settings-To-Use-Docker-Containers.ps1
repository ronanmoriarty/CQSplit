. "$PSScriptRoot\Docker.ps1"
. "$PSScriptRoot\FileOperations.ps1"

$keyValuePairs = GetKeyValuePairs

$keyValuePairs.Keys | ForEach-Object {
    Write-Output "$($_): $($keyValuePairs[$_])"
}

$path = GetFullPath ..\
SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles $path appSettings.json.template appSettings.json $keyValuePairs