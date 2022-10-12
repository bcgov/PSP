using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Areas.Property.Models.Property;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Property.Controllers
{
    /// <summary>
    /// PropertyController class, provides endpoints for interacting with properties.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("properties")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PropertyController : ControllerBase
    {
        #region Variables
        private readonly IPimsRepository _pimsRepository;
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsRepository"></param>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public PropertyController(IPimsRepository pimsRepository, IPimsService pimsService, IMapper mapper)
        {
            _pimsRepository = pimsRepository;
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the property associations for the specified unique 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/associations")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyAssociationModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult GetPropertyAssociationsWithId(long id)
        {
            var property = _pimsRepository.Property.GetAssociations(id);

            return new JsonResult(_mapper.Map<PropertyAssociationModel>(property));
        }
        #endregion

        #region Concept Endpoints

        /// <summary>
        /// Get the property for the specified unique 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Pims.Api.Models.Concepts.PropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult GetConceptPropertyWithId(long id)
        {
            var property = _pimsService.PropertyService.GetById(id);
            return new JsonResult(_mapper.Map<Pims.Api.Models.Concepts.PropertyModel>(property));
        }

        /// <summary>
        /// Update the specified property, and attached properties.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Pims.Api.Models.Concepts.PropertyModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult UpdateConceptProperty([FromBody] Pims.Api.Models.Concepts.PropertyModel propertyModel)
        {
            var propertyEntity = _mapper.Map<Pims.Dal.Entities.PimsProperty>(propertyModel);
            var updatedProperty = _pimsService.PropertyService.Update(propertyEntity);

            return new JsonResult(_mapper.Map<Pims.Api.Models.Concepts.PropertyModel>(updatedProperty));
        }
        #endregion
    }
}
