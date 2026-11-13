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

        public static void ThrowIfCannotEditDispositionFile(this PimsDispositionFile dispositionFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository, ILookupRepository lookupRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(dispositionFile);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());

            // Check if contractor is not in team
            if (pimsUser?.IsContractor == true)
            {
                var project = dispositionFile.ProjectId.HasValue ? projectRepository.TryGet(dispositionFile.ProjectId.Value) : null;
                if (!dispositionFile.HasAccessToFile(pimsUser, project))
                {
                    throw new ContractorNotInTeamException("Contractor is not assigned to the Disposition File's team or the associated Project's team");
                }
            }

            // General edit access check
            if (!dispositionFile.CanEditDispositionFile(
                principal,
                userRepository,
                projectRepository,
                lookupRepository))
            {
                throw new NotAuthorizedException(
                    "User does not have edit access to this Disposition File");
            }
        }

        public static bool CanEditDispositionFile(this PimsDispositionFile dispositionFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository, ILookupRepository lookupRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(dispositionFile);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            var project = dispositionFile.ProjectId.HasValue ? projectRepository.TryGet(dispositionFile.ProjectId.Value) : null;
            var cannotDetermineRegion = lookupRepository.GetAllRegions().FirstOrDefault(x => x.RegionName == "Cannot determine");

            return CanEditDispositionFile(dispositionFile, pimsUser, project, cannotDetermineRegion?.RegionCode);
        }

        private static bool IsUserAssignedToDispositionFileRegion(this PimsDispositionFile dispositionFile, PimsUser pimsUser)
        {
            ArgumentNullException.ThrowIfNull(dispositionFile);
            return pimsUser?.PimsRegionUsers.Any(ur => ur.RegionCode == dispositionFile.RegionCode) ?? false;
        }

        private static bool CanEditDispositionFile(this PimsDispositionFile dispositionFile, PimsUser pimsUser, PimsProject project, short? cannotDetermineRegionCode)
        {
            if (dispositionFile is null || pimsUser is null)
            {
                return false;
            }

            var isOnDispositionTeam =
                dispositionFile.PimsDispositionFileTeams.Any(
                    x => x.PersonId == pimsUser.PersonId);

            var isOnProjectTeam =
                project != null &&
                project.PimsProjectPeople.Any(
                    x => x.PersonId == pimsUser.PersonId);

            if (pimsUser.IsContractor)
            {
                return (isOnDispositionTeam || isOnProjectTeam)
                    && dispositionFile.IsUserAssignedToDispositionFileRegion(pimsUser);
            }

            var isCannotDetermineRegion = cannotDetermineRegionCode != null && dispositionFile.RegionCode == cannotDetermineRegionCode;

            return isCannotDetermineRegion
                || dispositionFile.IsUserAssignedToDispositionFileRegion(pimsUser)
                || isOnDispositionTeam
                || isOnProjectTeam;
        }

    }
}
