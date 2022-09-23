FROM registry.redhat.io/ubi8/dotnet-50:5.0-23

LABEL \
    io.k8s.display-name="DotNet 5.0" \
    io.k8s.description="Base image containing dotnet 5.0 SDK and runtime" \
    io.openshift.tags="openshift,dotnet,dotnet50"

USER 0

# Copy entitlements
COPY ./etc-pki-entitlement /etc/pki/entitlement

# Copy subscription manager configurations
COPY ./rhsm-conf /etc/rhsm
COPY ./rhsm-ca /etc/rhsm/ca

ENV PATH=$PATH:$HOME/.dotnet/tools

USER 1001
