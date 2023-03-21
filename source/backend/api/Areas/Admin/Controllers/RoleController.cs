using System;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Areas.Admin.Controllers
{
    /// <summary>
    /// RoleController class, provides endpoints for managing roles.
    /// </summary>
    [HasPermission(Permissions.AdminRoles)]
    [ApiController]
    [Area("admin")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/roles")]
    [Route("[area]/roles")]
    public class RoleController : ControllerBase
    {
        #region Variables
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a RoleController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="roleRepository"></param>
        /// <param name="mapper"></param>
        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// GET - Returns a paged array of roles from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="name"></param>
        /// <returns>Paged object with an array of roles.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Api.Models.PageModel<Model.RoleModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-role" })]
        public IActionResult GetRoles(int page = 1, int quantity = 10, string name = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (quantity < 1)
            {
                quantity = 1;
            }

            if (quantity > 50)
            {
                quantity = 50;
            }

            var paged = _roleRepository.GetPage(page, quantity, name);
            var result = _mapper.Map<Api.Models.PageModel<Model.RoleModel>>(paged);
            return new JsonResult(result);
        }

        /// <summary>
        /// GET - Returns a role for the specified 'key' from the datasource.
        /// </summary>
        /// <param name="key">The unique 'key' for the role to return.</param>
        /// <returns>The role requested.</returns>
        [HttpGet("{key}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.RoleModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-role" })]
        public IActionResult GetRole(Guid key)
        {
            var entity = _roleRepository.GetByKey(key);
            var role = _mapper.Map<Model.RoleModel>(entity);
            return new JsonResult(role);
        }
        #endregion
    }
}
