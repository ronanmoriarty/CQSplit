#escape=`
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app
COPY .\Nuget.config .\Nuget.config
COPY .\.nuget.local .\.nuget.local
COPY .\Cafe.Waiter.Contracts\Cafe.Waiter.Contracts.csproj .\Cafe.Waiter.Contracts\
RUN dotnet restore .\Cafe.Waiter.Contracts
COPY .\Cafe.Waiter.Events\Cafe.Waiter.Events.csproj .\Cafe.Waiter.Events\
RUN dotnet restore .\Cafe.Waiter.Events
COPY .\Cafe.Waiter.Queries.DAL\Cafe.Waiter.Queries.DAL.csproj .\Cafe.Waiter.Queries.DAL\
RUN dotnet restore .\Cafe.Waiter.Queries.DAL
COPY .\Cafe.Waiter.EventProjecting.Service\Cafe.Waiter.EventProjecting.Service.csproj .\Cafe.Waiter.EventProjecting.Service\
RUN dotnet restore .\Cafe.Waiter.EventProjecting.Service
COPY .\Cafe.Waiter.Contracts .\Cafe.Waiter.Contracts
COPY .\Cafe.Waiter.Events .\Cafe.Waiter.Events
COPY .\Cafe.Waiter.Queries.DAL .\Cafe.Waiter.Queries.DAL
COPY .\Cafe.Waiter.EventProjecting.Service .\Cafe.Waiter.EventProjecting.Service
RUN dotnet publish .\Cafe.Waiter.EventProjecting.Service
RUN move .\Cafe.Waiter.EventProjecting.Service\appSettings.docker.json .\Cafe.Waiter.EventProjecting.Service\bin\Debug\netcoreapp2.1\publish\appSettings.override.json

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=builder ["app\\Cafe.Waiter.EventProjecting.Service\\bin\\Debug\\netcoreapp2.1\\publish", ".\\Cafe.Waiter.EventProjecting.Service"]
CMD dotnet .\Cafe.Waiter.EventProjecting.Service\Cafe.Waiter.EventProjecting.Service.dll