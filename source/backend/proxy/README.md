# PIMS RESTful API - .NET CORE

The PIMS API provides an RESTful interface to interact with the configured data-source.

The API is configured to run in a Docker container and has the following dependencies with other containers; database, keycloak.

For more information refer to documentation [here](https://github.com/bcgov/PSP/wiki/api/API.md).

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

## Running Locally

to run the API locally with vscode, comment out the following lines, and add the `ConnectionStrings__PIMS` value in your `.env` file;

```conf
# ASPNETCORE_ENVIRONMENT=Development
# ASPNETCORE_URLS=http://*:8080
```

This is so that the `/.vscode/launch.json` configured environment variables are used instead. Specifically it will run with the following;

```json
{
    "configurations": [{
        ...
        "env": {
            "ASPNETCORE_ENVIRONMENT": "Local",
            "ASPNETCORE_URLS": "http://*:5002"
        }
        ...
    }]
}
```
