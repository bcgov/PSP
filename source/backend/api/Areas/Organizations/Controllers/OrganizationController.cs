using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.Organization;
using Pims.Core.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Json;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Organizations.Controllers
{
    /// <summary>
    /// OrganizationController class, provides endpoints for interacting with organization contacts.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("organizations")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class OrganizationController : ControllerBase
    {
        #region Variables
        private readonly IOrganizationService _organizationService;
        private readonly ILookupRepository _lookupRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PersonController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="organizationService"></param>
        /// <param name="lookupRepository"></param>
        /// <param name="mapper"></param>
        ///
        public OrganizationController(IOrganizationService organizationService, ILookupRepository lookupRepository, IMapper mapper)
        {
            _organizationService = organizationService;
            _lookupRepository = lookupRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the organization for the specified primary key 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrganizationModel), 200)]
        [SwaggerOperation(Tags = new[] { "organization" })]
        public IActionResult GetOrganization(int id)
        {
            var organization = _organizationService.GetOrganization(id);
            return new JsonResult(_mapper.Map<OrganizationModel>(organization));
        }

        /// <summary>
        /// Get the organization concept for the specified primary key 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("concept/{id:long}")]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrganizationModel), 200)]
        [SwaggerOperation(Tags = new[] { "organization" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetOrganizationConcept(int id)
        {
            var organization = _organizationService.GetOrganization(id);
            return new JsonResult(_mapper.Map<OrganizationModel>(organization));
        }

        /// <summary>
        /// Add the organization to the datastore.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.ContactAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrganizationModel), 201)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "organization" })]
        public IActionResult AddOrganization([FromBody] OrganizationModel model, bool userOverride = false)
        {
            // Business rule - support country free-form value if country code is "Other". Ignore field otherwise.
            var otherCountry = _lookupRepository.GetAllCountries().FirstOrDefault(x => x.Code == Dal.Entities.CountryCodes.Other);
            if (model?.OrganizationAddresses != null)
            {
                foreach (var organizationAddress in model?.OrganizationAddresses)
                {
                    if (otherCountry != null && organizationAddress?.Address != null && organizationAddress.Address.CountryId != otherCountry.CountryId)
                    {
                        organizationAddress.Address.CountryOther = null;
                    }
                }
            }

            var entity = _mapper.Map<Dal.Entities.PimsOrganization>(model);
            try
            {
                var createdOrganization = _organizationService.AddOrganization(entity, userOverride);
                var response = _mapper.Map<OrganizationModel>(createdOrganization);

                return new JsonResult(response);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }

        /// <summary>
        /// Update the specified organization, and attached properties.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [HasPermission(Permissions.ContactEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrganizationModel), 200)]
        [SwaggerOperation(Tags = new[] { "person" })]
        public IActionResult UpdateOrganization([FromBody] OrganizationModel organizationModel)
        {
            var orgEntity = _mapper.Map<Pims.Dal.Entities.PimsOrganization>(organizationModel);
            var updatedOrganization = _organizationService.UpdateOrganization(orgEntity, organizationModel.RowVersion);

            return new JsonResult(_mapper.Map<OrganizationModel>(updatedOrganization));
        }
        #endregion
    }
}
