#escape=`
FROM microsoft/dotnet:2.0-sdk
WORKDIR /app
COPY .\Cafe.Waiter.Contracts\Cafe.Waiter.Contracts.csproj .\Cafe.Waiter.Contracts\
COPY .\Cafe.Waiter.Events\Cafe.Waiter.Events.csproj .\Cafe.Waiter.Events\
COPY .\Cafe.Waiter.Queries.DAL\Cafe.Waiter.Queries.DAL.csproj .\Cafe.Waiter.Queries.DAL\
COPY .\Cafe.Waiter.EventProjecting.Service\Cafe.Waiter.EventProjecting.Service.csproj .\Cafe.Waiter.EventProjecting.Service\
COPY .\Nuget.config .\Nuget.config
COPY .\.nuget.local .\.nuget.local
RUN dotnet restore .\Cafe.Waiter.Contracts
RUN dotnet restore .\Cafe.Waiter.Events
RUN dotnet restore .\Cafe.Waiter.Queries.DAL
RUN dotnet restore .\Cafe.Waiter.EventProjecting.Service
COPY .\Cafe.Waiter.Contracts .\Cafe.Waiter.Contracts
COPY .\Cafe.Waiter.Events .\Cafe.Waiter.Events
COPY .\Cafe.Waiter.Queries.DAL .\Cafe.Waiter.Queries.DAL
COPY .\Cafe.Waiter.EventProjecting.Service .\Cafe.Waiter.EventProjecting.Service
RUN dotnet build .\Cafe.Waiter.EventProjecting.Service
RUN move .\Cafe.Waiter.EventProjecting.Service\appSettings.docker.json .\Cafe.Waiter.EventProjecting.Service\bin\Debug\netcoreapp2.0\appSettings.override.json
CMD dotnet .\Cafe.Waiter.EventProjecting.Service\bin\Debug\netcoreapp2.0\Cafe.Waiter.EventProjecting.Service.dll