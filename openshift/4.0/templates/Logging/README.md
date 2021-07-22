# pims-logging

The pims-logging can run as a sidecar or as a standalone container/pod - It will collect oc logs from the pims frontend and backend api every sleep_time and submit a zip to an azure blob container using curl

- Collect logs of PIMS-APP and PIMS-API every SLEEP_TIME (--since=1hr,2hr etc.)
- Archive all collected to logs after an EXPORT_TIME
- Send archieved logs to Azure blob or s3 container using curl
- Sleep and repeat.

Will require that your namespace default service account has view permission, so it can get read other pod's logs.

## Build dockerfile and push pims-logging image to openshift image registar

```bash
$ docker login -u $(oc whoami) -p $(oc whoami -t) image-registry.openshift-image-registry
```

Go to - `/openshift/4.0/template/Logging`

```bash
$ docker build -t pims-logging:latest .
$ docker tag pims-logging:latest image-registry.apps.silver.devops.gov.bc.ca/3cd915-tools/pims-logging:latest
$ docker push image-registry.apps.silver.devops.gov.bc.ca/3cd915-tools/pims-logging:latest
```

### or create pims-logging imagestream using the alpine base image and build config

```bash
$ docker pull frolvlad/alpine-glibc
$ docker tag frolvlad/alpine-glibc:latest image-registry.apps.silver.devops.gov.bc.ca/3cd915-tools/alpine-base:latest
$ docker login -u $(oc whoami) -p $(oc whoami -t) image-registry.openshift-image-registry
$ docker push image-registry.apps.silver.devops.gov.bc.ca/3cd915-tools/alpine-base:latest
```

Create a build configuration file here - `build.dev.env Update` the configuration file and required parameters. When build for uat and prod, change GIT_REF=`master`

### Example

```bash
GIT_URL=https://github.com/PSP.git
GIT_REF=dev
OUTPUT_IMAGE_TAG=latest
DOCKERFILE_PATH=Dockerfile
CPU_LIMIT=400m
MEMORY_LIMIT=1Gi
```

- use the alpine image as base image to create pims-logging build configuration

```bash
$ oc process -f .\oclogbc.yaml --param-file=build.dev.env | oc create -f -
```

Tag image to dev env

```bash
$ oc tag pims-logging:latest pims-logging:dev
```

### Create service account and role-binding to read pod's logs

```bash
oc process -f .\role-binding.yaml -p NAMESPACE=3cd915-dev | oc apply -f -
```

### Create pims-logging deployment config

Create a deploy config env file here in the base dir `deploy.dev.env` with a valid `AZ_SAS_TOKEN`

### Example

```bash
IMAGE_TAG=dev
SLEEP_TIME=3600
STORAGE_TYPE=Azure_blob
AZ_BLOB_URL=https://pimsapp.blob.core.windows.net
AZ_BLOB_CONTAINER=pims
AZ_SAS_TOKEN=?{TOKEN SECRET}
FRONTEND_APP_NAME=pims-app
API_NAME=pims-api
PROJECT_NAMESPACE=3cd915-dev
TOOLS_NAMESPACE=3cd915-tools
EXPORT_TIME=43200
```

To switch to Amazon S3 storage `deploy.dev.env` should look like ;

```bash
IMAGE_TAG=dev
SLEEP_TIME=3600
STORAGE_TYPE=Amazon_S3
AWS_HOST=moti-int.objectstore.gov.bc.ca
AWS_REGION=us-east-1
AWS_ACCESS_KEY_ID=tran_api_psp_logfiles_dev
AWS_BUCKET_NAME=tran_api_psp_logfiles_dev
AWS_SECRET_ACCESS_KEY=XXXXXXXXXX
FRONTEND_APP_NAME=pims-app
API_NAME=pims-api
PROJECT_NAMESPACE=3cd915-dev
TOOLS_NAMESPACE=3cd915-tools
EXPORT_TIME=43200
```

Create the logging deployment

```bash
oc process -f .\logging_dc.yaml --param-file=deploy.dev.env | oc create -f -
```

need to pass in AP_NAME, API_NAME, and AP_LOG_SERVER_URI (endpoint that accepts binary upload) as env vars to the sidecar. Below is a snippet from a Deployment Config to give you an idea of configuring when running as a sidecar or standalone pod container.

```
spec:
          volumes:
            - name: ${APP_NAME}-${ROLE_NAME}
              persistentVolumeClaim:
                claimName: ${APP_NAME}-${ROLE_NAME}
            - name: ${APP_NAME}-${ROLE_NAME}-root
              persistentVolumeClaim:
                claimName: ${APP_NAME}-${ROLE_NAME}-root
          containers:
            - name: ${APP_NAME}-${ROLE_NAME}
              image: ""
              ports:
                - containerPort: ${{APP_PORT}}
                  protocol: TCP
              env:
                - name: SLEEP_TIME
                  value: ${SLEEP_TIME}
                - name: AZ_BLOB_URL
                  value: ${AZ_BLOB_URL}
                - name: AZ_BLOB_CONTAINER
                  value: ${AZ_BLOB_CONTAINER}
                - name: AZ_SAS_TOKEN
                  valueFrom:
                    secretKeyRef:
                      name: ${APP_NAME}-${ROLE_NAME}
                      key: AZ_SAS_TOKEN
                - name: FRONTEND_APP_NAME
                  value: ${FRONTEND_APP_NAME}
                - name: API_NAME
                  value: ${API_NAME}
                - name: PROJECT_NAMESPACE
                  value: ${PROJECT_NAMESPACE}
                - name: EXPORT_TIME
                  value: ${EXPORT_TIME}
              resources:
                requests:
                  cpu: ${CPU_REQUEST}
                  memory: ${MEMORY_REQUEST}
                limits:
                  cpu: ${CPU_LIMIT}
                  memory: ${MEMORY_LIMIT}
              volumeMounts:
                - name: ${APP_NAME}-${ROLE_NAME}
                  mountPath: ${LOGGING_VOLUME_MOUNT_PATH}
                - name: ${APP_NAME}-${ROLE_NAME}-root
                  mountPath: ${APP_VOLUME_MOUNT_PATH}
```

## Run Locally

Go to - `/openshift/4.0/template/Logging` for Logging **_Dockerfile_** and **_Docker-compose.yml_**\
Create .env file with the required environment variables e.g.

```bash
SLEEP_TIME=30
STORAGE_TYPE=Azure_blob
AZ_BLOB_URL=https://pimsapp.blob.core.windows.net
AZ_BLOB_CONTAINER=pims
AZ_SAS_TOKEN=?XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
FRONTEND_APP_NAME=pims-app
API_NAME=pims-api
PROJECT_NAMESPACE=3cd915-dev
EXPORT_TIME=120
OC_TOKEN=sha256~XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
OC_SERVER=https://api.silver.devops.gov.bc.ca:6443
```

For Amazon S3 storage .env file should look like:
```bash
SLEEP_TIME=30
STORAGE_TYPE=Amazon_S3
AWS_HOST=moti-int.objectstore.gov.bc.ca
AWS_REGION=us-east-1
AWS_ACCESS_KEY_ID=tran_api_psp_logfiles_dev
AWS_BUCKET_NAME=tran_api_psp_logfiles_dev
AWS_SECRET_ACCESS_KEY=XXXXXXXXXX
FRONTEND_APP_NAME=pims-app
API_NAME=pims-api
PROJECT_NAMESPACE=3cd915-dev
EXPORT_TIME=60
OC_TOKEN=sha256~XXXXXXXXXXX
OC_SERVER=https://api.silver.devops.gov.bc.ca:6443
```

Build pims-logging Docker image

```bash
$ docker-compose build logging
```

Build and Run locally pims-logging imgage and run as container

```bash
$ docker-compose up logging
```
