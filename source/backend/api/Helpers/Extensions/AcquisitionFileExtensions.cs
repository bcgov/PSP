using System;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;

namespace Pims.Api.Helpers.Extensions
{
    public static class AcquisitionFileExtensions
    {
        public static void ThrowMissingContractorInTeam(this PimsAcquisitionFile acquisitionFile, ClaimsPrincipal principal, IUserRepository userRepository)
        {
            if (acquisitionFile is null)
            {
                throw new ArgumentNullException(nameof(acquisitionFile));
            }

            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());

            if (pimsUser?.IsContractor == true && !acquisitionFile.PimsAcquisitionFilePeople.Any(x => x.PersonId == pimsUser.PersonId))
            {
                throw new ContractorNotInTeamException("As a Contractor your user contact information should be assigned to the Acquisition File's team");
            }
        }

        public static void ThrowContractorRemovedFromTeam(this PimsAcquisitionFile acquisitionFile, ClaimsPrincipal principal, IUserRepository userRepository)
        {
            if (acquisitionFile is null)
            {
                throw new ArgumentNullException(nameof(acquisitionFile));
            }

            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());

            if (pimsUser?.IsContractor == true && !acquisitionFile.PimsAcquisitionFilePeople.Any(x => x.PersonId == pimsUser.PersonId))
            {
                throw new UserOverrideException(UserOverrideCode.ContractorSelfRemoved, "Contractors cannot remove themselves from a file. Please contact the admin at pims@gov.bc.ca");
            }
        }
    }
}
