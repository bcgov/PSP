using System;
using MapsterMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.User;
using Pims.Core.Json;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Users.Controllers
{

    /// <summary>
    /// SearchController class, provides endpoints for searching users.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Area("users")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class SearchController : ControllerBase
    {
        #region Variables
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SearchController(Users) class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        ///
        public SearchController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        #region Users List View Endpoints

        /// <summary>
        /// Get all the Users that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Users matching the filter.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PageModel<UserModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "users" })]
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
        [SwaggerOperation(Tags = new[] { "users" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetUsers([FromBody] UserFilter filter)
        {
            var page = _userRepository.GetUserLookup(filter);
            var result = _mapper.Map<PageModel<UserModel>>(page);
            return new JsonResult(result);
        }
        #endregion
        #endregion
    }
}
