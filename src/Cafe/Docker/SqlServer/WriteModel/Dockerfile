#escape=`
FROM microsoft/mssql-server-windows-express
WORKDIR /app
ENV sa_password=_
ENV commandServicePassword=_
COPY .\WriteModel\Scripts .\scripts
COPY .\PowerShell .\powershell
EXPOSE 1433
CMD $parameters = @(\"commandServicePassword=$env:commandServicePassword\"); .\powershell\setup.ps1 -sa_password $env:sa_password -Database "Cafe.Waiter.WriteModel" -DatabaseFolder "C:\app\databases" -DatabaseScriptFolder "C:\app\scripts" -Parameters $env:parameters