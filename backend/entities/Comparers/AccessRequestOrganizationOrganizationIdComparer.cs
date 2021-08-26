using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Entities.Comparers
{
    public class AccessRequestOrganizationOrganizationIdComparer : IEqualityComparer<AccessRequestOrganization>
    {
        public bool Equals([AllowNull] AccessRequestOrganization x, [AllowNull] AccessRequestOrganization y)
        {
            return x != null && y != null && GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] AccessRequestOrganization obj)
        {
            var hash = new HashCode();
            hash.Add(obj.OrganizationId);
            return hash.ToHashCode();
        }
    }
}
