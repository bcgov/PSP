using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Entities.Comparers
{
    public class OrganizationIdComparer : IEqualityComparer<PimsOrganization>
    {
        public bool Equals([AllowNull] PimsOrganization x, [AllowNull] PimsOrganization y)
        {
            return x != null && y != null && GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] PimsOrganization obj)
        {
            var hash = new HashCode();
            hash.Add(obj.Id);
            return hash.ToHashCode();
        }
    }
}
