[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
    [SecureString] $saPassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $waiterWebsitePassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $commandServicePassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $eventProjectingServicePassword
)

. .\src\CQSplit\PowerShell\Docker.ps1
. .\src\CQSplit\PowerShell\FileOperations.ps1

function GetJsonTemplateFiles()
{
    return @(`
        "$PSScriptRoot\src\Cafe\Cafe.Waiter.Web\appSettings.json.template", `
        "$PSScriptRoot\src\Cafe\Cafe.Waiter.Command.Service\appSettings.json.template", `
        "$PSScriptRoot\src\Cafe\Cafe.Waiter.EventProjecting.Service\appSettings.json.template" `
    )
}

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
$keyValuePairs = GetKeyValuePairsToUseInsideContainers
$jsonTemplateFiles = GetJsonTemplateFiles
SwapPlaceholdersToCreateNewJsonFiles $jsonTemplateFiles appSettings.docker.json $keyValuePairs

mkdir .\src\.nuget.local\
