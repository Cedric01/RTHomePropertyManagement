# RTHome Property Management API

A .NET 9 REST API for managing real estate property listings, built with ASP.NET Core, Entity Framework Core (PostgreSQL), and Auth0 authentication. Deployed to Azure App Service via GitHub Actions CI/CD.

---

## Table of Contents

- [Project Structure](#project-structure)
- [Local Development](#local-development)
- [Infrastructure — Azure Key Vault](#infrastructure--azure-key-vault)
- [Observability — OpenTelemetry & Application Insights](#observability--opentelemetry--application-insights)
- [Error Handling](#error-handling)
- [CI/CD Pipeline](#cicd-pipeline)
- [GitHub Secrets Reference](#github-secrets-reference)

---

## Project Structure

```
RTHomePropertyManagement/
├── infra/
│   └── keyvault.bicep              # Azure Key Vault Bicep template
├── RTHomePropertyManagement/
│   ├── Controllers/                # API endpoint handlers
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Extensions/                 # Service registration extensions
│   ├── Migrations/                 # EF Core database migrations
│   ├── Models/                     # Domain models + DbContext
│   ├── Repositories/               # Repository interfaces and implementations
│   ├── Program.cs                  # App entry point + Key Vault wiring
│   └── appsettings.json            # Config (secrets intentionally blank)
├── RTHomePropertManagementTests/   # Unit/integration tests
├── Dockerfile
└── .github/workflows/dotnet.yml   # CI/CD pipeline
```

---

## Local Development

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- PostgreSQL instance (local or Azure)
- Auth0 account with an API configured

### Setting up secrets locally

Secrets are kept out of `appsettings.json` and managed via [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) in development.

Run these commands from the `RTHomePropertyManagement/` project directory:

```bash
dotnet user-secrets set "ConnectionStrings:Postgres" "your-postgres-connection-string"
dotnet user-secrets set "Auth0:Domain" "your-auth0-domain"
dotnet user-secrets set "Auth0:Audience" "your-auth0-audience"
```

The app detects the `Development` environment and skips Key Vault entirely, reading secrets from user-secrets instead.

---

## Infrastructure — Azure Key Vault

### Why Key Vault?

In production, secrets (database connection strings, Auth0 credentials) must never be stored in `appsettings.json` or environment variables in plain text. Azure Key Vault provides a centralised, audited, access-controlled secret store.

### How it works

The integration is in [Program.cs](RTHomePropertyManagement/Program.cs):

```csharp
if (!builder.Environment.IsDevelopment())
{
    var keyVaultUri = builder.Configuration["KeyVaultUri"];
    if (!string.IsNullOrEmpty(keyVaultUri))
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
}
```

- In **Development**: Key Vault is skipped. Secrets come from user-secrets.
- In **Production**: The app reads `KeyVaultUri` from the App Service application settings, then loads all secrets from Key Vault into the configuration pipeline. Secrets override the blank values in `appsettings.json`.

`DefaultAzureCredential` authenticates using the App Service's **system-assigned managed identity** — no credentials need to be stored anywhere in the app.

### Secret naming convention

.NET maps Key Vault secret names to nested config keys by treating `--` as a separator:

| Key Vault Secret Name       | Maps to config key              |
|-----------------------------|---------------------------------|
| `ConnectionStrings--Postgres` | `ConnectionStrings:Postgres`  |
| `Auth0--Domain`              | `Auth0:Domain`                 |
| `Auth0--Audience`            | `Auth0:Audience`               |

### Bicep template — `infra/keyvault.bicep`

The file [infra/keyvault.bicep](infra/keyvault.bicep) provisions the Key Vault. It is deployed by the CI/CD pipeline on every push to `main`.

Key properties set:

| Property | Value | Reason |
|---|---|---|
| `enableRbacAuthorization` | `true` | Access controlled via Azure RBAC roles, not legacy access policies |
| `enableSoftDelete` | `true` | Deleted secrets are recoverable for 7 days |
| `sku` | `standard` | Sufficient for secrets storage |

The template outputs the `keyVaultUri` and `keyVaultId`, which are used by subsequent pipeline steps.

### Managed Identity and role assignment

The App Service uses a **system-assigned managed identity** to authenticate to Key Vault without any stored credentials. The CI/CD pipeline:

1. Enables the managed identity on the App Service (`az webapp identity assign`)
2. Retrieves the identity's principal ID
3. Assigns the built-in **Key Vault Secrets User** role to that principal, scoped to the Key Vault

`Key Vault Secrets User` grants read-only access to secret values — the minimum permission needed.

### Populating secrets (one-time manual step)

Secrets are added to Key Vault once manually. The CI/CD pipeline does **not** manage secret values — it only provisions the vault, configures access, and sets the `KeyVaultUri` app setting. This keeps the pipeline read-only against the vault and avoids unnecessary write permissions.

```bash
az keyvault secret set --vault-name <KEY_VAULT_NAME> --name "ConnectionStrings--Postgres" --value "<value>"
az keyvault secret set --vault-name <KEY_VAULT_NAME> --name "Auth0--Domain" --value "<value>"
az keyvault secret set --vault-name <KEY_VAULT_NAME> --name "Auth0--Audience" --value "<value>"
```

To update a secret in future, run the relevant command above manually (or via a separate, access-controlled workflow).

### Manual setup (if not using CI/CD)

If you need to set this up manually in Azure:

```bash
# 1. Create the Key Vault
az keyvault create \
  --name <KEY_VAULT_NAME> \
  --resource-group <RESOURCE_GROUP> \
  --enable-rbac-authorization true \
  --enable-soft-delete true

# 2. Enable managed identity on App Service
az webapp identity assign \
  --name <APP_SERVICE_NAME> \
  --resource-group <RESOURCE_GROUP>

# 3. Assign Key Vault Secrets User role (use principal ID from step 2)
az role assignment create \
  --role "Key Vault Secrets User" \
  --assignee <PRINCIPAL_ID> \
  --scope $(az keyvault show --name <KEY_VAULT_NAME> --query id -o tsv)

# 4. Add secrets
az keyvault secret set --vault-name <KEY_VAULT_NAME> --name "ConnectionStrings--Postgres" --value "<value>"
az keyvault secret set --vault-name <KEY_VAULT_NAME> --name "Auth0--Domain" --value "<value>"
az keyvault secret set --vault-name <KEY_VAULT_NAME> --name "Auth0--Audience" --value "<value>"

# 5. Set KeyVaultUri on App Service
az webapp config appsettings set \
  --name <APP_SERVICE_NAME> \
  --resource-group <RESOURCE_GROUP> \
  --settings KeyVaultUri=$(az keyvault show --name <KEY_VAULT_NAME> --query properties.vaultUri -o tsv)
```

---

## Observability — OpenTelemetry & Application Insights

The app uses the [Azure Monitor OpenTelemetry distro](https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-enable) (`Azure.Monitor.OpenTelemetry.AspNetCore`), which is Microsoft's recommended approach for new .NET projects. It bundles the full OpenTelemetry SDK and exports to Azure Application Insights.

### What gets collected automatically

| Signal | Examples |
|---|---|
| **Traces** | Incoming HTTP requests, outgoing HTTP calls, EF Core queries |
| **Metrics** | Request rate, failure rate, response duration |
| **Logs** | All `ILogger` output correlated to the active trace |

All three signals are correlated by trace ID, so you can go from a failed request in Application Insights directly to the EF Core query that caused it.

### Configuration

The connection string is stored in Azure Key Vault as `ApplicationInsights--ConnectionString` and is read in [Program.cs](RTHomePropertyManagement/Program.cs):

```csharp
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: "RTHomePropertyManagement", serviceVersion: "..."))
    .UseAzureMonitor(options =>
    {
        options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
        options.SamplingRatio = builder.Configuration.GetValue<float>("OpenTelemetry:SamplingRatio", 1.0f);
    })
    .WithTracing(tracing => tracing.AddSource("Npgsql"));
```

If the connection string is absent (e.g. local dev without it set), OpenTelemetry initialises but exports nowhere — no errors, no noise.

### Resource attributes

Every span and metric is tagged with the service name (`RTHomePropertyManagement`) and assembly version. In Application Insights this appears as the **Cloud role name**, making it easy to filter telemetry if multiple services share the same resource.

### Sampling

Sampling is controlled by `OpenTelemetry:SamplingRatio` in config — a float between `0.0` and `1.0`. It defaults to `1.0` (100%) in `appsettings.json`.

For production, add to Key Vault:

```bash
az keyvault secret set \
  --vault-name <KEY_VAULT_NAME> \
  --name "OpenTelemetry--SamplingRatio" \
  --value "0.2"
```

`0.2` sends 20% of traces — a reasonable starting point for a low-to-medium traffic API. Adjust based on volume and Application Insights ingestion cost.

### Postgres query tracing

The Npgsql activity source is explicitly registered so PostgreSQL query spans appear in the trace waterfall alongside the HTTP request and EF Core operations. This makes it easy to spot slow queries directly from a failing request trace.

### Adding the Application Insights connection string to Key Vault

```bash
az keyvault secret set \
  --vault-name <KEY_VAULT_NAME> \
  --name "ApplicationInsights--ConnectionString" \
  --value "<connection-string-from-azure-portal>"
```

Find the connection string in the Azure Portal under your Application Insights resource → **Overview** → **Connection String**.

### Local development

To see traces locally, set the connection string in user-secrets:

```bash
dotnet user-secrets set "ApplicationInsights:ConnectionString" "<your-connection-string>"
```

Or leave it unset — the app starts fine without it.

---

## Error Handling

All unhandled exceptions are caught by `UseExceptionHandler()` middleware and returned as [RFC 7807 Problem Details](https://www.rfc-editor.org/rfc/rfc7807) JSON, registered via `AddProblemDetails()` in [Program.cs](RTHomePropertyManagement/Program.cs).

**Example error response:**

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
  "title": "An error occurred while processing your request.",
  "status": 500
}
```

- In **Development**: the response includes the exception message and stack trace
- In **Production**: a safe, generic message is returned — no internal details leaked
- OpenTelemetry automatically captures the exception as part of the failed request trace

---

## CI/CD Pipeline

The pipeline at [.github/workflows/dotnet.yml](.github/workflows/dotnet.yml) has three jobs:

### `build-and-test`
Runs on every push and pull request. Restores, builds, and tests the solution.

### `docker`
Runs on push to `main` only. Builds and pushes the Docker image to Docker Hub with two tags:
- `latest`
- `sha-<commit-sha>` (for traceability)

### `deploy`
Runs on push to `main` only, after `docker`. Performs the full deployment sequence:

1. Azure login using `AZURE_CREDENTIALS`
2. Deploy `infra/keyvault.bicep` to provision the Key Vault (idempotent)
3. Enable system-assigned managed identity on the App Service
4. Assign `Key Vault Secrets User` role to the managed identity (idempotent)
5. Push secrets into Key Vault from GitHub secrets
6. Set `KeyVaultUri` as an App Service application setting
7. Deploy the Docker image to Azure App Service

---

## GitHub Secrets Reference

All secrets are configured under **Settings → Secrets and variables → Actions**.

| Secret | Description |
|---|---|
| `AZURE_CREDENTIALS` | Service principal JSON for Azure login (existing) |
| `AZURE_RESOURCE_GROUP` | Azure resource group containing the App Service |
| `AZURE_LOCATION` | Azure region for the Key Vault (e.g. `uksouth`, `eastus`) — must match the region of any existing Key Vault |
| `AZURE_APP_NAME` | Azure App Service name |
| `KEY_VAULT_NAME` | Desired Key Vault name (3–24 chars, alphanumeric + hyphens, globally unique) |
| `DOCKERHUB_USERNAME` | Docker Hub username (existing) |

**Key Vault secrets (set once manually):**

| Secret Name in Key Vault | Maps to config key |
|---|---|
| `ConnectionStrings--Postgres` | `ConnectionStrings:Postgres` |
| `Auth0--Domain` | `Auth0:Domain` |
| `Auth0--Audience` | `Auth0:Audience` |
| `ApplicationInsights--ConnectionString` | `ApplicationInsights:ConnectionString` |
| `DOCKERHUB_TOKEN` | Docker Hub access token (existing) |
