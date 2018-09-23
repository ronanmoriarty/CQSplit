[CmdletBinding()]
param (
    [switch]$IsCiBuild
)

. .\src\CQSplit\PowerShell\Docker.ps1
. .\src\CQSplit\PowerShell\FileOperations.ps1

$keyValuePairs = GetKeyValuePairsToUseOutsideContainers

if(-Not($IsCiBuild))
{
    $keyValuePairs.Keys | ForEach-Object {
        Write-Output "$($_): $($keyValuePairs[$_])"
    }
}

$paths = (Get-ChildItem -Path .\ -Filter appSettings.json.template -Recurse) | Select-Object -ExpandProperty FullName

SwapPlaceholdersToCreateNewJsonFiles $paths appSettings.json $keyValuePairs $IsCiBuild