# RedHat DotNet 6.0 Base Images

To speed up the build of the DotNet 6.0 API import the following images into the local registry.

```bash
oc import-image dotnet-60:6.0 --from=registry.access.redhat.com/ubi8/dotnet-60:6.0 --confirm
```

Optionally, tag them to identify their version.

```bash
oc tag dotnet-60:latest dotnet-60:6.0
```

Update relevant build configurations in OpenShift:

- `pims-api.dev`
- `pims-api.test`
- `pims-api.master`

```yaml
# sample configuration
kind: BuildConfig
apiVersion: build.openshift.io/v1
metadata:
  name: pims-api.dev
spec:
  strategy:
    type: Source
    sourceStrategy:
      from:
        kind: ImageStreamTag
        name: 'dotnet-60:6.0' # <-- use appropriate image version here
  source:
    type: Git
    ...
  output:
    to:
      kind: ImageStreamTag
      name: 'pims-api:latest-dev'
```
