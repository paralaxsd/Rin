$env:NODE_OPTIONS = "--openssl-legacy-provider"

Write-Host "==> Rin.Frontend: build" -ForegroundColor Cyan
Push-Location src/Rin.Frontend
yarn build
if ($LASTEXITCODE -ne 0) { Pop-Location; exit $LASTEXITCODE }

Write-Host "==> Rin.Frontend: pack -> ../Rin/Resources.zip" -ForegroundColor Cyan
yarn run pack
if ($LASTEXITCODE -ne 0) { Pop-Location; exit $LASTEXITCODE }
Pop-Location

Write-Host "==> Rin.Mvc.Frontend: build" -ForegroundColor Cyan
Push-Location src/Rin.Mvc.Frontend
yarn build
if ($LASTEXITCODE -ne 0) { Pop-Location; exit $LASTEXITCODE }
Pop-Location

Write-Host "==> Copy Rin.Mvc.Frontend -> Rin.Mvc/EmbeddedResources" -ForegroundColor Cyan
New-Item -ItemType Directory -Force src/Rin.Mvc/EmbeddedResources | Out-Null
Copy-Item src/Rin.Mvc.Frontend/dist/static/* src/Rin.Mvc/EmbeddedResources/ -Force

Write-Host "==> Done. Run 'dotnet build' next." -ForegroundColor Green
