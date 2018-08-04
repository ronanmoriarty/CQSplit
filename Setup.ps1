[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
    [SecureString] $saPassword,
    [Parameter(Mandatory=$True)]
    [string] $rabbitMqUserName,
    [Parameter(Mandatory=$True)]
    [SecureString] $rabbitMqPassword,
    [Parameter(Mandatory=$True)]
    [string] $writeModelUserName,
    [Parameter(Mandatory=$True)]
    [SecureString] $writeModelPassword,
    [Parameter(Mandatory=$True)]
    [string] $readModelUserName,
    [Parameter(Mandatory=$True)]
    [SecureString] $readModelPassword
)

function ConvertToPlainText([SecureString]$secureString){
    $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureString)
    return [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
}

function GetFullPath($relativePath){
    return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $relativePath))
}

function CreateEnvFile([SecureString] $secureStringPassword)
{
    $password = ConvertToPlainText $secureStringPassword
    Write-Output "sa_password=$password" | Out-File -encoding ASCII .env
    Write-Output "Created $(GetFullPath .env)"
}

CreateEnvFile $saPassword

function GetExampleFileWithPlaceholdersReplaced($filePath)
{
    $temp = (Get-Content $filePath).Replace("`$rabbitMqUserName", "$rabbitMqUserName")
    $temp = $temp.Replace("`$rabbitMqPassword", "$(ConvertToPlainText $rabbitMqPassword)")
    $temp = $temp.Replace("`$writeModelUserName", "$writeModelUserName")
    $temp = $temp.Replace("`$writeModelPassword", "$(ConvertToPlainText $writeModelPassword)")
    $temp = $temp.Replace("`$readModelUserName", "$readModelUserName")
    return $temp.Replace("`$readModelPassword", "$(ConvertToPlainText $readModelPassword)")
}

function SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles()
{
    Get-ChildItem -Path .\src\Cafe\ -Filter *.example -Recurse | ForEach-Object {
        $exampleFile = $_.FullName
        $dockerJsonPath = $exampleFile.Replace(".example", "")
        if(Test-Path $dockerJsonPath)
        {
            Remove-Item $dockerJsonPath
        }

        (GetExampleFileWithPlaceholdersReplaced $exampleFile) | Set-Content $dockerJsonPath
        Write-Output "Created $dockerJsonPath"
    }
}

SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles
