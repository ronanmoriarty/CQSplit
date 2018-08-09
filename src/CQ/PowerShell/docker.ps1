function GetEnvFilePath()
{
    return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine((get-item $PSScriptRoot).Parent.Parent.Parent.FullName, '.env'))
}

function GetContainerRunningWithImageName($imageName){
    return docker container list --filter ancestor=$imageName --format "{{.ID}}"
}

function GetIpAddress($containerId){
    return docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $containerId
}

function GetEnvironmentVariableFromEnvFile($environmentVariableName)
{
    return [regex]::Match((Get-Content $envPath),"$environmentVariableName='([^=]*)'").captures.groups[1].value
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

function GetPasswordKeyValuePairs()
{
    $waiterWebsitePassword = GetWaiterWebsitePassword
    $commandServicePassword = GetCommandServicePassword
    $eventProjectingServicePassword = GetEventProjectingServicePassword

    $keyValuePairs = @{}
    $keyValuePairs.Add("`$rabbitMqUsername", "guest")
    $keyValuePairs.Add("`$rabbitMqPassword", "guest")
    $keyValuePairs.Add("`$waiterWebsitePassword", $waiterWebsitePassword)
    $keyValuePairs.Add("`$commandServicePassword", $commandServicePassword)
    $keyValuePairs.Add("`$eventProjectingServicePassword", $eventProjectingServicePassword)
    return $keyValuePairs
}

function GetKeyValuePairs()
{
    $keyValuePairs = GetPasswordKeyValuePairs
    $rabbitMqServerAddress = GetRabbitMqAddress
    $writeModelSqlServerAddress = GetWriteModelSqlServerAddress
    $readModelSqlServerAddress = GetReadModelSqlServerAddress
    $waiterWebsiteUrl = GetWaiterWebsiteUrl

    $keyValuePairs.Add("`$rabbitMqServerAddress", $rabbitMqServerAddress)
    $keyValuePairs.Add("`$writeModelSqlServerAddress", $writeModelSqlServerAddress)
    $keyValuePairs.Add("`$readModelSqlServerAddress", $readModelSqlServerAddress)
    $keyValuePairs.Add("`$waiterWebsiteUrl", $waiterWebsiteUrl)
    return $keyValuePairs
}

$envPath = GetEnvFilePath
Write-Output "`$envPath: $envPath"