. "$PSScriptRoot\Docker.ps1"
. "$PSScriptRoot\FileOperations.ps1"

function GetCQSplitKeyValuePairs()
{
    $keyValuePairs = GetPasswordKeyValuePairs
    $rabbitMqServerAddress = GetRabbitMqAddress
    $keyValuePairs.Add("`$rabbitMqServerAddress", $rabbitMqServerAddress)
    return $keyValuePairs
}

$keyValuePairs = GetCQSplitKeyValuePairs

$keyValuePairs.Keys | ForEach-Object {
    Write-Output "$($_): $($keyValuePairs[$_])"
}

$path = GetFullPath ..\
SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles $path appSettings.json.template appSettings.json $keyValuePairs