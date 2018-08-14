
. "$PSScriptRoot\Docker.ps1"
. "$PSScriptRoot\FileOperations.ps1"

function GetCQSplitKeyValuePairs()
{
    $keyValuePairs = GetPasswordKeyValuePairs
    $rabbitMqServerAddress = GetRabbitMqAddress
    $keyValuePairs.Add("`$rabbitMqServerAddress", $rabbitMqServerAddress)
    return $keyValuePairs
}

function GetAppSettingsTemplateFiles()
{
    $cqRoot = GetFullPath "$PSScriptRoot\..\"
    return (Get-ChildItem -Path $cqRoot -Filter appSettings.json.template -Recurse) | Select-Object -ExpandProperty FullName
}

$keyValuePairs = GetCQSplitKeyValuePairs

$keyValuePairs.Keys | ForEach-Object {
    Write-Output "$($_): $($keyValuePairs[$_])"
}

$path = GetFullPath "$PSScriptRoot\..\"
Write-Output "`$path: $path"
$paths = GetAppSettingsTemplateFiles

SwapPlaceholdersToCreateNewJsonFiles $paths appSettings.json $keyValuePairs