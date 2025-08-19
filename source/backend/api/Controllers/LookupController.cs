using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Pims.Api.Constants;
using Pims.Dal.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Model = Pims.Api.Models.Lookup;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// LookupController class, provides endpoints for code lookups.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/lookup")]
    [Route("lookup")]
    public class LookupController : ControllerBase
    {
        #region Variables
        private const int OneHourInSeconds = 60 * 60;
        private readonly ILookupRepository _lookupRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LookupController class.
        /// </summary>
        /// <param name="lookupRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="memoryCache"></param>
        public LookupController(ILookupRepository lookupRepository, IMapper mapper, IMemoryCache memoryCache)
        {
            _lookupRepository = lookupRepository;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get all of the code values.
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.LookupModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lookup" })]
        [ResponseCache(Duration = OneHourInSeconds)]
        public IActionResult GetAll()
        {

            if (!_memoryCache.TryGetValue(CacheKeys.Lookup, out JsonResult cachedLookupResponse))
            {
                var areaUnitTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPropertyAreaUnitTypes());
                var contactMethodTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllContactMethodTypes());
                var countries = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllCountries());
                var districts = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDistricts());
                var insuranceTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllInsuranceTypes());
                var leaseInitiatorTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeaseInitiatorTypes());
                var leasePaymentFrequencyTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeasePmtFreqTypes());
                var leasePaymentMethodTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeasePaymentMethodTypes());
                var leasePaymentReceivableTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPaymentReceivableTypes());
                var leasePaymentStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeasePaymentStatusTypes());
                var leaseProgramTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeaseProgramTypes());
                var leasePurposeTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeasePurposeTypes());
                var leaseResponsibilityTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeaseResponsibilityTypes());
                var leaseStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeaseStatusTypes());
                var leasePeriodStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeasePeriodStatusTypes());
                var leaseTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeaseTypes());
                var organizationTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllOrganizationTypes());
                var propertyImprovementTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPropertyImprovementTypes());
                var propertyTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPropertyTypes());
                var provinces = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllProvinces());
                var regions = _mapper.Map<Model.LookupModel<short>[]>(_lookupRepository.GetAllRegions());
                var roleCodes = _mapper.Map<Model.RoleModel[]>(_lookupRepository.GetAllRoles());
                var securityDepositTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllSecurityDepositTypes());
                var tenureTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPropertyTenureTypes());
                var researchStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllResearchFileStatusTypes());
                var requestSourceTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllRequestSourceTypes());
                var researchPurposeTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllResearchPurposeTypes());
                var propertyResearchPurposeTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPropertyResearchPurposeTypes());
                var propertyAnomalyTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPropertyAnomalyTypes());
                var propertyRoadTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPropertyRoadTypes());
                var volumeUnitTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPropertyVolumeUnitTypes());
                var propertyVolumetricTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPropertyVolumetricTypes());
                var propertyManagementPurposeTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPropertyManagementPurposeTypes());
                var pphStatusType = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllPPHStatusType());
                var documentStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDocumentStatusTypes());
                var acquisitionFileStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcquisitionFileStatusTypes());
                var acquisitionPhysFileStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcquisitionPhysFileStatusTypes());
                var acquisitionTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcquisitionTypes());
                var acqFilePersonProfileTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcqFileTeamProfileTypes());
                var leaseStakeholderTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeaseStakeholderTypes());
                var acqFundingTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcquisitionFundingTypes());
                var projectStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllProjectStatusTypes());
                var formTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllFormTypes());
                var consultationTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllConsultationTypes());
                var consultationStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllConsultationStatusTypes());
                var takeTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllTakeTypes());
                var takeStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllTakeStatusTypes());
                var takeSiteContamTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllTakeSiteContamTypes());
                var landActTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLandActTypes());
                var acqChecklistSectionTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcquisitionChecklistSectionTypes());
                var checklistItemStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllChecklistItemStatusTypes());
                var agreementTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAgreementTypes());
                var agreementStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAgreementStatusTypes());
                var interestHolderInterestTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllInterestHolderInterestTypes());
                var expropriationPaymentItemTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllExpropriationPaymentItemTypes());
                var mgmtActivityStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllMgmtActivityStatusTypes());
                var mgmtActivitySubtypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllMgmtActivitySubtypes());
                var mgmtActivityTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllMgmtActivityTypes());
                var dispositionStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDispositionStatusTypes());
                var dispositionFileFundingTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDispositionFileFundingTypes());
                var dispositionFileStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDispositionFileStatusTypes());
                var dispositionFlTeamProfileTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDispositionFlTeamProfileTypes());
                var dispositionInitiatingBranchTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDispositionInitiatingBranchTypes());
                var dispositionPhysFileStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDispositionPhysFileStatusTypes());
                var dispositionInitiatingDocTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDispositionInitiatingDocTypes());
                var dispositionTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDispositionTypes());
                var dispositionOfferStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDispositionOfferStatusTypes());
                var dispositionChecklistSectionTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllDispositionChecklistSectionTypes());
                var historicalNumberTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllHistoricalNumberTypes());
                var leaseChecklistSectionTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeaseChecklistSectionTypes());
                var leasePaymentCategoryTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLeasePaymentCategoryTypes());
                var consultationOutcomeTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllConsultationOutcomeTypes());
                var subfileInterestTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllSubfileInterestTypes());
                var acquisitionFileProgressStatuses = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcquisitionFileProgressStatusTypes());
                var acquisitionFileAppraisalStatuses = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcquisitionFileAppraisalStatusTypes());
                var acquisitionFileLegalSurveyStatuses = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcquisitionFileLegalSurveyStatusTypes());
                var acquisitionFileTakeTypesStatuses = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcquisitionFileTakeStatusTypes());
                var acquisitionFileExpropiationRiskStatuses = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllAcquisitionFileExpropiationRiskStatusTypes());
                var llTeamProfileTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllLlTeamProfileTypes());
                var expropriationEventTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllExpropriationEventTypes());
                var managementFilePurposeTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllManagementFilePurposeTypes());
                var managementFileStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllManagementFileStatusTypes());
                var managementFileProfileTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAllManagementFileProfileTypes());

                var codes = new List<object>();
                codes.AddRange(areaUnitTypes);
                codes.AddRange(contactMethodTypes);
                codes.AddRange(countries);
                codes.AddRange(districts);
                codes.AddRange(insuranceTypes);
                codes.AddRange(leaseInitiatorTypes);
                codes.AddRange(leasePaymentFrequencyTypes);
                codes.AddRange(leasePaymentMethodTypes);
                codes.AddRange(leasePaymentReceivableTypes);
                codes.AddRange(leasePaymentStatusTypes);
                codes.AddRange(leaseProgramTypes);
                codes.AddRange(leasePurposeTypes);
                codes.AddRange(leaseResponsibilityTypes);
                codes.AddRange(leaseStatusTypes);
                codes.AddRange(leasePeriodStatusTypes);
                codes.AddRange(leaseTypes);
                codes.AddRange(organizationTypes);
                codes.AddRange(propertyImprovementTypes);
                codes.AddRange(propertyTypes);
                codes.AddRange(provinces);
                codes.AddRange(regions);
                codes.AddRange(roleCodes);
                codes.AddRange(securityDepositTypes);
                codes.AddRange(tenureTypes);
                codes.AddRange(researchStatusTypes);
                codes.AddRange(requestSourceTypes);
                codes.AddRange(researchPurposeTypes);
                codes.AddRange(propertyResearchPurposeTypes);
                codes.AddRange(propertyAnomalyTypes);
                codes.AddRange(propertyRoadTypes);
                codes.AddRange(volumeUnitTypes);
                codes.AddRange(propertyVolumetricTypes);
                codes.AddRange(propertyManagementPurposeTypes);
                codes.AddRange(pphStatusType);
                codes.AddRange(documentStatusTypes);
                codes.AddRange(acquisitionFileStatusTypes);
                codes.AddRange(acquisitionPhysFileStatusTypes);
                codes.AddRange(acquisitionTypes);
                codes.AddRange(acqFilePersonProfileTypes);
                codes.AddRange(leaseStakeholderTypes);
                codes.AddRange(acqFundingTypes);
                codes.AddRange(projectStatusTypes);
                codes.AddRange(formTypes);
                codes.AddRange(consultationTypes);
                codes.AddRange(consultationStatusTypes);
                codes.AddRange(takeTypes);
                codes.AddRange(takeStatusTypes);
                codes.AddRange(takeSiteContamTypes);
                codes.AddRange(landActTypes);
                codes.AddRange(acqChecklistSectionTypes);
                codes.AddRange(checklistItemStatusTypes);
                codes.AddRange(agreementTypes);
                codes.AddRange(agreementStatusTypes);
                codes.AddRange(interestHolderInterestTypes);
                codes.AddRange(expropriationPaymentItemTypes);
                codes.AddRange(mgmtActivityStatusTypes);
                codes.AddRange(mgmtActivitySubtypes);
                codes.AddRange(mgmtActivityTypes);
                codes.AddRange(dispositionFileFundingTypes);
                codes.AddRange(dispositionFileStatusTypes);
                codes.AddRange(dispositionFlTeamProfileTypes);
                codes.AddRange(dispositionInitiatingBranchTypes);
                codes.AddRange(dispositionPhysFileStatusTypes);
                codes.AddRange(dispositionInitiatingDocTypes);
                codes.AddRange(dispositionTypes);
                codes.AddRange(dispositionStatusTypes);
                codes.AddRange(dispositionOfferStatusTypes);
                codes.AddRange(dispositionChecklistSectionTypes);
                codes.AddRange(historicalNumberTypes);
                codes.AddRange(leaseChecklistSectionTypes);
                codes.AddRange(leasePaymentCategoryTypes);
                codes.AddRange(consultationOutcomeTypes);
                codes.AddRange(subfileInterestTypes);
                codes.AddRange(acquisitionFileProgressStatuses);
                codes.AddRange(acquisitionFileAppraisalStatuses);
                codes.AddRange(acquisitionFileLegalSurveyStatuses);
                codes.AddRange(acquisitionFileTakeTypesStatuses);
                codes.AddRange(acquisitionFileExpropiationRiskStatuses);
                codes.AddRange(llTeamProfileTypes);
                codes.AddRange(expropriationEventTypes);
                codes.AddRange(managementFilePurposeTypes);
                codes.AddRange(managementFileStatusTypes);
                codes.AddRange(managementFileProfileTypes);

                var response = new JsonResult(codes);

                return _memoryCache.Set(CacheKeys.Lookup, response);
            }

            return cachedLookupResponse;
        }
        #endregion
    }
}
