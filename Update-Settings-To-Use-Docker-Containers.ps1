. .\src\CQ\PowerShell\Docker.ps1
. .\src\CQ\PowerShell\FileOperations.ps1

$keyValuePairs = GetKeyValuePairs

$keyValuePairs.Keys | ForEach-Object {
    Write-Output "$($_): $($keyValuePairs[$_])"
}

$paths = (Get-ChildItem -Path .\ -Filter appSettings.json.template -Recurse) | Select-Object -ExpandProperty FullName

SwapPlaceholdersToCreateNewJsonFiles $paths appSettings.json $keyValuePairs
