$ErrorActionPreference = 'Stop'
$appName = "Snippy"

if ($env:APPVEYOR_REPO_TAG -eq 'true') {
  Write-Host "Publishing to the PowerShell Gallery..."
  Publish-Module -NuGetApiKey $env:psgallery -Path ".\publish\$appName"
  Write-Host "Package successfully published to the PowerShell Gallery."
} else {
  Write-Host "No tags pushed. Skipping deployment."
}