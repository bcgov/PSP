using System;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Helpers.Extensions
{
    public static class ProjectExtensions
    {
        public static void ThrowMissingContractorInTeam(this PimsProject project, ClaimsPrincipal principal, IUserRepository userRepository)
        {
            ArgumentNullException.ThrowIfNull(project);
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            if (!project.HasAccessToFile(pimsUser))
            {
                throw new ContractorNotInTeamException("As a contractor, you must add yourself as a team member to the Project in order to create or save changes.");
            }
        }

        public static void ThrowContractorRemovedFromTeam(this PimsProject project, ClaimsPrincipal principal, IUserRepository userRepository)
        {
            ArgumentNullException.ThrowIfNull(project);
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            if (!project.HasAccessToFile(pimsUser))
            {
                throw new ContractorNotInTeamException("Contractors cannot remove themselves from a Project. Please contact the admin at pims@gov.bc.ca");
            }
        }

        public static bool HasAccessToFile(this PimsProject project, PimsUser pimsUser)
        {
            if (project is null || pimsUser is null)
            {
                return false;
            }

            if (pimsUser.IsContractor)
            {
                var onProject = project.PimsProjectPeople.Any(x => x.PersonId == pimsUser.PersonId);
                return onProject && pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == project.RegionCode);
            }

            // Regular (non-contractor) users have access across regions
            return true;
        }
    }
}
