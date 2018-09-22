[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
    [SecureString] $saPassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $waiterWebsitePassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $commandServicePassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $eventProjectingServicePassword,
    [switch]$IsCiBuild
)

. .\src\CQSplit\PowerShell\Docker.ps1
. .\src\CQSplit\PowerShell\FileOperations.ps1

function CreateEnvFile()
{
    Write-Output "sa_password=$saPasswordPlainText" | Out-File -encoding ASCII .env
    Write-Output "waiterWebsitePassword='$waiterWebsitePasswordPlainText'" | Out-File -encoding ASCII -Append .env
    Write-Output "commandServicePassword='$commandServicePasswordPlainText'" | Out-File -encoding ASCII -Append .env
    Write-Output "eventProjectingServicePassword='$eventProjectingServicePasswordPlainText'" | Out-File -encoding ASCII -Append .env
    Write-Output "Created $(GetFullPath .env)"
}

$saPasswordPlainText = ConvertToPlainText $saPassword
$waiterWebsitePasswordPlainText = ConvertToPlainText $waiterWebsitePassword
$commandServicePasswordPlainText = ConvertToPlainText $commandServicePassword
$eventProjectingServicePasswordPlainText = ConvertToPlainText $eventProjectingServicePassword

CreateEnvFile
$dockerKeyValuePairs = GetKeyValuePairsToUseInsideContainers
$unitTestKeyValuePairs = GetKeyValuePairsForUnitTests
$jsonTemplateFiles = GetAppSettingsTemplateFiles
SwapPlaceholdersToCreateNewJsonFiles $jsonTemplateFiles appSettings.docker.json $dockerKeyValuePairs $IsCiBuild
SwapPlaceholdersToCreateNewJsonFiles $jsonTemplateFiles appSettings.json $unitTestKeyValuePairs $IsCiBuild

if(Test-Path .\src\.nuget.local\)
{
    rm -r .\src\.nuget.local\
}

mkdir .\src\.nuget.local\