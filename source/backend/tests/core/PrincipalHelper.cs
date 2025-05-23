using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Pims.Dal.Helpers.Extensions;
using Pims.Core.Security;

namespace Pims.Core.Test
{
    /// <summary>
    /// Principal static class, provides helper functions for principal identities.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class PrincipalHelper
    {
        /// <summary>
        /// Create a ClaimsPrincipal for the specified role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static ClaimsPrincipal CreateForRole(params string[] role)
        {
            role ??= new string[0];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            };

            foreach (var claim in role)
            {
                claims.Add(new Claim(ClaimTypes.Role, claim ?? "none"));
            }
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));

            return user;
        }

        /// <summary>
        /// Create a ClaimsPrincipal for the specified roles.
        /// Add it to the service collection in the test helper.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static ClaimsPrincipal CreateForRole(this TestHelper helper, params string[] role)
        {
            var user = CreateForRole(role);
            helper.AddSingleton(user);
            return user;
        }

        /// <summary>
        /// Create a ClaimsPrincipal for the specified permissions.
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public static ClaimsPrincipal CreateForPermission(params Permissions[] permission)
        {
            permission ??= new Permissions[0];

            var testGuid = Guid.NewGuid();
            var claims = new List<Claim>
            {
                new Claim("sub", $"{testGuid}@idir"),
                new Claim("idir_user_guid", testGuid.ToString().Replace("-", string.Empty)),
                new Claim("idir_username", "username@"),
                new Claim(ClaimTypes.Email, "test@test.com"),
            };

            foreach (var claim in permission)
            {
                claims.Add(new Claim("client_roles", claim.GetName()));
            }
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));

            return user;
        }

        /// <summary>
        /// Create a ClaimsPrincipal for the specified permission.
        /// Add it to the service collection in the test helper.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public static ClaimsPrincipal CreateForPermission(this TestHelper helper, params Permissions[] permission)
        {
            var user = CreateForPermission(permission);
            helper.AddSingleton(user);
            return user;
        }

        /// <summary>
        /// Add a claim to the specified 'user'.
        /// This will create a new user that has the claim.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ClaimsPrincipal AddClaim(this ClaimsPrincipal user, string type, string value)
        {
            var identity = new ClaimsIdentity(user.Identity);
            identity.AddClaim(new Claim(type, value));

            return new ClaimsPrincipal(identity);
        }

        /// <summary>
        /// Add the claim 'organizations' to the specified 'user'.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static ClaimsPrincipal AddOrganization(this ClaimsPrincipal user, params int[] organizationId)
        {
            var organizations = string.Join(",", organizationId);
            return user.AddClaim("organizations", organizations);
        }

        /// <summary>
        /// Add the claim 'organizations' to the specified 'user'.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="organizationIds"></param>
        /// <returns></returns>
        public static ClaimsPrincipal AddOrganization(this ClaimsPrincipal user, IEnumerable<int> organizationIds)
        {
            return user.AddClaim("organizations", string.Join(",", organizationIds));
        }
    }
}
