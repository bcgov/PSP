using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Pims.Core.Extensions;
using Pims.Keycloak.Extensions;
using Pims.Keycloak.Models;

namespace Pims.Keycloak
{
    /// <summary>
    /// KeycloakService class, provides a service for sending HTTP requests to the keycloak admin API.
    ///     - https://api.loginproxy.gov.bc.ca/openapi/swagger#/ .
    /// </summary>
    public partial class KeycloakService : IKeycloakService
    {
        #region Methods

        /// <summary>
        /// Get the user for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Models.UserModel> GetUserAsync(Guid id)
        {
            var response = await _client.GetAsync($"{this.Options.ServiceAccount.Api}/{this.Options.ServiceAccount.Environment}/idir/users?guid={id.ToString().Replace("-", string.Empty)}");
            var result = await response.HandleResponseAsync<ResponseWrapper<Models.UserModel>>();

            return result.Data.FirstOrDefault();
        }

        /// <summary>
        /// Get an array of the groups the user for the specified 'id' is a member of.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Models.RoleModel[]> GetUserGroupsAsync(Guid id)
        {
            var response = await _client.GetAsync($"{this.Options.ServiceAccount.Api}/integrations/{this.Options.ServiceAccount.Integration}/{this.Options.ServiceAccount.Environment}/user-role-mappings/?username={id.ToString().Replace("-", string.Empty)}@idir");

            var userRoleModel = await response.HandleResponseAsync<Models.UserRoleModel>();

            return userRoleModel.Roles.Where(r => r.Composite).ToArray();
        }

        /// <summary>
        /// Get the total number of groups the user for the specified 'id' is a member of.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> GetUserGroupCountAsync(Guid id)
        {
            var response = await GetUserGroupsAsync(id);
            return response.Length;
        }

        /// <summary>
        /// execute all passed operations.
        /// </summary>
        /// <param name="operations"></param>
        /// <returns></returns>
        public async Task ModifyUserRoleMappings(IEnumerable<UserRoleOperation> operations)
        {
            foreach (UserRoleOperation operation in operations)
            {
                var json = operation.Serialize();
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{this.Options.ServiceAccount.Api}/integrations/{this.Options.ServiceAccount.Integration}/{this.Options.ServiceAccount.Environment}/user-role-mappings", content);
                await response.HandleResponseAsync<Models.UserRoleModel>();
            }
        }
        #endregion
    }
}
