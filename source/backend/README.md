# Backend dependency management

NuGet package versions are managed centrally in `Directory.Packages.props`.

- Add or remove a project dependency using `PackageReference` in its `.csproj`, without a `Version` attribute or element.
- Add or update its version using `PackageVersion` in `Directory.Packages.props`. A version change affects all backend projects referencing that package.
- Keep shared analyzer references in `Directory.Build.props`, without inline versions. Preserve their asset metadata.
- Keep build settings in `Directory.Build.props`; do not move project-specific dependencies there.
- Do not add nested central package files or `VersionOverride` exceptions without reviewing and documenting the reason.
- Projects outside `source/backend` are not automatically covered by this central file.
- Transitive pinning is not enabled by this migration.

Validate changes with restore/build of `PimsBackend.sln` and restore/test of `tests/PimsUnitTest.sln`, including existing CI coverage checks.


## .NET 8 to .NET 10 migration status

Inventory reviewed on 2026-10-20. Backend projects target `net10.0`. This
document records the working tree and the package reports supplied during
the migration; target versions below are those reports' versions, not a
promise that they remain the latest releases.

Packages that need source edits, new project
references, authentication changes or deployment changes can be upgraded
separately after the .NET 10 baseline is accepted. This limits migration risk
and makes failures and rollbacks easier to isolate. An outdated-package
report does not mean every listed version is required for .NET 10.

### Applied package updates

The following versions are present in `Directory.Packages.props`. "Before"
means the version in the initial outdated-package report, not necessarily
the version used before the framework migration started.

| Package or coordinated family | Before | Current |
| --- | --- | --- |
| ASP.NET Core JwtBearer, OpenIdConnect, Diagnostics.EntityFrameworkCore, Identity.EntityFrameworkCore | 10.0.12 | 10.0.13 |
| EF Core, Design, InMemory, Relational, SqlServer, SqlServer.NetTopologySuite, Tools | 10.0.12 | 10.0.13 |
| Microsoft.Extensions.Caching.Memory, Http, Logging, Options | 10.0.12 | 10.0.13 |
| Mapster, Mapster.DependencyInjection, Mapster.EFCore | 10.0.12 | 10.0.13 |
| System.Drawing.Common | 10.0.12 | 10.0.13 |
| Microsoft.Data.SqlClient | 6.1.6 | 6.1.7 |
| DistributedLock.SqlServer | 1.0.6 | 1.0.7 |
| ClosedXML | 0.105.0 | 0.105.1 |
| System.Linq.Dynamic.Core | 1.7.1 | 1.7.4 |
| Serilog.Settings.Configuration | 10.0.0 | 10.0.1 |
| Hangfire, Hangfire.AspNetCore, Hangfire.Core | 1.8.17 | 1.8.25 |
| AspNetCore.Proxy | 4.4.0 | 4.5.0 |
| Azure.Identity | 1.17.1 | 1.21.0 |
| CsvHelper | 33.0.1 | 33.1.0 |
| LinqKit | 1.3.0 | 1.4.0 |
| ProjNet | 2.0.0 | 2.1.0 |
| JsonSubTypes | 2.0.1 | 2.2.0 |
| Serilog | 4.3.0 | 4.4.0 |
| Polly | 8.4.2 | 8.8.0 |
| Polly.Extensions | 8.5.1 | 8.8.0 |
| Hangfire.Console.Extensions and Hangfire.Console.Extensions.Serilog | 2.0.0 | 2.1.2 |
| Hangfire.Redis.StackExchange | 1.9.4 | 1.12.0 |
| Microsoft.IdentityModel.Protocols.OpenIdConnect | 8.19.2 | 8.23.0 |
| System.IdentityModel.Tokens.Jwt | 8.15.0 | 8.23.0 |
| KubernetesClient | 18.0.13 | 21.0.8 |
| nClam | 7.0.0 | 9.0.0 |
| NuGet.CommandLine | 6.12.5 | 7.9.0 |

SqlClient 6.1.7 satisfies the EF Core 10.0.13 SQL Server provider's minimum
dependency; retaining 6.1.6 caused NU1605 restore errors.

Earlier framework-related upgrades moved the health-check packages to 9.0.0
and Npgsql from 8.0.5 to 10.0.3, and removed direct System.Data.SqlClient
references. Subsequent updates replaced the earlier 10.0.12 package pins and
SqlClient version with the versions listed above.
System.Security.Cryptography.Pkcs and Xml are currently 10.0.13; their earlier
versions were not captured in the initial outdated-package report.

### Deferred upgrades: separate changes required or compatibility review pending

| Package | Current | Reported target | Reason and follow-up |
| --- | --- | --- | --- |
| Microsoft.CodeAnalysis.Common, CSharp, CSharp.Workspaces, VisualBasic, VisualBasic.Workspaces, Workspaces.Common | 5.0.0 | 5.9.0 | Add a central Workspaces.MSBuild 5.9.0 entry and explicit private PackageReferences in api/Pims.Api.csproj, dal/Pims.Dal.csproj and entities/Pims.Dal.Entities.csproj; align the listed Roslyn family at 5.9.0. Revalidate restore, build, unit tests and EF design-time commands. |
| Microsoft.CodeAnalysis.CSharp.CodeStyle | 4.11.0 | 5.9.0 | Separate analyzer/SDK review. Verify compiler compatibility and new diagnostics. Source fixes may be needed when diagnostics are enforced; this is not an established package-only upgrade. |
| DotNetEnv | 2.5.0 | 3.2.0 | The earlier combined upgrade exposed an unresolved `using Sprache;` in `tests/api/Services/CompensationRequisitionServiceTest.cs`.|
| Swashbuckle.AspNetCore, Annotations, SwaggerGen, SwaggerUI | 6.9.0 | 10.3.1 | Requires migrating Swagger configuration and filters in API, proxy, scheduler and core.api. Update namespaces, security references and parameter defaults together; verify all Swagger documents and authenticated UI calls. |
| Microsoft.OpenApi | 1.6.22 | 3.10.2 | Coupled to Swashbuckle. Do not independently select the newest major version. The earlier Swashbuckle 10.3.0 trial used its declared OpenApi dependency, 2.7.5. Recheck the chosen Swashbuckle release's dependency before migration. |
| Microsoft.Data.SqlClient | 6.1.7 | 7.1.1 |If connections use SQL Server authentication and no Microsoft Entra authentication, the Entra Azure extension package is not needed. Only deployments using driver-provided Entra authentication modes need the corresponding extension review and possible project-reference changes. This remains an optional major-version upgrade: review the selected release's breaking changes and validate SQL connections, TLS/certificate settings, transactions, spatial queries and EF design-time operations. 6.1.7 already satisfies the current EF Core provider dependency. |


These are follow-up work items, not reasons to force unrelated source changes
into the .NET 8 to .NET 10 migration. Retain the existing versions until each
separate migration is approved and validated.

## PSP configuration status and deployment boundary

The reviewed enabled PSP workflows have the following .NET 10 configuration:

- Backend applications, unit-test projects, the Keycloak sync tool and the
  TypeScript model generator target `net10.0`.
- VS Code API and Keycloak sync launch configurations reference `net10.0`.
- Backend CI (`api-dotnetcore.yml`, named `API (.NET 10)`) requests SDK
  10.0.112. `codecov-comment-pr.yml` now matches that name in both its trigger
  and API upload condition.
- All three backend Dockerfiles build with the Red Hat UBI 9 SDK image pinned
  to digest `39f09c87b0b44e3430c788fd6a775423f5aa78eefabe2854b677528392013358`,
  verified as SDK 10.0.112 with .NET/ASP.NET Core runtimes 10.0.12. Their final
  stages use Microsoft's `aspnet:10.0.12-noble` image. Build SDK parity does
  not imply identical runtime operating systems. Build-stage `USER 0` preserves
  write permissions previously provided by the Microsoft SDK image.
- Monthly image scanning targets `3cd915-tools/dotnet-100:10.0`, a combined
  Red Hat SDK/runtime builder. The API Dockerfile's EF CLI is 10.0.13, matching
  the EF Core package family; SDK, runtime and EF package versions are distinct.
- `openshift/4.0/templates/api/build.yaml` defaults to `dotnet-100:10.0`.
  The enabled `uat_pre_release_hotfix_argo.yml` still processes this template.
  Updating it affects newly generated BuildConfigs, not existing cluster ones.


### Intentionally deferred or legacy scope

- `image-scan-analysis.yml` is disabled and its .NET 8 targets remain unchanged.
  Align its image targets and event conditions before re-enabling it. It is
  separate from the enabled monthly scanner.
- Disabled legacy deployment workflows are outside this migration's enabled
  execution path. Modern workflows call `keycloak-sync.yml`, which starts
  environment-specific GitOps-managed pipelines rather than applying the local
  `openshift/4.0/templates/jobs/keycloak-sync-task.yaml`.
- The local legacy Keycloak task still references `dotnet-80:8.0`; the
  `base-images/dotnet80` definitions also remain. Upgrade any retained legacy
  execution path before using it with .NET 10 source, or retire it explicitly.
  YAML files in Git do not establish that the corresponding images are installed.
- `testing/PIMS.Tests.Automation` remains a separate `net8.0` Selenium suite.
  The enabled smoke/e2e workflows use Node/Playwright. Upgrading the Selenium
  runner is separate from the backend migration.
- Optional package migrations requiring code or project edits remain deferred
  as listed above; they are not prerequisites simply because newer versions exist.



