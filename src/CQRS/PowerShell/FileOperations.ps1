function WriteToFile($path, $contents){
    Write-Output "Writing $path..."
    Write-Output "Text: $contents"
    $contents | Out-File -encoding ASCII $path
}