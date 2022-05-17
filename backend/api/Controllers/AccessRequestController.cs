using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Exceptions;
using Pims.Dal;
using Swashbuckle.AspNetCore.Annotations;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.AccessRequest;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// AccessRequestController class, provides endpoints for managing access requests.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}")]
    [Route("")]
    public class AccessRequestController : ControllerBase
    {
        #region Variables
        private readonly IPimsRepository _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a AccessRequestController class.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        public AccessRequestController(IPimsRepository pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Get the most recent access request for the current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("access/requests")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.AccessRequestModel), 200)]
        [ProducesResponseType(204)]
        [SwaggerOperation(Tags = new[] { "user" })]
        public IActionResult GetAccessRequest()
        {
            var accessRequest = _pimsService.AccessRequest.Get();
            if (accessRequest == null)
            {
                return NoContent();
            }

            return new JsonResult(_mapper.Map<Model.AccessRequestModel>(accessRequest));
        }

        /// <summary>
        /// Get the most recent access request for the current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("access/requests/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.AccessRequestModel), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 403)]
        [SwaggerOperation(Tags = new[] { "user" })]
        public IActionResult GetAccessRequest(long id)
        {
            var accessRequest = _pimsService.AccessRequest.Get(id);
            return new JsonResult(_mapper.Map<Model.AccessRequestModel>(accessRequest));
        }

        /// <summary>
        /// Provides a way for a user to submit an access request to the system, associating a role and organization to their user.
        /// </summary>
        /// <returns></returns>
        [HttpPost("access/requests")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.AccessRequestModel), 201)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "user" })]
        public IActionResult AddAccessRequest([FromBody] Model.AccessRequestModel model)
        {
            if (model == null || model.RoleId == null)
            {
                throw new BadRequestException("Invalid access request specified");
            }

            var accessRequest = _mapper.Map<Entity.PimsAccessRequest>(model);
            accessRequest = _pimsService.AccessRequest.Add(accessRequest);

            return CreatedAtAction(nameof(GetAccessRequest), new { id = accessRequest.AccessRequestId }, _mapper.Map<Model.AccessRequestModel>(accessRequest));
        }

        /// <summary>
        /// Provides a way for a user to update their access request.
        /// </summary>
        /// <returns></returns>
        [HttpPut("access/requests/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.AccessRequestModel), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 403)]
        [SwaggerOperation(Tags = new[] { "user" })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Parameter 'id' is used for consistent routing.")]
        public IActionResult UpdateAccessRequest(long id, [FromBody] Model.AccessRequestModel model)
        {
            if (model == null || model.RoleId == null)
            {
                throw new BadRequestException("Invalid access request specified");
            }

            var accessRequest = _mapper.Map<Entity.PimsAccessRequest>(model);
            accessRequest = _pimsService.AccessRequest.Update(accessRequest);
            return new JsonResult(_mapper.Map<Model.AccessRequestModel>(accessRequest));
        }
        #endregion
    }
}
