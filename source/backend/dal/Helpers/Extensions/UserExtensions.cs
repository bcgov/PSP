using System.Linq;
using Pims.Dal.Exceptions;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// UserExtensions static class, provides an extensions methods for person entities.
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Get the expected keycloak idir username from a user's guid identifier.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetIdirUsername(this PimsUser user)
        {
            return $"{user.GuidIdentifierValue.ToString().Replace("-", string.Empty)}@idir";
        }

        public static void ThrowInvalidAccessToLeaseFile(this PimsUser pimsUser, short? leaseRegionCode)
        {
            if (leaseRegionCode.HasValue && !pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == leaseRegionCode))
            {
                throw new NotAuthorizedException("User is not assigned to the Lease File's region");
            }
        }
    }
}
