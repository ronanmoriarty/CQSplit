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

function GetAppSettingsTemplateFiles()
{
    $cqRoot = GetFullPath "$PSScriptRoot\..\"
    return (Get-ChildItem -Path $cqRoot -Filter appSettings.json.template -Recurse) | Select-Object -ExpandProperty FullName
}

$keyValuePairs = GetCQSplitKeyValuePairs

if(-not $IsCiBuild)
{
    $keyValuePairs.Keys | ForEach-Object {
        Write-Output "$($_): $($keyValuePairs[$_])"
    }
}

$paths = GetAppSettingsTemplateFiles

SwapPlaceholdersToCreateNewJsonFiles $paths appSettings.json $keyValuePairs $IsCiBuild