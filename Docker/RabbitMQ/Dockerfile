FROM ronanmoriarty/chocolatey:v1
WORKDIR /app
ENV RABBITMQ_VERSION=3.6.15
SHELL [ "powershell", "-command"]
RUN choco install erlang --version 20.2 -y
RUN Invoke-WebRequest -Uri "https://www.rabbitmq.com/releases/rabbitmq-server/v$env:RABBITMQ_VERSION/rabbitmq-server-windows-$env:RABBITMQ_VERSION.zip" -OutFile "c:\\rabbitmq.zip"; \
        Expand-Archive -Path "c:\\rabbitmq.zip" -DestinationPath "c:\\" ; \
        Remove-Item -Force -Path "c:\\rabbitmq.zip" ;
EXPOSE 15672