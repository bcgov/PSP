using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pims.Core.Http;
using Pims.Keycloak.Extensions;
using Pims.Keycloak.Models;

namespace Pims.Keycloak
{
    /// <summary>
    /// KeycloakRepository class, provides a service for sending HTTP requests to the keycloak admin API.
    ///     - https://www.keycloak.org/docs-api/5.0/rest-api/index.html#_overview.
    /// </summary>
    public partial class KeycloakRepository : IKeycloakRepository
    {
        #region Variables
        private readonly IOpenIdConnectRequestClient _client;
        #endregion

        #region Properties

        /// <summary>
        /// get - The configuration options for keycloak.
        /// </summary>
        public Configuration.KeycloakOptions Options { get; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a KeycloakAdmin class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        public KeycloakRepository(IOpenIdConnectRequestClient client, IOptions<Configuration.KeycloakOptions> options)
        {
            this.Options = options.Value;
            this.Options.Validate();
            this.Options.ServiceAccount.Validate();
            _client = client;
            _client.AuthClientOptions.Audience = this.Options.ServiceAccount.Audience ?? this.Options.Audience;
            _client.AuthClientOptions.Authority = this.Options.ServiceAccount.Authority ?? this.Options.Authority;
            _client.AuthClientOptions.Client = this.Options.ServiceAccount.Client;
            _client.AuthClientOptions.Secret = this.Options.ServiceAccount.Secret;
        }

        /// <summary>
        /// Get the user for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserModel> GetUserAsync(Guid id)
        {
            var users = await GetUsersAsync(id);
            return users.FirstOrDefault();
        }

        public async Task<List<UserModel>> GetUsersAsync(Guid id)
        {
            var response = await _client.GetAsync($"{this.Options.ServiceAccount.Api}/{this.Options.ServiceAccount.Environment}/idir/users?guid={id.ToString().Replace("-", string.Empty)}");
            var result = await response.HandleResponseAsync<ResponseWrapper<UserModel>>();

            return result.Data.ToList();
        }

        public async Task<HttpResponseMessage> AddRolesToUser(string username, IEnumerable<RoleModel> roles)
        {
            return await _client.PostJsonAsync($"{GetIntegrationUrl()}/users/{Uri.EscapeDataString(username)}/roles", roles);
        }

        public async Task<HttpResponseMessage> DeleteRoleFromUsers(string username, string roleName)
        {
            return await _client.DeleteAsync($"{GetIntegrationUrl()}/users/{Uri.EscapeDataString(username)}/roles/{Uri.EscapeDataString(roleName)}");
        }

        public async Task<ResponseWrapper<RoleModel>> GetAllRoles()
        {
            var response = await _client.GetAsync($"{GetIntegrationUrl()}/roles");

            var allKeycloakRoles = await response.HandleResponseAsync<ResponseWrapper<RoleModel>>();
            return allKeycloakRoles;
        }

        public async Task<ResponseWrapper<RoleModel>> GetAllGroupRoles(string groupName)
        {
            var response = await _client.GetAsync($"{GetIntegrationUrl()}/roles/{Uri.EscapeDataString(groupName)}/composite-roles");

            var groupedRoles = await response.HandleResponseAsync<ResponseWrapper<RoleModel>>();
            return groupedRoles;
        }

        public async Task<ResponseWrapper<RoleModel>> GetUserRoles(string username)
        {
            var response = await _client.GetAsync($"{GetIntegrationUrl()}/users/{Uri.EscapeDataString(username)}/roles");

            var groupedRoles = await response.HandleResponseAsync<ResponseWrapper<RoleModel>>();
            return groupedRoles;
        }

        public async Task<HttpResponseMessage> AddKeycloakRole(RoleModel role)
        {
            var response = await _client.PostJsonAsync($"{GetIntegrationUrl()}/roles", role);
            return response;
        }

        public async Task<HttpResponseMessage> AddKeycloakRolesToGroup(string groupName, IEnumerable<RoleModel> roles)
        {
            var response = await _client.PostJsonAsync($"{GetIntegrationUrl()}/roles/{Uri.EscapeDataString(groupName)}/composite-roles", roles);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteRole(string roleName)
        {
            var response = await _client.DeleteAsync($"{GetIntegrationUrl()}/roles/{Uri.EscapeDataString(roleName)}");
            return response;
        }

        public async Task<HttpResponseMessage> DeleteRoleFromGroup(string groupName, string roleName)
        {
            var response = await _client.DeleteAsync($"{GetIntegrationUrl()}/roles/{Uri.EscapeDataString(groupName)}/composite-roles/{Uri.EscapeDataString(roleName)}");
            return response;
        }

        private string GetIntegrationUrl()
        {
            return $"{this.Options.ServiceAccount.Api}/integrations/{this.Options.ServiceAccount.Integration}/{this.Options.ServiceAccount.Environment}";
        }
        #endregion

        #region Methods
        #endregion
    }
}
