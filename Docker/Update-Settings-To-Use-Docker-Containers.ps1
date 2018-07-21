function GetContainerRunningWithImageName($imageName){
    return docker container list --filter ancestor=$imageName --format "{{.ID}}"
}

$rabbitMqContainerId = GetContainerRunningWithImageName "rabbitmq"
$writeModelSqlServerContainerId = GetContainerRunningWithImageName "cqrs-write-db-server"

$rabbitMqServerIpAddress = docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $rabbitMqContainerId
Write-Output "`$rabbitMqServerIpAddress:$rabbitMqServerIpAddress"
$configuration = "Debug"

$appSettingsFiles = @(
    "..\src\Cafe\Cafe.Waiter.Web\appsettings.override.json",
    "..\src\Cafe\Cafe.Waiter.Command.Service\bin\$configuration\netcoreapp2.0\appsettings.override.json",
    "..\src\Cafe\Cafe.Waiter.EventProjecting.Service\bin\$configuration\netcoreapp2.0\appsettings.override.json"
)

$appSettingsFiles | ForEach-Object {
    $path = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $_))
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
