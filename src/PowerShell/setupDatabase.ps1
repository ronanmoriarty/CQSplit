[CmdletBinding()]
Param(
    [Parameter(Mandatory=$true)]
    [string]$dbName,
    [Parameter(Mandatory=$true)]
    [string]$dbFolder
)

[reflection.assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")

function CreateNewDatabase ($dbFolder, $mdfFilePath, $server, $dbName) {
    if(!(Test-Path $dbFolder))
    {
        mkdir $dbFolder
    }

    try
    {
        Write-Host "$mdfFilePath does not exist. Creating new database..."
        $database = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Database -ArgumentList $server, $dbName
        $database.DatabaseOptions.AutoShrink = $true
        $primaryFileGroup = New-Object -TypeName Microsoft.SqlServer.Management.Smo.FileGroup -ArgumentList $database, 'PRIMARY'
        $database.FileGroups.Add($primaryFileGroup)
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

function AttachExistingDatabase ($mdfFilePath, $ldfFilePath, $server, $dbName) {
    Write-Host "WriteModel database $mdfFilePath found. Attaching as $dbName..."
    $dataFiles = New-Object System.Collections.Specialized.StringCollection
    $dataFiles.Add($mdfFilePath)
    $dataFiles.Add($ldfFilePath)
    $server.AttachDatabase($dbName, $dataFiles)
    Write-Host $server.Databases
}

$server = new-object Microsoft.SqlServer.Management.Smo.Server -ArgumentList "."
$database = $server.Databases[$dbName]
if($database -eq $null)
{
    Write-Host "$dbName database not found."
    $mdfFilePath = "$dbFolder\$dbName.mdf"
    $ldfFilePath = "$dbFolder\$($dbName)_log.ldf"
    if(Test-Path $mdfFilePath)
    {
        AttachExistingDatabase $mdfFilePath $ldfFilePath $server $dbName
    }
    else
    {
        CreateNewDatabase $dbFolder $mdfFilePath $server $dbName
    }
}
else
{
    Write-Host "$dbName already attached."
}