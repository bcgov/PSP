using System;
using System.Collections.Generic;
using System.Linq;

namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// A read-only model that provides information about the current PIMS user.
    /// </summary>
    public class UserContextModel
    {
        public bool IsContractor { get; }

        public long PersonId { get; }

        public string FirstName { get; }

        public string Surname { get; }

        public string FullName => $"{FirstName} {Surname}".Trim();

        public HashSet<short> Regions { get; } = new HashSet<short>();

        public UserContextModel(PimsUser pimsUser)
        {
            IsContractor = pimsUser.IsContractor;
            PersonId = pimsUser.PersonId;
            FirstName = pimsUser.Person?.FirstName ?? string.Empty;
            Surname = pimsUser.Person?.Surname ?? string.Empty;
            Regions = pimsUser.PimsRegionUsers?.Select(ru => ru.RegionCode)?.ToHashSet() ?? new HashSet<short>();
        }

        public static UserContextModel FromPimsUser(PimsUser pimsUser)
        {
            ArgumentNullException.ThrowIfNull(nameof(pimsUser));
            return new UserContextModel(pimsUser);
        }
    }
}
