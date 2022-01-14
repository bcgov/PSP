using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Core.Exceptions;
using Pims.Dal;
using Pims.Dal.Security;
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
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PersonController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public OrganizationController(IPimsService pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Get the contact for the specified primary key 'id'.
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
            var otherCountry = _pimsService.Lookup.GetCountries().FirstOrDefault(x => x.Code == Dal.Entities.CountryCodes.Other);
            foreach (var address in model?.Addresses)
            {
                if (otherCountry != null && address != null && address.CountryId != otherCountry.CountryId)
                {
                    address.CountryOther = null;
                }
            }

            var entity = _mapper.Map<Dal.Entities.PimsOrganization>(model);

            // FIXME: Missed requirements lead to hardcoding these values here. This needs to be fixed next sprint!
            entity.OrganizationTypeCode = "OTHER";
            entity.OrgIdentifierTypeCode = "OTHINCORPNO";

            try
            {
                var createdOrganization = _pimsService.Organization.Add(entity, userOverride);
                var response = _mapper.Map<Areas.Contact.Models.Contact.OrganizationModel>(createdOrganization);

                return new JsonResult(response);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }
        #endregion
    }
}
