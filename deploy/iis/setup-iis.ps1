#Requires -RunAsAdministrator
<#
.SYNOPSIS
    Configura (uma vez) o site de pré-visualização Project Energy no IIS local.

.DESCRIPTION
    Cria apenas o application pool e o site do Project Energy; não altera outros sites
    (ex.: Extratos.Web na porta 8080 ou o Default Web Site na porta 80).
    - Binding só em 127.0.0.1:8090 (não fica exposto na rede local; o acesso externo é feito pelo túnel).
    - Application pool sem código gerido, sempre ativo e sem timeout por inatividade.
    - Ambiente "Preview" (appsettings.Preview.json: SQLite). A palavra-passe de pré-visualização é lida
      dos user-secrets e guardada como variável de ambiente do pool (fora do repositório e do site).
    - O utilizador que publica recebe permissão de escrita na pasta, para que publish-iis.ps1
      corra sem privilégios de administrador.

    Executar numa PowerShell aberta como Administrador, a partir da raiz do repositório:
        .\deploy\iis\setup-iis.ps1
#>
param(
    [string]$SiteName = 'ProjectEnergy',
    [string]$AppPoolName = 'ProjectEnergyPool',
    [int]$Port = 8090,
    [string]$PhysicalPath = 'C:\inetpub\ProjectEnergy',
    # Utilizador que corre publish-iis.ps1 (por omissão, quem executa este script).
    [string]$DeployUser = "$env:USERDOMAIN\$env:USERNAME",
    # Base de dados de desenvolvimento a copiar se o site ainda não tiver uma.
    [string]$SeedDatabase = (Join-Path $PSScriptRoot '..\..\src\ProjectEnergy.Web\umbraco\Data\Umbraco.sqlite.db')
)

$ErrorActionPreference = 'Stop'
$appCmdPath = Join-Path $env:windir 'System32\inetsrv\appcmd.exe'
$projectDir = Resolve-Path (Join-Path $PSScriptRoot '..\..\src\ProjectEnergy.Web')

# Executa appcmd sem que mensagens no stderr interrompam o script; falha se o código de saída indicar erro.
function Invoke-AppCmd([string[]]$Arguments, [switch]$AllowFailure) {
    $previous = $ErrorActionPreference
    $ErrorActionPreference = 'Continue'
    try { $output = & $appCmdPath @Arguments 2>&1 } finally { $ErrorActionPreference = $previous }
    if ($LASTEXITCODE -ne 0 -and -not $AllowFailure) {
        throw "appcmd $($Arguments -join ' ') falhou: $output"
    }
    return $output
}

# 1. Pré-requisitos
$ancm = Join-Path $env:ProgramFiles 'IIS\Asp.Net Core Module\V2\aspnetcorev2.dll'
if (-not (Test-Path $ancm)) {
    throw 'ASP.NET Core Module não encontrado. Instale o ASP.NET Core 10 Hosting Bundle.'
}
$ancmVersion = [version](Get-Item $ancm).VersionInfo.FileVersion
if ($ancmVersion.Major -lt 20) {
    Write-Warning "ASP.NET Core Module $ancmVersion é anterior ao .NET 10. Recomenda-se instalar o ASP.NET Core 10 Hosting Bundle."
}

$siteExists = [bool](Invoke-AppCmd @('list', 'site', "/name:$SiteName") -AllowFailure | Where-Object { $_ -like 'SITE *' })
$poolExists = [bool](Invoke-AppCmd @('list', 'apppool', "/name:$AppPoolName") -AllowFailure | Where-Object { $_ -like 'APPPOOL *' })

if (-not $siteExists -and (Get-NetTCPConnection -State Listen -LocalPort $Port -ErrorAction SilentlyContinue)) {
    throw "A porta $Port já está em uso por outro processo. Escolha outra com -Port."
}

$previewPassword = (& dotnet user-secrets list --project $projectDir |
    Where-Object { $_ -like 'Preview:Password = *' }) -replace '^Preview:Password = ', ''
if (-not $previewPassword) {
    throw 'Preview:Password não está definida nos user-secrets. Defina-a: dotnet user-secrets set "Preview:Password" "<valor>"'
}
if ($previewPassword -match "['""]") {
    throw 'A palavra-passe de pré-visualização não pode conter aspas.'
}

# 2. Pastas e base de dados inicial
foreach ($dir in @($PhysicalPath, "$PhysicalPath\umbraco\Data", "$PhysicalPath\umbraco\Logs", "$PhysicalPath\wwwroot\media")) {
    New-Item -ItemType Directory -Force -Path $dir | Out-Null
}

$targetDb = Join-Path $PhysicalPath 'umbraco\Data\Umbraco.sqlite.db'
if (-not (Test-Path $targetDb)) {
    if (-not (Test-Path $SeedDatabase)) {
        throw "Base de dados inicial não encontrada em $SeedDatabase. Execute o site uma vez com 'dotnet run' para a criar."
    }
    if (Get-Process -Name 'ProjectEnergy.Web' -ErrorAction SilentlyContinue) {
        throw "Pare o 'dotnet run' do ProjectEnergy.Web antes de copiar a base de dados."
    }
    Copy-Item $SeedDatabase $targetDb
    Write-Host "Base de dados copiada para $targetDb"
}

# 3. Application pool (sempre ativo, sem código gerido)
if (-not $poolExists) {
    Invoke-AppCmd @('add', 'apppool', "/name:$AppPoolName") | Out-Null
}
Invoke-AppCmd @('set', 'apppool', $AppPoolName, '/managedRuntimeVersion:', '/managedPipelineMode:Integrated',
    '/startMode:AlwaysRunning', '/processModel.loadUserProfile:true', '/processModel.idleTimeout:00:00:00',
    '/recycling.periodicRestart.time:00:00:00') | Out-Null

$environment = [ordered]@{
    'ASPNETCORE_ENVIRONMENT' = 'Preview'
    'Preview__Password'      = $previewPassword
}
foreach ($name in $environment.Keys) {
    Invoke-AppCmd @('set', 'config', '-section:system.applicationHost/applicationPools',
        "/-[name='$AppPoolName'].environmentVariables.[name='$name']", '/commit:apphost') -AllowFailure | Out-Null
    Invoke-AppCmd @('set', 'config', '-section:system.applicationHost/applicationPools',
        "/+[name='$AppPoolName'].environmentVariables.[name='$name',value='$($environment[$name])']", '/commit:apphost') | Out-Null
}

# 4. Site (binding apenas em loopback)
$binding = "http/127.0.0.1:${Port}:"
if (-not $siteExists) {
    Invoke-AppCmd @('add', 'site', "/name:$SiteName", "/bindings:$binding", "/physicalPath:$PhysicalPath") | Out-Null
} else {
    Invoke-AppCmd @('set', 'site', "/site.name:$SiteName", "/bindings:$binding") | Out-Null
    Invoke-AppCmd @('set', 'vdir', "$SiteName/", "/physicalPath:$PhysicalPath") | Out-Null
}
Invoke-AppCmd @('set', 'app', "$SiteName/", "/applicationPool:$AppPoolName") | Out-Null

# 5. Permissões: o pool escreve (BD, logs, media, templates); o utilizador de deploy publica.
& icacls $PhysicalPath /grant "IIS AppPool\${AppPoolName}:(OI)(CI)M" /T /C /Q | Out-Null
& icacls $PhysicalPath /grant "${DeployUser}:(OI)(CI)M" /T /C /Q | Out-Null

Invoke-AppCmd @('recycle', 'apppool', "/apppool.name:$AppPoolName") -AllowFailure | Out-Null
Invoke-AppCmd @('start', 'site', "/site.name:$SiteName") -AllowFailure | Out-Null

Write-Host ''
Write-Host "Site '$SiteName' configurado em http://127.0.0.1:$Port (pasta $PhysicalPath)."
Write-Host 'Publique a aplicação com: .\deploy\iis\publish-iis.ps1 (sem privilégios de administrador).'
