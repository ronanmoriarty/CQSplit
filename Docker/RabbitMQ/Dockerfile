FROM ronanmoriarty/chocolatey:v1
WORKDIR /app
ENV RABBITMQ_VERSION=3.6.15
SHELL [ "powershell", "-command"]
RUN choco install erlang --version 20.2 -y
EXPOSE 15672