using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Exceptions;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Core.Extensions
{
    public static class PrincipalExtensions
    {
        public static void ThrowInvalidAccessToAcquisitionFile(this ClaimsPrincipal principal, IUserRepository userRepository, IAcquisitionFileRepository acquisitionFileRepository, long acquisitionFileId)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsAcquisitionFile acquisitionFile = acquisitionFileRepository.GetById(acquisitionFileId);

            if (pimsUser?.IsContractor == true && !acquisitionFile.PimsAcquisitionFileTeams.Any(x => x.PersonId == pimsUser.PersonId) && (acquisitionFile.Project == null || !acquisitionFile.Project.PimsProjectPeople.Any(x => x.PersonId == pimsUser.PersonId)))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Acquisition File's team or the associated Project's team");
            }
        }

        public static void ThrowInvalidAccessToDispositionFile(this ClaimsPrincipal principal, IUserRepository userRepository, IDispositionFileRepository dispositionFileRepository, long dispositionFileId)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsDispositionFile dispostionFile = dispositionFileRepository.GetById(dispositionFileId);

            if (pimsUser?.IsContractor == true && !dispostionFile.PimsDispositionFileTeams.Any(x => x.PersonId == pimsUser.PersonId))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Disposition File's team");
            }
        }

        public static void ThrowInvalidAccessToLeaseFile(this ClaimsPrincipal principal, IUserRepository userRepository, ILeaseRepository leaseRepository, IProjectRepository projectRepository, long leaseFileId)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var leaseFile = leaseRepository.GetNoTracking(leaseFileId);
            principal.ThrowInvalidAccessToLeaseFile(leaseFile, userRepository, projectRepository);
        }

        public static void ThrowInvalidAccessToLeaseFile(this ClaimsPrincipal principal, PimsLease leaseFile, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(leaseFile);

            // Check region access
            principal.ThrowInvalidRegion(leaseFile, userRepository);

            // Check team/project access for contractors
            if (!principal.HasAccessToLeaseFile(leaseFile, userRepository, projectRepository))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Lease File's team or the associated Project's team");
            }
        }

        public static void ThrowInvalidRegion(this ClaimsPrincipal principal, PimsLease leaseFile, IUserRepository userRepository)
        {
            if (!principal.IsAssignedToLeaseFileRegion(leaseFile, userRepository))
            {
                throw new NotAuthorizedException("User is not assigned to the Lease File's region");
            }
        }

        public static void ThrowMissingContractorInTeam(this ClaimsPrincipal principal, PimsLease leaseFile, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(leaseFile);

            if (!principal.HasAccessToLeaseFile(leaseFile, userRepository, projectRepository))
            {
                throw new ContractorNotInTeamException("As a contractor, you must add yourself as a team member to the Lease File in order to create or save changes");
            }
        }

        public static void ThrowContractorRemovedFromTeam(this ClaimsPrincipal principal, PimsLease leaseFile, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(leaseFile);

            if (!principal.HasAccessToLeaseFile(leaseFile, userRepository, projectRepository))
            {
                throw new ContractorNotInTeamException("Contractors cannot remove themselves from a Lease File. Please contact the admin at pims@gov.bc.ca");
            }
        }

        public static bool IsAssignedToLeaseFileRegion(this ClaimsPrincipal principal, PimsLease leaseFile, IUserRepository userRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(leaseFile);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());

            // User is not assigned to the Lease File's region
            if (leaseFile.RegionCode.HasValue && !pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == leaseFile.RegionCode))
            {
                return false;
            }

            return true;
        }

        public static bool HasAccessToLeaseFile(this ClaimsPrincipal principal, PimsLease leaseFile, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(leaseFile);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());

            PimsProject project = null;
            if (leaseFile.ProjectId.HasValue)
            {
                project = projectRepository.TryGet(leaseFile.ProjectId.Value);
            }

            // Contractors must be assigned to the Lease File's team or the associated Project's team.
            if (pimsUser?.IsContractor == true && !leaseFile.PimsLeaseLicenseTeams.Any(x => x.PersonId == pimsUser.PersonId) && (project == null || !project.PimsProjectPeople.Any(x => x.PersonId == pimsUser.PersonId)))
            {
                return false;
            }

            return true;
        }

        public static HashSet<short> GetUserRegions(this ClaimsPrincipal principal, IUserRepository userRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(userRepository);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            return pimsUser?.PimsRegionUsers?.Select(r => r.RegionCode)?.ToHashSet();
        }
    }
}
