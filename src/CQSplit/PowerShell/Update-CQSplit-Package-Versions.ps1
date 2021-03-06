$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. $scriptDir\FileOperations.ps1

$ErrorActionPreference = "Stop"

function GetVersion()
{
    $xml = New-Object -TypeName System.Xml.XmlDocument
    $filePath = GetFullPath '.\src\CQSplit\CQSplit\CQSplit.nuspec'
    $xml.Load($filePath)
    $versionNode = GetVersionNode $xml
    return $versionNode.InnerXml
}

function GetVersionNode([xml] $xml)
{
    $ns = New-Object System.Xml.XmlNamespaceManager -ArgumentList $xml.NameTable
    $ns.AddNamespace("ns", $xml.DocumentElement.NamespaceURI)
    return $xml.SelectSingleNode('//ns:version', $ns)
}

function GetCQDependencyNodes([xml] $xml)
{
    $ns = New-Object System.Xml.XmlNamespaceManager -ArgumentList $xml.NameTable
    $ns.AddNamespace("ns", $xml.DocumentElement.NamespaceURI)
    return $xml.SelectNodes("//ns:dependency[starts-with(@id,'CQSplit')]", $ns)
}

function GetMajorVersion([string] $versionNumber)
{
    return [int] $versionNumber.Split('.')[0]
}

function GetMinorVersion([string] $versionNumber)
{
    return [int] $versionNumber.Split('.')[1]
}

function GetBuildNumber([string] $versionNumber)
{
    $textAfterSecondDot = $versionNumber.Split('.')[2]
    $textBeforeHyphen = $textAfterSecondDot.Substring(0, $textAfterSecondDot.indexOf('-'))
    return [int] $textBeforeHyphen
}

function GetSuffix([string] $versionNumber)
{
    $textAfterSecondDot = $versionNumber.Split('.')[2]
    $textFromHyphenOnwards = $textAfterSecondDot.Substring($textAfterSecondDot.indexOf('-'))
    return $textFromHyphenOnwards
}

function GetVersionWithIncrementedBuildNumber([string] $versionNumber)
{
    $MajorVersionNumber = GetMajorVersion $VersionNumber
    $MinorVersionNumber = GetMinorVersion $VersionNumber
    $BuildNumber = GetBuildNumber $VersionNumber
    $Suffix = GetSuffix $VersionNumber

    return "$MajorVersionNumber.$MinorVersionNumber.$($BuildNumber + 1)$Suffix"
}

function UpdateCQSplitPackageVersions()
{
    $VersionNumber = GetVersion
    $NewVersion = GetVersionWithIncrementedBuildNumber $VersionNumber
    Write-Host "`$NewVersion: $NewVersion"

    Get-ChildItem -Path .\src\CQSplit\ -Filter *.nuspec -Recurse | ForEach-Object {
        $xml = New-Object -TypeName System.Xml.XmlDocument
        $xml.Load($_.FullName)
        $versionNode = GetVersionNode $xml
        $versionNode.InnerXml = $NewVersion
        $dependencyNodes = GetCQDependencyNodes $xml
        foreach ($dependencyNode in $dependencyNodes) {
            $dependencyNode.version = $NewVersion
        }
        $xml.Save($_.FullName)
    }

    git add *.nuspec
    git commit -m "Update CQSplit package versions to $NewVersion"
}