using System.Collections.Generic;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class FinancialCodeService : BaseService, IFinancialCodeService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IBusinessFunctionCodeRepository _businessFunctionRepository;
        private readonly IChartOfAccountsCodeRepository _chartOfAccountsRepository;
        private readonly IYearlyFinancialCodeRepository _yearlyFinancialRepository;
        private readonly ICostTypeCodeRepository _costTypeRepository;
        private readonly IFinancialActivityCodeRepository _financialActivityRepository;
        private readonly IWorkActivityCodeRepository _workActivityRepository;
        private readonly IResponsibilityCodeRepository _responsibilityRepository;

        public FinancialCodeService(
            ClaimsPrincipal user,
            ILogger<FinancialCodeService> logger,
            IMapper mapper,
            IBusinessFunctionCodeRepository businessFunctionRepository,
            IChartOfAccountsCodeRepository chartOfAccountsRepository,
            IYearlyFinancialCodeRepository yearlyFinancialRepository,
            ICostTypeCodeRepository costTypeRepository,
            IFinancialActivityCodeRepository financialActivityRepository,
            IWorkActivityCodeRepository workActivityRepository,
            IResponsibilityCodeRepository responsibilityRepository)
            : base(user, logger)
        {
            _logger = logger;
            _mapper = mapper;
            _businessFunctionRepository = businessFunctionRepository;
            _chartOfAccountsRepository = chartOfAccountsRepository;
            _yearlyFinancialRepository = yearlyFinancialRepository;
            _costTypeRepository = costTypeRepository;
            _financialActivityRepository = financialActivityRepository;
            _workActivityRepository = workActivityRepository;
            _responsibilityRepository = responsibilityRepository;
        }

        public IList<FinancialCodeModel> GetAllFinancialCodes()
        {
            _logger.LogInformation("Getting all financial codes");
            User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            var businessFunctions = _mapper.Map<FinancialCodeModel[]>(_businessFunctionRepository.GetAllBusinessFunctionCodes());
            var chartOfAccounts = _mapper.Map<FinancialCodeModel[]>(_chartOfAccountsRepository.GetAllChartOfAccountCodes());
            var yearlyFinancials = _mapper.Map<FinancialCodeModel[]>(_yearlyFinancialRepository.GetAllYearlyFinancialCodes());
            var costTypes = _mapper.Map<FinancialCodeModel[]>(_costTypeRepository.GetAllCostTypeCodes());
            var financialActivities = _mapper.Map<FinancialCodeModel[]>(_financialActivityRepository.GetAllFinancialActivityCodes());
            var workActivities = _mapper.Map<FinancialCodeModel[]>(_workActivityRepository.GetAllWorkActivityCodes());
            var responsibilities = _mapper.Map<FinancialCodeModel[]>(_responsibilityRepository.GetAllResponsibilityCodes());

            var financialCodes = new List<FinancialCodeModel>();
            financialCodes.AddRange(businessFunctions);
            financialCodes.AddRange(chartOfAccounts);
            financialCodes.AddRange(yearlyFinancials);
            financialCodes.AddRange(costTypes);
            financialCodes.AddRange(financialActivities);
            financialCodes.AddRange(workActivities);
            financialCodes.AddRange(responsibilities);

            return financialCodes;
        }

        public FinancialCodeModel Add(FinancialCodeTypes type, FinancialCodeModel model)
        {
            _logger.LogInformation("Adding financial code with type {type} and model {model}", type, model);
            User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            switch (type)
            {
                case FinancialCodeTypes.BusinessFunction:
                    var pimsEntity = _mapper.Map<PimsBusinessFunctionCode>(model);
                    var createdEntity = _businessFunctionRepository.Add(pimsEntity);
                    _businessFunctionRepository.CommitTransaction();
                    return _mapper.Map<FinancialCodeModel>(createdEntity);
                default:
                    throw new BadRequestException("Financial code type not valid.");
            }
        }
    }
}
