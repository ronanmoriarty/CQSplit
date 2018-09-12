#escape=`
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app
COPY .\Nuget.config .\Nuget.config
COPY .\.nuget.local .\.nuget.local
COPY .\Cafe\Cafe.Waiter.Contracts\Cafe.Waiter.Contracts.csproj .\Cafe\Cafe.Waiter.Contracts\
RUN dotnet restore .\Cafe\Cafe.Waiter.Contracts
COPY .\Cafe\Cafe.Waiter.Events\Cafe.Waiter.Events.csproj .\Cafe\Cafe.Waiter.Events\
RUN dotnet restore .\Cafe\Cafe.Waiter.Events
COPY .\Cafe\Cafe.Waiter.Domain\Cafe.Waiter.Domain.csproj .\Cafe\Cafe.Waiter.Domain\
RUN dotnet restore .\Cafe\Cafe.Waiter.Domain
COPY .\Cafe\Cafe.DAL.Sql\Cafe.DAL.Sql.csproj .\Cafe\Cafe.DAL.Sql\
RUN dotnet restore .\Cafe\Cafe.DAL.Sql
COPY .\Cafe\Cafe.DAL.Sql\Cafe.DAL.Sql.csproj .\Cafe\Cafe.DAL.Sql\
RUN dotnet restore .\Cafe\Cafe.DAL.Sql
COPY .\Cafe\Cafe.Waiter.Command.Service\Cafe.Waiter.Command.Service.csproj .\Cafe\Cafe.Waiter.Command.Service\
RUN dotnet restore .\Cafe\Cafe.Waiter.Command.Service
COPY .\Cafe\Cafe.Waiter.Contracts .\Cafe\Cafe.Waiter.Contracts
COPY .\Cafe\Cafe.Waiter.Events .\Cafe\Cafe.Waiter.Events
COPY .\Cafe\Cafe.Waiter.Domain .\Cafe\Cafe.Waiter.Domain
COPY .\Cafe\Cafe.DAL.Sql .\Cafe\Cafe.DAL.Sql
COPY .\Cafe\Cafe.Waiter.Command.Service .\Cafe\Cafe.Waiter.Command.Service
COPY .\Cafe\Cafe.Waiter.Command.Service\appSettings.docker.json .\Cafe\Cafe.Waiter.Command.Service\appSettings.json
RUN dotnet publish .\Cafe\Cafe.Waiter.Command.Service

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Command.Service\\bin\\Debug\\netcoreapp2.1\\publish", ".\\Cafe.Waiter.Command.Service"]
CMD dotnet .\Cafe.Waiter.Command.Service\Cafe.Waiter.Command.Service.dll