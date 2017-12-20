FROM microsoft/dotnet-framework:4.7
WORKDIR /app
COPY . .
RUN powershell.exe -Command Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
CMD choco list --lo
