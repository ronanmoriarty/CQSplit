function GetContainerRunningWithImageName($imageName){
    return docker container list --filter ancestor=$imageName --format "{{.ID}}"
}

function GetIpAddress($containerId){
    return docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $containerId
}

function GetFullPath($relativePath){
    return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $_))
}

$rabbitMqContainerId = GetContainerRunningWithImageName "rabbitmq"
$rabbitMqServerIpAddress = GetIpAddress $rabbitMqContainerId
Write-Output "`$rabbitMqServerIpAddress:$rabbitMqServerIpAddress"

$writeModelSqlServerContainerId = GetContainerRunningWithImageName "cqrs-write-db-server"
$writeModelSqlServerIpAddress = GetIpAddress $writeModelSqlServerContainerId
Write-Output "`$writeModelSqlServerIpAddress:$writeModelSqlServerIpAddress"

$configuration = "Debug"

$appSettingsFiles = @(
    "..\src\Cafe\Cafe.Waiter.Web\appsettings.override.json",
    "..\src\Cafe\Cafe.Waiter.Command.Service\bin\$configuration\netcoreapp2.0\appsettings.override.json",
    "..\src\Cafe\Cafe.Waiter.EventProjecting.Service\bin\$configuration\netcoreapp2.0\appsettings.override.json"
)

$appSettingsFiles | ForEach-Object {
    $path = GetFullPath $_
    if(Test-Path $path) {
        Remove-Item $path
    }

    $text = @"
{
    "rabbitmq": {
        "uri": "rabbitmq://$rabbitMqServerIpAddress",
        "username": "guest",
        "password": "guest"
    }
}
"@

    $text | Out-File -encoding ASCII $path
    Write-Output "Created $path"
}
