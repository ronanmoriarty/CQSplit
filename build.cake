#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin "Cake.Powershell"
#addin "Cake.Docker"

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

Task("Run-Acceptance-Tests")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    RunCafeAcceptanceTests();
});

Task("Run-Acceptance-Tests-Without-Build")
    .Does(() =>
{
    RunCafeAcceptanceTests();
});

private void RunCafeUnitTests()
{
    RunDotNetCoreUnitTests("./src/Cafe/**/*.Tests.csproj");
}

private void RunCafeAcceptanceTests()
{
    RunDotNetCoreUnitTests("./src/Cafe/**/*.AcceptanceTests.csproj");
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

Task("Clean-CQSplit")
    .Does(() =>
{
    var cleanDirectoriesSearchPattern = "./src/CQSplit/**/bin/" + configuration;
    Information("CleanDirectories at: " + cleanDirectoriesSearchPattern);
    CleanDirectories(cleanDirectoriesSearchPattern);
});

Task("Restore-CQSplit-NuGet-Packages")
    .IsDependentOn("Clean-CQSplit")
    .Does(() =>
{
    DotNetCoreRestore("./src/CQSplit/CQSplit.sln");
});

Task("Build-CQSplit-Docker-Images")
    .Does(() =>
{
    DockerComposeBuild(new DockerComposeBuildSettings{Files = new []{"./src/CQSplit/docker-compose.yml"}});
});

Task("Start-CQSplit-Docker-Containers")
    .IsDependentOn("Build-CQSplit-Docker-Images")
    .Does(() =>
{
    DockerComposeUp(new DockerComposeUpSettings
    {
        Files = new []
        {
            "./src/CQSplit/docker-compose.yml"
        },
        DetachedMode = true
    });
});

Task("Update-CQSplit-Settings")
    .IsDependentOn("Start-CQSplit-Docker-Containers")
    .Does(() =>
{
    StartPowershellScript("./src/CQSplit/PowerShell/Update-Settings-To-Use-Docker-Containers.ps1");
});

Task("Stop-CQSplit-Docker-Containers")
    .Does(() =>
{
    StopDockerContainers();
});

private void StopDockerContainers()
{
    DockerComposeDown(new DockerComposeDownSettings
    {
        Files = new []
        {
            "./src/CQSplit/docker-compose.yml"
        }
    });
}

Task("Build-CQSplit")
    .IsDependentOn("Restore-CQSplit-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./src/CQSplit/CQSplit.sln", settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild("./src/CQSplit/CQSplit.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-CQSplit-Unit-Tests")
    .IsDependentOn("Build-CQSplit")
    .Does(() =>
{
    RunDotNetCoreUnitTests("./src/CQSplit/**/*.Tests.csproj");
});

Task("Run-CQSplit-Tests")
    .IsDependentOn("Update-CQSplit-Settings")
    .IsDependentOn("Build-CQSplit")
    .IsDependentOn("Run-CQSplit-Unit-Tests")
    .Does(() =>
{
    RunDotNetCoreUnitTests("./src/CQSplit/**/*.IntegrationTests.csproj");
    RunDotNetCoreUnitTests("./src/CQSplit/**/*.AcceptanceTests.csproj");
})
.Finally(() => {
    StopDockerContainers();
});

Task("Run-CQSplit-Unit-Tests-Without-Build")
    .Does(() =>
{
    RunDotNetCoreUnitTests("./src/CQSplit/**/*Tests.csproj");
});

Task("Create-CQSplit-Nuget-Packages")
    .IsDependentOn("Run-CQSplit-Unit-Tests")
    .Does(() =>
{
    var nuGetPackSettings = new NuGetPackSettings {
        OutputDirectory = "./src/.nuget.local"
    };

    var testProjects = GetFiles("./src/CQSplit/**/*.nuspec");
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
