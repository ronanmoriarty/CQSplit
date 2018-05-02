#escape=`
FROM microsoft/mssql-server-windows-express
WORKDIR /app
ENV sa_password=_
COPY .\WriteModel\Scripts .\scripts
COPY .\PowerShell .\powershell
EXPOSE 1433
CMD .\powershell\setup.ps1 -sa_password $env:sa_password -Database "CQRSTutorial.Cafe.Waiter.WriteModel" -DatabaseFolder "C:\app\databases" -DatabaseScriptFolder "C:\app\scripts"