using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using Pims.Keycloak.Configuration;

namespace Pims.Api.Policies
{
    /// <summary>
    /// PermissionFilter class, provides a authorization filter that validates the specified permissions.
    /// </summary>
    public class PermissionFilter : IAuthorizationFilter
    {
        #region Variables
        private readonly Permissions[] _permissions;
        private readonly IOptionsMonitor<KeycloakOptions> _keycloakOptions;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PermissionFilter class, initializes it with the specified permission.
        /// This will ensure the user has the specified permission.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="options"></param>
        public PermissionFilter(IOptionsMonitor<KeycloakOptions> options, Permissions permission)
        {
            _permissions = new[] { permission };
            _keycloakOptions = options;
        }

        /// <summary>
        /// Creates a new instance of a PermissionFilter class, initializes it with the specified permissions.
        /// This will ensure the user has at least one of the specified permissions.
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="options"></param>
        public PermissionFilter(IOptionsMonitor<KeycloakOptions> options, params Permissions[] permissions)
        {
            _permissions = permissions;
            _keycloakOptions = options;
        }
        #endregion

        #region Methods

        /// <summary>
        /// On the authorization trigger validated that the user has the specified claim permissions.
        /// If they do not return an HTTP 403.
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasRole = context.HttpContext.User.HasPermission(_permissions.ToArray());
            var isServiceAccount = context.HttpContext.User.IsServiceAccount(_keycloakOptions);
            if (!hasRole && !isServiceAccount)
            {
                context.Result = new ForbidResult();
            }
        }
        #endregion
    }
}
