<#
.SYNOPSIS
    Publica a versão atual do Project Energy no site IIS local (sem privilégios de administrador).

.DESCRIPTION
    1. Compila em Release (inclui o CSS Tailwind) para artifacts/publish.
    2. Coloca o site em manutenção com app_offline.htm (o IIS para a aplicação).
    3. Copia os ficheiros sem apagar dados do site: base de dados, logs e media nunca são tocados.
    4. Retira app_offline.htm e faz um pedido de aquecimento.

    Requer setup-iis.ps1 executado uma vez como Administrador.
        .\deploy\iis\publish-iis.ps1
#>
param(
    [string]$PhysicalPath = 'C:\inetpub\ProjectEnergy',
    [int]$Port = 8090
)

$ErrorActionPreference = 'Stop'
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..\..')
$project = Join-Path $repoRoot 'src\ProjectEnergy.Web\ProjectEnergy.Web.csproj'
$output = Join-Path $repoRoot 'artifacts\publish'

if (-not (Test-Path (Join-Path $PhysicalPath 'umbraco\Data'))) {
    throw "Site IIS não configurado em $PhysicalPath. Execute primeiro setup-iis.ps1 como Administrador."
}

Write-Host '> A compilar (Release)...'
if (Test-Path $output) { Remove-Item $output -Recurse -Force }
& dotnet publish $project -c Release -o $output --nologo -v quiet
if ($LASTEXITCODE -ne 0) { throw 'dotnet publish falhou.' }

$offline = Join-Path $PhysicalPath 'app_offline.htm'
Set-Content -Path $offline -Encoding utf8 -Value '<!doctype html><meta charset="utf-8"><title>Project Energy</title><p style="font-family:sans-serif">A atualizar o website. Volte a tentar dentro de alguns segundos.</p>'
Start-Sleep -Seconds 3

try {
    Write-Host '> A copiar ficheiros...'
    # /E sem purga: ficheiros existentes no site (BD, logs, media) nunca são apagados.
    & robocopy $output $PhysicalPath /E /R:3 /W:2 /NFL /NDL /NJH /NJS /NP `
        /XD (Join-Path $output 'umbraco\Data') (Join-Path $output 'umbraco\Logs') (Join-Path $output 'wwwroot\media') `
        /XF app_offline.htm | Out-Null
    if ($LASTEXITCODE -ge 8) { throw "robocopy falhou (código $LASTEXITCODE)." }
}
finally {
    Remove-Item $offline -Force -ErrorAction SilentlyContinue
}

Write-Host '> A arrancar...'
$url = "http://127.0.0.1:$Port/pt/"
for ($i = 0; $i -lt 30; $i++) {
    try {
        $response = Invoke-WebRequest $url -UseBasicParsing -TimeoutSec 30
        Write-Host "Publicado: $url ($($response.StatusCode))"
        exit 0
    }
    catch { Start-Sleep -Seconds 2 }
}
throw "O site não respondeu em $url. Veja os logs em $PhysicalPath\umbraco\Logs."
