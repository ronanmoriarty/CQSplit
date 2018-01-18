FROM ronanmoriarty/chocolatey:v1
WORKDIR /app
COPY . .
RUN choco install rabbitmq -y
RUN powershell -Command [Environment]::SetEnvironmentVariable('PATH', [Environment]::GetEnvironmentVariable('PATH') + ';C:\Program Files\RabbitMQ Server\rabbitmq_server-3.6.14\sbin\', 'Machine')
EXPOSE 15672