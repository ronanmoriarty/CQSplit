. .\src\CQSplit\PowerShell\Docker.ps1

function WaitUntilTestContainersAreFinished()
{
    $testContainerImageNames = @(
        "cqsplit_cafe-waiter-command-service-tests",
        "cqsplit_cafe-waiter-event-projecting-service-tests",
        "cqsplit_cafe-waiter-web-tests",
        "cqsplit_cafe-waiter-acceptance-tests"
    )

    $atLeastOneTestContainerStillRunning = $true
    while($atLeastOneTestContainerStillRunning)
    {
        $atLeastOneTestContainerStillRunning = $false
        $testContainerImageNames | ForEach-Object {
            $testContainerId = GetContainerRunningWithImageName $_
            if(-not $testContainerId -eq $null)
            {
                $atLeastOneTestContainerStillRunning = $true
                Write-Host "Test container $testContainerId still running using image $_. Will wait 5 seconds and try again."
                Start-Sleep -Seconds 5
            }
        }
    }
}

function UploadTestResults()
{
    $testResultFiles = @(
        "cafe.waiter.command.service.tests.trx",
        "cafe.waiter.eventprojecting.service.tests.trx",
        "cafe.waiter.web.tests.trx",
        "cafe.waiter.acceptance.tests.trx"
    )

    $testResultFiles | ForEach-Object {
        if ("${ENV:APPVEYOR_JOB_ID}" -ne "") {
            Write-Host "Uploading $_ to Appveyor..."
            $wc = New-Object 'System.Net.WebClient'
            $wc.UploadFile("https://ci.appveyor.com/api/testresults/mstest/$($env:APPVEYOR_JOB_ID)", (Resolve-Path .\test-results\$_))
        }
    }
}

WaitUntilTestContainersAreFinished
UploadTestResults
