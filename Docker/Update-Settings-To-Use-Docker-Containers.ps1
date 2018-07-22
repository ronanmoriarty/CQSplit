function GetContainerRunningWithImageName($imageName){
    return docker container list --filter ancestor=$imageName --format "{{.ID}}"
}

function GetIpAddress($containerId){
    return docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $containerId
}

function GetFullPath($relativePath){
    return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $relativePath))
}

function GetWaiterWebsiteSettings($rabbitMqServerAddress){
    return @"
{
    "rabbitmq": {
        "uri": "rabbitmq://$rabbitMqServerAddress",
        "username": "guest",
        "password": "guest"
    }
}
"@
}

function GetWaiterCommandServiceSettings($rabbitMqServerAddress, $writeModelSqlServerAddress){
    return @"
{
    "rabbitmq": {
        "uri": "rabbitmq://$rabbitMqServerAddress",
        "username": "guest",
        "password": "guest"
    },
    "connectionString": "Server=$writeModelSqlServerAddress\\SQLEXPRESS;Database=CQRSWriteModel;Trusted_Connection=True;"
}
"@
}

function GetWaiterEventProjectingServiceSettings($rabbitMqServerAddress){
    return @"
{
    "rabbitmq": {
        "uri": "rabbitmq://$rabbitMqServerAddress",
        "username": "guest",
        "password": "guest"
    }
}
"@
}

$rabbitMqContainerId = GetContainerRunningWithImageName "rabbitmq"
$rabbitMqServerIpAddress = GetIpAddress $rabbitMqContainerId
Write-Output "`$rabbitMqServerIpAddress:$rabbitMqServerIpAddress"

$writeModelSqlServerContainerId = GetContainerRunningWithImageName "cqrs-write-db-server"
$writeModelSqlServerIpAddress = GetIpAddress $writeModelSqlServerContainerId
Write-Output "`$writeModelSqlServerIpAddress:$writeModelSqlServerIpAddress"

$configuration = "Debug"

$waiterWebsite = @{
    FilePath = "..\src\Cafe\Cafe.Waiter.Web\appSettings.override.json"
    Text = GetWaiterWebsiteSettings $rabbitMqServerIpAddress
}

$waiterCommandService = @{
    FilePath = "..\src\Cafe\Cafe.Waiter.Command.Service\bin\$configuration\netcoreapp2.0\appSettings.override.json";
    Text = GetWaiterCommandServiceSettings $rabbitMqServerIpAddress $writeModelSqlServerIpAddress
}

$waiterEventProjectingService = @{
    FilePath = "..\src\Cafe\Cafe.Waiter.EventProjecting.Service\bin\$configuration\netcoreapp2.0\appSettings.override.json"
    Text = GetWaiterEventProjectingServiceSettings $rabbitMqServerIpAddress
}

$appSettings = @(
    $waiterWebsite,
    $waiterCommandService,
    $waiterEventProjectingService
)

$appSettings | ForEach-Object {
    $path = GetFullPath $_.FilePath
    if(Test-Path $path) {
        Remove-Item $path
    }

    Write-Output "Text: $($_.Text)"

    $_.Text | Out-File -encoding ASCII $path
    Write-Output "Created $path"
}
