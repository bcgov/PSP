using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Dal;
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
        private readonly IPimsRepository _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LookupController class.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        public LookupController(IPimsRepository pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Get all of the organization code values
        /// </summary>
        /// <returns></returns>
        [HttpGet("organizations")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.OrganizationModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lookup" })]
        public IActionResult GetOrganizations()
        {
            var organizationCodes = _mapper.Map<Model.OrganizationModel[]>(_pimsService.Lookup.GetOrganizations());
            return new JsonResult(organizationCodes.ToArray());
        }

        /// <summary>
        /// Get all of the role code values
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.RoleModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lookup" })]
        public IActionResult GetRoles()
        {
            var roleCodes = _mapper.Map<Model.RoleModel[]>(_pimsService.Lookup.GetRoles());
            return new JsonResult(roleCodes.ToArray());
        }

        /// <summary>
        /// Get all of the property classification code values
        /// </summary>
        /// <returns></returns>
        [HttpGet("property/classifications")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.LookupModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lookup" })]
        public IActionResult GetPropertyClassificationTypes()
        {
            var classifications = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetPropertyClassificationTypes());
            return new JsonResult(classifications.ToArray());
        }

        /// <summary>
        /// Get all of the code values
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.LookupModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lookup" })]
        public IActionResult GetAll()
        {
            var organizations = _mapper.Map<Model.OrganizationModel[]>(_pimsService.Lookup.GetOrganizations());
            var organizationTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetOrganizationTypes());
            var roleCodes = _mapper.Map<Model.RoleModel[]>(_pimsService.Lookup.GetRoles());
            var provinces = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetProvinces());
            var countries = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetCountries());
            var regions = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetRegions());
            var districts = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetDistricts());
            var classificationTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetPropertyClassificationTypes());
            var areaUnitTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetPropertyAreaUnitTypes());
            var tenureTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetPropertyTenureTypes());
            var propertyTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetPropertyTypes());
            var leasePaymentReceivableTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetPaymentReceivableTypes());
            var leaseProgramTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetLeaseProgramTypes());
            var leaseStatusTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetLeaseStatusTypes());
            var leaseTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetLeaseTypes());
            var leaseCategoryTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetLeaseCategoryTypes());
            var leasePurposeTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetLeasePurposeTypes());
            var leaseResponsibilityTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetLeaseResponsibilityTypes());
            var leaseInitiatorTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetLeaseInitiatorTypes());
            var leaseTermStatusTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetLeaseTermStatusTypes());
            var leasePaymentFrequencyTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetLeasePmtFreqTypes());
            var insuranceTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetInsuranceTypes());
            var contactMethodTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetContactMethodTypes());
            var propertyImprovementTypes = _mapper.Map<Model.LookupModel[]>(_pimsService.Lookup.GetPropertyImprovementTypes());

            var codes = new List<object>();
            codes.AddRange(roleCodes);
            codes.AddRange(organizations);
            codes.AddRange(organizationTypes);
            codes.AddRange(countries);
            codes.AddRange(provinces);
            codes.AddRange(regions);
            codes.AddRange(districts);
            codes.AddRange(classificationTypes);
            codes.AddRange(areaUnitTypes);
            codes.AddRange(tenureTypes);
            codes.AddRange(propertyTypes);
            codes.AddRange(leasePaymentReceivableTypes);
            codes.AddRange(leaseProgramTypes);
            codes.AddRange(leaseStatusTypes);
            codes.AddRange(leaseTypes);
            codes.AddRange(leaseCategoryTypes);
            codes.AddRange(leasePurposeTypes);
            codes.AddRange(leaseResponsibilityTypes);
            codes.AddRange(leaseInitiatorTypes);
            codes.AddRange(leaseTermStatusTypes);
            codes.AddRange(leasePaymentFrequencyTypes);
            codes.AddRange(insuranceTypes);
            codes.AddRange(contactMethodTypes);
            codes.AddRange(propertyImprovementTypes);

            return new JsonResult(codes);
        }
        #endregion
    }
}
