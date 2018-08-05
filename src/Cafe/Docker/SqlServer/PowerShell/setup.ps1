[CmdletBinding()]
Param(
    [Parameter(Mandatory=$true)]
    [string]$sa_password,
    [Parameter(Mandatory=$true)]
    [string]$Database,
    [Parameter(Mandatory=$true)]
    [string]$DatabaseFolder,
    [Parameter(Mandatory=$true)]
    [string]$DatabaseScriptFolder
)

& $PSScriptRoot\setupDatabase.ps1 $Database $DatabaseFolder $DatabaseScriptFolder $sa_password
& C:\start.ps1 -sa_password $sa_password -ACCEPT_EULA Y