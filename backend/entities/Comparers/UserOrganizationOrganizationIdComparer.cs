using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Entities.Comparers
{
    public class UserOrganizationOrganizationIdComparer : IEqualityComparer<UserOrganization>
    {
        public bool Equals([AllowNull] UserOrganization x, [AllowNull] UserOrganization y)
        {
            return x != null && y != null && GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] UserOrganization obj)
        {
            var hash = new HashCode();
            hash.Add(obj.OrganizationId);
            hash.Add(obj.RoleId);
            return hash.ToHashCode();
        }
    }
}
