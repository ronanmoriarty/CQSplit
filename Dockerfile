FROM microsoft/dotnet-framework-build:4.7.1
WORKDIR /app
COPY . .
RUN dotnet restore
RUN msbuild.exe /t:Build /p:Configuration=Release /p:OutputPath=out