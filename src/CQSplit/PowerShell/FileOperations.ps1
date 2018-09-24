function GetFullPath($relativePath){
    return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PWD, $relativePath))
}

function ConvertToPlainText([SecureString]$secureString){
    $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureString)
    return [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
}

function WriteToFile($path, $contents){
    Write-Output "Writing $path..."
    Write-Output "Text: $contents"
    $contents | Out-File -encoding ASCII $path
}

function GetAppSettingsTemplateFiles()
{
    $cqRoot = GetFullPath "$PSScriptRoot\..\..\..\"
    return (Get-ChildItem -Path $cqRoot -Filter appSettings.json.template -Recurse) | Select-Object -ExpandProperty FullName
}

function GetTemplateFileWithPlaceholdersReplaced([string] $templateFilePath, [hashtable] $keyValuePairs)
{
    $temp = (Get-Content $templateFilePath)

    $keyValuePairs.Keys | ForEach-Object {
        $value = $keyValuePairs[$_]
        $temp = $temp.Replace($_, $value)
    }

    return $temp
}

function SwapPlaceholdersToCreateNewJsonFiles([string[]] $jsonTemplatePaths, [string] $targetName, [hashtable] $keyValuePairs, [bool] $IsCiBuild)
{
    if($jsonTemplatePaths.Length -eq 0)
    {
        Write-Output "No template files supplied."
        return
    }

    $jsonTemplatePaths | ForEach-Object {
        $sourcePath = $_
        $sourceName = [System.IO.Path]::GetFileName($sourcePath)
        Write-Output "Replacing placeholders in '$sourcePath' to create new file called '$targetName' in same directory."
        $targetJsonPath = $sourcePath.Replace($sourceName, $targetName)
        if(Test-Path $targetJsonPath)
        {
            Remove-Item $targetJsonPath
        }

        (GetTemplateFileWithPlaceholdersReplaced $sourcePath $keyValuePairs) | Set-Content $targetJsonPath
        if(-not $IsCiBuild)
        {
            Write-Output "Created $targetJsonPath"
            Write-Output (Get-Content $targetJsonPath)
        }
    }
}