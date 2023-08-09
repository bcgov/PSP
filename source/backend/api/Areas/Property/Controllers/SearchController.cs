using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Areas.Property.Models.Search;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Policies;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using BModel = Pims.Api.Models;

namespace Pims.Api.Areas.Property.Controllers
{
    /// <summary>
    /// SearchController class, provides endpoints for searching properties.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("properties")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class SearchController : ControllerBase
    {
        #region Variables
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SearchController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyRepository"></param>
        /// <param name="mapper"></param>
        ///
        public SearchController(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        #region Property Paging Endpoints (for list view)

        /// <summary>
        /// Get all the properties that satisfy the filter parameters.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BModel.PageModel<Models.Search.PropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult GetProperties()
        {
            var uri = new Uri(this.Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return GetProperties(new PropertyFilterModel(query));
        }

        /// <summary>
        /// Get all the properties that satisfy the filter parameters.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BModel.PageModel<Models.Search.PropertyModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult GetProperties([FromBody] PropertyFilterModel filter)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Property filter must contain valid values.");
            }

            var page = _propertyRepository.GetPage((PropertyFilter)filter);
            var result = _mapper.Map<BModel.PageModel<Models.Search.PropertyModel>>(page);
            return new JsonResult(result);
        }

        /// <summary>
        /// Get all the properties that satisfy the advanced filter parameters.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("advanced-filter")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(HashSet<long>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult FilterProperties([FromBody] PropertyFilterCriteria filter)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (filter == null)
            {
                throw new BadRequestException("Property filter must contain valid values.");
            }

            var page = _propertyRepository.GetMatchingIds(filter);
            return new JsonResult(page);
        }
        #endregion
        #endregion
    }
}
