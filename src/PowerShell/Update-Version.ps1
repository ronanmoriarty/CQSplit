[CmdletBinding()]
param (
    [string] $xmlPath = $(throw "xmlPath is a required parameter")
)

function GetCurrentVersion([xml] $xml)
{
    if(VersionElementExists)
    {
        $versionElement = $xml.GetElementsByTagName("Version")[0]
        $currentVersion = $versionElement.InnerText
    }
    else
    {
        $currentVersion = "1.0.0"
    }

    return $currentVersion
}

function GetNewVersion([string] $currentVersion)
{
    $versionParts = $currentVersion.Split(".")
    $majorVersion = [int]$versionParts[0]
    $minorVersion = [int]$versionParts[1]
    $patch = [int]$versionParts[2]
    $incrementedPatch = $patch + 1
    return "$majorVersion.$minorVersion.$incrementedPatch"
}

function VersionElementExists()
{
   return $xml.Project.PropertyGroup.Version -ne $null
}

function UpdateVersion([xml] $xml, [string] $newVersion)
{
    if(VersionElementExists)
    {
        $versionElement = $xml.GetElementsByTagName("Version")[0]
    }
    else
    {
        Write-Host "Creating Version Element as it doesn't exist."
        $versionElement = $xml.CreateElement('Version')
        $xml.Project.PropertyGroup.AppendChild($versionElement)
    }

    $versionElement.InnerText = $newVersion
}

function UpdateXml([xml] $xml, [string] $xmlPath)
{
    $currentWorkingDirectory = (Get-Item -Path '.\').FullName
    $path = Join-Path $currentWorkingDirectory $xmlPath
    $xml.Save($path)
}

$xml = [xml](Get-Content $xmlPath)
$currentVersion = GetCurrentVersion $xml
$newVersion = GetNewVersion $currentVersion
UpdateVersion $xml $newVersion
UpdateXml $xml $xmlPath
Write-Host "Updating version number from $currentVersion to $newVersion"