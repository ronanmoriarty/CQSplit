[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
    [string] $userName,
    [Parameter(Mandatory=$True)]
    [SecureString] $password
)

function GetContainerRunningWithImageName($imageName){
    return docker container list --filter ancestor=$imageName --format "{{.ID}}"
}

function GetIpAddress($containerId){
    return docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $containerId
}

function GetFullPath($relativePath){
    return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $relativePath))
}

function ConvertToPlainText([SecureString]$secureString){
    $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureString)
    return [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
}

function GetCQRSDALSettings($writeModelConnectionString){
    return @"
{
    "connectionString": "$writeModelConnectionString"
}
"@
}

function GetWaiterWebsiteSettings($rabbitMqServerAddress, $readModelConnectionString){
    return @"
{
    "rabbitmq": {
        "uri": "rabbitmq://$rabbitMqServerAddress",
        "username": "guest",
        "password": "guest"
    },
    "connectionString": "$readModelConnectionString"
}
"@
}

function GetWaiterWebsiteTestSettings($readModelConnectionString){
    return @"
{
    "connectionString": "$readModelConnectionString"
}
"@
}

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

function GetWaiterEventProjectingServiceSettings($rabbitMqServerAddress, $readModelConnectionString){
    return @"
{
    "rabbitmq": {
        "uri": "rabbitmq://$rabbitMqServerAddress",
        "username": "guest",
        "password": "guest"
    },
    "connectionString": "$readModelConnectionString"
}
"@
}

function GetWaiterEventProjectingServiceTestSettings($readModelConnectionString){
    return @"
{
    "connectionString": "$readModelConnectionString"
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

function WriteToFile($path, $contents){
    Write-Output "Writing $path..."
    Write-Output "Text: $contents"
    $_.Text | Out-File -encoding ASCII $path
}

function GetRabbitMqAddress(){
    $rabbitMqContainerId = GetContainerRunningWithImageName "cqrs-nu-tutorial_rabbitmq"
    $rabbitMqServerIpAddress = GetIpAddress $rabbitMqContainerId
    return $rabbitMqServerIpAddress
}

function GetWriteModelSqlServerAddress(){
    $writeModelSqlServerContainerId = GetContainerRunningWithImageName "cqrs-nu-tutorial_waiter-write-db-server"
    $writeModelSqlServerIpAddress = GetIpAddress $writeModelSqlServerContainerId
    return $writeModelSqlServerIpAddress
}

function GetReadModelSqlServerAddress(){
    $readModelSqlServerContainerId = GetContainerRunningWithImageName "cqrs-nu-tutorial_waiter-read-db-server"
    $readModelSqlServerIpAddress = GetIpAddress $readModelSqlServerContainerId
    return $readModelSqlServerIpAddress
}

function GetWaiterWebsiteUrl()
{
    $waiterWebsiteContainerId = GetContainerRunningWithImageName "cqrs-nu-tutorial_cafe-waiter-web"
    $waiterWebsiteIpAddress = GetIpAddress $waiterWebsiteContainerId
    return "http://$waiterWebsiteIpAddress"
}

function GetWriteModelConnectionString($username, [SecureString] $secureStringPassword)
{
    $password = ConvertToPlainText $secureStringPassword
    $writeModelSqlServerAddress = GetWriteModelSqlServerAddress
    return "Server=$writeModelSqlServerAddress;Database=CQRSTutorial.Cafe.Waiter.WriteModel;User Id=$username;Password=$password;"
}

function GetReadModelConnectionString($username, [SecureString] $secureStringPassword)
{
    $password = ConvertToPlainText $secureStringPassword
    $readModelSqlServerAddress = GetReadModelSqlServerAddress
    return "Server=$readModelSqlServerAddress;Database=CQRSTutorial.Cafe.Waiter.ReadModel;User Id=$username;Password=$password;"
}

$rabbitMqServerIpAddress = GetRabbitMqAddress
Write-Output "`$rabbitMqServerIpAddress:$rabbitMqServerIpAddress"
$writeModelConnectionString = GetWriteModelConnectionString $userName $password
Write-Output "`$writeModelConnectionString: $writeModelConnectionString"
$readModelConnectionString = GetReadModelConnectionString $userName $password
Write-Output "`$readModelConnectionString: $readModelConnectionString"
$waiterWebsiteUrl = GetWaiterWebsiteUrl
Write-Output "`$waiterWebsiteUrl: $waiterWebsiteUrl"

$configuration = "Debug"

$cqrsDALTests = @{
    FilePath = ".\src\CQRS\CQRSTutorial.DAL.Tests\bin\$configuration\netcoreapp2.0\appSettings.override.json"
    Text = GetCQRSDALSettings $writeModelConnectionString
}

$waiterWebsite = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.Web\appSettings.override.json"
    Text = GetWaiterWebsiteSettings $rabbitMqServerIpAddress $readModelConnectionString
}

$waiterWebsiteTest = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.Web.Tests\bin\$configuration\netcoreapp2.1\appSettings.override.json"
    Text = GetWaiterWebsiteTestSettings $readModelConnectionString
}

$waiterCommandService = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.Command.Service\bin\$configuration\netcoreapp2.1\appSettings.override.json";
    Text = GetWaiterCommandServiceSettings $rabbitMqServerIpAddress $writeModelConnectionString
}

$waiterCommandServiceTest = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.Command.Service.Tests\bin\$configuration\netcoreapp2.1\appSettings.override.json";
    Text = GetWaiterCommandServiceTestSettings $rabbitMqServerIpAddress $writeModelConnectionString
}

$waiterEventProjectingService = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.EventProjecting.Service\bin\$configuration\netcoreapp2.1\appSettings.override.json"
    Text = GetWaiterEventProjectingServiceSettings $rabbitMqServerIpAddress $readModelConnectionString
}

$waiterEventProjectingServiceTest = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.EventProjecting.Service.Tests\bin\$configuration\netcoreapp2.1\appSettings.override.json"
    Text = GetWaiterEventProjectingServiceTestSettings $readModelConnectionString
}

$waiterAcceptanceTest = @{
    FilePath = ".\src\Cafe\Cafe.Waiter.AcceptanceTests\bin\$configuration\netcoreapp2.1\appSettings.override.json";
    Text = GetWaiterAcceptanceTestsSettings $readModelConnectionString $waiterWebsiteUrl
}

$appSettings = @(
    $cqrsDALTests,
    $waiterWebsite,
    $waiterWebsiteTest,
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
