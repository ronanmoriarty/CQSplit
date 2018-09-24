[CmdletBinding()]
param (
    [switch]$IsCiBuild
)

. "$PSScriptRoot\Docker.ps1"
. "$PSScriptRoot\FileOperations.ps1"

function GetCQSplitKeyValuePairs()
{
    $keyValuePairs = GetPasswordKeyValuePairs
    $keyValuePairs.Add("`$rabbitMqServerAddress", "localhost:35672")
    return $keyValuePairs
}

$keyValuePairs = GetCQSplitKeyValuePairs

if(-not $IsCiBuild)
{
    $keyValuePairs.Keys | ForEach-Object {
        Write-Output "$($_): $($keyValuePairs[$_])"
    }
}

$jsonTemplateFiles = GetAppSettingsTemplateFiles

SwapPlaceholdersToCreateNewJsonFiles $jsonTemplateFiles appSettings.json $keyValuePairs $IsCiBuild