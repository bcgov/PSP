using System.Collections.Generic;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Core.Extensions;
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

        public IList<FinancialCodeModel> GetFinancialCodesByType(FinancialCodeTypes type)
        {
            _logger.LogInformation("Getting financial codes with type {type}", type);
            User.ThrowIfNotAuthorized(Permissions.ProjectView);

            return type switch
            {
                FinancialCodeTypes.BusinessFunction => _mapper.Map<FinancialCodeModel[]>(_businessFunctionRepository.GetAllBusinessFunctionCodes()),
                FinancialCodeTypes.ChartOfAccounts => _mapper.Map<FinancialCodeModel[]>(_chartOfAccountsRepository.GetAllChartOfAccountCodes()),
                FinancialCodeTypes.YearlyFinancial => _mapper.Map<FinancialCodeModel[]>(_yearlyFinancialRepository.GetAllYearlyFinancialCodes()),
                FinancialCodeTypes.CostType => _mapper.Map<FinancialCodeModel[]>(_costTypeRepository.GetAllCostTypeCodes()),
                FinancialCodeTypes.FinancialActivity => _mapper.Map<FinancialCodeModel[]>(_financialActivityRepository.GetAllFinancialActivityCodes()),
                FinancialCodeTypes.WorkActivity => _mapper.Map<FinancialCodeModel[]>(_workActivityRepository.GetAllWorkActivityCodes()),
                FinancialCodeTypes.Responsibility => _mapper.Map<FinancialCodeModel[]>(_responsibilityRepository.GetAllResponsibilityCodes()),
                _ => throw new BadRequestException("Financial code type must be a known type"),
            };
        }

        public FinancialCodeModel GetById(FinancialCodeTypes type, long codeId)
        {
            _logger.LogInformation("Retrieving financial code with type {type} and Id {codeId}", type, codeId);
            User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            return type switch
            {
                FinancialCodeTypes.BusinessFunction => _mapper.Map<FinancialCodeModel>(_businessFunctionRepository.GetById(codeId)),
                FinancialCodeTypes.ChartOfAccounts => _mapper.Map<FinancialCodeModel>(_chartOfAccountsRepository.GetById(codeId)),
                FinancialCodeTypes.YearlyFinancial => _mapper.Map<FinancialCodeModel>(_yearlyFinancialRepository.GetById(codeId)),
                FinancialCodeTypes.CostType => _mapper.Map<FinancialCodeModel>(_costTypeRepository.GetById(codeId)),
                FinancialCodeTypes.FinancialActivity => _mapper.Map<FinancialCodeModel>(_financialActivityRepository.GetById(codeId)),
                FinancialCodeTypes.WorkActivity => _mapper.Map<FinancialCodeModel>(_workActivityRepository.GetById(codeId)),
                FinancialCodeTypes.Responsibility => _mapper.Map<FinancialCodeModel>(_responsibilityRepository.GetById(codeId)),
                _ => throw new BadRequestException("Financial code type must be a known type"),
            };
        }

        public FinancialCodeModel Add(FinancialCodeTypes type, FinancialCodeModel model)
        {
            model.ThrowIfNull(nameof(model));

            _logger.LogInformation("Adding financial code with type {type} and model {model}", type, model);
            User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            switch (type)
            {
                case FinancialCodeTypes.BusinessFunction:
                    {
                        var pimsEntity = _mapper.Map<PimsBusinessFunctionCode>(model);
                        var createdEntity = _businessFunctionRepository.Add(pimsEntity);
                        _businessFunctionRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(createdEntity);
                    }
                case FinancialCodeTypes.ChartOfAccounts:
                    {
                        var pimsEntity = _mapper.Map<PimsChartOfAccountsCode>(model);
                        var createdEntity = _chartOfAccountsRepository.Add(pimsEntity);
                        _chartOfAccountsRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(createdEntity);
                    }
                case FinancialCodeTypes.YearlyFinancial:
                    {
                        var pimsEntity = _mapper.Map<PimsYearlyFinancialCode>(model);
                        var createdEntity = _yearlyFinancialRepository.Add(pimsEntity);
                        _yearlyFinancialRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(createdEntity);
                    }
                case FinancialCodeTypes.CostType:
                    {
                        var pimsEntity = _mapper.Map<PimsCostTypeCode>(model);
                        var createdEntity = _costTypeRepository.Add(pimsEntity);
                        _costTypeRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(createdEntity);
                    }
                case FinancialCodeTypes.FinancialActivity:
                    {
                        var pimsEntity = _mapper.Map<PimsFinancialActivityCode>(model);
                        var createdEntity = _financialActivityRepository.Add(pimsEntity);
                        _financialActivityRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(createdEntity);
                    }
                case FinancialCodeTypes.WorkActivity:
                    {
                        var pimsEntity = _mapper.Map<PimsWorkActivityCode>(model);
                        var createdEntity = _workActivityRepository.Add(pimsEntity);
                        _workActivityRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(createdEntity);
                    }
                case FinancialCodeTypes.Responsibility:
                    {
                        var pimsEntity = _mapper.Map<PimsResponsibilityCode>(model);
                        var createdEntity = _responsibilityRepository.Add(pimsEntity);
                        _responsibilityRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(createdEntity);
                    }

                default:
                    throw new BadRequestException("Financial code type must be a known type");
            }
        }

        public FinancialCodeModel Update(FinancialCodeTypes type, FinancialCodeModel model)
        {
            model.ThrowIfNull(nameof(model));

            _logger.LogInformation("Updating financial code with type {type} and model {model}", type, model);
            User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            ValidateVersion(type, model.Id, model.RowVersion);

            switch (type)
            {
                case FinancialCodeTypes.BusinessFunction:
                    {
                        var pimsEntity = _mapper.Map<PimsBusinessFunctionCode>(model);
                        var updatedEntity = _businessFunctionRepository.Update(pimsEntity);
                        _businessFunctionRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(updatedEntity);
                    }
                case FinancialCodeTypes.ChartOfAccounts:
                    {
                        var pimsEntity = _mapper.Map<PimsChartOfAccountsCode>(model);
                        var updatedEntity = _chartOfAccountsRepository.Update(pimsEntity);
                        _chartOfAccountsRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(updatedEntity);
                    }
                case FinancialCodeTypes.YearlyFinancial:
                    {
                        var pimsEntity = _mapper.Map<PimsYearlyFinancialCode>(model);
                        var updatedEntity = _yearlyFinancialRepository.Update(pimsEntity);
                        _yearlyFinancialRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(updatedEntity);
                    }
                case FinancialCodeTypes.CostType:
                    {
                        var pimsEntity = _mapper.Map<PimsCostTypeCode>(model);
                        var updatedEntity = _costTypeRepository.Update(pimsEntity);
                        _costTypeRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(updatedEntity);
                    }
                case FinancialCodeTypes.FinancialActivity:
                    {
                        var pimsEntity = _mapper.Map<PimsFinancialActivityCode>(model);
                        var updatedEntity = _financialActivityRepository.Update(pimsEntity);
                        _financialActivityRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(updatedEntity);
                    }
                case FinancialCodeTypes.WorkActivity:
                    {
                        var pimsEntity = _mapper.Map<PimsWorkActivityCode>(model);
                        var updatedEntity = _workActivityRepository.Update(pimsEntity);
                        _workActivityRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(updatedEntity);
                    }
                case FinancialCodeTypes.Responsibility:
                    {
                        var pimsEntity = _mapper.Map<PimsResponsibilityCode>(model);
                        var updatedEntity = _responsibilityRepository.Update(pimsEntity);
                        _responsibilityRepository.CommitTransaction();
                        return _mapper.Map<FinancialCodeModel>(updatedEntity);
                    }

                default:
                    throw new BadRequestException("Financial code type must be a known type");
            }
        }

        private void ValidateVersion(FinancialCodeTypes type, long id, long? rowVersion)
        {
            long currentRowVersion = type switch
            {
                FinancialCodeTypes.BusinessFunction => _businessFunctionRepository.GetRowVersion(id),
                FinancialCodeTypes.ChartOfAccounts => _chartOfAccountsRepository.GetRowVersion(id),
                FinancialCodeTypes.YearlyFinancial => _yearlyFinancialRepository.GetRowVersion(id),
                FinancialCodeTypes.CostType => _costTypeRepository.GetRowVersion(id),
                FinancialCodeTypes.FinancialActivity => _financialActivityRepository.GetRowVersion(id),
                FinancialCodeTypes.WorkActivity => _workActivityRepository.GetRowVersion(id),
                FinancialCodeTypes.Responsibility => _responsibilityRepository.GetRowVersion(id),
                _ => throw new BadRequestException("Financial code type must be a known type"),
            };

            if (currentRowVersion != rowVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this financial code, please refresh the application and retry.");
            }
        }
    }
}
