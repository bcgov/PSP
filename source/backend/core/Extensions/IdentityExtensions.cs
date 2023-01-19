using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

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
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (role.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(role));
            }

            var count = user.Claims.Count(c => c.Type == ClaimTypes.Role && role.Contains(c.Value));

            return count == role.Length;
        }
    }
}
