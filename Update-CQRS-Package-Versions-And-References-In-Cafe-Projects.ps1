. .\Update-CQRS-Package-Versions.ps1

$oldVersion = GetVersion
UpdateCQRSPackageVersions
.\build.ps1 -Target Create-CQRS-NuGet-Packages
$newVersion = GetVersion
Update-CQRS-Package-Versions-In-Cafe-Projects.ps1 -oldVersion $oldVersion -newVersion $newVersion