using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Core.Api.Exceptions;
using Pims.Api.Models.Concepts.Property;
using Pims.Core.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Property.Controllers
{
    /// <summary>
    /// PropertyContactController class, provides endpoints for interacting with properties.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("properties")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PropertyContactController : ControllerBase
    {
        #region Variables
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyContactController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        ///
        public PropertyContactController(IPropertyService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the contacts for the property with 'propertyId'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{propertyId}/contacts")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PropertyContactModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetPropertyContacts(long propertyId)
        {
            var propertyContacts = _propertyService.GetContacts(propertyId);

            return new JsonResult(_mapper.Map<List<PropertyContactModel>>(propertyContacts));
        }

        /// <summary>
        /// Get the contact for the property with 'propertyId' and contact with "contactId".
        /// </summary>
        /// <returns></returns>
        [HttpGet("{propertyId}/contacts/{contactId}")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyContactModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetPropertyContacts(long propertyId, long contactId)
        {
            var propertyContact = _propertyService.GetContact(propertyId, contactId);

            return new JsonResult(_mapper.Map<PropertyContactModel>(propertyContact));
        }

        /// <summary>
        /// Create the specified property contact.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{propertyId}/contacts")]
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyContactModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult CreateContact(long propertyId, [FromBody] PropertyContactModel contactModel)
        {
            if (propertyId != contactModel.PropertyId)
            {
                throw new BadRequestException("Invalid property id.");
            }
            var contactEntity = _mapper.Map<PimsPropertyContact>(contactModel);
            var updatedProperty = _propertyService.CreateContact(contactEntity);

            return new JsonResult(_mapper.Map<PropertyContactModel>(updatedProperty));
        }

        /// <summary>
        /// Update the specified property contact.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{propertyId}/contacts/{contactId}")]
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyContactModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateContact(long propertyId, long contactId, [FromBody] PropertyContactModel contactModel)
        {
            if (propertyId != contactModel.PropertyId || contactId != contactModel.Id)
            {
                throw new BadRequestException("Invalid property contact identifiers.");
            }
            var contactEntity = _mapper.Map<PimsPropertyContact>(contactModel);
            var updatedProperty = _propertyService.UpdateContact(contactEntity);

            return new JsonResult(_mapper.Map<PropertyContactModel>(updatedProperty));
        }

        /// <summary>
        /// Deletes the property contact with the matching id.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="contactId">Used to identify the entity to delete.</param>
        /// <returns></returns>
        [HttpDelete("{propertyId}/contacts/{contactId}")]
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteContact([FromRoute] long propertyId, [FromRoute] long contactId)
        {
            var result = _propertyService.DeleteContact(contactId);
            return new JsonResult(result);
        }
        #endregion
    }
}
