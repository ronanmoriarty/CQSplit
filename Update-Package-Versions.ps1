function GetVersion()
{
    $xml = New-Object -TypeName System.Xml.XmlDocument
    $xml.Load('.\src\CQRS\CQRSTutorial.Core\CQRSTutorial.Core.nuspec')
    $versionNode = GetVersionNode $xml
    return $versionNode.InnerXml
}

function GetVersionNode([xml] $xml)
{
    $ns = New-Object System.Xml.XmlNamespaceManager -ArgumentList $xml.NameTable
    $ns.AddNamespace("ns", $xml.DocumentElement.NamespaceURI)
    return $xml.SelectSingleNode('//ns:version', $ns)
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

$VersionNumber = GetVersion
$MajorVersionNumber = GetMajorVersion $VersionNumber
$MinorVersionNumber = GetMinorVersion $VersionNumber
$BuildNumber = GetBuildNumber $VersionNumber
$Suffix = GetSuffix $VersionNumber
Write-Host "Major Version: $MajorVersionNumber"
Write-Host "Minor Version: $MinorVersionNumber"
Write-Host "Build Number: $BuildNumber"
Write-Host "Suffix: $Suffix"

$NewVersion = "$MajorVersionNumber.$MinorVersionNumber.$($BuildNumber + 1)$Suffix"
Write-Host "`$NewVersion: $NewVersion"

Get-ChildItem -Path .\src\CQRS\ -Filter *.nuspec -Recurse | ForEach-Object {
    $xml = New-Object -TypeName System.Xml.XmlDocument
    $xml.Load($_.FullName)
    $versionNode = GetVersionNode $xml
    $versionNode.InnerXml = $NewVersion
    $xml.Save($_.FullName)
}