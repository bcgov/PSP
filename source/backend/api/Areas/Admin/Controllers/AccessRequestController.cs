using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using EModel = Pims.Dal.Entities.Models;
using Entity = Pims.Dal.Entities;

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
        private readonly IAccessRequestRepository _accessRequestRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an AccessRequestController object, initializes with specified parameters.
        /// </summary>
        /// <param name="accessRequestRepository"></param>
        /// <param name="mapper"></param>
        public AccessRequestController(IAccessRequestRepository accessRequestRepository, IMapper mapper)
        {
            _accessRequestRepository = accessRequestRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get a list of access requests.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="searchText"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PageModel<AccessRequestModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-access-requests" })]
        public IActionResult GetPage(int page = 1, int quantity = 10, string searchText = null, string sort = null)
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

            var filter = new EModel.AccessRequestFilter(page, quantity, searchText, new[] { sort });
            filter.StatusType = new Entity.PimsAccessRequestStatusType() { Id = "Received" };

            var result = _accessRequestRepository.GetAll(filter);
            var models = _mapper.Map<AccessRequestModel[]>(result.Items);
            var paged = new PageModel<AccessRequestModel>(models, page, quantity, result.Total);
            return new JsonResult(paged);
        }

        /// <summary>
        /// Get the most recent access request for the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AccessRequestModel), 200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 403)]
        [SwaggerOperation(Tags = new[] { "user" })]
        public IActionResult GetAccessRequest(long id)
        {
            var accessRequest = _accessRequestRepository.GetById(id);
            return new JsonResult(_mapper.Map<AccessRequestModel>(accessRequest));
        }

        /// <summary>
        /// Delete an access requests.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AccessRequestModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-access-requests" })]
        public IActionResult Delete(long id, [FromBody] AccessRequestModel model)
        {
            var entity = _mapper.Map<Entity.PimsAccessRequest>(model);
            _accessRequestRepository.Delete(entity);
            return new JsonResult(model);
        }
        #endregion

    }
}
