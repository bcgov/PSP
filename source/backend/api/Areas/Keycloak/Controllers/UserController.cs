using System;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.User;
using Pims.Api.Policies;
using Pims.Core.Json;
using Pims.Dal.Keycloak;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Areas.Keycloak.Controllers
{
    /// <summary>
    /// UserController class, provides endpoints for managing users within keycloak.
    /// </summary>
    [HasPermission(Permissions.AdminUsers)]
    [ApiController]
    [Area("keycloak")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/users")]
    [Route("[area]/users")]
    public class UserController : ControllerBase
    {
        #region Variables
        private readonly IMapper _mapper;
        private readonly IPimsKeycloakService _keycloakService;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a UserController class.
        /// </summary>
        /// <param name="keycloakService"></param>
        /// <param name="mapper"></param>
        public UserController(IMapper mapper, IPimsKeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Update the user for the specified 'key'.
        /// If the user does not exist in Keycloak or PIMS return a 400-BadRequest.
        /// </summary>
        /// <exception type="KeyNotFoundException">The user does not exist in Keycloak or PIMS.</exception>
        /// <returns></returns>
        [HttpPut("{key:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-user" })]
        [HasPermission(Permissions.AdminUsers)]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> UpdateUserAsync(Guid key, [FromBody] UserModel model)
        {
            var user = _mapper.Map<Entity.PimsUser>(model);
            var entity = await _keycloakService.UpdateUserAsync(user);
            var result = _mapper.Map<UserModel>(entity);
            return new JsonResult(result);
        }
        #endregion
    }
}
