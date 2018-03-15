#escape=`
FROM microsoft/mssql-server-windows-express
WORKDIR /app
COPY .\src\Scripts\Cafe.Waiter.WriteModel .\scripts
SHELL [ "powershell" ]
ENTRYPOINT [ "powershell" ]