# DotNet 5.0 Base Images

To speed up the build of the DotNet 5.0 API import the following images into the local registry.
The images provided by Red Hat do not work, instead use the images provided by Microsoft.

```bash
oc import-image dotnet-aspnet --from=mcr.microsoft.com/dotnet/aspnet:6.0 --confirm
oc import-image dotnet-sdk --from=mcr.microsoft.com/dotnet/sdk:6.0 --confirm
```

Tag them to identify their version.

```bash
oc tag dotnet-aspnet:latest dotnet-aspnet:6.0
oc tag dotnet-sdk:latest dotnet-sdk:6.0
```

Update the Dockerfile that is in the backend that will be used by API Build Configuration.

- image-registry.apps.silver.devops.gov.bc.ca/3cd915-tools/dotnet-aspnet:6.0
- image-registry.apps.silver.devops.gov.bc.ca/3cd915-tools/dotnet-sdk:6.0

## RedHat DotNet Images

Red Hat provides two types of images, those that require a subscription to download and the Centos opensource version.
They no longer appear to provide RHEL7 versions, only RHEL8.

Container images here - [link](https://catalog.redhat.com/software/containers/search?q=dotnet%205&p=1)

### Red Hat Universal Base Images

These images require a subscription manager and account to be setup.  The Lab provides the required configuration and secrets within the tools namespace.
Read more here - [link](https://github.com/BCDevOps/OpenShift4-Migration/issues/15)

Edit your `BuildConfig` to include the required ConfigMaps and Secrets.

```yaml
        secrets:
          - secret:
              name: platform-services-controlled-etc-pki-entitlement
            destinationDir: etc-pki-entitlement
        configMaps:
          - configMap:
              name: platform-services-controlled-rhsm-conf
            destinationDir: rhsm-conf
          - configMap:
              name: platform-services-controlled-rhsm-ca
            destinationDir: rhsm-ca
```

Also edit the `BuildConfig` to squash all the layers so that the private key will be used for pulling all images.

```yaml
      strategy:
        type: Docker
        dockerStrategy:
          imageOptimizationPolicy: SkipLayers
```

Edit your `Dockerfile` to load the configuration files and secrets.

```docker
# Copy entitlements
COPY ./etc-pki-entitlement /etc/pki/entitlement

# Copy subscription manager configurations
COPY ./rhsm-conf /etc/rhsm
COPY ./rhsm-ca /etc/rhsm/ca

# Install some packages and clean up
RUN INSTALL_PKGS="space separated list of packages" && \
    # Initialize /etc/yum.repos.d/redhat.repo
    # See https://access.redhat.com/solutions/1443553
    rm /etc/rhsm-host && \
    yum repolist --disablerepo=* && \
    yum install -y --setopt=tsflags=nodocs $INSTALL_PKGS && \
    rpm -V $INSTALL_PKGS && \
    yum -y clean all --enablerepo='*' && \
    # Remove entitlements and Subscription Manager configs
    rm -rf /etc/pki/entitlement && \
    rm -rf /etc/rhsm
```

### Red Hat Centos Images

The Centos images are open-source and therefore do not need a subscription or account to pull.
