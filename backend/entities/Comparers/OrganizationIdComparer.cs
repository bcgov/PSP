using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Entities.Comparers
{
    public class OrganizationIdComparer : IEqualityComparer<Organization>
    {
        public bool Equals([AllowNull] Organization x, [AllowNull] Organization y)
        {
            return x != null && y != null && GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] Organization obj)
        {
            var hash = new HashCode();
            hash.Add(obj.Id);
            return hash.ToHashCode();
        }
    }
}
