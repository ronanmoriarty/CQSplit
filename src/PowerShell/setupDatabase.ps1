[CmdletBinding()]
Param(
    [string]$dbName = 'CQRSTutorial.Cafe.Waiter.WriteModel',
    [string]$dbFolder = 'C:\app\databases'
)

[reflection.assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")

$server = new-object ("Microsoft.SqlServer.Management.Smo.Server") "."
$database = $server.Databases[$dbName]
if($database -eq $null)
{
    Write-Host "$dbName database not found."
    $mdfFilePath = "$dbFolder\$dbName.mdf"
    if(Test-Path $mdfFilePath)
    {
        Write-Host "WriteModel database $mdfFilePath found. Attaching as $dbName..."
    }
    else
    {
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
        }
        catch [Exception]
        {
            Write-Output $_.Exception|format-list -force
        }
    }
}
else
{
    Write-Host "$dbName already attached."
}