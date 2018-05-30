Get-ChildItem -Path .\ -Filter *.nuspec -Recurse | % {
    Write-Output $_.FullName
}