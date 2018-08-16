[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
    [string] $apiKey
)

. "$PSScriptRoot\..\..\..\Update-CQSplit-Package-Versions.ps1"

$version = GetVersion
Write-Output "`$version: $version"

git tag -a $version -m "Corresponds to $version nuget packages."
git push --tags

dotnet nuget push .\src\.nuget.local\CQSplit.*.$version.nupkg -k $apiKey -s https://api.nuget.org/v3/index.json