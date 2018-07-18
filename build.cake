#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin "Cake.Powershell"

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
    var cleanDirectoriesSearchPattern = "./src/Cafe/**/bin/" + configuration;
    Information("CleanDirectories at: " + cleanDirectoriesSearchPattern);
    CleanDirectories(cleanDirectoriesSearchPattern);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("./src/Cafe/Cafe.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./src/Cafe/Cafe.sln", settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild("./src/Cafe/Cafe.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    RunCafeUnitTests();
});

Task("Run-Unit-Tests-Without-Build")
    .Does(() =>
{
    RunCafeUnitTests();
});

private void RunCafeUnitTests()
{
    RunDotNetCoreUnitTests("./src/Cafe/**/*.Tests.csproj");
}

private void RunDotNetCoreUnitTests(string filePattern)
{
    var testProjects = GetFiles(filePattern);
    foreach (var testProject in testProjects)
    {
        DotNetCoreTest(testProject.FullPath);
    }

    KillNUnitAgentProcesses();
}

Task("Clean-CQRS")
    .Does(() =>
{
    var cleanDirectoriesSearchPattern = "./src/CQRS/**/bin/" + configuration;
    Information("CleanDirectories at: " + cleanDirectoriesSearchPattern);
    CleanDirectories(cleanDirectoriesSearchPattern);
});

Task("Restore-CQRS-NuGet-Packages")
    .IsDependentOn("Clean-CQRS")
    .Does(() =>
{
    DotNetCoreRestore("./src/CQRS/CQRS.sln");
});

Task("Build-CQRS")
    .IsDependentOn("Restore-CQRS-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./src/CQRS/CQRS.sln", settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild("./src/CQRS/CQRS.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-CQRS-Unit-Tests")
    .IsDependentOn("Build-CQRS")
    .Does(() =>
{
    RunDotNetCoreUnitTests("./src/CQRS/**/*Tests.csproj");
});

Task("Run-CQRS-Unit-Tests-Without-Build")
    .Does(() =>
{
    RunDotNetCoreUnitTests("./src/CQRS/**/*Tests.csproj");
});

Task("Create-CQRS-Nuget-Packages")
    .IsDependentOn("Run-CQRS-Unit-Tests")
    .Does(() =>
{
    var nuGetPackSettings = new NuGetPackSettings {
        OutputDirectory = "C:\\.nuget.local"
    };

    var testProjects = GetFiles("./src/CQRS/**/*.nuspec");
    foreach (var testProject in testProjects)
    {
        NuGetPack(testProject.FullPath, nuGetPackSettings);
    }
});

void RunNUnitTests(string nunitSearchPattern)
{
    Information("NUnit Search Pattern:" + nunitSearchPattern);
    NUnit3(nunitSearchPattern, new NUnit3Settings {
        NoResults = true
    });
}

void KillNUnitAgentProcesses()
{
    Information("Killing NUnit Agent processes...");
    StartPowershellScript("Get-Process -Name nunit-agent -ErrorAction SilentlyContinue | Stop-Process");
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
