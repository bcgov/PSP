using System;
using MapsterMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.User;
using Pims.Api.Policies;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Admin.Controllers
{
    /// <summary>
    /// UserController class, provides endpoints for managing users.
    /// </summary>
    [HasPermission(Permissions.AdminUsers)]
    [ApiController]
    [Area("admin")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/users")]
    [Route("[area]/users")]
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
        /// GET - Returns a paged array of users from the datasource.
        /// </summary>
        /// <returns>Paged object with an array of users.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PageModel<UserModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-user" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetUsers()
        {
            var uri = new Uri(this.Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return GetUsers(new UserFilter(query));
        }

        /// <summary>
        /// GET - Returns a paged array of users from the datasource.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Paged object with an array of users.</returns>
        [HttpPost("filter")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PageModel<UserModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-user" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetUsers(UserFilter filter)
        {
            var page = _userRepository.GetAllByFilter(filter);
            var result = _mapper.Map<PageModel<UserModel>>(page);
            return new JsonResult(result);
        }

        /// <summary>
        /// GET - Returns a user for the specified 'id' from the datasource.
        /// </summary>
        /// <param name="id">The unique 'id' for the user to return.</param>
        /// <returns>The user requested.</returns>
        [HttpGet("{id:long}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-user" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetUser(long id)
        {
            var entity = _userRepository.GetById(id);
            var user = _mapper.Map<UserModel>(entity);
            return new JsonResult(user);
        }

        /// <summary>
        /// GET - Returns a user for the specified 'key' from the datasource.
        /// </summary>
        /// <param name="key">The unique 'key' for the user to return.</param>
        /// <returns>The user requested.</returns>
        [HttpGet("{key}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-user" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetUser(Guid key)
        {
            var entity = _userRepository.GetByKeycloakUserId(key);
            var user = _mapper.Map<UserModel>(entity);
            return new JsonResult(user);
        }

        /// <summary>
        /// POST - Add a new user to the datasource.
        /// </summary>
        /// <param name="model">The user model.</param>
        /// <returns>The user added.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserModel), 201)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-user" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddUser([FromBody] UserModel model)
        {
            var entity = _mapper.Map<PimsUser>(model);
            _userRepository.Add(entity);

            var user = _mapper.Map<UserModel>(entity);

            return CreatedAtAction(nameof(GetUser), new { key = user.GuidIdentifierValue }, user);
        }

        /// <summary>
        /// PUT - Update the user in the datasource.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="model">The user model.</param>
        /// <returns>The user updated.</returns>
        [HttpPut("{key}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-user" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateUser(Guid key, [FromBody] UserModel model)
        {
            var entity = _mapper.Map<PimsUser>(model);
            _userRepository.Update(entity);

            var user = _mapper.Map<UserModel>(entity);
            return new JsonResult(user);
        }

        /// <summary>
        /// DELETE - Delete the user from the datasource.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="model">The user model.</param>
        /// <returns>The user who was deleted.</returns>
        [HttpDelete("{key}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-user" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteUser(Guid key, [FromBody] UserModel model)
        {
            var entity = _mapper.Map<PimsUser>(model);
            _userRepository.Delete(entity);

            return new JsonResult(model);
        }
        #endregion
    }
}
