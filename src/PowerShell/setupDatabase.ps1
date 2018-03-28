[reflection.assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")

$server = new-object ("Microsoft.SqlServer.Management.Smo.Server") "."
$dbName = 'CQRSTutorial.Cafe.Waiter.WriteModel'

if($server.Databases[$dbName] -eq $null)
{
    Write-Host "$dbName database not found."
    $mdfFilePath = ".\databases\$dbName.mdf"
    if(Test-Path $mdfFilePath)
    {
        Write-Host "WriteModel database $mdfFilePath found. Attaching as $dbName..."
    }
    else
    {
        Write-Host "$mdfFilePath does not exist. Creating new database..."
    }
}
else
{
    Write-Host "$dbName already attached."
}