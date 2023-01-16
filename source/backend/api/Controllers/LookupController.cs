using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILookupRepository _lookupRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LookupController class.
        /// </summary>
        /// <param name="lookupRepository"></param>
        /// <param name="mapper"></param>
        public LookupController(ILookupRepository lookupRepository, IMapper mapper)
        {
            _lookupRepository = lookupRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get all of the role code values.
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.RoleModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lookup" })]
        public IActionResult GetRoles()
        {
            var roleCodes = _mapper.Map<Model.RoleModel[]>(_lookupRepository.GetRoles());
            return new JsonResult(roleCodes.ToArray());
        }

        /// <summary>
        /// Get all of the property classification code values.
        /// </summary>
        /// <returns></returns>
        [HttpGet("property/classifications")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.LookupModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lookup" })]
        public IActionResult GetPropertyClassificationTypes()
        {
            var classifications = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyClassificationTypes());
            return new JsonResult(classifications.ToArray());
        }

        /// <summary>
        /// Get all of the code values.
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.LookupModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lookup" })]
        public IActionResult GetAll()
        {
            var areaUnitTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyAreaUnitTypes());
            var classificationTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyClassificationTypes());
            var contactMethodTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetContactMethodTypes());
            var countries = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetCountries());
            var districts = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetDistricts());
            var insuranceTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetInsuranceTypes());
            var leaseCategoryTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeaseCategoryTypes());
            var leaseInitiatorTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeaseInitiatorTypes());
            var leasePaymentFrequencyTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeasePmtFreqTypes());
            var leasePaymentMethodTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeasePaymentMethodTypes());
            var leasePaymentReceivableTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPaymentReceivableTypes());
            var leasePaymentStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeasePaymentStatusTypes());
            var leaseProgramTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeaseProgramTypes());
            var leasePurposeTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeasePurposeTypes());
            var leaseResponsibilityTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeaseResponsibilityTypes());
            var leaseStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeaseStatusTypes());
            var leaseTermStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeaseTermStatusTypes());
            var leaseTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetLeaseTypes());
            var organizationTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetOrganizationTypes());
            var propertyImprovementTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyImprovementTypes());
            var propertyTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyTypes());
            var provinces = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetProvinces());
            var regions = _mapper.Map<Model.LookupModel<short>[]>(_lookupRepository.GetRegions());
            var roleCodes = _mapper.Map<Model.RoleModel[]>(_lookupRepository.GetRoles());
            var securityDepositTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetSecurityDepositTypes());
            var tenureTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyTenureTypes());
            var researchStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetResearchFileStatusTypes());
            var requestSourceTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GeRequestSourceTypes());
            var researchPurposeTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetResearchPurposeTypes());
            var propertyResearchPurposeTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyResearchPurposeTypes());
            var propertyAnomalyTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyAnomalyTypes());
            var propertyRoadTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyRoadTypes());
            var propertyAdjacentLandTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyAdjacentLandTypes());
            var volumeUnitTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyVolumeUnitTypes());
            var propertyVolumetricTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPropertyVolumetricTypes());
            var pphStatusType = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetPPHStatusType());
            var documentStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetDocumentStatusTypes());
            var acquisitionFileStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAcquisitionFileStatusTypes());
            var acquisitionPhysFileStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAcquisitionPhysFileStatusTypes());
            var acquisitionTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAcquisitionTypes());
            var activityTemplateTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetActivityTemplateTypes());
            var activityStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetActivityStatusTypes());
            var acqFilePersonProfileTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAcqFilePersonProfileTypes());
            var tenantTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetTenantTypes());
            var acqFundingTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetAcquisitionFundingTypes());
            var projectStatusTypes = _mapper.Map<Model.LookupModel[]>(_lookupRepository.GetProjectStatusTypes());

            var codes = new List<object>();
            codes.AddRange(areaUnitTypes);
            codes.AddRange(classificationTypes);
            codes.AddRange(contactMethodTypes);
            codes.AddRange(countries);
            codes.AddRange(districts);
            codes.AddRange(insuranceTypes);
            codes.AddRange(leaseCategoryTypes);
            codes.AddRange(leaseInitiatorTypes);
            codes.AddRange(leasePaymentFrequencyTypes);
            codes.AddRange(leasePaymentMethodTypes);
            codes.AddRange(leasePaymentReceivableTypes);
            codes.AddRange(leasePaymentStatusTypes);
            codes.AddRange(leaseProgramTypes);
            codes.AddRange(leasePurposeTypes);
            codes.AddRange(leaseResponsibilityTypes);
            codes.AddRange(leaseStatusTypes);
            codes.AddRange(leaseTermStatusTypes);
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
            codes.AddRange(propertyAdjacentLandTypes);
            codes.AddRange(volumeUnitTypes);
            codes.AddRange(propertyVolumetricTypes);
            codes.AddRange(pphStatusType);
            codes.AddRange(documentStatusTypes);
            codes.AddRange(acquisitionFileStatusTypes);
            codes.AddRange(acquisitionPhysFileStatusTypes);
            codes.AddRange(acquisitionTypes);
            codes.AddRange(activityTemplateTypes);
            codes.AddRange(activityStatusTypes);
            codes.AddRange(acqFilePersonProfileTypes);
            codes.AddRange(tenantTypes);
            codes.AddRange(acqFundingTypes);
            codes.AddRange(projectStatusTypes);

            return new JsonResult(codes);
        }
        #endregion
    }
}
