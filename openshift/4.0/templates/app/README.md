# PIMS React Web Application

Configure **two** build configurations; one for DEV builds and one for PROD builds. The development build should target the **`dev`** branch in GitHub, while the production build should target the **`master`** branch. Make sure you have `.env` files setup for each configuration.

Go to - `/pims/openshift/4.0/templates/app`

Create a build configuration file here - `build.dev.env`
Update the configuration file and set the appropriate parameters.

**Example**

```conf
BUILDIMAGE_NAME=nodejs-14-ubi8
BUILDIMAGE_TAG=1-35
RUNTIMEIMAGE_NAME=nginx-runtime
RUNTIMEIMAGE_TAG=dev
GIT_URL=https://github.com/bcgov/PSP.git
GIT_REF=dev
SOURCE_CONTEXT_DIR=frontend
OUTPUT_IMAGE_TAG=latest
CPU_LIMIT=1
MEMORY_LIMIT=6Gi
```

:warning: **IMPORTANT -** The RUNTIMEIMAGE_TAG parameter **must match** the nginx-runtime image for the environment you are setting up (DEV, TEST or UAT)

```conf
# build.test.env
RUNTIMEIMAGE_TAG=test

# build.uat.env
RUNTIMEIMAGE_TAG=uat
```

> Note that we don't have a `build.prod.env` here. That's because the PROD environment gets updated by tagging a properly tested and vetted UAT image instead of re-building from source code.

Create the React frontend build and save the template.

```bash
oc project 3cd915-tools
oc process -f build.yaml --param-file=build.dev.env | oc create --save-config=true -f -
```

Tag the image so that the appropriate environment can pull the image.

```bash
oc tag pims-app:latest pims-app:dev
```

Create a deployment configuration file here - `deploy.dev.env`
Update the configuration file and set the appropriate parameters.

**Example**

```conf
ENV_NAME=dev
IMAGE_TAG=dev
APP_DOMAIN=pims-app-3cd915-dev.apps.silver.devops.gov.bc.ca
APP_PORT=8080
KEYCLOAK_REALM=72x8v9rw
KEYCLOAK_CONFIG_FILE_NAME=keycloak.json
KEYCLOAK_CONFIG_MOUNT_PATH=/tmp/app/dist/
KEYCLOAK_AUTHORITY_URL=https://dev.oidc.gov.bc.ca/auth
REAL_IP_FROM=172.51.0.0/16
API_PATH=/api
CPU_REQUEST=100m
CPU_LIMIT=1
MEMORY_REQUEST=100Mi
MEMORY_LIMIT=2Gi
```

Create the frontend deployment and save the template.

```bash
oc project 3cd915-dev
oc process -f deploy.yaml --param-file=deploy.dev.env | oc create --save-config=true -f -
```

Create a deployment configuration file here for the route - `deploy-routes.dev.env`
Update the configuration file and set the appropriate parameters.

In **PROD** you will need to get the SSL certificate values and update the `deploy-routes.yaml` file `tls` section.

```yaml
tls:
  insecureEdgeTerminationPolicy: Redirect
  termination: edge
  caCertificate: "{ENTER YOUR CA CERT HERE}"
  certificate: "{ENTER YOUR CERT HERE}"
  key: "{ENTER YOUR CERT KEY HERE}"
```

> Do not check in your certificate values to git.

**Example**

```conf
ENV_NAME=dev
APP_DOMAIN=pims-app-3cd915-dev.apps.silver.devops.gov.bc.ca
APP_PORT=8080
```

```bash
oc process -f deploy-routes.yaml --param-file=deploy-routes.dev.env | oc create --save-config=true -f -
```
