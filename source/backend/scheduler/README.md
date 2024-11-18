# PIMS scheduler

The PROXY scheduler provides a keycloak-authenticated hangfire instance.

To run the API locally you will need to create the appropriate environment variable `.env` files. You can do this through using the prebuilt scripts [here](../../scripts/README.md).

## Hangfire

1. All serilog logging is piped into hangfire and visible in the job dashboard.
2. All recurring hangfire jobs must be registered in code (currently within Startup.cs). They can then be controlled in the appsettings files or by env.
3. Hangfire is dependent on redis to store job state. This is handled by a separate container with storage in local docker and OS.
4. See best practices for hangfire jobs, but all jobs should be written such that they can continue at any point if the are interupted.

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
