function GetContainerRunningWithImageName($imageName){
    return docker container list --filter ancestor=$imageName --format "{{.ID}}"
}

function GetIpAddress($containerId){
    return docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $containerId
}

function GetEnvironmentVariableFromEnvFile($environmentVariableName)
{
    return [regex]::Match((Get-Content .env),"$environmentVariableName='([^=]*)'").captures.groups[1].value
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
