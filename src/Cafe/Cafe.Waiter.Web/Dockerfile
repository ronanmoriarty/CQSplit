#escape=`
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app
COPY .\Nuget.config .\Nuget.config
COPY .\.nuget.local .\.nuget.local
COPY .\Cafe\Cafe.Waiter.Contracts\Cafe.Waiter.Contracts.csproj .\Cafe\Cafe.Waiter.Contracts\
RUN dotnet restore .\Cafe\Cafe.Waiter.Contracts
COPY .\Cafe\Cafe.Waiter.Commands\Cafe.Waiter.Commands.csproj .\Cafe\Cafe.Waiter.Commands\
RUN dotnet restore .\Cafe\Cafe.Waiter.Commands
COPY .\Cafe\Cafe.DAL.Sql\Cafe.DAL.Sql.csproj .\Cafe\Cafe.DAL.Sql\
RUN dotnet restore .\Cafe\Cafe.DAL.Sql
COPY .\Cafe\Cafe.Waiter.Queries.DAL\Cafe.Waiter.Queries.DAL.csproj .\Cafe\Cafe.Waiter.Queries.DAL\
RUN dotnet restore .\Cafe\Cafe.Waiter.Queries.DAL
COPY .\Cafe\Cafe.Waiter.Web\Cafe.Waiter.Web.csproj .\Cafe\Cafe.Waiter.Web\
RUN dotnet restore .\Cafe\Cafe.Waiter.Web
COPY .\Cafe\Cafe.Waiter.Contracts .\Cafe\Cafe.Waiter.Contracts
COPY .\Cafe\Cafe.Waiter.Commands .\Cafe\Cafe.Waiter.Commands
COPY .\Cafe\Cafe.DAL.Sql .\Cafe\Cafe.DAL.Sql
COPY .\Cafe\Cafe.Waiter.Queries.DAL .\Cafe\Cafe.Waiter.Queries.DAL
COPY .\Cafe\Cafe.Waiter.Web .\Cafe\Cafe.Waiter.Web
COPY .\Cafe\Cafe.Waiter.Web\appSettings.docker.json .\Cafe\Cafe.Waiter.Web\appSettings.json
RUN dotnet publish .\Cafe\Cafe.Waiter.Web

FROM microsoft/dotnet:2.1-sdk AS test
WORKDIR /app
COPY --from=builder ["app\\Cafe\\Cafe.DAL.Sql", ".\\Cafe\\Cafe.DAL.Sql"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Contracts", ".\\Cafe\\Cafe.Waiter.Contracts"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Commands", ".\\Cafe\\Cafe.Waiter.Commands"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Queries.DAL", ".\\Cafe\\Cafe.Waiter.Queries.DAL"]
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Web", ".\\Cafe\\Cafe.Waiter.Web"]
COPY .\Cafe\Cafe.DAL.Tests.Common\Cafe.DAL.Tests.Common.csproj .\Cafe\Cafe.DAL.Tests.Common\
RUN dotnet restore .\Cafe\Cafe.DAL.Tests.Common
COPY .\Cafe\Cafe.Waiter.Web.Tests\Cafe.Waiter.Web.Tests.csproj .\Cafe\Cafe.Waiter.Web.Tests\
RUN dotnet restore .\Cafe\Cafe.Waiter.Web.Tests
COPY .\Cafe\Cafe.DAL.Tests.Common .\Cafe\Cafe.DAL.Tests.Common
COPY .\Cafe\Cafe.Waiter.Web.Tests .\Cafe\Cafe.Waiter.Web.Tests
RUN dotnet build .\Cafe\Cafe.Waiter.Web.Tests\Cafe.Waiter.Web.Tests.csproj
CMD dotnet test C:\app\Cafe\Cafe.Waiter.Web.Tests\Cafe.Waiter.Web.Tests.csproj

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=builder ["app\\Cafe\\Cafe.Waiter.Web\\bin\\Debug\\netcoreapp2.1\\publish", ".\\Cafe.Waiter.Web"]
WORKDIR /app/Cafe.Waiter.Web
CMD dotnet .\Cafe.Waiter.Web.dll
EXPOSE 80