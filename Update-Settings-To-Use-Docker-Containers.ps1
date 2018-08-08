. .\src\CQRS\PowerShell\Docker.ps1
. .\src\CQRS\PowerShell\FileOperations.ps1

function GetWaiterWebsiteUrl()
{
    $waiterWebsiteContainerId = GetContainerRunningWithImageName "cqrs-nu-tutorial_cafe-waiter-web"
    $waiterWebsiteIpAddress = GetIpAddress $waiterWebsiteContainerId
    return "http://$waiterWebsiteIpAddress"
}

function GetWaiterWebsiteConnectionString([string] $password)
{
    $readModelSqlServerAddress = GetReadModelSqlServerAddress
    return "Server=$readModelSqlServerAddress;Database=CQRSTutorial.Cafe.Waiter.ReadModel;User Id=WaiterWebsite;Password=$password;"
}

function GetCommandServiceConnectionString([string] $password)
{
    $writeModelSqlServerAddress = GetWriteModelSqlServerAddress
    return "Server=$writeModelSqlServerAddress;Database=CQRSTutorial.Cafe.Waiter.WriteModel;User Id=CommandService;Password=$password;"
}

function GetEventProjectingServiceConnectionString([string] $password)
{
    $readModelSqlServerAddress = GetReadModelSqlServerAddress
    return "Server=$readModelSqlServerAddress;Database=CQRSTutorial.Cafe.Waiter.ReadModel;User Id=EventProjectingService;Password=$password;"
}

function GetWaiterWebsitePassword()
{
    return GetEnvironmentVariableFromEnvFile "waiterWebsitePassword"
}

function GetCommandServicePassword()
{
    return GetEnvironmentVariableFromEnvFile "commandServicePassword"
}

function GetEventProjectingServicePassword()
{
    return GetEnvironmentVariableFromEnvFile "eventProjectingServicePassword"
}

function GetKeyValuePairs()
{
    $keyValuePairs = @{}
    $keyValuePairs.Add("`$rabbitMqServerAddress", $rabbitMqServerAddress)
    $keyValuePairs.Add("`$rabbitMqUsername", "guest")
    $keyValuePairs.Add("`$rabbitMqPassword", "guest")
    $keyValuePairs.Add("`$writeModelSqlServerAddress", $writeModelSqlServerAddress)
    $keyValuePairs.Add("`$readModelSqlServerAddress", $readModelSqlServerAddress)
    $keyValuePairs.Add("`$waiterWebsitePassword", $waiterWebsitePassword)
    $keyValuePairs.Add("`$commandServicePassword", $commandServicePassword)
    $keyValuePairs.Add("`$eventProjectingServicePassword", $eventProjectingServicePassword)
    $keyValuePairs.Add("`$waiterWebsiteUrl", $waiterWebsiteUrl)
    return $keyValuePairs
}

$rabbitMqServerAddress = GetRabbitMqAddress
$writeModelSqlServerAddress = GetWriteModelSqlServerAddress
$readModelSqlServerAddress = GetReadModelSqlServerAddress
$waiterWebsitePassword = GetWaiterWebsitePassword
$commandServicePassword = GetCommandServicePassword
$eventProjectingServicePassword = GetEventProjectingServicePassword
Write-Output "`$rabbitMqServerAddress:$rabbitMqServerAddress"
$commandServiceConnectionString = GetCommandServiceConnectionString $commandServicePassword
Write-Output "`$commandServiceConnectionString: $commandServiceConnectionString"
$eventProjectingServiceConnectionString = GetEventProjectingServiceConnectionString $eventProjectingServicePassword
Write-Output "`$eventProjectingServiceConnectionString: $eventProjectingServiceConnectionString"
$waiterWebsiteUrl = GetWaiterWebsiteUrl
Write-Output "`$waiterWebsiteUrl: $waiterWebsiteUrl"

$keyValuePairs = GetKeyValuePairs
SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles .\src\Cafe\ appSettings.override.json.example appSettings.override.json $keyValuePairs
