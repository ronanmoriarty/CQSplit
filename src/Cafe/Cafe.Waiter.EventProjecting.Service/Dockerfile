#escape=`
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app
COPY .\Nuget.config .\Nuget.config
COPY .\.nuget.local .\.nuget.local
COPY .\Cafe\Cafe.Waiter.Contracts\Cafe.Waiter.Contracts.csproj .\Cafe\Cafe.Waiter.Contracts\
RUN dotnet restore .\Cafe\Cafe.Waiter.Contracts
COPY .\Cafe\Cafe.Waiter.Events\Cafe.Waiter.Events.csproj .\Cafe\Cafe.Waiter.Events\
RUN dotnet restore .\Cafe\Cafe.Waiter.Events
COPY .\Cafe\Cafe.DAL.Common\Cafe.DAL.Common.csproj .\Cafe\Cafe.DAL.Common\
RUN dotnet restore .\Cafe\Cafe.DAL.Common
COPY .\Cafe\Cafe.Waiter.Queries.DAL\Cafe.Waiter.Queries.DAL.csproj .\Cafe\Cafe.Waiter.Queries.DAL\
RUN dotnet restore .\Cafe\Cafe.Waiter.Queries.DAL
COPY .\Cafe\Cafe.Waiter.EventProjecting.Service\Cafe.Waiter.EventProjecting.Service.csproj .\Cafe\Cafe.Waiter.EventProjecting.Service\
RUN dotnet restore .\Cafe\Cafe.Waiter.EventProjecting.Service
COPY .\Cafe\Cafe.Waiter.Contracts .\Cafe\Cafe.Waiter.Contracts
COPY .\Cafe\Cafe.Waiter.Events .\Cafe\Cafe.Waiter.Events
COPY .\Cafe\Cafe.DAL.Common .\Cafe\Cafe.DAL.Common
COPY .\Cafe\Cafe.Waiter.Queries.DAL .\Cafe\Cafe.Waiter.Queries.DAL
COPY .\Cafe\Cafe.Waiter.EventProjecting.Service .\Cafe\Cafe.Waiter.EventProjecting.Service
RUN dotnet publish .\Cafe\Cafe.Waiter.EventProjecting.Service
RUN move .\Cafe\Cafe.Waiter.EventProjecting.Service\appSettings.docker.json .\Cafe\Cafe.Waiter.EventProjecting.Service\bin\Debug\netcoreapp2.1\publish\appSettings.override.json

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.EventProjecting.Service\\bin\\Debug\\netcoreapp2.1\\publish", ".\\Cafe.Waiter.EventProjecting.Service"]
CMD dotnet .\Cafe.Waiter.EventProjecting.Service\Cafe.Waiter.EventProjecting.Service.dll