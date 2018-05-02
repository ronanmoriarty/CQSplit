[CmdletBinding()]
Param(
    [Parameter(Mandatory=$true)]
    [string]$dbName,
    [Parameter(Mandatory=$true)]
    [string]$dbFolder,
    [Parameter(Mandatory=$true)]
    [string]$dbScriptsFolder
)

. "$PSScriptRoot\attachDatabase.ps1"

[reflection.assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")

function GetMdfFilePath($dbName)
{
    return "$dbFolder\$dbName.mdf"
}

function GetLdfFilePath($dbName)
{
    return "$dbFolder\$($dbName)_log.ldf"
}

function CreateNewDatabase () {
    if(!(Test-Path $dbFolder))
    {
        mkdir $dbFolder
    }

    try
    {
        Write-Host "Creating new database $dbName..."
        $server = GetLocalSqlServer
        $database = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Database -ArgumentList $server, $dbName
        $database.DatabaseOptions.AutoShrink = $true
        $primaryFileGroup = New-Object -TypeName Microsoft.SqlServer.Management.Smo.FileGroup -ArgumentList $database, 'PRIMARY'
        $database.FileGroups.Add($primaryFileGroup)
        $mdfFilePath = GetMdfFilePath $dbName
        $dataFile = New-Object -TypeName Microsoft.SqlServer.Management.Smo.DataFile -ArgumentList $primaryFileGroup, "$($dbName)_Data", $mdfFilePath
        $primaryFileGroup.Files.Add($dataFile)
        $database.Create()
        Get-ChildItem $dbFolder
    }
    catch [Exception]
    {
        Write-Host $_.Exception|format-list -force
    }
}

function DatabaseFilesExist ()
{
    $mdfFilePath = GetMdfFilePath $dbName
    $ldfFilePath = GetLdfFilePath $dbName
    $dbFilesExist = (Test-Path ($mdfFilePath)) -and (Test-Path ($ldfFilePath))
    if($dbFilesExist)
    {
        Write-Host "Files $mdfFilePath and $ldfFilePath found for $dbName database."
    }
    else
    {
        Write-Host "No files found for $dbName database."
    }

    return $dbFilesExist
}

function GetLocalSqlServer()
{
    return new-object Microsoft.SqlServer.Management.Smo.Server -ArgumentList "."
}

function DatabaseExists()
{
    return ((GetLocalSqlServer).Databases[$dbName]) -ne $null
}

function EnsureDatabaseExists()
{
    if(-Not (DatabaseExists))
    {
        Write-Host "$dbName database not found."
        if(DatabaseFilesExist)
        {
            AttachExistingDatabase
        }
        else
        {
            CreateNewDatabase
        }
    }
    else
    {
        Write-Host "$dbName already attached."
    }
}

function ApplyDatabaseMigrations()
{
    Write-Host "Applying scripts from $dbScriptsFolder..."
    Get-ChildItem $dbScriptsFolder | Sort-Object | ForEach-Object { Invoke-SqlCmd -InputFile $_.FullName -ServerInstance "." -Database $dbName }
    Write-Host "Finished applying scripts for $dbName"
}

EnsureDatabaseExists
ApplyDatabaseMigrations