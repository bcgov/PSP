# pims-logging

The pims-logging can run as a sidecar or as a standalone container/pod - It will collect oc logs from the pims frontend and backend api every sleep_time and submit a zip to an azure blob container using curl

- Collect logs of PIMS-APP and PIMS-API every SLEEP_TIME (--since=1hr,2hr etc.)
- Archive all collected to logs after an EXPORT_TIME
- Send archieved logs to Azure blob or s3 container using curl
- Sleep and repeat.

Will require that your namespace default service account has view permission, so it can get read other pod's logs.

## Build dockerfile and push to openshift image registar

```bash
$ docker login -u $(oc whoami) -p $(oc whoami -t) image-registry.openshift-image-registry
```

Go to - `/opnenshift/4.0/template/Logging`

```bash
$ docker build -t pims-sidecar:latest .
$ docker tag pims-sidecar:latest image-registry.apps.silver.devops.gov.bc.ca/3cd915-tools/pims-sidecar:latest
$ docker push image-registry.apps.silver.devops.gov.bc.ca/3cd915-tools/pims-sidecar:latest
```

### create service account and role-binding to read pod's logs

```bash
oc -apply -f role-binding.yaml
```

### create pims-logging deployment config

```bash
oc process -f .\logging_dc.yaml | oc create -f -
```

need to pass in AP_NAME, API_NAME, and AP_LOG_SERVER_URI (endpoint that accepts binary upload) as env vars to the sidecar. Below is a snippet from a Deployment Config to give you an idea of configuring when running as a sidecar or standalone pod container.

```
spec:
          volumes:
            # This volume will need to be created by the database deployment configuration.
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
                  value: ${AZ_SAS_TOKEN}
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
