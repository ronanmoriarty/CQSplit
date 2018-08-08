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