using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Entities.Comparers
{
    public class AccessRequestOrganizationOrganizationIdComparer : IEqualityComparer<PimsAccessRequestOrganization>
    {
        public bool Equals([AllowNull] PimsAccessRequestOrganization x, [AllowNull] PimsAccessRequestOrganization y)
        {
            return x != null && y != null && GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] PimsAccessRequestOrganization obj)
        {
            var hash = new HashCode();
            hash.Add(obj.OrganizationId);
            return hash.ToHashCode();
        }
    }
}
