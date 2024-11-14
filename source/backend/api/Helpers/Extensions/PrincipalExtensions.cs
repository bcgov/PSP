using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Exceptions;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Core.Extensions
{
    public static class PrincipalExtensions
    {
        public static void ThrowInvalidAccessToAcquisitionFile(this ClaimsPrincipal principal, IUserRepository userRepository, IAcquisitionFileRepository acquisitionFileRepository, long acquisitionFileId)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsAcquisitionFile acquisitionFile = acquisitionFileRepository.GetById(acquisitionFileId);

            if (pimsUser?.IsContractor == true && !acquisitionFile.PimsAcquisitionFileTeams.Any(x => x.PersonId == pimsUser.PersonId))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Acquisition File's team");
            }
        }

        public static void ThrowInvalidAccessToDispositionFile(this ClaimsPrincipal principal, IUserRepository userRepository, IDispositionFileRepository dispositionFileRepository, long dispositionFileId)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsDispositionFile dispostionFile = dispositionFileRepository.GetById(dispositionFileId);

            if (pimsUser?.IsContractor == true && !dispostionFile.PimsDispositionFileTeams.Any(x => x.PersonId == pimsUser.PersonId))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Disposition File's team");
            }
        }

        public static HashSet<short> GetUserRegions(this ClaimsPrincipal principal, IUserRepository userRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(userRepository);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            return pimsUser.PimsRegionUsers.Select(r => r.RegionCode).ToHashSet();
        }
    }
}
