using System;
using System.Security.Claims;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;

namespace Pims.Core.Extensions
{
    public static class PrincipalExtensions
    {
        public static void HasAccessToAcquisitionFile(this ClaimsPrincipal principal, IUserRepository userRepository, PimsAcquisitionFile acquisitionFile)
        {
            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            if (pimsUser.IsContractor && !acquisitionFile.PersonIsAssignedToFile(pimsUser.PersonId))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Acquisition File's team");
            }
        }

        public static void HasAccessToAcquisitionFile(this ClaimsPrincipal principal, IUserRepository userRepository, IAcquisitionFileRepository acquisitionFileRepository, long acquisitionFileId)
        {
            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            var acquisitionFile = acquisitionFileRepository.GetById(acquisitionFileId);

            if (pimsUser.IsContractor && !acquisitionFile.PersonIsAssignedToFile(pimsUser.PersonId))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Acquisition File's team");
            }
        }
    }
}
