[CmdletBinding()]
Param (
    [Parameter(Mandatory=$true)]
    [string]$oldVersion,
    [Parameter(Mandatory=$true)]
    [string]$newVersion
)

Get-ChildItem -Path .\src\Cafe\ -Filter *.csproj -Recurse | ForEach-Object {
    (Get-Content $_.FullName).Replace($oldVersion, $newVersion) | Set-Content $_.FullName
}

git add .\src\Cafe\*.csproj
git commit -m "Update Cafe projects to reference $newVersion CQRS packages"