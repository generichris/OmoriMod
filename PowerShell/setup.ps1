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


$dotnetCommand = Get-Command dotnet `
    -CommandType Application `
    -ErrorAction SilentlyContinue |
    Select-Object -First 1

if ($null -eq $dotnetCommand) {
    Write-Warning ".NET SDK was not found. Install it and run setup.ps1 again."
} else {
    $dotnetLocation = $dotnetCommand.Source

    [System.Environment]::SetEnvironmentVariable(
        "DOTNETLOCATION",
        $dotnetLocation,
        "User"
    )

    Write-Host "DOTNETLOCATION set to: $dotnetLocation"
}

& $dotnetLocation tool restore