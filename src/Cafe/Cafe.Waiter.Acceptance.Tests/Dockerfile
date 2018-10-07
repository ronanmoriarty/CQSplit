# escape=`

# Installer image
FROM microsoft/windowsservercore:1803 AS installer-env

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.403

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
    $dotnet_sha512 = '52bb1117f170587eaceec1f78cdc41a41d4272154b5535bf61c86bfb75287323cac248434b05eabe4bc7716facabdb0f6475015cbb63f38d91af662618a06720'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    `
    Expand-Archive dotnet.zip -DestinationPath dotnet; `
    Remove-Item -Force dotnet.zip


# SDK image
FROM microsoft/windowsservercore:1803

COPY --from=installer-env ["dotnet", "C:\\Program Files\\dotnet"]

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

# Configure Kestrel web server to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 `
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help

USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\;C:\chromedriver_win32"
USER ContainerUser

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

USER ContainerAdministrator
RUN Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
RUN choco install googlechrome -y
RUN choco install selenium-chrome-driver -y
RUN choco install phantomjs -y
USER ContainerUser

ENV CHROME_LOG_FILE C:\chromedriver-logs\chrome.log

# RUN Invoke-WebRequest -OutFile .\chrome.exe `
#     http://dl.google.com/chrome/install/375.126/chrome_installer.exe
# RUN .\chrome.exe /silent /install
# RUN Invoke-WebRequest -OutFile .\chromedriver_win32.zip `
#     https://chromedriver.storage.googleapis.com/2.42/chromedriver_win32.zip
# RUN Expand-Archive .\chromedriver_win32.zip -DestinationPath .\chromedriver_win32\;

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

CMD dotnet test C:\app\Cafe\Cafe.Waiter.Acceptance.Tests\Cafe.Waiter.Acceptance.Tests.csproj --logger "trx;LogFileName=C:\\test-results\\cafe.waiter.acceptance.tests.trx"