[CmdletBinding()]
Param(
    [Parameter(Mandatory=$true)]
    [string]$VersionNumber
)
Get-ChildItem -Path .\src\CQRS\ -Filter *.nuspec -Recurse | ForEach-Object {
    $xml = New-Object -TypeName System.Xml.XmlDocument
    $xml.Load($_.FullName)
    $ns = New-Object System.Xml.XmlNamespaceManager -ArgumentList $xml.NameTable
    $ns.AddNamespace("ns", $xml.DocumentElement.NamespaceURI)
    $versionNode = $xml.SelectSingleNode('//ns:version', $ns)
    Write-Output "$($_.FullName) has version number $($versionNode.InnerXml)"
}