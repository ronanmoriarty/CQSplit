$rabbitMqContainerId = docker container list --filter ancestor=rabbitmq --format "{{.ID}}"
Write-Output "`$rabbitMqContainerId: $rabbitMqContainerId"