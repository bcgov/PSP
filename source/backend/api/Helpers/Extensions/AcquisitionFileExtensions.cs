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
    }
}
