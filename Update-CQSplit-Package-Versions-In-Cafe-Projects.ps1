[CmdletBinding()]
Param (
    [Parameter(Mandatory=$true)]
    [string]$oldVersion,
    [Parameter(Mandatory=$true)]
    [string]$newVersion
)

Get-ChildItem -Path .\src\Cafe\ -Filter *.csproj -Recurse | ForEach-Object {
    # TODO: could do with tightening this up - on the offchance that there's some other
    # 3rd party package with the same version number we'll end up updating that one too.
    # Chances are that would cause a failing build though so would be easily spotted.
    (Get-Content $_.FullName).Replace($oldVersion, $newVersion) | Set-Content $_.FullName
}

git add .\src\Cafe\*.csproj
git commit -m "Update Cafe projects to reference $newVersion CQSplit packages"