#escape=`
FROM microsoft/windowsservercore

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.402

RUN Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip;
RUN $dotnet_sha512 = '405cbd7c65d63b36e3bd6bcdfc897ac6474c4eaf93db9db478a80ab511bfa7a1c4a84024cc6e4af0df0af86bcc0a1a96a8ba0864c77bf579f32bce437c28d5a8'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    };
RUN Expand-Archive dotnet.zip -DestinationPath dotnet; `
    Remove-Item -Force dotnet.zip

USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet"
USER ContainerUser

RUN dotnet --version

RUN Invoke-WebRequest -OutFile .\chrome.exe `
    http://dl.google.com/chrome/install/375.126/chrome_installer.exe
RUN .\chrome.exe /silent /install
RUN Invoke-WebRequest -OutFile .\chromedriver_win32.zip `
    https://chromedriver.storage.googleapis.com/2.42/chromedriver_win32.zip
RUN Expand-Archive .\chromedriver_win32.zip -DestinationPath .\chromedriver_win32\;

USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\dotnet;C:\Program Files (x86)\Google\Chrome\Application;C:\chromedriver_win32"
USER ContainerUser

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