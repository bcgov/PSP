using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Pims.Core.Exceptions;
using Pims.Core.Http.Configuration;
using Pims.Core.Security;

namespace Pims.Core.Extensions
{
    /// <summary>
    /// IdentityExtensions static class, provides extension methods for user identity.
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Get the currently logged in user's ClaimTypes.NameIdentifier.
        /// Return an empty Guid if no user is logged in.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Guid GetUserKey(this ClaimsPrincipal user)
        {
            var value = user?.FindFirstValue("idir_user_guid");
            return Guid.TryParse(value, out var newGuid) ? newGuid : Guid.Empty;
        }

        /// <summary>
        /// Get the user's list of organizations they have access to.
        /// Return 'null' if no organizations are found.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static long[] GetOrganizations(this ClaimsPrincipal user, string delimiter = ",")
        {
            var organizations = user?.FindAll("organizations");
            var results = new List<long>();

            organizations?.ForEach(c =>
            {
                var split = c.Value.Split(delimiter);
                results.AddRange(split.Select(v => long.TryParse(v, out long value) ? value : (long?)null).NotNull().Select(v => (long)v));
            });

            return results.ToArray();
        }

        /// <summary>
        /// Get the user's list of organizations they have access to.
        /// Return 'null' if no organizations are found.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static long?[] GetOrganizationsAsNullable(this ClaimsPrincipal user, string delimiter = ",")
        {
            var organizations = user?.FindAll("organizations");
            var results = new List<long?>();

            organizations?.ForEach(c =>
            {
                var split = c.Value.Split(delimiter);
                results.AddRange(split.Select(v => long.TryParse(v, out long value) ? value : (long?)null));
            });

            return results.ToArray();
        }

        /// <summary>
        /// Get the user's username.
        /// Extracts the username from the Keycloak value (username@idir).
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var value = user?.FindFirstValue("idir_username");
            return value?.Split("@").First();
        }

        /// <summary>
        /// Get the user's directory.
        /// Extracts the user directory from the Keycloak value (username@idir).
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserDirectory(this ClaimsPrincipal user)
        {
            var value = user?.FindFirstValue("identity_provider");
            return value;
        }

        /// <summary>
        /// Get the user's display name.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetDisplayName(this ClaimsPrincipal user)
        {
            var value = user?.FindFirstValue("name");
            return value;
        }

        /// <summary>
        /// Get the user's first name.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetFirstName(this ClaimsPrincipal user)
        {
            var value = user?.FindFirstValue(ClaimTypes.GivenName);
            return value;
        }

        /// <summary>
        /// Get the user's last name.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetLastName(this ClaimsPrincipal user)
        {
            var value = user?.FindFirstValue(ClaimTypes.Surname);
            return value;
        }

        /// <summary>
        /// Get the user's email.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetEmail(this ClaimsPrincipal user)
        {
            var value = user?.FindFirstValue(ClaimTypes.Email);
            return value;
        }

        /// <summary>
        /// Determine if the user any of the specified roles.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns>True if the user has any of the roles.</returns>
        public static bool HasRole(this ClaimsPrincipal user, params string[] role)
        {
            ArgumentNullException.ThrowIfNull(role);

            if (role.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(role));
            }

            return user.Claims.Any(c => c.Type == ClaimTypes.Role && role.Contains(c.Value));
        }

        /// <summary>
        /// Determine if the user has all of the specified roles.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns>True if the user has all of the roles.</returns>
        public static bool HasRoles(this ClaimsPrincipal user, params string[] role)
        {
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            if (role.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(role));
            }

            var count = user.Claims.Count(c => c.Type == ClaimTypes.Role && role.Contains(c.Value));

            return count == role.Length;
        }

        /// <summary>
        /// Determine if the user any of the specified permission.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <returns>True if the user has any of the permission.</returns>
        public static bool HasPermission(this ClaimsPrincipal user, params Permissions[] permission)
        {
            ArgumentNullException.ThrowIfNull(permission);

            if (permission.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(permission));
            }

            var roles = permission.Select(r => r.GetName()).ToArray();
            return user.Claims.AsEnumerable().Any(c => c.Type == "client_roles" && roles.Contains(c.Value));
        }

        /// <summary>
        /// Determine if the user all of the specified permissions.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <returns>True if the user has all of the permissions.</returns>
        public static bool HasPermissions(this ClaimsPrincipal user, params Permissions[] permission)
        {
            ArgumentNullException.ThrowIfNull(permission);

            if (permission.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(permission));
            }

            var roles = permission.Select(r => r.GetName()).ToArray();
            var claims = user.Claims.Where(c => c.Type == "client_roles");
            return roles.All(r => claims.Any(c => c.Value == r));
        }

        /// <summary>
        /// Determine if the user is the keycloak service account for the API.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="keycloakOptions"></param>
        /// <returns>True if the user has any of the permission.</returns>
        public static bool IsServiceAccount(this ClaimsPrincipal user, IOptionsMonitor<AuthClientOptions> keycloakOptions)
        {
            return user.Claims.AsEnumerable().Any(c => c.Type == "clientId" && c.Value == keycloakOptions.CurrentValue.Client);
        }

        /// <summary>
        /// If the user does has not been authenticated throw a NotAuthorizedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <exception type="NotAuthorizedException">User does not have the specified 'role'.</exception>
        /// <returns></returns>
        public static ClaimsPrincipal ThrowIfNotAuthorized(this ClaimsPrincipal user, string message = null)
        {
            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new NotAuthorizedException(message);
            }

            return user;
        }

        /// <summary>
        /// If the user does not have the specified 'role' and is not the pims API service account, throw a NotAuthorizedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <param name="keycloakOptions"></param>
        /// <param name="message"></param>
        /// <exception type="NotAuthorizedException">User does not have the specified 'role'.</exception>
        /// <returns></returns>
        public static ClaimsPrincipal ThrowIfNotAuthorizedOrServiceAccount(this ClaimsPrincipal user, Permissions permission, IOptionsMonitor<AuthClientOptions> keycloakOptions, string message = null)
        {
            if (user == null || (!user.HasPermission(permission) && !user.IsServiceAccount(keycloakOptions)))
            {
                throw new NotAuthorizedException(message);
            }

            return user;
        }

        /// <summary>
        /// If the user does not have the specified 'role' throw a NotAuthorizedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <param name="message"></param>
        /// <exception type="NotAuthorizedException">User does not have the specified 'role'.</exception>
        /// <returns></returns>
        public static ClaimsPrincipal ThrowIfNotAuthorized(this ClaimsPrincipal user, string role, string message)
        {
            if (user == null || !user.HasRole(role))
            {
                throw new NotAuthorizedException(message);
            }

            return user;
        }

        /// <summary>
        /// If the user does not have the specified 'permission' throw a NotAuthorizedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <param name="message"></param>
        /// <exception type="NotAuthorizedException">User does not have the specified 'permission'.</exception>
        /// <returns></returns>
        public static ClaimsPrincipal ThrowIfNotAuthorized(this ClaimsPrincipal user, Permissions permission, string message = null)
        {
            if (user == null || !user.HasPermission(permission))
            {
                throw new NotAuthorizedException(message);
            }

            return user;
        }

        /// <summary>
        /// If the user does not have any of the specified 'permission' throw a NotAuthorizedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <exception type="NotAuthorizedException">User does not have the specified 'role'.</exception>
        /// <returns></returns>
        public static ClaimsPrincipal ThrowIfNotAuthorized(this ClaimsPrincipal user, params Permissions[] permission)
        {
            if (user == null || !user.HasPermission(permission))
            {
                throw new NotAuthorizedException();
            }

            return user;
        }

        /// <summary>
        /// If the user does not have the all specified 'permission' throw a NotAuthorizedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <param name="message"></param>
        /// <exception type="NotAuthorizedException">User does not have the specified 'permission'.</exception>
        /// <returns></returns>
        public static ClaimsPrincipal ThrowIfNotAllAuthorized(this ClaimsPrincipal user, Permissions permission, string message = null)
        {
            if (user == null || !user.HasPermissions(permission))
            {
                throw new NotAuthorizedException(message);
            }

            return user;
        }

        /// <summary>
        /// If the user does not have all of the specified 'permission' throw a NotAuthorizedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <exception type="NotAuthorizedException">User does not have the specified 'role'.</exception>
        /// <returns></returns>
        public static ClaimsPrincipal ThrowIfNotAllAuthorized(this ClaimsPrincipal user, params Permissions[] permission)
        {
            if (user == null || !user.HasPermissions(permission))
            {
                throw new NotAuthorizedException();
            }

            return user;
        }

        /// <summary>
        /// If the user does not have any of the specified 'permission' throw a NotAuthorizedException.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <param name="message"></param>
        /// <exception type="NotAuthorizedException">User does not have the specified 'role'.</exception>
        /// <returns></returns>
        public static ClaimsPrincipal ThrowIfNotAuthorized(this ClaimsPrincipal user, Permissions[] permission, string message = null)
        {
            if (user == null || !user.HasPermission(permission))
            {
                throw new NotAuthorizedException(message);
            }

            return user;
        }
    }
}
