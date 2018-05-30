Get-ChildItem -Path .\src\CQRS\ -Filter *.nuspec -Recurse | ForEach-Object {
    $VersionNumber = '1.0.1'
    $xml = New-Object -TypeName System.Xml.XmlDocument
    $xml.Load($_.FullName)
    $ns = New-Object System.Xml.XmlNamespaceManager -ArgumentList $xml.NameTable
    $ns.AddNamespace("ns", $xml.DocumentElement.NamespaceURI)
    $versionNode = $xml.SelectSingleNode('//ns:version', $ns)
    $versionNode.InnerXml = $VersionNumber
    $xml.Save($_.FullName)
}