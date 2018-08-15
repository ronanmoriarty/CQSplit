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

Task("Build-Sample-Application-Docker-Images")
    .Does(() =>
{
    DockerComposeBuild(new DockerComposeBuildSettings{Files = new []{"./docker-compose.yml"}});
});

Task("Start-Sample-Application-Docker-Containers")
    .IsDependentOn("Build-Sample-Application-Docker-Images")
    .Does(() =>
{
    DockerComposeUp(new DockerComposeUpSettings
    {
        Files = new []
        {
            "./docker-compose.yml"
        },
        DetachedMode = true
    });
});

Task("Update-Sample-Application-Settings")
    .IsDependentOn("Start-Sample-Application-Docker-Containers")
    .Does(() =>
{
    StartPowershellScript("./Update-Settings-To-Use-Docker-Containers.ps1");
});

Task("Stop-Sample-Application-Docker-Containers")
    .Does(() =>
{
    StopSampleApplicationDockerContainers();
});

private void StopSampleApplicationDockerContainers()
{
    StopDockerContainers("./docker-compose.yml");
}

Task("Clean-Sample-Application")
    .Does(() =>
{
    var cleanDirectoriesSearchPattern = "./src/Cafe/**/bin/" + configuration;
    Information("CleanDirectories at: " + cleanDirectoriesSearchPattern);
    CleanDirectories(cleanDirectoriesSearchPattern);
});

Task("Restore-Sample-Application-NuGet-Packages")
    .IsDependentOn("Clean-Sample-Application")
    .Does(() =>
{
    DotNetCoreRestore("./src/Cafe/Cafe.sln");
});

Task("Build-Sample-Application")
    .IsDependentOn("Restore-Sample-Application-NuGet-Packages")
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

Task("Run-Sample-Application-Unit-Tests")
    .IsDependentOn("Build-Sample-Application")
    .Does(() =>
{
    RunSampleApplicationUnitTests();
});

Task("Run-Sample-Application-Tests")
    .IsDependentOn("Run-Sample-Application-Unit-Tests")
    .Does(() =>
{
    RunSampleApplicationAcceptanceTests();
});

Task("Run-Sample-Application-Unit-Tests-Without-Build")
    .Does(() =>
{
    RunSampleApplicationUnitTests();
});

Task("Run-Sample-Application-Tests-Without-Build")
    .IsDependentOn("Run-Sample-Application-Unit-Tests-Without-Build")
    .Does(() =>
{
    RunSampleApplicationAcceptanceTests();
});

private void RunSampleApplicationUnitTests()
{
    RunDotNetCoreUnitTests("./src/Cafe/**/*.Tests.csproj");
}

private void RunSampleApplicationAcceptanceTests()
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
    StopCQSplitDockerContainers();
});

private void StopCQSplitDockerContainers()
{
    StopDockerContainers("./src/CQSplit/docker-compose.yml");
}

private void StopDockerContainers(string dockerComposePath)
{
    DockerComposeDown(new DockerComposeDownSettings
    {
        Files = new []
        {
            dockerComposePath
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

void RunCQSplitUnitTests()
{
    RunDotNetCoreUnitTests("./src/CQSplit/**/*.Tests.csproj");
}

void RunCQSplitIntegrationTests()
{
    RunDotNetCoreUnitTests("./src/CQSplit/**/*.IntegrationTests.csproj");
}

void RunCQSplitAcceptanceTests()
{
    RunDotNetCoreUnitTests("./src/CQSplit/**/*.AcceptanceTests.csproj");
}

Task("Run-CQSplit-Unit-Tests")
    .IsDependentOn("Build-CQSplit")
    .Does(() =>
{
    RunCQSplitUnitTests();
});

Task("Run-CQSplit-Tests")
    .IsDependentOn("Update-CQSplit-Settings")
    .IsDependentOn("Run-CQSplit-Unit-Tests")
    .Does(() =>
{
    RunCQSplitIntegrationTests();
    RunCQSplitAcceptanceTests();
})
.Finally(() => {
    StopCQSplitDockerContainers();
});

Task("Run-CQSplit-Unit-Tests-Without-Build")
    .Does(() =>
{
    RunCQSplitUnitTests();
});

Task("Create-CQSplit-Nuget-Packages")
    .IsDependentOn("Run-CQSplit-Tests")
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
    .IsDependentOn("Run-Sample-Application-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
