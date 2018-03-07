#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
// var buildDir = Directory("./**/bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    var cleanDirectoriesSearchPattern = "./**/bin/" + configuration;
    Information("CleanDirectories at: " + cleanDirectoriesSearchPattern);
    CleanDirectories(cleanDirectoriesSearchPattern);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./CQRSTutorial.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./CQRSTutorial.sln", settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild("./CQRSTutorial.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    RunNUnitTests();
});

Task("Run-Unit-Tests-Without-Build")
    .Does(() =>
{
    RunNUnitTests();
});

void RunNUnitTests(){
    var nunitSearchPattern = "./**/bin/" + configuration + "/net461/*.Tests.dll";
    Information("NUnit Search Pattern:" + nunitSearchPattern);
    NUnit3(nunitSearchPattern, new NUnit3Settings {
        NoResults = true
    });
}

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
