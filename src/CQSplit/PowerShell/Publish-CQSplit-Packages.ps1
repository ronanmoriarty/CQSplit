[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
    [string] $apiKey
)

$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. $scriptDir\Update-CQSplit-Package-Versions.ps1

$version = GetVersion
Write-Output "`$version: $version"

git fetch --tags --force
git push --tags

dotnet nuget push .\src\.nuget.local\CQSplit*.$version.nupkg -k $apiKey -s https://api.nuget.org/v3/index.json