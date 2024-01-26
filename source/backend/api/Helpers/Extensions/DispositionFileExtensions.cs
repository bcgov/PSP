using System;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Helpers.Extensions
{
    public static class DispositionFileExtensions
    {
        public static void ThrowMissingContractorInTeam(this PimsDispositionFile dispositionFile, ClaimsPrincipal principal, IUserRepository userRepository)
        {
            ArgumentNullException.ThrowIfNull(dispositionFile);

            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());

            if (pimsUser?.IsContractor == true && !dispositionFile.PimsDispositionFileTeams.Any(x => x.PersonId == pimsUser.PersonId))
            {
                throw new ContractorNotInTeamException("As a contractor, you must add yourself as a team member to the file in order to create or save changes.");
            }
        }

        public static void ThrowContractorRemovedFromTeam(this PimsDispositionFile dispositionFile, ClaimsPrincipal principal, IUserRepository userRepository)
        {
            ArgumentNullException.ThrowIfNull(dispositionFile);

            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());

            if (pimsUser?.IsContractor == true && !dispositionFile.PimsDispositionFileTeams.Any(x => x.PersonId == pimsUser.PersonId))
            {
                throw new ContractorNotInTeamException("Contractors cannot remove themselves from a Disposition file. Please contact the admin at pims@gov.bc.ca");
            }
        }
    }
}
