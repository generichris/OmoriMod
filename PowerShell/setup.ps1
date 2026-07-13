$default = "C:\Program Files (x86)\Steam\steamapps\common\tModLoader"
$path = Read-Host "Enter your tModLoader install path (press Enter to use default: $default)"

if ([string]::IsNullOrWhiteSpace($path)) {
    $path = $default
}

if (-Not (Test-Path (Join-Path $path "tModLoader.dll"))) {
    Write-Warning "tModLoader.dll not found at $path. Double-check the path."
} else {
    Write-Host "Found tModLoader.dll - path looks correct."
}

[System.Environment]::SetEnvironmentVariable("TMLSTEAMPATH", $path, "User")
Write-Host "TMLSTEAMPATH set to: $path"
Write-Host "Restart VS Code (or your terminal) for the change to take effect."