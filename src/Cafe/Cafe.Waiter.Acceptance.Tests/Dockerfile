#escape=`
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app
COPY .\Nuget.config .\Nuget.config
COPY .\.nuget.local .\.nuget.local
COPY .\Cafe\Cafe.DAL.Sql\Cafe.DAL.Sql.csproj .\Cafe\Cafe.DAL.Sql\
RUN dotnet restore .\Cafe\Cafe.DAL.Sql
COPY .\Cafe\Cafe.DAL.Tests.Common\Cafe.DAL.Tests.Common.csproj .\Cafe\Cafe.DAL.Tests.Common\
RUN dotnet restore .\Cafe\Cafe.DAL.Tests.Common
COPY .\Cafe\Cafe.Waiter.Acceptance.Tests\Cafe.Waiter.Acceptance.Tests.csproj .\Cafe\Cafe.Waiter.Acceptance.Tests\
RUN dotnet restore .\Cafe\Cafe.Waiter.Acceptance.Tests
COPY .\Cafe\Cafe.DAL.Sql .\Cafe\Cafe.DAL.Sql
COPY .\Cafe\Cafe.DAL.Tests.Common .\Cafe\Cafe.DAL.Tests.Common
COPY .\Cafe\Cafe.Waiter.Acceptance.Tests .\Cafe\Cafe.Waiter.Acceptance.Tests
COPY .\Cafe\Cafe.Waiter.Acceptance.Tests\appSettings.docker.json .\Cafe\Cafe.Waiter.Acceptance.Tests\appSettings.json
RUN dotnet build .\Cafe\Cafe.Waiter.Acceptance.Tests\Cafe.Waiter.Acceptance.Tests.csproj
CMD dotnet test C:\app\Cafe\Cafe.Waiter.Acceptance.Tests\Cafe.Waiter.Acceptance.Tests.csproj