using System.Linq;
using Pims.Tools.Keycloak.Sync.Configuration.Realm;

namespace Pims.Tools.Keycloak.Sync.Models.Keycloak
{
    /// <summary>
    /// ClientModel class, provides a model to represent a keycloak client.
    /// </summary>
    public class ClientModel : Core.Keycloak.Models.ClientModel
    {
        #region Properties
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ClientModel class.
        /// </summary>
        public ClientModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a ClientModel class, initializes with specified arguments.
        /// </summary>
        /// <param name="client"></param>
        public ClientModel(ClientOptions client)
        {
            ClientId = client.ClientId;
            Name = client.Name;
            Description = client.Description;
            Enabled = client.Enabled;

            Protocol = client.Protocol;
            Secret = client.Secret;

            ConsentRequired = client.ConsentRequired;
            ClientAuthenticatorType = client.ClientAuthenticatorType;
            DefaultRoles = client.DefaultRoles;
            FullScopeAllowed = client.FullScopeAllowed;
            SurrogateAuthRequired = client.SurrogateAuthRequired;

            PublicClient = client.PublicClient;
            AuthorizationServicesEnabled = client.AuthorizationServicesEnabled;
            BearerOnly = client.BearerOnly;
            DirectAccessGrantsEnabled = client.DirectAccessGrantsEnabled;
            ImplicitFlowEnabled = client.ImplicitFlowEnabled;
            ServiceAccountsEnabled = client.ServiceAccountsEnabled;
            StandardFlowEnabled = client.StandardFlowEnabled;

            BaseUrl = client.BaseUrl;
            RootUrl = client.RootUrl;
            RedirectUris = client.RedirectUris;
            Origin = client.Origin;
            WebOrigins = client.WebOrigins;
            AdminUrl = client.AdminUrl;

            if (client.ProtocolMappers != null)
            {
                ProtocolMappers = client.ProtocolMappers.Select(m => new ProtocolMapperModel(m)).ToArray();
            }
        }
        #endregion
    }
}
