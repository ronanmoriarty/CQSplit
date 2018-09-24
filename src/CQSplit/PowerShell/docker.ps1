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

function GetKeyValuePairsToUseOutsideContainers()
{
    $keyValuePairs = GetPasswordKeyValuePairs
    $keyValuePairs.Add("`$rabbitMqServerAddress", "localhost:35672")
    $keyValuePairs.Add("`$writeModelSqlServerAddress", "localhost,1400")
    $keyValuePairs.Add("`$readModelSqlServerAddress", "localhost,1401")
    $keyValuePairs.Add("`$waiterWebsiteUrl", "http://localhost:8080")
    return $keyValuePairs
}

function GetKeyValuePairsToUseInsideContainers()
{
    $keyValuePairs = GetPasswordKeyValuePairs
    $keyValuePairs.Add("`$rabbitMqServerAddress", "$($repositoryName)_rabbitmq_1")
    $keyValuePairs.Add("`$writeModelSqlServerAddress", "$($repositoryName)_waiter-write-db-server_1")
    $keyValuePairs.Add("`$readModelSqlServerAddress", "$($repositoryName)_waiter-read-db-server_1")
    $keyValuePairs.Add("`$waiterWebsiteUrl", "http://$($repositoryName)_cafe-waiter-web_1")
    return $keyValuePairs
}

$envPath = GetEnvFilePath
Write-Output "`$envPath: $envPath"