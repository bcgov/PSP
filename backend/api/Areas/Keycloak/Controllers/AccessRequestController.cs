using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal.Keycloak;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.AccessRequest;

namespace Pims.Api.Areas.Keycloak.Controllers
{
    /// <summary>
    /// UserController class, provides endpoints for managing users within keycloak.
    /// </summary>
    [HasPermission(Permissions.AdminUsers)]
    [ApiController]
    [Area("keycloak")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/access/requests")]
    [Route("[area]/access/requests")]
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
        [HttpPut("access/request")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.AccessRequestModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "keycloak-user" })]
        [HasPermission(Permissions.AdminUsers)]
        public async Task<IActionResult> UpdateAccessRequestAsync(Model.AccessRequestModel model)
        {
            var accessRequest = _mapper.Map<Entity.AccessRequest>(model);
            var result = await _keycloakService.UpdateAccessRequestAsync(accessRequest);
            return new JsonResult(_mapper.Map<Model.AccessRequestModel>(result));
        }
        #endregion
    }
}
