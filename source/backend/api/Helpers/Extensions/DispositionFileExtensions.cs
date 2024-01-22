using System;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
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
                throw new UserOverrideException(UserOverrideCode.ContractorNotInTeam, "As a Contractor your user contact information should be assigned to the Disposition File's team");
            }
        }
    }
}
