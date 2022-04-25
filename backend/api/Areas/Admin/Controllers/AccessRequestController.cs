using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using EModel = Pims.Dal.Entities.Models;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.AccessRequest;
using PModel = Pims.Api.Models;

namespace Pims.Api.Areas.Admin.Controllers
{
    /// <summary>
    /// AccessRequestController class, provides endpoints for managing access requests.
    /// </summary>
    [HasPermission(Permissions.AdminUsers)]
    [ApiController]
    [Area("admin")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/access/requests")]
    [Route("[area]/access/requests")]
    public class AccessRequestController : Controller
    {
        #region Properties
        private readonly IPimsRepository _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an AccessRequestController object, initializes with specified parameters.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        public AccessRequestController(IPimsRepository pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get a list of access requests
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="searchText"></param>
        /// <param name="role"></param>
        /// <param name="organization"></param>
        /// <param name="status"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PModel.PageModel<Model.AccessRequestModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-access-requests" })]
        public IActionResult GetPage(int page = 1, int quantity = 10, string searchText = null, string role = null, string organization = null, string status = null, string sort = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (quantity < 1)
            {
                quantity = 1;
            }

            if (quantity > 20)
            {
                quantity = 20;
            }

            var filter = new EModel.AccessRequestFilter(page, quantity, searchText, role, organization, status, new[] { sort });

            var result = _pimsService.AccessRequest.Get(filter);
            var models = _mapper.Map<Model.AccessRequestModel[]>(result.Items);
            var paged = new PModel.PageModel<Model.AccessRequestModel>(models, page, quantity, result.Total);
            return new JsonResult(paged);
        }

        /// <summary>
        /// Delete an access requests
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.AccessRequestModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-access-requests" })]
        public IActionResult Delete(long id, [FromBody] Model.AccessRequestModel model)
        {
            var entity = _mapper.Map<Entity.PimsAccessRequest>(model);
            _pimsService.AccessRequest.Delete(entity);
            return new JsonResult(model);
        }
        #endregion

    }
}

