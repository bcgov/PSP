using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Models.Concepts.Product;
using Pims.Api.Services;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Repositories;
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
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        #endregion

        /// <summary>
        /// Creates a new instance of a ProductController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="projectService"></param>
        /// <param name="productRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public ProductController(IProjectService projectService, IProductRepository productRepository, IMapper mapper, ILogger<ProductController> logger)
        {
            _projectService = projectService;
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
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

        [HttpGet("{id:long}/historical")]
        [Produces("application/json")]
        [HasPermission(Permissions.ProjectView)]
        [ProducesResponseType(typeof(ProductModel), 200)]
        [SwaggerOperation(Tags = new[] { "product" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetProductAtTime([FromRoute] long id, [FromQuery] DateTime time)
        {
            _logger.LogInformation(
             "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
             nameof(ProductController),
             nameof(GetProductAtTime),
             User.GetUsername(),
             DateTime.Now);

            var pimsProduct = _productRepository.GetProductAtTime(id, time);
            return new JsonResult(_mapper.Map<ProductModel>(pimsProduct));
        }
    }
}
