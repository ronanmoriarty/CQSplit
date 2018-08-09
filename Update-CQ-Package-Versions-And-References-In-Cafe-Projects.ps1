. .\Update-CQ-Package-Versions.ps1

$oldVersion = GetVersion
UpdateCQPackageVersions
.\build.ps1 -Target Create-CQ-NuGet-Packages
$newVersion = GetVersion
.\Update-CQ-Package-Versions-In-Cafe-Projects.ps1 -oldVersion $oldVersion -newVersion $newVersion