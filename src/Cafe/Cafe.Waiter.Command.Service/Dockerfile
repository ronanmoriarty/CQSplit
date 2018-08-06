#escape=`
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app
COPY .\Nuget.config .\Nuget.config
COPY .\.nuget.local .\.nuget.local
COPY .\Cafe.Waiter.Contracts\Cafe.Waiter.Contracts.csproj .\Cafe.Waiter.Contracts\
RUN dotnet restore .\Cafe.Waiter.Contracts
COPY .\Cafe.Waiter.Events\Cafe.Waiter.Events.csproj .\Cafe.Waiter.Events\
RUN dotnet restore .\Cafe.Waiter.Events
COPY .\Cafe.Waiter.Domain\Cafe.Waiter.Domain.csproj .\Cafe.Waiter.Domain\
RUN dotnet restore .\Cafe.Waiter.Domain
COPY .\Cafe.DAL.Common\Cafe.DAL.Common.csproj .\Cafe.DAL.Common\
RUN dotnet restore .\Cafe.DAL.Common
COPY .\Cafe.DAL.Sql\Cafe.DAL.Sql.csproj .\Cafe.DAL.Sql\
RUN dotnet restore .\Cafe.DAL.Sql
COPY .\Cafe.Waiter.Command.Service\Cafe.Waiter.Command.Service.csproj .\Cafe.Waiter.Command.Service\
RUN dotnet restore .\Cafe.Waiter.Command.Service
COPY .\Cafe.Waiter.Contracts .\Cafe.Waiter.Contracts
COPY .\Cafe.Waiter.Events .\Cafe.Waiter.Events
COPY .\Cafe.Waiter.Domain .\Cafe.Waiter.Domain
COPY .\Cafe.DAL.Common .\Cafe.DAL.Common
COPY .\Cafe.DAL.Sql .\Cafe.DAL.Sql
COPY .\Cafe.Waiter.Command.Service .\Cafe.Waiter.Command.Service
RUN dotnet publish .\Cafe.Waiter.Command.Service
RUN move .\Cafe.Waiter.Command.Service\appSettings.docker.json .\Cafe.Waiter.Command.Service\bin\Debug\netcoreapp2.1\publish\appSettings.override.json

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=builder ["app\\Cafe.Waiter.Command.Service\\bin\\Debug\\netcoreapp2.1\\publish", ".\\Cafe.Waiter.Command.Service"]
CMD dotnet .\Cafe.Waiter.Command.Service\Cafe.Waiter.Command.Service.dll