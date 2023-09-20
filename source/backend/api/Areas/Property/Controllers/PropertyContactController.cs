using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
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
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyContactController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyRepository"></param>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        ///
        public PropertyContactController(IPropertyRepository propertyRepository, IPropertyService propertyService, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _propertyService = propertyService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the contacts for the property with 'id'.
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
            // var property = _propertyRepository.GetAllAssociationsById(propertyId);
            var propertyContacts = new List<PropertyContactModel>()
            {
                new PropertyContactModel()
                {
                    Id = 1,
                    Person = new PersonModel() { Id = 1, FirstName = "John", Surname = "Doe" },
                    Purpose="Test Purpouse",
                    },
                };

            return new JsonResult(propertyContacts);
        }
        #endregion

        #region Concept Endpoints

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
        public IActionResult UpdateConceptProperty([FromBody] PropertyModel propertyModel)
        {
            /*var propertyEntity = _mapper.Map<Pims.Dal.Entities.PimsProperty>(propertyModel);
            var updatedProperty = _propertyService.Update(propertyEntity);

            return new JsonResult(_mapper.Map<PropertyModel>(updatedProperty));*/
            return new JsonResult(string.Empty);
        }
        #endregion
    }
}
