. .\Update-CQSplit-Package-Versions.ps1

$oldVersion = GetVersion
UpdateCQSplitPackageVersions
.\build.ps1 -Target Create-CQSplit-NuGet-Packages
$newVersion = GetVersion
.\Update-CQSplit-Package-Versions-In-Cafe-Projects.ps1 -oldVersion $oldVersion -newVersion $newVersion