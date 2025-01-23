using System.Linq;
using Hangfire.Dashboard;
using Microsoft.Extensions.Options;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Keycloak.Configuration;

namespace Pims.Scheduler.Policies
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
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
        public HangfireDashboardAuthorizationFilter(IOptionsMonitor<KeycloakOptions> options, Permissions permission)
        {
            _permissions = new[] { permission };
            _keycloakOptions = options;
        }
        #endregion

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var hasRole = httpContext.User.HasPermission(_permissions.ToArray());
            var isServiceAccount = httpContext.User.IsServiceAccount(_keycloakOptions);
            return !hasRole && !isServiceAccount;
        }
    }
}
