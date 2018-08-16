function GetRepositoryName()
{
    return (Get-Item $PSScriptRoot).Parent.Parent.Parent.ToString().ToLower()
}

$repositoryName = GetRepositoryName
Write-Output "`$repositoryName: $repositoryName"

function GetEnvFilePath()
{
    return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine((get-item $PSScriptRoot).Parent.Parent.Parent.FullName, '.env'))
}

function GetContainerRunningWithImageName($imageName){
    Write-Host "Finding container based on image named '$imageName'..."
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
    $rabbitMqContainerId = GetContainerRunningWithImageName "ronanmoriarty/rabbitmq-windowsservercore"
    $rabbitMqServerIpAddress = GetIpAddress $rabbitMqContainerId
    return $rabbitMqServerIpAddress
}

function GetWriteModelSqlServerAddress(){
    $writeModelSqlServerContainerId = GetContainerRunningWithImageName "$($repositoryName)_waiter-write-db-server"
    $writeModelSqlServerIpAddress = GetIpAddress $writeModelSqlServerContainerId
    return $writeModelSqlServerIpAddress
}

function GetReadModelSqlServerAddress(){
    $readModelSqlServerContainerId = GetContainerRunningWithImageName "$($repositoryName)_waiter-read-db-server"
    $readModelSqlServerIpAddress = GetIpAddress $readModelSqlServerContainerId
    return $readModelSqlServerIpAddress
}


function GetWaiterWebsiteUrl()
{
    $waiterWebsiteContainerId = GetContainerRunningWithImageName "$($repositoryName)_cafe-waiter-web"
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

function GetKeyValuePairsToUseInsideContainers()
{
    $keyValuePairs = GetPasswordKeyValuePairs
    $keyValuePairs.Add("`$rabbitMqServerAddress", "$($repositoryName)_rabbitmq_1")
    $keyValuePairs.Add("`$writeModelSqlServerAddress", "$($repositoryName)_waiter-write-db-server_1")
    $keyValuePairs.Add("`$readModelSqlServerAddress", "$($repositoryName)_waiter-read-db-server_1")
    return $keyValuePairs
}

$envPath = GetEnvFilePath
Write-Output "`$envPath: $envPath"