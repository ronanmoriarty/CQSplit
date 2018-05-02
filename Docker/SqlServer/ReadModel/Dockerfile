#escape=`
FROM microsoft/mssql-server-windows-express
WORKDIR /app
ENV sa_password=_
COPY .\ReadModel\Scripts .\scripts
COPY .\PowerShell .\powershell
EXPOSE 1433
CMD .\powershell\setup.ps1 -sa_password $env:sa_password -Database "CQRSTutorial.Cafe.Waiter.ReadModel" -DatabaseFolder "C:\app\databases" -DatabaseScriptFolder "C:\app\scripts"