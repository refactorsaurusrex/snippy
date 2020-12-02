param(
  [string]$Version = ''
)

$ErrorActionPreference = 'Stop'
$appName = "Snippy"
$guid = "e901963b-8083-42c2-829c-c1ed650383a6"
$description = "A low-key, cli-oriented code snippet manager"
$tags = "snippets-manager","powershell","vscode"
$licenseUri = "https://github.com/refactorsaurusrex/snippy/blob/master/LICENSE"
$projectUri = "https://github.com/refactorsaurusrex/snippy"

if ($env:APPVEYOR_REPO_TAG -eq 'true') {
  $Version = [regex]::match($env:APPVEYOR_REPO_TAG_NAME,'[0-9]+\.[0-9]+\.[0-9]+').Groups[0].Value
  $module = Find-Module -Name $appName -ErrorAction SilentlyContinue
  if ($null -eq $module) {
    Write-Host "First time publishing module. Version is $Version."
  } else {
    $lastPublishedVersion = [Version]::new(($module | Select-Object -ExpandProperty Version))
    if ([Version]::new($Version) -le $lastPublishedVersion) {
      throw "Version must be greater than the last published version, which is 'v$lastPublishedVersion'."
    }
    Write-Host "Last published version: 'v$lastPublishedVersion'. Current version: 'v$Version'"
  }
} elseif ($null -ne $env:APPVEYOR_BUILD_NUMBER) {
  $Version = "0.0.$env:APPVEYOR_BUILD_NUMBER"
} elseif ($Version -eq '') {
  $Version = "0.0.0"
}

Write-Host "Building version '$Version'..."

if (Test-Path "$PSScriptRoot\publish") {
  Remove-Item -Path "$PSScriptRoot\publish" -Recurse -Force
}

$publishOutputDir = "$PSScriptRoot\publish\$appName"
$proj = Get-ChildItem -Filter "$appName.csproj" -Recurse -Path $PSScriptRoot | Select-Object -First 1 -ExpandProperty FullName
dotnet publish $proj --output $publishOutputDir -c Release

if ($LASTEXITCODE -ne 0) {
  throw "Failed to publish application."
}

Remove-Item "$publishOutputDir\*.pdb"

Import-Module "$publishOutputDir\$appName.dll"
$moduleInfo = Get-Module $appName
Install-Module WhatsNew
Import-Module WhatsNew
$cmdletNames = Export-BinaryCmdletNames -ModuleInfo $moduleInfo
$cmdletAliases = Export-BinaryCmdletAliases -ModuleInfo $moduleInfo

$manifestPath = "$publishOutputDir\$appName.psd1"

$newManifestArgs = @{
  Path = $manifestPath
}

$updateManifestArgs = @{
  Path = $manifestPath
  CopyRight = "(c) $((Get-Date).Year) Nick Spreitzer"
  Description = $description
  Guid = $guid
  Author = 'Nick Spreitzer'
  CompanyName = 'RAWR! Productions'
  ModuleVersion = $Version
  AliasesToExport = $cmdletAliases
  NestedModules = ".\$appName.dll"
  CmdletsToExport = $cmdletNames
  CompatiblePSEditions = @("Core")
  HelpInfoUri = $projectUri
  PowerShellVersion = "6.0"
  PrivateData = @{
    Tags = $tags
    LicenseUri = $licenseUri
    ProjectUri = $projectUri
  }
}

New-ModuleManifest @newManifestArgs
Update-ModuleManifest @updateManifestArgs
Remove-ModuleManifestComments $manifestPath -NoConfirm