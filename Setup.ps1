[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
    [SecureString] $saPassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $waiterWebsitePassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $commandServicePassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $eventProjectingServicePassword
)

function ConvertToPlainText([SecureString]$secureString){
    $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureString)
    return [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
}

function GetFullPath($relativePath){
    return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $relativePath))
}

function CreateEnvFile()
{
    Write-Output "sa_password=$saPasswordPlainText" | Out-File -encoding ASCII .env
    Write-Output "waiterWebsitePassword='$waiterWebsitePasswordPlainText'" | Out-File -encoding ASCII -Append .env
    Write-Output "commandServicePassword='$commandServicePasswordPlainText'" | Out-File -encoding ASCII -Append .env
    Write-Output "eventProjectingServicePassword='$eventProjectingServicePasswordPlainText'" | Out-File -encoding ASCII -Append .env
    Write-Output "Created $(GetFullPath .env)"
    Write-Output "sa_password=$saPasswordPlainText" | Out-File -encoding ASCII .\src\CQRS\.env
    Write-Output "commandServicePassword='$commandServicePasswordPlainText'" | Out-File -encoding ASCII -Append .\src\CQRS\.env
    Write-Output "Created $(GetFullPath .\src\CQRS\.env)"
}

$saPasswordPlainText = ConvertToPlainText $saPassword
$waiterWebsitePasswordPlainText = ConvertToPlainText $waiterWebsitePassword
$commandServicePasswordPlainText = ConvertToPlainText $commandServicePassword
$eventProjectingServicePasswordPlainText = ConvertToPlainText $eventProjectingServicePassword

CreateEnvFile

function GetExampleFileWithPlaceholdersReplaced([string] $filePath, [hashtable] $keyValuePairs)
{
    $temp = (Get-Content $filePath)

    $keyValuePairs.Keys | ForEach-Object {
        $value = $keyValuePairs[$_]
        $temp = $temp.Replace($_, $value)
    }

    return $temp
}

function SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles([string] $path, [string] $sourceName, [string] $targetName, [hashtable] $keyValuePairs)
{
    Get-ChildItem -Path $path -Filter "$sourceName" -Recurse | ForEach-Object {
        $sourcePath = $_.FullName
        $targetJsonPath = $sourcePath.Replace($sourceName, $targetName)
        if(Test-Path $targetJsonPath)
        {
            Remove-Item $targetJsonPath
        }

        (GetExampleFileWithPlaceholdersReplaced $sourcePath $keyValuePairs) | Set-Content $targetJsonPath
        Write-Output "Created $targetJsonPath"
        Write-Output (Get-Content $targetJsonPath)
    }
}

function GetKeyValuePairs()
{
    $keyValuePairs = @{}
    $keyValuePairs.Add("`$rabbitMqPassword", "guest")
    $keyValuePairs.Add("`$waiterWebsitePassword", "$waiterWebsitePasswordPlainText")
    $keyValuePairs.Add("`$commandServicePassword", "$commandServicePasswordPlainText")
    $keyValuePairs.Add("`$eventProjectingServicePassword", "$eventProjectingServicePasswordPlainText")
    return $keyValuePairs
}

$keyValuePairs = GetKeyValuePairs
SwapPlaceholdersInExampleFilesToCreateNewDockerJsonFiles .\src\Cafe\ appSettings.docker.json.example appSettings.docker.json $keyValuePairs
