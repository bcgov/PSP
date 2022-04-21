using System;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.Role;

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
        private readonly IPimsRepository _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a RoleController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        public RoleController(IPimsRepository pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
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

            var paged = _pimsService.Role.Get(page, quantity, name);
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
            var entity = _pimsService.Role.Get(key);
            var role = _mapper.Map<Model.RoleModel>(entity);
            return new JsonResult(role);
        }

        /// <summary>
        /// GET - Returns a role for the specified 'name' from the datasource.
        /// </summary>
        /// <param name="name">The unique 'name' for the role to return.</param>
        /// <returns>The role requested.</returns>
        [HttpGet("name/{name}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.RoleModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-role" })]
        public IActionResult GetRoleByName(string name)
        {
            var entity = _pimsService.Role.GetByName(name);
            var role = _mapper.Map<Model.RoleModel>(entity);
            return new JsonResult(role);
        }

        /// <summary>
        /// POST - Add a new role to the datasource.
        /// </summary>
        /// <param name="model">The role model.</param>
        /// <returns>The role added.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.RoleModel), 201)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-role" })]
        public IActionResult AddRole([FromBody] Model.RoleModel model)
        {
            var entity = _mapper.Map<Entity.PimsRole>(model); // TODO: Return bad request.
            _pimsService.Role.Add(entity);
            var role = _mapper.Map<Model.RoleModel>(entity);

            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }

        /// <summary>
        /// PUT - Update the role in the datasource.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="model">The role model.</param>
        /// <returns>The role updated.</returns>
        [HttpPut("{key}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.RoleModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-role" })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Parameter 'key' is required for route.")]
        public IActionResult UpdateRole(Guid key, [FromBody] Model.RoleModel model)
        {
            var entity = _mapper.Map<PimsRole>(model);
            _pimsService.Role.Update(entity);

            var role = _mapper.Map<Model.RoleModel>(entity);
            return new JsonResult(role);
        }

        /// <summary>
        /// DELETE - Delete the role from the datasource.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="model">The role model.</param>
        /// <returns>The role who was deleted.</returns>
        [HttpDelete("{key}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.RoleModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-role" })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Parameter 'key' is required for route.")]
        public IActionResult DeleteRole(Guid key, [FromBody] Model.RoleModel model)
        {
            var entity = _mapper.Map<PimsRole>(model);
            _pimsService.Role.Delete(entity);

            return new JsonResult(model);
        }
        #endregion
    }
}
