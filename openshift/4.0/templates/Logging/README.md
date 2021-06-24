# oclogs-sidecar
openshift sidecar - will collect oc logs and submit a zip to an endpoint

will require that your namespace default service account has view permission, so it can get read other pod's logs.  

need to pass in POD_NAME, CONTAINER_NAME, and LOG_SERVER_URI (endpoint that accepts binary upload) as env vars to the sidecar.  Below is a snippet from a Deployment Config to give you an idea of configuring alongside your app container, and how best to get the pod name.

```
        spec:
          containers:
 ...
            - image: docker-registry.default.svc:5000/${NAMESPACE}/${whatever you named the oclogs-sidecar image}
              imagePullPolicy: IfNotPresent
              name: "${name your oclogs-sidecar pod}"
              livenessProbe:
                exec:
                  command:
                    - cat
                    - /opt/app/liveness
                initialDelaySeconds: 5
                periodSeconds: 5
              readinessProbe:
                exec:
                  command:
                    - cat
                    - /opt/app/readiness
                initialDelaySeconds: 10
                periodSeconds: 10
                successThreshold: 1
                timeoutSeconds: 1
              env:
                - name: POD_NAME
                  valueFrom:
                    fieldRef:
                      fieldPath: metadata.name
                - name: CONTAINER_NAME
                  value: "${name of your oclogs-sidecar container}"
                - name: LOG_SERVER_URI
                  value: "https://blahblah/api/upload"
```
