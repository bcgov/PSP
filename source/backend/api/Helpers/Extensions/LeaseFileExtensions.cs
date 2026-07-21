using System;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Helpers.Extensions
{
    public static class LeaseFileExtensions
    {
        public static void ThrowInvalidRegion(this PimsLease leaseFile, ClaimsPrincipal principal, IUserRepository userRepository)
        {
            if (!leaseFile.IsAssignedToLeaseFileRegion(principal, userRepository))
            {
                throw new NotAuthorizedException("User is not assigned to the Lease File's region");
            }
        }

        public static bool HasAccessToLeaseFile(this PimsLease leaseFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(leaseFile);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            var project = leaseFile.ProjectId.HasValue ? projectRepository.TryGet(leaseFile.ProjectId.Value) : null;
            return leaseFile.HasAccessToLeaseFile(pimsUser, project);
        }

        public static void ThrowMissingContractorInTeam(this PimsLease leaseFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(leaseFile);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            var project = leaseFile.ProjectId.HasValue ? projectRepository.TryGet(leaseFile.ProjectId.Value) : null;

            if (!leaseFile.HasAccessToLeaseFile(pimsUser, project))
            {
                throw new ContractorNotInTeamException("As a contractor, you must add yourself as a team member to the Lease File in order to create or save changes");
            }
        }

        public static void ThrowContractorRemovedFromTeam(this PimsLease leaseFile, ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(leaseFile);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            var project = leaseFile.ProjectId.HasValue ? projectRepository.TryGet(leaseFile.ProjectId.Value) : null;

            if (!leaseFile.HasAccessToLeaseFile(pimsUser, project))
            {
                throw new ContractorNotInTeamException("Contractors cannot remove themselves from a Lease File. Please contact the admin at pims@gov.bc.ca");
            }
        }

        public static bool IsAssignedToLeaseFileRegion(this PimsLease leaseFile, ClaimsPrincipal principal, IUserRepository userRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(leaseFile);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            return leaseFile.IsUserAssignedToLeaseRegion(pimsUser);
        }

        private static bool IsUserAssignedToLeaseRegion(this PimsLease leaseFile, PimsUser pimsUser)
        {
            ArgumentNullException.ThrowIfNull(leaseFile);

            if (pimsUser is null)
            {
                return false;
            }

            if (pimsUser.IsContractor)
            {
                // Contractors can only access leases with a valid region
                return leaseFile.RegionCode != null && pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == leaseFile.RegionCode.Value);
            }
            else
            {
                // For ministry staff, they can access leases without any region attached.
                return leaseFile.RegionCode == null || pimsUser.PimsRegionUsers.Any(ur => ur.RegionCode == leaseFile.RegionCode.Value);
            }
        }

        private static bool HasAccessToLeaseFile(this PimsLease leaseFile, PimsUser pimsUser, PimsProject project)
        {
            if (leaseFile is null || pimsUser is null)
            {
                return false;
            }

            if (pimsUser.IsContractor)
            {
                // Contractors must be assigned to the Lease File's team or the associated Project's team,
                // AND must be assigned to the file's region.
                var onTeamOrProject = leaseFile.PimsLeaseLicenseTeams.Any(x => x.PersonId == pimsUser.PersonId)
                    || (project != null && project.PimsProjectPeople.Any(x => x.PersonId == pimsUser.PersonId));

                return onTeamOrProject && leaseFile.IsUserAssignedToLeaseRegion(pimsUser);
            }
            else
            {
                // Regular (non-contractor) users only need access to the lease region
                return leaseFile.IsUserAssignedToLeaseRegion(pimsUser);
            }
        }
    }
}
