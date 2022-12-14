namespace Pims.Tools.Core.Keycloak
{
    /// <summary>
    /// IRequestClient interface, provides an HTTP client to make requests and handle refresh token.
    /// </summary>
    public interface IKeycloakRequestClient : IRequestClient
    {
        public string GetIntegrationEnvUri();

        public string GetIntegrationUri();

        public string GetEnvUri();
    }
}
