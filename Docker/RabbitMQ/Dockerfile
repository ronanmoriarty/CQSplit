FROM microsoft/windowsservercore
ENV ERLANG_HOME="c:\\erlang"
SHELL [ "powershell", "-command"]
RUN Invoke-WebRequest -Uri "http://erlang.org/download/otp_win64_20.2.exe" -OutFile "c:\\erlang_install.exe" ; \
        Start-Process -Wait -FilePath "c:\\erlang_install.exe" -ArgumentList /S, /D=$env:ERLANG_HOME ; \
        Remove-Item -Force -Path "C:\\erlang_install.exe" ;
ENV RABBITMQ_VERSION=3.6.15
RUN Invoke-WebRequest -Uri "https://www.rabbitmq.com/releases/rabbitmq-server/v$env:RABBITMQ_VERSION/rabbitmq-server-windows-$env:RABBITMQ_VERSION.zip" -OutFile "c:\\rabbitmq.zip"; \
        Expand-Archive -Path "c:\\rabbitmq.zip" -DestinationPath "c:\\" ; \
        Remove-Item -Force -Path "c:\\rabbitmq.zip" ;
RUN $path = [Environment]::GetEnvironmentVariable('Path', 'Machine'); \
    [Environment]::SetEnvironmentVariable('Path', $path + ';C:\rabbitmq_server-' + $env:RABBITMQ_VERSION + '\sbin', 'Machine')
EXPOSE 15672