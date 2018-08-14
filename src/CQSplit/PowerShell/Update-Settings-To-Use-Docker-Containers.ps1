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
    return @() # no appSettings.json.template files just yet that need IP addresses injected - this will change shortly.
}

$keyValuePairs = GetCQSplitKeyValuePairs

$keyValuePairs.Keys | ForEach-Object {
    Write-Output "$($_): $($keyValuePairs[$_])"
}

$path = GetFullPath "$PSScriptRoot\..\"
Write-Output "`$path: $path"
$paths = GetAppSettingsTemplateFiles

SwapPlaceholdersToCreateNewJsonFiles $paths appSettings.json $keyValuePairs