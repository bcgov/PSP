using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class AcquisitionFileService : IAcquisitionFileService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IAcquisitionFileRepository _acqFileRepository;
        private readonly IUserRepository _userRepository;

        public AcquisitionFileService(
            ClaimsPrincipal user,
            ILogger<AcquisitionFileService> logger,
            IAcquisitionFileRepository acqFileRepository,
            IUserRepository userRepository)
        {
            _user = user;
            _logger = logger;
            _acqFileRepository = acqFileRepository;
            _userRepository = userRepository;
        }

        public Paged<PimsAcquisitionFile> GetPage(AcquisitionFilter filter)
        {
            _logger.LogInformation("Searching for acquisition files...");
            _logger.LogDebug("Acquisition file search with filter", filter);

            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            // Limit search results to user's assigned region(s)
            var pimsUser = _userRepository.GetUserInfo(_user.GetUserKey());
            var userRegions = pimsUser.PimsRegionUsers.Select(r => r.RegionCode).ToHashSet();

            return _acqFileRepository.GetPage(filter, userRegions);
        }

        public PimsAcquisitionFile GetById(long id)
        {
            _logger.LogInformation("Getting acquisition file with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            var acqFile = _acqFileRepository.GetById(id);
            return acqFile;
        }

        public PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile)
        {
            acquisitionFile.ThrowIfNull(nameof(acquisitionFile));

            _logger.LogInformation("Adding acquisition file with id {id}", acquisitionFile.Id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileAdd);

            acquisitionFile.AcquisitionFileStatusTypeCode = "ACTIVE";

            var newAcqFile = _acqFileRepository.Add(acquisitionFile);
            _acqFileRepository.CommitTransaction();
            return newAcqFile;
        }

        public PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile)
        {
            acquisitionFile.ThrowIfNull(nameof(acquisitionFile));

            _logger.LogInformation("Updating acquisition file with id {id}", acquisitionFile.Id);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);

            ValidateVersion(acquisitionFile.Id, acquisitionFile.ConcurrencyControlNumber);

            // TODO: Implementation pending
            throw new System.NotImplementedException();
        }

        private void ValidateVersion(long acqFileId, long acqFileVersion)
        {
            long currentRowVersion = _acqFileRepository.GetRowVersion(acqFileId);
            if (currentRowVersion != acqFileVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this acquisition file, please refresh the application and retry.");
            }
        }
    }
}
