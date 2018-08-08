. .\src\CQRS\PowerShell\Docker.ps1
. .\src\CQRS\PowerShell\FileOperations.ps1

function GetWaiterCommandServiceSettings($rabbitMqServerAddress, $writeModelConnectionString){
    return @"
{
    "rabbitmq": {
        "uri": "rabbitmq://$rabbitMqServerAddress",
        "username": "guest",
        "password": "guest"
    },
    "connectionString": "$writeModelConnectionString"
}
"@
}

function GetWaiterCommandServiceTestSettings($rabbitMqServerAddress, $writeModelConnectionString){
    return @"
{
    "rabbitmq": {
        "uri": "rabbitmq://$rabbitMqServerAddress",
        "username": "guest",
        "password": "guest"
    },
    "connectionString": "$writeModelConnectionString"
}
"@
}

function GetWaiterEventProjectingServiceSettings($rabbitMqServerAddress, $eventProjectingServiceConnectionString){
    return @"
{
    "rabbitmq": {
        "uri": "rabbitmq://$rabbitMqServerAddress",
        "username": "guest",
        "password": "guest"
    },
    "connectionString": "$eventProjectingServiceConnectionString"
}
"@
}

function GetWaiterEventProjectingServiceTestSettings($eventProjectingServiceConnectionString){
    return @"
{
    "connectionString": "$eventProjectingServiceConnectionString"
}
"@
}

function GetWaiterAcceptanceTestsSettings($readModelConnectionString, $waiterWebsiteUrl){
    return @"
{
    "connectionString": "$readModelConnectionString",
    "cafe": {
      "waiter": {
        "web": {
          "url": "$waiterWebsiteUrl"
        }
      }
    }
}
"@
}

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

$configuration = "Debug"

$waiterCommandService = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.Command.Service\bin\$configuration\netcoreapp2.1\appSettings.override.json";
    Text = GetWaiterCommandServiceSettings $rabbitMqServerAddress $commandServiceConnectionString
}

$waiterCommandServiceTest = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.Command.Service.Tests\bin\$configuration\netcoreapp2.1\appSettings.override.json";
    Text = GetWaiterCommandServiceTestSettings $rabbitMqServerAddress $commandServiceConnectionString
}

$waiterEventProjectingService = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.EventProjecting.Service\bin\$configuration\netcoreapp2.1\appSettings.override.json"
    Text = GetWaiterEventProjectingServiceSettings $rabbitMqServerAddress $eventProjectingServiceConnectionString
}

$waiterEventProjectingServiceTest = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.EventProjecting.Service.Tests\bin\$configuration\netcoreapp2.1\appSettings.override.json"
    Text = GetWaiterEventProjectingServiceTestSettings $eventProjectingServiceConnectionString
}

$waiterAcceptanceTest = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.AcceptanceTests\bin\$configuration\netcoreapp2.1\appSettings.override.json";
    Text = GetWaiterAcceptanceTestsSettings $eventProjectingServiceConnectionString $waiterWebsiteUrl
}

$appSettings = @(
    $waiterCommandService,
    $waiterCommandServiceTest,
    $waiterEventProjectingService,
    $waiterEventProjectingServiceTest
    $waiterAcceptanceTest
)

$appSettings | ForEach-Object {
    $path = GetFullPath $_.FilePath
    if(Test-Path $path) {
        Remove-Item $path
    }

    WriteToFile $path $_.Text
}

$keyValuePairs = GetKeyValuePairs
SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles .\src\Cafe\ appSettings.override.json.example appSettings.override.json $keyValuePairs
