using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

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
        private readonly IPimsRepository _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PropertyController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public PropertyController(IPimsRepository pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Get the property for the specified unique 'pid'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{pid}")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Property.PropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult GetPropertyWithPid(string pid)
        {
            var property = _pimsService.Property.GetForPID(pid);

            return new JsonResult(_mapper.Map<Models.Property.PropertyModel>(property));
        }

        /// <summary>
        /// Get the property for the specified primary key 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Property.PropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult GetProperty(int id)
        {
            var property = _pimsService.Property.Get(id);

            return new JsonResult(_mapper.Map<Models.Property.PropertyModel>(property));
        }
        #endregion
    }
}
