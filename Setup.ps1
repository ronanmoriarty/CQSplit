[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
    [SecureString] $saPassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $writeModelPassword,
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

function CreateEnvFile([SecureString] $secureStringSystemAdminPassword, [SecureString] $secureStringWriteModelPassword, [SecureString] $secureStringReadModelPassword)
{
    $systemAdminPassword = ConvertToPlainText $secureStringSystemAdminPassword
    $writePassword = ConvertToPlainText $secureStringWriteModelPassword
    $readPassword = ConvertToPlainText $secureStringReadModelPassword
    Write-Output "sa_password=$systemAdminPassword" | Out-File -encoding ASCII .env
    Write-Output "commandServicePassword='$writePassword'" | Out-File -encoding ASCII -Append .env
    Write-Output "eventProjectingServicePassword='$readPassword'" | Out-File -encoding ASCII -Append .env
    Write-Output "Created $(GetFullPath .env)"
    Write-Output "sa_password=$systemAdminPassword" | Out-File -encoding ASCII .\src\CQRS\.env
    Write-Output "commandServicePassword='$writePassword'" | Out-File -encoding ASCII -Append .\src\CQRS\.env
    Write-Output "Created $(GetFullPath .\src\CQRS\.env)"
}

CreateEnvFile $saPassword $writeModelPassword $readModelPassword

function GetExampleFileWithPlaceholdersReplaced($filePath)
{
    $temp = (Get-Content $filePath).Replace("`$rabbitMqPassword", "guest")
    $temp = $temp.Replace("`$writeModelPassword", "$(ConvertToPlainText $writeModelPassword)")
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
