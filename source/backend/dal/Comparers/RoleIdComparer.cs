using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pims.Dal.Entities.Comparers
{
    public class RoleIdComparer : IEqualityComparer<PimsRole>
    {
        public bool Equals([AllowNull] PimsRole x, [AllowNull] PimsRole y)
        {
            return x != null && y != null && GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode([DisallowNull] PimsRole obj)
        {
            var hash = default(HashCode);
            hash.Add(obj.RoleId);
            return hash.ToHashCode();
        }
    }
}
