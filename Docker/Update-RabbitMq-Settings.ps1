$rabbitMqContainerId = docker container list --filter ancestor=rabbitmq --format "{{.ID}}"
$rabbitMqServerIpAddress = docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $rabbitMqContainerId
Write-Output "`$rabbitMqServerIpAddress:$rabbitMqServerIpAddress"