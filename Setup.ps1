[CmdletBinding()]
param (
    [Parameter(Mandatory=$True)]
    [string] $rabbitMqUserName,
    [Parameter(Mandatory=$True)]
    [SecureString] $rabbitMqPassword,
    [Parameter(Mandatory=$True)]
    [SecureString] $saPassword
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
    Write-Output "sa_password=$password" > .env
    Write-Output "Created $(GetFullPath .env)"
}

CreateEnvFile $saPassword
