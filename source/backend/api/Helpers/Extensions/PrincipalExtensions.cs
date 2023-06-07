using System;
using System.Linq;
using System.Security.Claims;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;

namespace Pims.Core.Extensions
{
    public static class PrincipalExtensions
    {
        public static void ThrowInvalidAccessToAcquisitionFile(this ClaimsPrincipal principal, IUserRepository userRepository, IAcquisitionFileRepository acquisitionFileRepository, long acquisitionFileId)
        {
            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            var acquisitionFile = acquisitionFileRepository.GetById(acquisitionFileId);

            if (pimsUser?.IsContractor == true && !acquisitionFile.PimsAcquisitionFilePeople.Any(x => x.PersonId == pimsUser.PersonId))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Acquisition File's team");
            }
        }
    }
}
