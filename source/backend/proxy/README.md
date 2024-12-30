# PIMS PROXY API

The PROXY API provides a keycloak-authenticated passthrough to the PIMS geoserver instance (via service account credentials).

To run the API locally you will need to create the appropriate environment variable `.env` files. You can do this through using the prebuilt scripts [here](../../scripts/README.md).

## API Environment Variables

The current environment is initialized through the environment variable `ASPNETCORE_ENVIRONMENT`.

When running the solution it applies the configuration setting in the following order;

> NOTE: When the environment is Development it will look for your _User Secrets_ file.

1. appsettings.json
2. appsettings.`[environment]`.json
3. User Secrets `(if environment=Development)`
4. Environment Variables

To run the solution with docker-compose create a `.env` file within the `/api` directory and populate with the following;

```conf
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://*:8080
ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
```
