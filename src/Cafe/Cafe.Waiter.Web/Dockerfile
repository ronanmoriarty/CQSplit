#escape=`
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app
COPY .\Nuget.config .\Nuget.config
COPY .\.nuget.local .\.nuget.local
COPY .\Cafe.Waiter.Contracts\Cafe.Waiter.Contracts.csproj .\Cafe.Waiter.Contracts\
RUN dotnet restore .\Cafe.Waiter.Contracts
COPY .\Cafe.Waiter.Commands\Cafe.Waiter.Commands.csproj .\Cafe.Waiter.Commands\
RUN dotnet restore .\Cafe.Waiter.Commands
COPY .\Cafe.Waiter.Queries.DAL\Cafe.Waiter.Queries.DAL.csproj .\Cafe.Waiter.Queries.DAL\
RUN dotnet restore .\Cafe.Waiter.Queries.DAL
COPY .\Cafe.Waiter.Web\Cafe.Waiter.Web.csproj .\Cafe.Waiter.Web\
RUN dotnet restore .\Cafe.Waiter.Web
COPY .\Cafe.Waiter.Contracts .\Cafe.Waiter.Contracts
COPY .\Cafe.Waiter.Commands .\Cafe.Waiter.Commands
COPY .\Cafe.Waiter.Queries.DAL .\Cafe.Waiter.Queries.DAL
COPY .\Cafe.Waiter.Web .\Cafe.Waiter.Web
RUN dotnet publish .\Cafe.Waiter.Web
RUN move .\Cafe.Waiter.Web\appSettings.docker.json .\Cafe.Waiter.Web\bin\Debug\netcoreapp2.1\publish\appSettings.override.json

FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=builder ["app\\Cafe.Waiter.Web\\bin\\Debug\\netcoreapp2.1\\publish", ".\\Cafe.Waiter.Web"]
CMD dotnet .\Cafe.Waiter.Web\Cafe.Waiter.Web.dll