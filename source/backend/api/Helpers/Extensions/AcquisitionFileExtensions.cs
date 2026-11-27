using System;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;

namespace Pims.Api.Helpers.Extensions
{
    public static class AcquisitionFileExtensions
    {
        public static void ThrowMissingContractorInTeam(this PimsAcquisitionFile acquisitionFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(acquisitionFile);
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsProject project = acquisitionFile.ProjectId.HasValue ? projectRepository.TryGet(acquisitionFile.ProjectId.Value) : null;

            if (!acquisitionFile.HasAccessToFile(pimsUser, project))
            {
                throw new ContractorNotInTeamException("As a Contractor your user contact information should be assigned to the Acquisition File's team");
            }
        }

        public static void ThrowContractorRemovedFromTeam(this PimsAcquisitionFile acquisitionFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(acquisitionFile);
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsProject project = acquisitionFile.ProjectId.HasValue ? projectRepository.TryGet(acquisitionFile.ProjectId.Value) : null;

            if (!acquisitionFile.HasAccessToFile(pimsUser, project))
            {
                throw new UserOverrideException(UserOverrideCode.ContractorSelfRemoved, "Contractors cannot remove themselves from a file. Please contact the admin at pims@gov.bc.ca");
            }
        }

        public static void ThrowContractorLegacyFileForbidden(this PimsAcquisitionFile acquisitionFile, ClaimsPrincipal principal, IUserRepository userRepository)
        {
            ArgumentNullException.ThrowIfNull(acquisitionFile);
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            if (pimsUser?.IsContractor == true && (acquisitionFile.OverrideFileNumberSequence
                || acquisitionFile.FileNo != null))
            {
                throw new BadRequestException("Contractors cannot create Acquisition files with legacy numbers. Please contact the admin at pims@gov.bc.ca");
            }
        }

        public static bool HasAccessToFile(this PimsAcquisitionFile acquisitionFile, PimsUser pimsUser, PimsProject project)
        {
            if (acquisitionFile is null || pimsUser is null)
            {
                return false;
            }

            if (pimsUser.IsContractor)
            {
                var onTeamOrProject = acquisitionFile.PimsAcquisitionFileTeams.Any(x => x.PersonId == pimsUser.PersonId)
                    || (project != null && project.PimsProjectPeople.Any(x => x.PersonId == pimsUser.PersonId));

                return onTeamOrProject && pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == acquisitionFile.RegionCode);
            }

            // Regular (non-contractor) users have access across regions
            return true;
        }

        public static void ThrowIfCannotEditAcquisitionFile(this PimsAcquisitionFile acquisitionFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository, ILookupRepository lookupRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(acquisitionFile);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(
                principal.GetUserKey());

            // Check if contractor is not in team
            if (pimsUser?.IsContractor == true)
            {
                if (!acquisitionFile.HasAccessToFile(pimsUser, acquisitionFile.Project))
                {
                    throw new ContractorNotInTeamException(
                        "Contractor is not assigned to the Acquisition File's team or the associated Project's team");
                }
            }

            // General edit access check
            if (!acquisitionFile.CanEditAcquisitionFile(
                principal,
                userRepository,
                projectRepository,
                lookupRepository))
            {
                throw new NotAuthorizedException(
                    "User does not have edit access to this Acquisition File");
            }
        }

        public static bool CanEditAcquisitionFile(this PimsAcquisitionFile acquisitionFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository, ILookupRepository lookupRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(acquisitionFile);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(
                principal.GetUserKey());

            var project = acquisitionFile.Project;

            var cannotDetermineRegion = lookupRepository.GetAllRegions()
                .FirstOrDefault(x => x.RegionName == "Cannot determine");

            return acquisitionFile.CanEditAcquisitionFile(
                pimsUser,
                project,
                cannotDetermineRegion?.RegionCode);
        }

        private static bool CanEditAcquisitionFile(this PimsAcquisitionFile acquisitionFile, PimsUser pimsUser, PimsProject project, short? cannotDetermineRegionCode)
        {
            if (acquisitionFile is null || pimsUser is null)
            {
                return false;
            }

            var isOnAcquisitionTeam =
                acquisitionFile.PimsAcquisitionFileTeams.Any(
                    x => x.PersonId == pimsUser.PersonId);

            var isOnProjectTeam =
                project != null &&
                project.PimsProjectPeople.Any(
                    x => x.PersonId == pimsUser.PersonId);

            if (pimsUser.IsContractor)
            {
                return (isOnAcquisitionTeam || isOnProjectTeam)
                    && pimsUser.PimsRegionUsers.Any(
                        ur => ur.RegionCode == acquisitionFile.RegionCode);
            }

            var isCannotDetermineRegion =
                cannotDetermineRegionCode != null &&
                acquisitionFile.RegionCode == cannotDetermineRegionCode;

            return isCannotDetermineRegion
                || pimsUser.PimsRegionUsers.Any(
                    ur => ur.RegionCode == acquisitionFile.RegionCode)
                || isOnAcquisitionTeam
                || isOnProjectTeam;
        }
    }
}
