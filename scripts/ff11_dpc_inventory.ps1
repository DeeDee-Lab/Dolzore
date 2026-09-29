param(
  [Parameter(Mandatory=$true)][string]$OutputDir
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

function Redact-Path([string]$Path) {
  if ([string]::IsNullOrWhiteSpace($Path)) { return $Path }
  $p = $Path
  if ($env:USERPROFILE) { $p = $p.Replace($env:USERPROFILE, '%USERPROFILE%') }
  if ($env:LOCALAPPDATA) { $p = $p.Replace($env:LOCALAPPDATA, '%LOCALAPPDATA%') }
  if ($env:APPDATA) { $p = $p.Replace($env:APPDATA, '%APPDATA%') }
  return $p
}
function Safe-Rel([string]$Base,[string]$Full) {
  $b = [IO.Path]::GetFullPath($Base).TrimEnd('\') + '\'
  $f = [IO.Path]::GetFullPath($Full)
  if ($f.StartsWith($b,[StringComparison]::OrdinalIgnoreCase)) { return $f.Substring($b.Length).Replace('\','/') }
  return (Redact-Path $Full)
}
function Add-Candidate([System.Collections.Generic.List[string]]$List,[string]$Path) {
  if ([string]::IsNullOrWhiteSpace($Path)) { return }
  try {
    if (Test-Path -LiteralPath $Path -PathType Container) {
      $full = (Get-Item -LiteralPath $Path -Force).FullName
      if (-not $List.Contains($full)) { [void]$List.Add($full) }
    }
  } catch {}
}
function Write-JsonFile([string]$Path,$Object) {
  $Object | ConvertTo-Json -Depth 12 | Set-Content -LiteralPath $Path -Encoding UTF8
}

New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
$scanUtc = [DateTime]::UtcNow.ToString('o')
$hostName = [Environment]::MachineName
if ($hostName.ToUpperInvariant() -ne 'DPC') { throw "Refusing non-DPC host: $hostName" }

$products = @()
$uninstallRoots = @(
 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*',
 'HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*',
 'HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*'
)
foreach ($rp in $uninstallRoots) {
  try {
    foreach ($x in Get-ItemProperty $rp -ErrorAction SilentlyContinue) {
      $n = [string]$x.DisplayName
      if ($n -match '(?i)FINAL FANTASY XI|PlayOnline') {
        $products += [ordered]@{
          display_name = $n
          display_version = [string]$x.DisplayVersion
          publisher = [string]$x.Publisher
          install_location = Redact-Path ([string]$x.InstallLocation)
          registry_source = $rp
        }
      }
    }
  } catch {}
}

$candidates = [System.Collections.Generic.List[string]]::new()
foreach ($p in $products) {
  if ($p.install_location -and -not $p.install_location.Contains('%')) { Add-Candidate $candidates $p.install_location }
}
$pf86 = [Environment]::GetFolderPath('ProgramFilesX86')
$pf64 = [Environment]::GetFolderPath('ProgramFiles')
$known = @(
  (Join-Path $pf86 'PlayOnline\SquareEnix\FINAL FANTASY XI'),
  (Join-Path $pf64 'PlayOnline\SquareEnix\FINAL FANTASY XI'),
  (Join-Path $pf86 'SquareEnix\FINAL FANTASY XI'),
  (Join-Path $pf64 'SquareEnix\FINAL FANTASY XI')
)
foreach ($k in $known) { Add-Candidate $candidates $k }
foreach ($base in @((Join-Path $pf86 'PlayOnline\SquareEnix'),(Join-Path $pf64 'PlayOnline\SquareEnix'))) {
  try {
    foreach ($d in Get-ChildItem -LiteralPath $base -Directory -ErrorAction SilentlyContinue) {
      if ($d.Name -match '(?i)FINAL FANTASY XI') { Add-Candidate $candidates $d.FullName }
    }
  } catch {}
}
if ($candidates.Count -eq 0) { throw 'No FINAL FANTASY XI installation directory discovered from registry/default roots.' }

$rootScores = @()
foreach ($c in $candidates) {
  $score = 0
  foreach ($marker in @('ROM','ROM2','FFXiMain.dll','FFXi.dll','FINAL FANTASY XI Config.exe')) {
    if (Test-Path -LiteralPath (Join-Path $c $marker)) { $score += 10 }
  }
  try { $score += [Math]::Min(500,(Get-ChildItem -LiteralPath $c -File -Recurse -ErrorAction SilentlyContinue | Measure-Object).Count) } catch {}
  $rootScores += [pscustomobject]@{path=$c;score=$score}
}
$root = ($rootScores | Sort-Object score -Descending | Select-Object -First 1).path
$files = @(Get-ChildItem -LiteralPath $root -File -Recurse -Force -ErrorAction SilentlyContinue)
if ($files.Count -eq 0) { throw "FF11 root has no readable files: $root" }

$extStats = @()
foreach ($g in ($files | Group-Object { if ([string]::IsNullOrWhiteSpace($_.Extension)) {'<none>'} else {$_.Extension.ToLowerInvariant()} } | Sort-Object Count -Descending)) {
  $sum = ($g.Group | Measure-Object Length -Sum).Sum
  $extStats += [ordered]@{extension=$g.Name;count=$g.Count;bytes=[int64]$sum}
}
$topStats = @()
$topGroups = $files | Group-Object {
  $rel = Safe-Rel $root $_.FullName
  $parts = $rel -split '/'
  if ($parts.Count -gt 1) {$parts[0]} else {'<root>'}
}
foreach ($g in ($topGroups | Sort-Object Count -Descending)) {
  $sum = ($g.Group | Measure-Object Length -Sum).Sum
  $topStats += [ordered]@{segment=$g.Name;count=$g.Count;bytes=[int64]$sum}
}

$datFiles = @($files | Where-Object {$_.Extension -ieq '.dat'})
$datTopology = @()
foreach ($g in ($datFiles | Group-Object {
  $rel=Safe-Rel $root $_.FullName
  $parts=$rel -split '/'
  if ($parts.Count -ge 2) { "$($parts[0])/$($parts[1])" } elseif ($parts.Count -eq 1) { '<root>' } else { '<unknown>' }
} | Sort-Object Name)) {
  $sum = ($g.Group | Measure-Object Length -Sum).Sum
  $datTopology += [ordered]@{bucket=$g.Name;dat_count=$g.Count;bytes=[int64]$sum}
}
$datTopology | Export-Csv -LiteralPath (Join-Path $OutputDir 'dpc_dat_topology.csv') -NoTypeInformation -Encoding UTF8

$rootFiles = @()
foreach ($f in ($files | Where-Object {$_.DirectoryName -eq $root} | Sort-Object Name)) {
  $rootFiles += [ordered]@{name=$f.Name;bytes=[int64]$f.Length;last_write_utc=$f.LastWriteTimeUtc.ToString('o')}
}

$pe = @()
$peFiles = @($files | Where-Object {$_.Extension -match '^(?i)\.(exe|dll)$'})
foreach ($f in $peFiles) {
  $vi = [Diagnostics.FileVersionInfo]::GetVersionInfo($f.FullName)
  $hash = $null
  try { $hash = (Get-FileHash -LiteralPath $f.FullName -Algorithm SHA256).Hash } catch {}
  $sig = $null
  try {
    $s = Get-AuthenticodeSignature -LiteralPath $f.FullName
    $sig = [ordered]@{status=[string]$s.Status;subject=if($s.SignerCertificate){[string]$s.SignerCertificate.Subject}else{$null}}
  } catch {}
  $pe += [ordered]@{
    relative_path = Safe-Rel $root $f.FullName
    bytes = [int64]$f.Length
    last_write_utc = $f.LastWriteTimeUtc.ToString('o')
    sha256 = $hash
    file_version = $vi.FileVersion
    product_version = $vi.ProductVersion
    product_name = $vi.ProductName
    company_name = $vi.CompanyName
    original_filename = $vi.OriginalFilename
    signature = $sig
  }
}
Write-JsonFile (Join-Path $OutputDir 'dpc_pe_binaries.json') $pe

$configCandidates = @()
$profileRoots = @(
  (Join-Path $env:USERPROFILE 'Documents\My Games\FINAL FANTASY XI'),
  (Join-Path $env:USERPROFILE 'Documents\My Games\FINAL FANTASY XI\USER'),
  (Join-Path $env:USERPROFILE 'Documents\PlayOnline')
)
foreach ($cr in $profileRoots) {
  if (Test-Path -LiteralPath $cr -PathType Container) {
    $cfs = @(Get-ChildItem -LiteralPath $cr -File -Recurse -ErrorAction SilentlyContinue)
    $configCandidates += [ordered]@{
      root = Redact-Path $cr
      file_count = $cfs.Count
      total_bytes = [int64](($cfs | Measure-Object Length -Sum).Sum)
      extensions = @($cfs | Group-Object Extension | Sort-Object Count -Descending | Select-Object @{n='extension';e={$_.Name}},Count)
      files = @($cfs | Select-Object -First 200 | ForEach-Object {[ordered]@{path=(Redact-Path $_.FullName);bytes=[int64]$_.Length;last_write_utc=$_.LastWriteTimeUtc.ToString('o')}})
      content_read = $false
    }
  }
}

$manifestPath = Join-Path $OutputDir 'dpc_file_manifest.tsv'
('relative_path' + [char]9 + 'bytes' + [char]9 + 'last_write_utc') | Set-Content -LiteralPath $manifestPath -Encoding UTF8
foreach ($f in ($files | Sort-Object FullName)) {
  $rel = (Safe-Rel $root $f.FullName).Replace([char]9,' ')
  ($rel + [char]9 + [string][int64]$f.Length + [char]9 + $f.LastWriteTimeUtc.ToString('o')) | Add-Content -LiteralPath $manifestPath -Encoding UTF8
}
$manifestBytes = (Get-Item -LiteralPath $manifestPath).Length
$manifestSha = (Get-FileHash -LiteralPath $manifestPath -Algorithm SHA256).Hash
if ($manifestBytes -gt 20000000) {
  $gzPath = "$manifestPath.gz"
  $in = [IO.File]::OpenRead($manifestPath)
  try {
    $out = [IO.File]::Create($gzPath)
    try {
      $gz = New-Object IO.Compression.GzipStream($out,[IO.Compression.CompressionMode]::Compress)
      try { $in.CopyTo($gz) } finally { $gz.Dispose() }
    } finally { $out.Dispose() }
  } finally { $in.Dispose() }
  Remove-Item -LiteralPath $manifestPath -Force
  $manifestCommitted = [IO.Path]::GetFileName($gzPath)
} else {
  $manifestCommitted = [IO.Path]::GetFileName($manifestPath)
}

$totalBytes = [int64](($files | Measure-Object Length -Sum).Sum)
$inventory = [ordered]@{
  schema = 'dolzore.ff11.dpc_static_inventory.v1'
  scan_utc = $scanUtc
  host_assertion = 'DPC'
  runner_name = $env:RUNNER_NAME
  os = [ordered]@{
    caption = (Get-CimInstance Win32_OperatingSystem).Caption
    version = [Environment]::OSVersion.VersionString
    architecture = $env:PROCESSOR_ARCHITECTURE
  }
  discovery = [ordered]@{
    detected_products = $products
    candidate_roots = @($rootScores | ForEach-Object {[ordered]@{path=Redact-Path $_.path;score=$_.score}})
    selected_root = Redact-Path $root
  }
  inventory = [ordered]@{
    file_count = $files.Count
    total_bytes = $totalBytes
    dat_count = $datFiles.Count
    pe_count = $peFiles.Count
    extension_stats = $extStats
    top_level_stats = $topStats
    root_files = $rootFiles
    manifest_file = $manifestCommitted
    manifest_uncompressed_sha256 = $manifestSha
    manifest_uncompressed_bytes = [int64]$manifestBytes
  }
  user_config_metadata = $configCandidates
  privacy = [ordered]@{
    binary_bodies_copied = $false
    dat_contents_read = $false
    user_config_contents_read = $false
    credentials_read = $false
    user_profile_path_redacted = $true
  }
}
Write-JsonFile (Join-Path $OutputDir 'dpc_static_inventory.json') $inventory

$romRows = @($topStats | Where-Object {$_.segment -match '^(?i)ROM\d*$'})
$obs = New-Object System.Collections.Generic.List[string]
$obs.Add('# FF11 DPC static client observations')
$obs.Add('')
$obs.Add("Scan UTC: $scanUtc")
$obs.Add('Evidence class: OBSERVED_STATIC_CLIENT unless explicitly marked INFERENCE.')
$obs.Add('')
$obs.Add('## Installation')
$obs.Add("- Selected installation root: $(Redact-Path $root)")
$obs.Add("- Total files: $($files.Count)")
$obs.Add("- Total bytes: $totalBytes")
$obs.Add("- DAT files: $($datFiles.Count)")
$obs.Add("- PE executables/libraries: $($peFiles.Count)")
$obs.Add("- Full relative-path manifest: $manifestCommitted (uncompressed SHA-256 $manifestSha)")
$obs.Add('')
$obs.Add('## Top-level storage segments')
foreach ($r in $topStats) { $obs.Add("- $($r.segment): files=$($r.count), bytes=$($r.bytes)") }
$obs.Add('')
$obs.Add('## ROM/DAT topology')
foreach ($r in $romRows) { $obs.Add("- $($r.segment): files=$($r.count), bytes=$($r.bytes)") }
$obs.Add('- Detailed DAT bucket counts are in dpc_dat_topology.csv.')
$obs.Add('')
$obs.Add('## Binary/version evidence')
$obs.Add('- Executable and DLL SHA-256, version-resource metadata and Authenticode status are in dpc_pe_binaries.json.')
$obs.Add('- Binary bodies were not copied.')
$obs.Add('')
$obs.Add('## User configuration evidence')
if ($configCandidates.Count -eq 0) { $obs.Add('- No expected user configuration directory was found in the probed paths.') }
else {
  foreach ($c in $configCandidates) { $obs.Add("- $($c.root): files=$($c.file_count), bytes=$($c.total_bytes), content_read=false") }
}
$obs.Add('')
$obs.Add('## Interpretation boundary')
$obs.Add('- OBSERVED_STATIC_CLIENT: file topology, sizes, hashes, versions, timestamps and registry product metadata.')
$obs.Add('- NOT YET OBSERVED: runtime combat logic, packets, render call behavior, UI state transitions, timing, input latency, zone streaming behavior.')
$obs.Add('- SERVER-SIDE UNKNOWN: authoritative battle formulas, AI decision logic, loot RNG, persistence/database, anti-cheat, matchmaking/auction internals.')
$obs.Add('- DOLZORE agents must not treat static DAT presence as proof of server-side rules.')
$obs | Set-Content -LiteralPath (Join-Path $OutputDir 'dpc_static_observations.md') -Encoding UTF8

$latest = [ordered]@{
  schema='dolzore.ff11.latest_scan.v1'
  scan_utc=$scanUtc
  host='DPC'
  selected_root=Redact-Path $root
  file_count=$files.Count
  total_bytes=$totalBytes
  dat_count=$datFiles.Count
  pe_count=$peFiles.Count
  manifest_file=$manifestCommitted
  manifest_uncompressed_sha256=$manifestSha
}
Write-JsonFile (Join-Path $OutputDir 'LATEST_SCAN.json') $latest
Write-Host "FF11_SCAN_UTC=$scanUtc"
Write-Host "FF11_ROOT=$(Redact-Path $root)"
Write-Host "FF11_FILE_COUNT=$($files.Count)"
Write-Host "FF11_TOTAL_BYTES=$totalBytes"
Write-Host "FF11_DAT_COUNT=$($datFiles.Count)"
Write-Host "FF11_PE_COUNT=$($peFiles.Count)"
Write-Host "FF11_MANIFEST=$manifestCommitted"
Write-Host 'FF11_SCAN=PASS'
