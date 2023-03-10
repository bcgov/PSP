using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Entities.Comparers
{
    public class UserOrganizationOrganizationIdComparer : IEqualityComparer<PimsUserOrganization>
    {
        public bool Equals([AllowNull] PimsUserOrganization x, [AllowNull] PimsUserOrganization y)
        {
            return x != null && y != null && GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] PimsUserOrganization obj)
        {
            HashCode hashCode = default(HashCode);
            var hash = hashCode;
            hash.Add(obj.OrganizationId);
            hash.Add(obj.RoleId);
            return hash.ToHashCode();
        }
    }
}
