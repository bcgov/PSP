using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Helpers.Extensions
{
    public static class PrincipalExtensions
    {
        /// <summary>
        /// Contractors must be assigned to the Acquisition File's team (or associated Project's team) AND must be assigned to the file's region.
        /// Team/project membership from a different region does not grant access.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="userRepository"></param>
        /// <param name="acquisitionFileRepository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="acquisitionFileId"></param>
        public static void ThrowInvalidAccessToAcquisitionFile(this ClaimsPrincipal principal, IUserRepository userRepository, IAcquisitionFileRepository acquisitionFileRepository, IProjectRepository projectRepository, long acquisitionFileId)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsAcquisitionFile acquisitionFile = acquisitionFileRepository.GetById(acquisitionFileId);
            PimsProject project = acquisitionFile.ProjectId.HasValue ? projectRepository.TryGet(acquisitionFile.ProjectId.Value) : null;

            if (pimsUser?.IsContractor == true && !acquisitionFile.HasAccessToFile(pimsUser, project))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Acquisition File's team or the associated Project's team");
            }
        }

        /// <summary>
        /// Contractors must be assigned to the Disposition File's team (or associated Project's team) AND must be assigned to the file's region.
        /// Team membership from a different region does not grant access.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="userRepository"></param>
        /// <param name="dispositionFileRepository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="dispositionFileId"></param>
        public static void ThrowInvalidAccessToDispositionFile(this ClaimsPrincipal principal, IUserRepository userRepository, IDispositionFileRepository dispositionFileRepository, IProjectRepository projectRepository, long dispositionFileId)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsDispositionFile dispositionFile = dispositionFileRepository.GetById(dispositionFileId);
            PimsProject project = dispositionFile.ProjectId.HasValue ? projectRepository.TryGet(dispositionFile.ProjectId.Value) : null;

            if (pimsUser?.IsContractor == true && !dispositionFile.HasAccessToFile(pimsUser, project))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Disposition File's team or the associated Project's team");
            }
        }

        /// <summary>
        /// Contractors must be assigned to the Lease team (or associated Project's team) AND must be assigned to the file's region.
        /// Team membership from a different region does not grant access.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="userRepository"></param>
        /// <param name="leaseRepository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="leaseFileId"></param>
        public static void ThrowInvalidAccessToLeaseFile(this ClaimsPrincipal principal, IUserRepository userRepository, ILeaseRepository leaseRepository, IProjectRepository projectRepository, long leaseFileId)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var leaseFile = leaseRepository.GetNoTracking(leaseFileId);
            principal.ThrowInvalidAccessToLeaseFile(leaseFile, userRepository, projectRepository);
        }

        /// <summary>
        /// Contractors must be assigned to the Lease team (or associated Project's team) AND must be assigned to the file's region.
        /// Team membership from a different region does not grant access.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="leaseFile"></param>
        /// <param name="userRepository"></param>
        /// <param name="projectRepository"></param>
        public static void ThrowInvalidAccessToLeaseFile(this ClaimsPrincipal principal, PimsLease leaseFile, IUserRepository userRepository, IProjectRepository projectRepository)
        {
            ArgumentNullException.ThrowIfNull(principal);
            ArgumentNullException.ThrowIfNull(leaseFile);

            // Check team/project access (and, for contractors, region access) for this lease file.
            if (!leaseFile.HasAccessToLeaseFile(principal, userRepository, projectRepository))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Lease File's team or the associated Project's team");
            }
        }

        /// <summary>
        /// Contractors must be assigned to the Management File's team (or associated Project's team) AND must be assigned to the file's region.
        /// Team/project membership from a different region does not grant access.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="userRepository"></param>
        /// <param name="managementFileRepository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="managementFileId"></param>
        public static void ThrowInvalidAccessToManagementFile(this ClaimsPrincipal principal, IUserRepository userRepository, IManagementFileRepository managementFileRepository, IProjectRepository projectRepository, long managementFileId)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsManagementFile managementFile = managementFileRepository.GetById(managementFileId);
            PimsProject project = managementFile.ProjectId.HasValue ? projectRepository.TryGet(managementFile.ProjectId.Value) : null;

            if (pimsUser?.IsContractor == true && !managementFile.HasAccessToFile(pimsUser, project))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Management File's team or the associated Project's team");
            }
        }

        /// <summary>
        /// Contractors must be assigned to the Project's team AND must be assigned to the file's region.
        /// Team membership from a different region does not grant access.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="userRepository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="projectId"></param>
        public static void ThrowInvalidAccessToProject(this ClaimsPrincipal principal, IUserRepository userRepository, IProjectRepository projectRepository, long projectId)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var pimsUser = userRepository.GetUserInfoByKeycloakUserId(principal.GetUserKey());
            PimsProject project = projectRepository.TryGet(projectId);

            if (pimsUser?.IsContractor == true && project != null && !project.HasAccessToFile(pimsUser))
            {
                throw new NotAuthorizedException("Contractor is not assigned to the Project's team");
            }
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
