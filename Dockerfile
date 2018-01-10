FROM microsoft/windowsservercore
WORKDIR /app
COPY . .
RUN powershell.exe -Command Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
RUN choco install rabbitmq -y
EXPOSE 15672
