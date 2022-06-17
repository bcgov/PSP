namespace Pims.Api.Areas.Research.Controllers
{
    using System;
    using System.Collections.Generic;
    using MapsterMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Pims.Api.Areas.Research.Models.Search;
    using Pims.Api.Helpers.Exceptions;
    using Pims.Api.Helpers.Extensions;
    using Pims.Api.Models.Concepts;
    using Pims.Api.Policies;
    using Pims.Dal.Entities.Models;
    using Pims.Dal.Security;
    using Pims.Dal.Services;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// SearchController class, provides endpoints for searching research files.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("researchfiles")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class SearchController : ControllerBase
    {
        #region Variables
        private readonly IPimsService pimsService;
        private readonly IMapper mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a SearchController(Research) class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public SearchController(IPimsService pimsService, IMapper mapper)
        {
            this.pimsService = pimsService;
            this.mapper = mapper;
        }
        #endregion

        #region Endpoints
        #region Research List View Endpoints
        /// <summary>
        /// Get all the Research Files that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Research Files matching the filter</returns>
        [HttpGet]
        [HasPermission(Permissions.ResearchFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ResearchFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "research", "file" })]
        public IActionResult GetResearchFiles()
        {
            var uri = new Uri(this.Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return GetResearchFiles(new ResearchFilterModel(query));
        }

        /// <summary>
        /// Get all the researches that satisfy the filter parameters.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>An array of research files matching the filter</returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.ResearchFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ResearchFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "research", "file" })]
        public IActionResult GetResearchFiles([FromBody] ResearchFilterModel filter)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Research filter must contain valid values.");
            }

            var researchFiles = pimsService.ResearchFileService.GetPage((ResearchFilter)filter);
            return new JsonResult(mapper.Map<Api.Models.PageModel<ResearchFileModel>>(researchFiles));
        }
        #endregion
        #endregion
    }
}
