# Build Local Test NuGet Packages Script
# This script builds local NuGet packages with a test version for testing the RadRequestBuilder fixes

param(
    [string]$PackageVersion = "1.0.1-test-$(Get-Date -Format 'yyyyMMdd-HHmmss')",
    [string]$AssemblyVersion = "1.0.1.0",
    [string]$OutputPath = ".\local-packages"
)

Write-Host "Building local test NuGet packages..." -ForegroundColor Green
Write-Host "Package Version: $PackageVersion" -ForegroundColor Yellow
Write-Host "Assembly Version: $AssemblyVersion" -ForegroundColor Yellow
Write-Host "Output Path: $OutputPath" -ForegroundColor Yellow

# Create output directory if it doesn't exist
if (!(Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Path $OutputPath -Force
    Write-Host "Created output directory: $OutputPath" -ForegroundColor Blue
}

# Clean previous builds
Write-Host "`nCleaning previous builds..." -ForegroundColor Yellow
dotnet clean --configuration Release

# Build RadEndpoints package first (since RadEndpoints.Testing depends on it)
Write-Host "`nBuilding RadEndpoints package..." -ForegroundColor Yellow
dotnet pack .\RadEndpoints\RadEndpoints.csproj `
    --configuration Release `
    --output $OutputPath `
    --property:PackageVersion=$PackageVersion `
    --property:AssemblyVersion=$AssemblyVersion `
    --property:FileVersion=$AssemblyVersion `
    --verbosity normal

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to build RadEndpoints package"
    exit 1
}

# Build RadEndpoints.Testing package
Write-Host "`nBuilding RadEndpoints.Testing package..." -ForegroundColor Yellow
dotnet pack .\RadEndpoints.Testing\RadEndpoints.Testing.csproj `
    --configuration Release `
    --output $OutputPath `
    --property:PackageVersion=$PackageVersion `
    --property:AssemblyVersion=$AssemblyVersion `
    --property:FileVersion=$AssemblyVersion `
    --verbosity normal

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to build RadEndpoints.Testing package"
    exit 1
}

Write-Host "`nPackages built successfully!" -ForegroundColor Green
Write-Host "Location: $OutputPath" -ForegroundColor Blue

# List the created packages
Write-Host "`nCreated packages:" -ForegroundColor Blue
Get-ChildItem -Path $OutputPath -Filter "*.nupkg" | ForEach-Object {
    Write-Host "  - $($_.Name)" -ForegroundColor Cyan
}

Write-Host "`nTo use these packages in another solution:" -ForegroundColor Yellow
Write-Host "1. Add a local package source:" -ForegroundColor White
Write-Host "   dotnet nuget add source `"$(Resolve-Path $OutputPath)`" --name `"RadEndpoints-Local-Test`"" -ForegroundColor Gray
Write-Host "`n2. In your test project, update package references:" -ForegroundColor White
Write-Host "   <PackageReference Include=`"RadEndpoints`" Version=`"$PackageVersion`" />" -ForegroundColor Gray
Write-Host "   <PackageReference Include=`"RadEndpoints.Testing`" Version=`"$PackageVersion`" />" -ForegroundColor Gray
Write-Host "`n3. Restore packages:" -ForegroundColor White
Write-Host "   dotnet restore" -ForegroundColor Gray

Write-Host "`nTo remove the local package source later:" -ForegroundColor Yellow
Write-Host "   dotnet nuget remove source `"RadEndpoints-Local-Test`"" -ForegroundColor Gray