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

. .\src\CQRS\PowerShell\FileOperations.ps1

function CreateEnvFile()
{
    Write-Output "sa_password=$saPasswordPlainText" | Out-File -encoding ASCII .env
    Write-Output "waiterWebsitePassword='$waiterWebsitePasswordPlainText'" | Out-File -encoding ASCII -Append .env
    Write-Output "commandServicePassword='$commandServicePasswordPlainText'" | Out-File -encoding ASCII -Append .env
    Write-Output "eventProjectingServicePassword='$eventProjectingServicePasswordPlainText'" | Out-File -encoding ASCII -Append .env
    Write-Output "Created $(GetFullPath .env)"
    Write-Output "sa_password=$saPasswordPlainText" | Out-File -encoding ASCII .\src\CQRS\.env
    Write-Output "commandServicePassword='$commandServicePasswordPlainText'" | Out-File -encoding ASCII -Append .\src\CQRS\.env
    Write-Output "Created $(GetFullPath .\src\CQRS\.env)"
}

function GetKeyValuePairs()
{
    $keyValuePairs = @{}
    $keyValuePairs.Add("`$rabbitMqPassword", "guest")
    $keyValuePairs.Add("`$waiterWebsitePassword", "$waiterWebsitePasswordPlainText")
    $keyValuePairs.Add("`$commandServicePassword", "$commandServicePasswordPlainText")
    $keyValuePairs.Add("`$eventProjectingServicePassword", "$eventProjectingServicePasswordPlainText")
    return $keyValuePairs
}

$saPasswordPlainText = ConvertToPlainText $saPassword
$waiterWebsitePasswordPlainText = ConvertToPlainText $waiterWebsitePassword
$commandServicePasswordPlainText = ConvertToPlainText $commandServicePassword
$eventProjectingServicePasswordPlainText = ConvertToPlainText $eventProjectingServicePassword

CreateEnvFile
$keyValuePairs = GetKeyValuePairs
SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles .\src\Cafe\ appSettings.docker.json.example appSettings.docker.json $keyValuePairs
