# Jenkins Pipelines

_For details on how these pipelines are deployed into OpenShift, refer to the [OpenShift documentation](../README.md)_

# 1 Application Deployment

### Application (`cicd-pipeline`)

This GitHub webhook triggered pipeline is the main CI/CD pipeline for the project. It is triggered by each commit to the `dev` branch of the [Property Inventory Management System](https://github.com/bcgov/PSP) GitHub repository.

This pipeline performs the following operations in sequential order;

- Builds the PIMS application container image which includes `frontend`, `backend`, `database` and other components within a microservices architecture.
- Deploys the resulting artifacts to the `DEV` environment.
- _(TODO) Triggers an asynchronous OWASP Security Scan on the deployed application._

### Test (`promote-to-test-pipeline`)

> :bulb: This section is in progress

This manually triggered pipeline promotes deployments from the `dev` environment to the `test` environment. To perform the promotion click the **Start Pipeline** on the `promote-to-test-pipeline`.

### Prod (`promote-to-prod-pipeline`)

> :bulb: This section is in progress

This manually triggered pipeline promotes deployments from the `test` environment to the `prod` environment. To perform the promotion click the **Star Pipeline** on the `promote-to-prod-pipeline`.
