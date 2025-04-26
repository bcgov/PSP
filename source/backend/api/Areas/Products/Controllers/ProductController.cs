using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Core.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;
using Pims.Api.Models.Concepts.Product;
using System;
using Pims.Dal.Repositories;

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
        #endregion

        /// <summary>
        /// Creates a new instance of a ProductController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="projectService"></param>
        /// <param name="productRepository"></param>
        /// <param name="mapper"></param>
        ///
        public ProductController(IProjectService projectService, IProductRepository productRepository, IMapper mapper)
        {
            _projectService = projectService;
            _productRepository = productRepository;
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

        /// <summary>
        /// Gets a collection of documents for the specified type and owner id.
        /// </summary>
        /// <param name="id">Used to identify document type.</param>
        /// <param name="time">Used to identify document's parent entity.</param>
        /// <returns></returns>
        [HttpGet("{id:long}/test-time")]
        [Produces("application/json")]
        [HasPermission(Permissions.ProjectView)]
        [ProducesResponseType(typeof(ProductModel), 200)]
        [SwaggerOperation(Tags = new[] { "product" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetProductAtTime([FromRoute] long id, [FromQuery] DateTime time)
        {
            var pimsProduct = _productRepository.GetProductAtTime(id, time);
            return new JsonResult(_mapper.Map<ProductModel>(pimsProduct));
        }
    }
}
