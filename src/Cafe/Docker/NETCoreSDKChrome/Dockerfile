#escape=`
FROM microsoft/windowsservercore as installer-env
SHELL [ "powershell", "-command"]
WORKDIR c:\temp
RUN Invoke-WebRequest -OutFile .\chrome.exe `
    http://dl.google.com/chrome/install/375.126/chrome_installer.exe
RUN .\chrome.exe /silent /install
RUN Invoke-WebRequest -OutFile .\chromedriver_win32.zip `
    https://chromedriver.storage.googleapis.com/2.42/chromedriver_win32.zip
RUN Expand-Archive .\chromedriver_win32.zip -DestinationPath .\chromedriver_win32\;

FROM microsoft/dotnet:2.1-sdk
COPY --from=installer-env ["C:\\Program Files (x86)\\Google\\", "C:\\Program Files (x86)\\Google\\"]
COPY --from=installer-env ["c:\\temp\\chromedriver_win32\\", "C:\\chromedriver_win32"]

USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files (x86)\Google\Chrome\Application;C:\chromedriver_win32"
USER ContainerUser