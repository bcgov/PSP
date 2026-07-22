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
        public static void ThrowMissingContractorInTeam(this PimsDispositionFile dispositionFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(dispositionFile);
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsProject project = dispositionFile.ProjectId.HasValue ? projectRepository.TryGet(dispositionFile.ProjectId.Value) : null;

            if (!dispositionFile.HasAccessToFile(pimsUser, project))
            {
                throw new ContractorNotInTeamException("As a contractor, you must add yourself as a team member to the file in order to create or save changes.");
            }
        }

        public static void ThrowContractorRemovedFromTeam(this PimsDispositionFile dispositionFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(dispositionFile);
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsProject project = dispositionFile.ProjectId.HasValue ? projectRepository.TryGet(dispositionFile.ProjectId.Value) : null;

            if (!dispositionFile.HasAccessToFile(pimsUser, project))
            {
                throw new ContractorNotInTeamException("Contractors cannot remove themselves from a Disposition file. Please contact the admin at pims@gov.bc.ca");
            }
        }

        public static bool HasAccessToFile(this PimsDispositionFile dispositionFile, PimsUser pimsUser, PimsProject project)
        {
            if (dispositionFile is null || pimsUser is null)
            {
                return false;
            }

            if (pimsUser.IsContractor)
            {
                var onTeamOrProject = dispositionFile.PimsDispositionFileTeams.Any(x => x.PersonId == pimsUser.PersonId)
                    || (project != null && project.PimsProjectPeople.Any(x => x.PersonId == pimsUser.PersonId));

                return onTeamOrProject && pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == dispositionFile.RegionCode);
            }

            // Regular (non-contractor) users have access across regions
            return true;
        }
    }
}
