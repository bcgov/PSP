using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Exceptions;
using Pims.Dal.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Entity = Pims.Dal.Entities;

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
        private readonly IAccessRequestRepository _accessRequestRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AccessRequestController class.
        /// </summary>
        /// <param name="accessRequestRepository"></param>
        /// <param name="mapper"></param>
        public AccessRequestController(IAccessRequestRepository accessRequestRepository, IMapper mapper)
        {
            _accessRequestRepository = accessRequestRepository;
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
        [ProducesResponseType(typeof(Models.Concepts.AccessRequestModel), 200)]
        [ProducesResponseType(204)]
        [SwaggerOperation(Tags = new[] { "user" })]
        public IActionResult GetAccessRequest()
        {
            var accessRequest = _accessRequestRepository.TryGet();
            if (accessRequest == null)
            {
                return NoContent();
            }

            return new JsonResult(_mapper.Map<Pims.Api.Models.Concepts.AccessRequestModel>(accessRequest));
        }

        /// <summary>
        /// Provides a way for a user to submit an access request to the system, associating a role and organization to their user.
        /// </summary>
        /// <returns></returns>
        [HttpPost("access/requests")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.Concepts.AccessRequestModel), 201)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "user" })]
        public IActionResult AddAccessRequest([FromBody] Pims.Api.Models.Concepts.AccessRequestModel model)
        {
            if (model == null || model.RoleId == null || model.RegionCode == null || model.AccessRequestStatusTypeCode == null)
            {
                throw new BadRequestException("Invalid access request specified");
            }
            var accessRequest = _mapper.Map<Entity.PimsAccessRequest>(model);
            accessRequest = _accessRequestRepository.Add(accessRequest);

            return CreatedAtAction(nameof(GetAccessRequest), new { id = accessRequest.AccessRequestId }, _mapper.Map<Pims.Api.Models.Concepts.AccessRequestModel>(accessRequest));
        }

        /// <summary>
        /// Provides a way for a user to update their access request.
        /// </summary>
        /// <returns></returns>
        [HttpPut("access/requests/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.Concepts.AccessRequestModel), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 403)]
        [SwaggerOperation(Tags = new[] { "user" })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Parameter 'id' is used for consistent routing.")]
        public IActionResult UpdateAccessRequest(long id, [FromBody] Pims.Api.Models.Concepts.AccessRequestModel model)
        {
            if (model == null || model.RoleId == null || model.RegionCode == null || model.AccessRequestStatusTypeCode == null)
            {
                throw new BadRequestException("Invalid access request specified");
            }

            var accessRequest = _mapper.Map<Entity.PimsAccessRequest>(model);
            accessRequest = _accessRequestRepository.Update(accessRequest);
            return new JsonResult(_mapper.Map<Pims.Api.Models.Concepts.AccessRequestModel>(accessRequest));
        }
        #endregion
    }
}
