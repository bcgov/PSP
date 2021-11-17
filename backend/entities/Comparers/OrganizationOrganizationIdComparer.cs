using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Entities.Comparers
{
    public class OrganizationOrganizationIdComparer : IEqualityComparer<PimsUserOrganization>
    {
        public bool Equals([AllowNull] PimsUserOrganization x, [AllowNull] PimsUserOrganization y)
        {
            return x != null && y != null && GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] PimsUserOrganization obj)
        {
            var hash = new HashCode();
            hash.Add(obj.OrganizationId);
            return hash.ToHashCode();
        }
    }
}
