# DotNet 8.0 Base Images

To speed up builds and align with application runtime/SDK versions, import .NET 8 images into the local registry.

```bash
# Import upstream Microsoft images into OpenShift ImageStreams
oc import-image dotnet-aspnet:8.0 --from=mcr.microsoft.com/dotnet/aspnet:8.0 --confirm -n 3cd915-tools
oc import-image dotnet-sdk:8.0   --from=mcr.microsoft.com/dotnet/sdk:8.0   --confirm -n 3cd915-tools
```

If you prefer Red Hat UBI-based images, update the ImageStream sources accordingly or use the `build.yaml` to produce hardened internal variants.

Tag them to identify their version if needed (latest → 8.0):

```bash
oc tag -n 3cd915-tools dotnet-aspnet:latest dotnet-aspnet:8.0
oc tag -n 3cd915-tools dotnet-sdk:latest   dotnet-sdk:8.0
```

Update backend Dockerfiles/BuildConfigs to reference the internal registry paths:

- `image-registry.apps.silver.devops.gov.bc.ca/3cd915-tools/dotnet-aspnet:8.0`
- `image-registry.apps.silver.devops.gov.bc.ca/3cd915-tools/dotnet-sdk:8.0`

## Red Hat DotNet Images

If using Red Hat UBI images, ensure entitlement/config secrets are mounted as described in `build.yaml` and mirror the `dotnet50` approach (entitlement and rhsm config maps).
