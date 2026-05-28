# Base Images Setup

This directory contains OpenShift ImageStream templates for base images used by PIMS application builds and security scanning.

## Prerequisites

- Access to the `3cd915-tools` namespace in OpenShift
- `oc` CLI authenticated to the cluster

## Initial Setup

Run these commands **once** to provision base ImageStreams in `3cd915-tools` before monthly scans or builds:

### .NET 8 Runtime and SDK

```bash
oc apply -f openshift/4.0/templates/base-images/dotnet80/dotnet-aspnet-80.yaml
oc apply -f openshift/4.0/templates/base-images/dotnet80/dotnet-sdk-80.yaml
```

### Node.js 20 (UBI9)

```bash
oc apply -f openshift/4.0/templates/base-images/nodejs-20-ubi9.yaml
```

### NGINX Base (if applicable)

```bash
# Verify if nginx-base ImageStream exists or needs provisioning
oc get imagestream nginx-base -n 3cd915-tools
```

## Required GitHub Secrets

New secret configured in GitHub Actions:

- `NODEJS20_BASE_TAG`

## Verification

After applying the templates, verify the ImageStreams were created:

```bash
oc get imagestream -n 3cd915-tools | grep -E 'dotnet-aspnet|dotnet-sdk|nodejs-20-ubi9|nginx-base'
```

## When Are These Used?

These ImageStreams are referenced by:

1. **Monthly Security Scans** (`.github/workflows/scheduled-monthly-image-scan.yml`)

   - Scans base images on the 1st of each month
   - Requires these ImageStreams to exist before the first scan runs

2. **BuildConfigs** (optional optimization)
   - Can be used to speed up builds by caching base images locally
   - See `dotnet80/README.md` for optional `oc import-image` commands

## Maintenance

- ImageStreams should be updated when upgrading base image versions
- Update corresponding GitHub secrets when changing image tags
- Re-apply templates after version changes

## Notes

- These ImageStreams reference external registries (MCR, Red Hat)
- Images are pulled from upstream registries during builds and scans
- For performance optimization, consider importing images to the internal registry (see `dotnet80/README.md`)
