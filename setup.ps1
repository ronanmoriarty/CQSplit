choco install rabbitmq -y

[Environment]::SetEnvironmentVariable("RABBITMQ_SERVER", "C:\Program Files\RabbitMQ Server\rabbitmq_server-3.6.10", "Machine")

$path = [Environment]::GetEnvironmentVariable("PATH", "Machine")
if($path -notcontains "%RABBITMQ_SERVER%\sbin")
{
    [Environment]::SetEnvironmentVariable("PATH", "$path;%RABBITMQ_SERVER%\sbin", "Machine")
    refreshenv
}

rabbitmq-plugins enable rabbitmq_management