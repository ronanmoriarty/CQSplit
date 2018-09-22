[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
    [string] $apiKey
)

$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. $scriptDir\Update-CQSplit-Package-Versions.ps1

$version = GetVersion
Write-Output "`$version: $version"

$ErrorActionPreference = 'Continue' # fetch fails if up-to-date. tag push also "fails" but pushes the tags anyway! Just ignore the lot and fix up tags manually afterwards if need be!
git fetch --tags --force
git push --tags

$ErrorActionPreference = 'Stop'
dotnet nuget push .\src\.nuget.local\CQSplit*.$version.nupkg -k $apiKey -s https://api.nuget.org/v3/index.json