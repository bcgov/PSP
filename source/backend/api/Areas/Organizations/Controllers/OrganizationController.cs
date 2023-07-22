using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using Concepts = Pims.Api.Models.Concepts;

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
        [ProducesResponseType(typeof(Models.Organization.OrganizationModel), 200)]
        [SwaggerOperation(Tags = new[] { "organization" })]
        public IActionResult GetOrganization(int id)
        {
            var organization = _organizationService.GetOrganization(id);
            return new JsonResult(_mapper.Map<Models.Organization.OrganizationModel>(organization));
        }

        /// <summary>
        /// Get the organization concept for the specified primary key 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("concept/{id:long}")]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Concepts.OrganizationModel), 200)]
        [SwaggerOperation(Tags = new[] { "organization" })]
        public IActionResult GetOrganizationConcept(int id)
        {
            var organization = _organizationService.GetOrganization(id);
            return new JsonResult(_mapper.Map<Concepts.OrganizationModel>(organization));
        }

        /// <summary>
        /// Add the organization to the datastore.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.ContactAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Areas.Contact.Models.Contact.ContactModel), 201)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "organization" })]
        public IActionResult AddOrganization([FromBody] Models.Organization.OrganizationModel model, bool userOverride = false)
        {
            // Business rule - support country free-form value if country code is "Other". Ignore field otherwise.
            var otherCountry = _lookupRepository.GetAllCountries().FirstOrDefault(x => x.Code == Dal.Entities.CountryCodes.Other);
            foreach (var address in model?.Addresses)
            {
                if (otherCountry != null && address != null && address.CountryId != otherCountry.CountryId)
                {
                    address.CountryOther = null;
                }
            }

            var entity = _mapper.Map<Dal.Entities.PimsOrganization>(model);
            try
            {
                var createdOrganization = _organizationService.AddOrganization(entity, userOverride);
                var response = _mapper.Map<Areas.Contact.Models.Contact.OrganizationModel>(createdOrganization);

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
        [ProducesResponseType(typeof(Models.Organization.OrganizationModel), 200)]
        [SwaggerOperation(Tags = new[] { "person" })]
        public IActionResult UpdateOrganization([FromBody] Models.Organization.OrganizationModel organizationModel)
        {
            var orgEntity = _mapper.Map<Pims.Dal.Entities.PimsOrganization>(organizationModel);
            var updatedOrganization = _organizationService.UpdateOrganization(orgEntity, organizationModel.RowVersion);

            return new JsonResult(_mapper.Map<Models.Organization.OrganizationModel>(updatedOrganization));
        }
        #endregion
    }
}
