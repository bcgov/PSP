using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
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

        public AcquisitionFileService(
            ClaimsPrincipal user,
            ILogger<AcquisitionFileService> logger,
            IAcquisitionFileRepository acqFileRepository)
        {
            _user = user;
            _logger = logger;
            _acqFileRepository = acqFileRepository;
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
            _logger.LogInformation("Adding acquisition file...");
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileAdd);

            acquisitionFile.AcquisitionFileStatusTypeCode = "ACTIVE";

            var newAcqFile = _acqFileRepository.Add(acquisitionFile);
            _acqFileRepository.CommitTransaction();
            return newAcqFile;
        }

        public PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile)
        {
            _logger.LogInformation("Updating acquisition file...");
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
