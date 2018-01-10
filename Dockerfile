FROM microsoft/windowsservercore
WORKDIR /app
COPY Docker .
RUN ["powershell", ".\\install-chocolatey.ps1"]
RUN choco install rabbitmq -y
EXPOSE 15672
