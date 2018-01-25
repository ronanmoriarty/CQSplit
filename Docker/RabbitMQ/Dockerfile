FROM ronanmoriarty/chocolatey:v1
WORKDIR /app
COPY . .
SHELL [ "powershell", "-command"]
# RUN choco install vim -y
# RUN choco install rabbitmq -y

RUN $downloader = New-Object System.Net.WebClient; \
     $url = 'https://github.com/rabbitmq/rabbitmq-server/releases/download/rabbitmq_v3_6_14/rabbitmq-server-3.6.14.exe'; \
     $localFileName = 'rabbitmq-server-3.6.14.exe'; \
     $downloader.DownloadFile($url, $localFileName); \
     $exec = New-Object -com shell.application; \
     $exec.shellexecute($localFileName);
RUN [Environment]::SetEnvironmentVariable('PATH', [Environment]::GetEnvironmentVariable('PATH') + ';C:\Program Files\RabbitMQ Server\rabbitmq_server-3.6.14\sbin\', 'Machine')
EXPOSE 15672