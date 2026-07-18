using System;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Helpers.Extensions
{
    public static class ManagementFileExtensions
    {
        public static void ThrowMissingContractorInTeam(this PimsManagementFile managementFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(managementFile);
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsProject project = managementFile.ProjectId.HasValue ? projectRepository.TryGet(managementFile.ProjectId.Value) : null;

            if (!managementFile.HasAccessToFile(pimsUser, project))
            {
                throw new ContractorNotInTeamException("As a contractor, you must add yourself as a team member to the file in order to create or save changes.");
            }
        }

        public static void ThrowContractorRemovedFromTeam(this PimsManagementFile managementFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(managementFile);
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsProject project = managementFile.ProjectId.HasValue ? projectRepository.TryGet(managementFile.ProjectId.Value) : null;

            if (!managementFile.HasAccessToFile(pimsUser, project))
            {
                throw new ContractorNotInTeamException("Contractors cannot remove themselves from a Management file. Please contact the admin at pims@gov.bc.ca");
            }
        }

        public static bool IsUserAssignedToManagementFileRegion(this PimsManagementFile managementFile, PimsUser pimsUser)
        {
            if (pimsUser is null || managementFile is null)
            {
                return false;
            }

            if (pimsUser.IsContractor)
            {
                return managementFile.RegionCode != null && pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == managementFile.RegionCode.Value);
            }

            // Regular (non-contractor) users are not bound by region access here.
            return true;
        }

        public static bool HasAccessToFile(this PimsManagementFile managementFile, PimsUser pimsUser, PimsProject project)
        {
            if (managementFile is null || pimsUser is null)
            {
                return false;
            }

            if (pimsUser.IsContractor)
            {
                var onTeamOrProject = managementFile.PimsManagementFileTeams.Any(x => x.PersonId == pimsUser.PersonId)
                    || (project != null && project.PimsProjectPeople.Any(x => x.PersonId == pimsUser.PersonId));

                return onTeamOrProject && managementFile.IsUserAssignedToManagementFileRegion(pimsUser);
            }

            // Regular (non-contractor) users have access across regions
            return true;
        }
    }
}
