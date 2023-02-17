using System;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Dal.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// UserController class, provides endpoints for managing users.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/users")]
    [Route("users")]
    public class UserController : ControllerBase
    {
        #region Variables
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a UserController class.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Returns user person info for given keycloakUserId.
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <returns>User person info.</returns>
        [HttpGet("info/{keycloakUserId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.UserModel), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "userInfo" })]
        public IActionResult UserBasicInfo([FromRoute] Guid keycloakUserId)
        {
            if (keycloakUserId == Guid.Empty)
            {
                return new JsonResult(new Models.ErrorResponseModel("Invalid keycloakUserId", "keycloakUserId should be a valid non empty guid"));
            }
            var entity = _userRepository.GetUserInfoByKeycloakUserId(keycloakUserId);
            var user = _mapper.Map<Model.UserModel>(entity);
            return new JsonResult(user);
        }
        #endregion
    }
}
