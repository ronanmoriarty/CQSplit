choco install rabbitmq -y
[Environment]::SetEnvironmentVariable("PATH", [Environment]::GetEnvironmentVariable("PATH") + ";C:\Program Files\RabbitMQ Server\rabbitmq_server-3.6.14\sbin\", "Machine")
rabbitmq-plugins enable rabbitmq_management
