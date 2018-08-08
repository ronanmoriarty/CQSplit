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

. .\src\CQ\PowerShell\Docker.ps1
. .\src\CQ\PowerShell\FileOperations.ps1

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
$keyValuePairs = GetPasswordKeyValuePairs
SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles .\src\Cafe\ appSettings.docker.json.example appSettings.docker.json $keyValuePairs
