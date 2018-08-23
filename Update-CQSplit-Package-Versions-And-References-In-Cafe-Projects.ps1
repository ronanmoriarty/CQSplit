. .\Update-CQSplit-Package-Versions.ps1

$oldVersion = GetVersion
UpdateCQSplitPackageVersions
.\build.ps1 -Target Create-CQSplit-NuGet-Packages
$newVersion = GetVersion
git tag -a $newVersion -m "Corresponds to $newVersion nuget packages."

.\Update-CQSplit-Package-Versions-In-Cafe-Projects.ps1 -oldVersion $oldVersion -newVersion $newVersion