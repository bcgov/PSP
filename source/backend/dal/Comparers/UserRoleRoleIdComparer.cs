using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Entities.Comparers
{
    public class UserRoleRoleIdComparer : IEqualityComparer<PimsUserRole>
    {
        public bool Equals([AllowNull] PimsUserRole x, [AllowNull] PimsUserRole y)
        {
            return x != null && y != null && GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] PimsUserRole obj)
        {
            HashCode hashCode = default(HashCode);
            var hash = hashCode;
            hash.Add(obj.RoleId);
            return hash.ToHashCode();
        }
    }
}
