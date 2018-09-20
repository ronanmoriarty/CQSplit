#escape=`
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app
COPY .\Nuget.config .\Nuget.config
COPY .\.nuget.local .\.nuget.local
COPY .\Cafe\Cafe.Waiter.Contracts\Cafe.Waiter.Contracts.csproj .\Cafe\Cafe.Waiter.Contracts\
RUN dotnet restore .\Cafe\Cafe.Waiter.Contracts
COPY .\Cafe\Cafe.Waiter.Events\Cafe.Waiter.Events.csproj .\Cafe\Cafe.Waiter.Events\
RUN dotnet restore .\Cafe\Cafe.Waiter.Events
COPY .\Cafe\Cafe.DAL.Sql\Cafe.DAL.Sql.csproj .\Cafe\Cafe.DAL.Sql\
RUN dotnet restore .\Cafe\Cafe.DAL.Sql
COPY .\Cafe\Cafe.Waiter.Queries.DAL\Cafe.Waiter.Queries.DAL.csproj .\Cafe\Cafe.Waiter.Queries.DAL\
RUN dotnet restore .\Cafe\Cafe.Waiter.Queries.DAL
COPY .\Cafe\Cafe.Waiter.EventProjecting.Service\Cafe.Waiter.EventProjecting.Service.csproj .\Cafe\Cafe.Waiter.EventProjecting.Service\
RUN dotnet restore .\Cafe\Cafe.Waiter.EventProjecting.Service
COPY .\Cafe\Cafe.Waiter.Contracts .\Cafe\Cafe.Waiter.Contracts
COPY .\Cafe\Cafe.Waiter.Events .\Cafe\Cafe.Waiter.Events
COPY .\Cafe\Cafe.DAL.Sql .\Cafe\Cafe.DAL.Sql
COPY .\Cafe\Cafe.Waiter.Queries.DAL .\Cafe\Cafe.Waiter.Queries.DAL
COPY .\Cafe\Cafe.Waiter.EventProjecting.Service .\Cafe\Cafe.Waiter.EventProjecting.Service
COPY .\Cafe\Cafe.Waiter.EventProjecting.Service\appSettings.docker.json .\Cafe\Cafe.Waiter.EventProjecting.Service\appSettings.json
RUN dotnet publish .\Cafe\Cafe.Waiter.EventProjecting.Service

FROM microsoft/dotnet:2.1-sdk AS test
WORKDIR /app
COPY .\Nuget.config .\Nuget.config
COPY .\.nuget.local .\.nuget.local
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Events", ".\\Cafe\\Cafe.Waiter.Events"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Contracts", ".\\Cafe\\Cafe.Waiter.Contracts"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Queries.DAL", ".\\Cafe\\Cafe.Waiter.Queries.DAL"]
COPY --from=builder ["app\\Cafe\\Cafe.DAL.Sql", ".\\Cafe\\Cafe.DAL.Sql"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.EventProjecting.Service", ".\\Cafe\\Cafe.Waiter.EventProjecting.Service"]
COPY .\Cafe\Cafe.DAL.Tests.Common\Cafe.DAL.Tests.Common.csproj .\Cafe\Cafe.DAL.Tests.Common\
RUN dotnet restore .\Cafe\Cafe.DAL.Tests.Common
COPY .\Cafe\Cafe.Waiter.EventProjecting.Service.Tests\Cafe.Waiter.EventProjecting.Service.Tests.csproj .\Cafe\Cafe.Waiter.EventProjecting.Service.Tests\
RUN dotnet restore .\Cafe\Cafe.Waiter.EventProjecting.Service.Tests
COPY .\Cafe\Cafe.DAL.Tests.Common .\Cafe\Cafe.DAL.Tests.Common
COPY .\Cafe\Cafe.Waiter.EventProjecting.Service.Tests .\Cafe\Cafe.Waiter.EventProjecting.Service.Tests
RUN dotnet build .\Cafe\Cafe.Waiter.EventProjecting.Service.Tests\Cafe.Waiter.EventProjecting.Service.Tests.csproj
CMD dotnet test C:\app\Cafe\Cafe.Waiter.EventProjecting.Service.Tests\Cafe.Waiter.EventProjecting.Service.Tests.csproj --logger "trx;LogFileName=C:\\test-results\\cafe.waiter.eventprojecting.service.tests.trx"

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.EventProjecting.Service\\bin\\Debug\\netcoreapp2.1\\publish", ".\\Cafe.Waiter.EventProjecting.Service"]
EXPOSE 1433
CMD dotnet .\Cafe.Waiter.EventProjecting.Service\Cafe.Waiter.EventProjecting.Service.dll