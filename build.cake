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
    RunNUnitTests("./src/Cafe/**/bin/" + configuration + "/net461/*.Tests.dll");
    DotNetCoreTest("./src/Cafe/Cafe.Waiter.Web.Tests/Cafe.Waiter.Web.Tests.csproj");
    KillNUnitAgentProcesses();
}

Task("Clean-CQRS")
    .Does(() =>
{
    var cleanDirectoriesSearchPattern = "./src/CQRS/**/bin/" + configuration;
    Information("CleanDirectories at: " + cleanDirectoriesSearchPattern);
    CleanDirectories(cleanDirectoriesSearchPattern);
});

Task("Restore-NuGet-Packages-CQRS")
    .IsDependentOn("Clean-CQRS")
    .Does(() =>
{
    NuGetRestore("./src/CQRS/CQRS.sln");
});

Task("Build-CQRS")
    .IsDependentOn("Restore-NuGet-Packages-CQRS")
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

Task("Run-Unit-Tests-CQRS")
    .IsDependentOn("Build-CQRS")
    .Does(() =>
{
    RunNUnitTests("./src/CQRS/**/bin/" + configuration + "/net461/*.Tests.dll");
});

Task("Create-Nuget-Packages-CQRS")
    .IsDependentOn("Run-Unit-Tests-CQRS")
    .Does(() =>
{
    var nuGetPackSettings = new NuGetPackSettings {
        OutputDirectory = "C:\\.nuget.local"
    };
    NuGetPack("./src/CQRS/CQRSTutorial.Core/CQRSTutorial.Core.nuspec", nuGetPackSettings);
    NuGetPack("./src/CQRS/CQRSTutorial.Messaging/CQRSTutorial.Messaging.nuspec", nuGetPackSettings);
    NuGetPack("./src/CQRS/CQRSTutorial.Messaging.Tests.Common/CQRSTutorial.Messaging.Tests.Common.nuspec", nuGetPackSettings);
    NuGetPack("./src/CQRS/CQRSTutorial.Messaging.RabbitMq/CQRSTutorial.Messaging.RabbitMq.nuspec", nuGetPackSettings);
    NuGetPack("./src/CQRS/CQRSTutorial.DAL/CQRSTutorial.DAL.nuspec", nuGetPackSettings);
    NuGetPack("./src/CQRS/CQRSTutorial.DAL.Common/CQRSTutorial.DAL.Common.nuspec", nuGetPackSettings);
    NuGetPack("./src/CQRS/CQRSTutorial.DAL.Tests.Common/CQRSTutorial.DAL.Tests.Common.nuspec", nuGetPackSettings);
    NuGetPack("./src/CQRS/CQRSTutorial.Publish/CQRSTutorial.Publish.nuspec", nuGetPackSettings);
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
