# Filmaholic

Filmaholic is a movie catalog with a .NET MAUI Blazor client, an ASP.NET Core minimal API, and shared request/response DTOs.

## Projects

- `Filmaholic.Api` provides authentication and the protected movie API.
- `Filmaholic.App` is the MAUI Blazor client for Windows, Android, iOS, and Mac Catalyst.
- `Filmaholic.Shared` contains DTOs shared between the API and app.
- `Filmaholic.Api.Tests` contains API unit tests.

## Requirements

- .NET 10 SDK
- .NET MAUI workload for the client targets you intend to build
- Visual Studio 2022 with the MAUI workload on Windows for Windows/Android development

Install the MAUI workload if needed:

```powershell
dotnet workload install maui
```

## Configure And Run The API

The API uses SQLite by default. The development database is `Filmaholic.Api/movies.db`.
The JWT signing key is not stored in `appsettings.json`. A local development key is stored in .NET User Secrets, outside the repository. To create your own key in PowerShell:

```powershell
$bytes = New-Object byte[] 32
$rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
$rng.GetBytes($bytes)
$jwtKey = [Convert]::ToBase64String($bytes)
dotnet user-secrets set "Jwt:Key" $jwtKey --project Filmaholic.Api
$rng.Dispose()
[Array]::Clear($bytes, 0, $bytes.Length)
Remove-Variable jwtKey, bytes, rng
```

For production, set `Jwt__Key` in the deployment environment or secret manager to a new random value of at least 32 bytes. Do not reuse the old key from Git history; treat it as exposed and rotate it before deployment. Issuer and audience are identifiers, not secrets, and remain in `appsettings.json`.

Run the API from the repository root:

```powershell
dotnet run --project Filmaholic.Api
```

Swagger is enabled in Development. The API expects the configured HTTPS development certificate to be trusted by the client platform.

Apply database migrations explicitly when setting up or upgrading a database:

```powershell
dotnet ef database update --project Filmaholic.Api
```

The current baseline migration creates the schema for a fresh test database, including Identity and unique movie title/year indexes. This project does not preserve data from the earlier pre-identity schema. To reset the local test database, stop the API and remove `Filmaholic.Api/movies.db`, then run the migration command above.

## Run The App

Debug builds use `https://10.0.2.2:7047/` on Android emulators and `https://localhost:7047/` on other targets. Make sure the API is running and the development certificate is trusted.

Release builds read `Api:BaseUrl` from `Filmaholic.App/appsettings.json`. Replace the placeholder with the deployed HTTPS API URL before packaging. The app intentionally refuses to start a Release build while the placeholder domain is configured.

Build an individual target, for example:

```powershell
dotnet build Filmaholic.App -f net10.0-windows10.0.19041.0
```

## Tests

Run the API unit tests from the repository root:

```powershell
dotnet test Filmaholic.Api.Tests
```

## Current Scope

The app supports registration, sign-in, viewing the movie catalog, and adding, editing, and deleting movies. Production deployment still requires environment-specific secrets, a real API URL, durable database hosting and backups, and an operational migration process.