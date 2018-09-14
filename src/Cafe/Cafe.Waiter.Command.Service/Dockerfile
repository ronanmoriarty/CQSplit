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
COPY .\Cafe\Cafe.Waiter.Command.Service\Cafe.Waiter.Command.Service.csproj .\Cafe\Cafe.Waiter.Command.Service\
RUN dotnet restore .\Cafe\Cafe.Waiter.Command.Service
COPY .\Cafe\Cafe.Waiter.Commands\Cafe.Waiter.Commands.csproj .\Cafe\Cafe.Waiter.Commands\
RUN dotnet restore .\Cafe\Cafe.Waiter.Commands
COPY .\Cafe\Cafe.Waiter.Domain.Tests\Cafe.Waiter.Domain.Tests.csproj .\Cafe\Cafe.Waiter.Domain.Tests\
RUN dotnet restore .\Cafe\Cafe.Waiter.Domain.Tests
COPY .\Cafe\Cafe.Waiter.Contracts .\Cafe\Cafe.Waiter.Contracts
COPY .\Cafe\Cafe.Waiter.Events .\Cafe\Cafe.Waiter.Events
COPY .\Cafe\Cafe.Waiter.Domain .\Cafe\Cafe.Waiter.Domain
COPY .\Cafe\Cafe.DAL.Sql .\Cafe\Cafe.DAL.Sql
COPY .\Cafe\Cafe.Waiter.Command.Service .\Cafe\Cafe.Waiter.Command.Service
COPY .\Cafe\Cafe.Waiter.Command.Service\appSettings.docker.json .\Cafe\Cafe.Waiter.Command.Service\appSettings.json
COPY .\Cafe\Cafe.Waiter.Commands .\Cafe\Cafe.Waiter.Commands
COPY .\Cafe\Cafe.Waiter.Domain.Tests .\Cafe\Cafe.Waiter.Domain.Tests
RUN dotnet test .\Cafe\Cafe.Waiter.Domain.Tests\Cafe.Waiter.Domain.Tests.csproj
RUN dotnet publish .\Cafe\Cafe.Waiter.Command.Service

FROM microsoft/dotnet:2.1-sdk AS test
WORKDIR /app
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Events", ".\\Cafe\\Cafe.Waiter.Events"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Contracts", ".\\Cafe\\Cafe.Waiter.Contracts"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Domain", ".\\Cafe\\Cafe.Waiter.Domain"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Commands", ".\\Cafe\\Cafe.Waiter.Commands"]
COPY --from=builder ["app\\Cafe\\Cafe.DAL.Sql", ".\\Cafe\\Cafe.DAL.Sql"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Command.Service", ".\\Cafe\\Cafe.Waiter.Command.Service"]
COPY .\Cafe\Cafe.DAL.Tests.Common\Cafe.DAL.Tests.Common.csproj .\Cafe\Cafe.DAL.Tests.Common\
RUN dotnet restore .\Cafe\Cafe.DAL.Tests.Common
COPY .\Cafe\Cafe.DAL.Tests.Common .\Cafe\Cafe.DAL.Tests.Common
COPY .\Cafe\Cafe.Waiter.Command.Service.Tests\Cafe.Waiter.Command.Service.Tests.csproj .\Cafe\Cafe.Waiter.Command.Service.Tests\
RUN dotnet restore .\Cafe\Cafe.Waiter.Command.Service.Tests
COPY .\Cafe\Cafe.Waiter.Command.Service.Tests .\Cafe\Cafe.Waiter.Command.Service.Tests
RUN dotnet build .\Cafe\Cafe.Waiter.Command.Service.Tests\Cafe.Waiter.Command.Service.Tests.csproj
CMD dotnet test C:\app\Cafe\Cafe.Waiter.Command.Service.Tests\Cafe.Waiter.Command.Service.Tests.csproj

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Command.Service\\bin\\Debug\\netcoreapp2.1\\publish", ".\\Cafe.Waiter.Command.Service"]
CMD dotnet .\Cafe.Waiter.Command.Service\Cafe.Waiter.Command.Service.dll