using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class CompReqFinancialService : ICompReqFinancialService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly ICompReqFinancialRepository _compReqFinancialRepository;

        public CompReqFinancialService(ClaimsPrincipal user, ILogger<CompReqFinancialService> logger, ICompReqFinancialRepository compReqFinancialRepository)
        {
            _user = user;
            _logger = logger;
            _compReqFinancialRepository = compReqFinancialRepository;
        }

        public IEnumerable<PimsCompReqFinancial> GetAllByAcquisitionFileId(long acquisitionFileId, bool? finalOnly)
        {
            _logger.LogInformation($"Getting pims comp req financials by {acquisitionFileId}", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            return _compReqFinancialRepository.GetAllByAcquisitionFileId(acquisitionFileId, finalOnly);
        }

        public IEnumerable<PimsCompReqFinancial> SearchCompensationRequisitionTransactions(AcquisitionReportFilterModel filter)
        {
            // TODO:
            throw new System.NotImplementedException();
        }
    }
}
