#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin "Cake.Powershell"
#addin "Cake.Docker"
#addin nuget:?package=Cake.Json
#addin nuget:?package=Newtonsoft.Json&version=9.0.1

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var isCiBuild = Argument<bool>("isCiBuild", false);

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
// var buildDir = Directory("./**/bin") + Directory(configuration);
private const string AllSampleApplicationTestProjects = "./src/Cafe/**/*.Tests.csproj";
private const string AllCQSplitTestProjects = "./src/CQSplit/**/*.Tests.csproj";
private const string CafeSolutionPath = "./src/Cafe/Cafe.sln";
private const string CQSplitSolutionPath = "./src/CQSplit/CQSplit.sln";
private const string DockerComposeFilePath = "./docker-compose.yml";
private const string CQSplitDockerComposeFilePath = "./src/CQSplit/docker-compose.yml";
private const string IntegrationTestsDockerComposeFilePath = "./docker-compose.integration-tests.yml";

private DotNetCoreTestSettings OnlyUnitTests = new DotNetCoreTestSettings
    {
        ArgumentCustomization = args => args.Append("--filter TestCategory!=\"Integration\"&TestCategory!=\"Acceptance\"")
    };

private DotNetCoreTestSettings OnlyIntegrationTests = new DotNetCoreTestSettings
    {
        ArgumentCustomization = args => args.Append("--filter TestCategory=\"Integration\"")
    };

private DotNetCoreTestSettings OnlyAcceptanceTests = new DotNetCoreTestSettings
    {
        ArgumentCustomization = args => args.Append("--filter TestCategory=\"Acceptance\"")
    };

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Build-Sample-Application-Docker-Images")
    .Does(() =>
{
    DockerComposeBuild(new DockerComposeBuildSettings{Files = new []{DockerComposeFilePath}});
});

Task("Start-Sample-Application-Docker-Containers")
    .IsDependentOn("Build-Sample-Application-Docker-Images")
    .Does(() =>
{
    DockerComposeUp(new DockerComposeUpSettings
    {
        Files = new []
        {
            DockerComposeFilePath
        },
        DetachedMode = true
    });
});

Task("Build-Sample-Application-Docker-Images-For-Integration-Testing")
    .Does(() =>
{
    DockerComposeBuild(new DockerComposeBuildSettings{Files = new []{IntegrationTestsDockerComposeFilePath}});
});


Task("Start-Sample-Application-Docker-Containers-For-Integration-Testing")
    .IsDependentOn("Build-Sample-Application-Docker-Images-For-Integration-Testing")
    .Does(() =>
{
    CreateBlankTestResultsDirectory();
    FixHNSErrorInAppveyor();
    RunIntegrationTests();
    UploadTestResultsToAppveyor();
})
.Finally(() =>
{
    StopDockerContainers(IntegrationTestsDockerComposeFilePath);
});

private void CreateBlankTestResultsDirectory()
{
    var dir = "test-results";
    if (DirectoryExists(dir))
    {
        DeleteDirectory(dir, recursive: true);
    }

    CreateDirectory(dir);
}

private void FixHNSErrorInAppveyor()
{
    // See https://github.com/docker/for-win/issues/598
    // Also see https://stackoverflow.com/questions/45394360/hns-failed-with-error-the-parameter-is-incorrect
    StartPowershellScript("Get-NetNat | Remove-NetNat -Confirm");
}

private void RunIntegrationTests()
{
    DockerComposeUp(new DockerComposeUpSettings
    {
        Files = new []
        {
            IntegrationTestsDockerComposeFilePath
        },
        DetachedMode = true
    });
}

private void UploadTestResultsToAppveyor()
{
    StartPowershellScript("./Upload-Test-Results-To-Appveyor.ps1");
}

Task("Update-Sample-Application-Settings")
    .IsDependentOn("Start-Sample-Application-Docker-Containers")
    .Does(() =>
{
    if(isCiBuild)
    {
        StartPowershellScript($"./Update-Settings-To-Use-Docker-Containers.ps1 -isCiBuild");
    }
    else
    {
        StartPowershellScript($"./Update-Settings-To-Use-Docker-Containers.ps1");
    }
});

Task("Stop-Sample-Application-Docker-Containers")
    .Does(() =>
{
    StopDockerContainers(DockerComposeFilePath);
});

Task("Stop-Sample-Application-Docker-Containers-For-Integration-Testing")
    .Does(() =>
{
    StopDockerContainers(IntegrationTestsDockerComposeFilePath);
});

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
    DotNetCoreRestore(CafeSolutionPath);
});

Task("Build-Sample-Application")
    .IsDependentOn("Restore-Sample-Application-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild(CafeSolutionPath, settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild(CafeSolutionPath, settings =>
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
    .IsDependentOn("Update-Sample-Application-Settings")
    .IsDependentOn("Run-Sample-Application-Unit-Tests")
    .Does(() =>
{
    RunSampleApplicationIntegrationTests();
    RunSampleApplicationAcceptanceTests();
})
.Finally(() => {
    StopDockerContainers(DockerComposeFilePath);
});

Task("Run-Sample-Application-Unit-Tests-Without-Build")
    .Does(() =>
{
    RunSampleApplicationUnitTests();
});

Task("Run-Sample-Application-Tests-Without-Build")
    .IsDependentOn("Update-Sample-Application-Settings")
    .IsDependentOn("Run-Sample-Application-Unit-Tests-Without-Build")
    .Does(() =>
{
    RunSampleApplicationIntegrationTests();
    RunSampleApplicationAcceptanceTests();
})
.Finally(() => {
    StopDockerContainers(DockerComposeFilePath);
});

private void RunSampleApplicationUnitTests()
{
    RunDotNetTests(AllSampleApplicationTestProjects, OnlyUnitTests);
}

private void RunSampleApplicationIntegrationTests()
{
    RunDotNetTests(AllSampleApplicationTestProjects, OnlyIntegrationTests);
}

private void RunSampleApplicationAcceptanceTests()
{
    RunDotNetTests(AllSampleApplicationTestProjects, OnlyAcceptanceTests);
}

private void RunDotNetTests(string filePattern, DotNetCoreTestSettings dotNetCoreTestSettings)
{
    var testProjects = GetFiles(filePattern);
    foreach (var testProject in testProjects)
    {
        DotNetCoreTest(testProject.FullPath, dotNetCoreTestSettings);
    }

    KillNUnitAgentProcesses();
}

Task("Run-Sample-Application")
    .IsDependentOn("Update-Sample-Application-Settings")
    .Does(() =>
{
    RunSampleApplication();
});

private void RunSampleApplication()
{
    var waiterWebsiteEntryPointUrl = GetWaiterWebsiteEntryPointUrl();
    Information($"The sample application is now running at {waiterWebsiteEntryPointUrl}");
}

private string GetWaiterWebsiteEntryPointUrl()
{
    return $"{GetWaiterWebsiteAddress()}/app/index.html#!/tabs";
}

private string GetWaiterWebsiteAddress()
{
    var settings = ParseJsonFromFile("./src/Cafe/Cafe.Waiter.Acceptance.Tests/appSettings.json");
    var host = settings["cafe"]["waiter"]["web"]["url"];
    return host.ToString();
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
    DotNetCoreRestore(CQSplitSolutionPath);
});

Task("Build-CQSplit-Docker-Images")
    .Does(() =>
{
    DockerComposeBuild(new DockerComposeBuildSettings{Files = new []{CQSplitDockerComposeFilePath}});
});

Task("Start-CQSplit-Docker-Containers")
    .IsDependentOn("Build-CQSplit-Docker-Images")
    .Does(() =>
{
    DockerComposeUp(new DockerComposeUpSettings
    {
        Files = new []
        {
            CQSplitDockerComposeFilePath
        },
        DetachedMode = true
    });
});

Task("Update-CQSplit-Settings")
    .IsDependentOn("Start-CQSplit-Docker-Containers")
    .Does(() =>
{
    var command = "./src/CQSplit/PowerShell/Update-Settings-To-Use-Docker-Containers.ps1";
    if(isCiBuild)
    {
        command += " -isCiBuild";
    }

    StartPowershellScript(command);
});

Task("Stop-CQSplit-Docker-Containers")
    .Does(() =>
{
    StopDockerContainers(CQSplitDockerComposeFilePath);
});

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
      MSBuild(CQSplitSolutionPath, settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild(CQSplitSolutionPath, settings =>
        settings.SetConfiguration(configuration));
    }
});

void RunCQSplitUnitTests()
{
    RunDotNetTests(AllCQSplitTestProjects, OnlyUnitTests);
}

void RunCQSplitIntegrationTests()
{
    RunDotNetTests(AllCQSplitTestProjects, OnlyIntegrationTests);
}

void RunCQSplitAcceptanceTests()
{
    RunDotNetTests(AllCQSplitTestProjects, OnlyAcceptanceTests);
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

Task("Publish-CQSplit-Nuget-Packages")
    .Does(() =>
{
    var apiKey = Argument("NugetApiKey", string.Empty);
    StartPowershellScript("./src/CQSplit/PowerShell/Publish-CQSplit-Packages.ps1",
        processArgumentBuilder => {
            processArgumentBuilder.Append("apiKey", apiKey);
        }
    );
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
