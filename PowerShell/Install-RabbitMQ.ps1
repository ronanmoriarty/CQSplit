$erlangInstallerPath = "c:\erlang_install.exe"
[Environment]::SetEnvironmentVariable("ERLANG_HOME", "c:\erlang", "Machine")
refreshenv

Invoke-WebRequest -Uri "http://erlang.org/download/otp_win64_20.2.exe" -OutFile $erlangInstallerPath
Start-Process -Wait -FilePath $erlangInstallerPath -ArgumentList /S, /D=$env:ERLANG_HOME
Remove-Item -Force -Path $erlangInstallerPath

$rabbitMqVersion = '3.6.15'
$rabbitInstallDirectory = "C:\rabbitmq_server-$rabbitMqVersion"
[Environment]::SetEnvironmentVariable("RABBITMQ_SERVER", $rabbitInstallDirectory, "Machine")
$path = [Environment]::GetEnvironmentVariable('Path', 'Machine')
[Environment]::SetEnvironmentVariable('Path', "$path;$rabbitInstallDirectory\sbin", 'Machine')
# [Environment]::SetEnvironmentVariable("RABBITMQ_VERSION", $rabbitMqVersion, "Machine")
refreshenv

$rabbitMqInstallerPath = "c:\rabbitmq.zip"
Invoke-WebRequest -Uri "https://www.rabbitmq.com/releases/rabbitmq-server/v$rabbitMqVersion/rabbitmq-server-windows-$rabbitMqVersion.zip" -OutFile $rabbitMqInstallerPath
Expand-Archive -Path $rabbitMqInstallerPath -DestinationPath "c:\"
Remove-Item -Force -Path $rabbitMqInstallerPath

rabbitmq-plugins enable rabbitmq_management --offline

$rabbitMqConfigPath = Join-Path -Path $PSScriptRoot -ChildPath "..\Docker\RabbitMQ\rabbitmq.config"
Copy-Item $rabbitMqConfigPath "$env:USERPROFILE\AppData\Roaming\RabbitMQ\rabbitmq.config"

rabbitmq-service install
rabbitmq-service start