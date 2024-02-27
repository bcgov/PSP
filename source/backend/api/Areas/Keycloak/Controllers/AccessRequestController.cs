using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.AccessRequest;
using Pims.Api.Policies;
using Pims.Core.Json;
using Pims.Dal.Keycloak;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Areas.Keycloak.Controllers
{
    /// <summary>
    /// AccessRequestController class, provides endpoints for managing access requests.
    /// </summary>
    [HasPermission(Permissions.AdminUsers)]
    [ApiController]
    [Area("keycloak")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class AccessRequestController : ControllerBase
    {
        #region Variables
        private readonly IMapper _mapper;
        private readonly IPimsKeycloakService _keycloakService;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AccessRequestController class.
        /// </summary>
        /// <param name="keycloakService"></param>
        /// <param name="mapper"></param>
        public AccessRequestController(IMapper mapper, IPimsKeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Update an access request, generally to grant/deny it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("access/requests")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AccessRequestModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "keycloak-user" })]
        [HasPermission(Permissions.AdminUsers)]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> UpdateAccessRequestAsync(AccessRequestModel model)
        {
            var accessRequest = _mapper.Map<Entity.PimsAccessRequest>(model);
            var result = await _keycloakService.UpdateAccessRequestAsync(accessRequest);
            return new JsonResult(_mapper.Map<AccessRequestModel>(result));
        }
        #endregion
    }
}
