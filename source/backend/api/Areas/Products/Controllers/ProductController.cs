using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Projects.Controllers
{
    /// <summary>
    /// ProductController class, provides endpoints for interacting with products.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("products")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ProductController : ControllerBase
    {
        #region fields
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        #endregion

        /// <summary>
        /// Creates a new instance of a ProductController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="projectService"></param>
        /// <param name="mapper"></param>
        ///
        public ProductController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the acquisition files for the given product.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>An array of contacts matching the filter.</returns>
        [HttpGet("{productId}/acquisitionFiles")]
        [HasPermission(Permissions.ProjectView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<AcquisitionFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "product" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetFiles(long productId)
        {
            var acquisitionFiles = _projectService.GetProductFiles(productId);

            return new JsonResult(_mapper.Map<List<AcquisitionFileModel>>(acquisitionFiles));
        }
    }
}
