using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class CompReqH120Service : ICompReqH120Service
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly ICompReqH120Repository _compReqH120Repository;

        public CompReqH120Service(ClaimsPrincipal user, ILogger<CompReqH120Service> logger, ICompReqH120Repository compReqH120Repository)
        {
            _user = user;
            _logger = logger;
            _compReqH120Repository = compReqH120Repository;
        }

        public IEnumerable<PimsCompReqH120> GetAllByAcquisitionFileId(long acquisitionFileId, bool? finalOnly)
        {
            _logger.LogInformation($"Getting pims comp req h120s by {acquisitionFileId}", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            return _compReqH120Repository.GetAllByAcquisitionFileId(acquisitionFileId, finalOnly);
        }

    }
}
