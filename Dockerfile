FROM microsoft/windowsservercore
WORKDIR /app
COPY Docker .
RUN ["powershell", ".\\install-chocolatey.ps1"]
