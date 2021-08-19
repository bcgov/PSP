using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Core.Extensions;
using Pims.Dal;
using Pims.Dal.Keycloak;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using EModel = Pims.Dal.Entities.Models;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.Organization;

namespace Pims.Api.Areas.Admin.Controllers
{
    /// <summary>
    /// OrganizationController class, provides endpoints for managing organizations.
    /// </summary>
    [HasPermission(Permissions.OrganizationAdmin)]
    [ApiController]
    [Area("admin")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/organizations")]
    [Route("[area]/organizations")]
    public class OrganizationController : ControllerBase
    {
        #region Variables
        private readonly IPimsService _pimsService;
        private readonly IPimsKeycloakService _pimsKeycloakService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OrganizationController class.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="keycloakService"></param>
        /// <param name="mapper"></param>
        public OrganizationController(IPimsService pimsService, IPimsKeycloakService keycloakService, IMapper mapper)
        {
            _pimsService = pimsService;
            _pimsKeycloakService = keycloakService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// GET - Returns a paged array of organizations from the datasource.
        /// </summary>
        /// <returns>Paged object with an array of organizations.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Api.Models.PageModel<Model.OrganizationModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-organization" })]
        public IActionResult GetOrganizations()
        {
            var organizations = _pimsService.Organization.GetAll();
            return new JsonResult(_mapper.Map<Model.OrganizationModel[]>(organizations));
        }

        /// <summary>
        /// GET - Returns a paged array of organizations from the datasource.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Paged object with an array of organizations.</returns>
        [HttpPost("filter")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Api.Models.PageModel<Model.OrganizationModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-organization" })]
        public IActionResult GetOrganizations(EModel.OrganizationFilter filter)
        {
            var page = _pimsService.Organization.Get(filter);
            var result = _mapper.Map<Api.Models.PageModel<Model.OrganizationModel>>(page);
            return new JsonResult(result);
        }

        /// <summary>
        /// GET - Returns a organization for the specified 'id' from the datasource.
        /// </summary>
        /// <param name="id">The unique 'id' for the organization to return.</param>
        /// <returns>The organization requested.</returns>
        [HttpGet("{id:long}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.OrganizationModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-organization" })]
        public IActionResult GetOrganization(long id)
        {
            var organization = _pimsService.Organization.Get(id);
            return new JsonResult(_mapper.Map<Model.OrganizationModel>(organization));
        }

        /// <summary>
        /// POST - Add a new organization to the datasource.
        /// </summary>
        /// <param name="model">The organization model.</param>
        /// <returns>The organization added.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.OrganizationModel), 201)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-organization" })]
        public async Task<IActionResult> AddOrganizationAsync([FromBody] Model.OrganizationModel model)
        {
            var entity = _mapper.Map<Entity.Organization>(model);
            _pimsService.Organization.Add(entity);

            // TODO: This isn't ideal as the db update may be successful but this request may not.
            await entity.Users.ForEachAsync(async u =>
            {
                var user = _pimsService.User.Get(u.KeycloakUserId.Value);
                await _pimsKeycloakService.UpdateUserAsync(user);
            });

            var organization = _mapper.Map<Model.OrganizationModel>(entity);

            return CreatedAtAction(nameof(GetOrganization), new { id = organization.Id }, organization);
        }

        /// <summary>
        /// PUT - Update the organization in the datasource.
        /// </summary>
        /// <param name="model">The organization model.</param>
        /// <returns>The organization updated.</returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.OrganizationModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-organization" })]
        public async Task<IActionResult> UpdateOrganizationAsync([FromBody] Model.OrganizationModel model)
        {
            var entity = _mapper.Map<Entity.Organization>(model);
            _pimsService.Organization.Update(entity);

            // TODO: This isn't ideal as the db update may be successful but this request may not.
            await entity.Users.ForEachAsync(async u =>
            {
                var user = _pimsService.User.Get(u.KeycloakUserId.Value);
                await _pimsKeycloakService.UpdateUserAsync(user);
            });

            var organization = _mapper.Map<Model.OrganizationModel>(entity);
            return new JsonResult(organization);
        }

        /// <summary>
        /// DELETE - Delete the organization from the datasource.
        /// </summary>
        /// <param name="model">The organization model.</param>
        /// <returns>The organization who was deleted.</returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.OrganizationModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-organization" })]
        public async Task<IActionResult> DeleteOrganizationAsync([FromBody] Model.OrganizationModel model)
        {
            var entity = _mapper.Map<Entity.Organization>(model);
            _pimsService.Organization.Delete(entity);

            // TODO: This isn't ideal as the db update may be successful but this request may not.
            await entity.Users.ForEachAsync(async u =>
            {
                var user = _pimsService.User.Get(u.KeycloakUserId.Value);
                await _pimsKeycloakService.UpdateUserAsync(user);
            });

            return new JsonResult(model);
        }
        #endregion
    }
}
