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
    using Pims.Api.Models.Base;
    using Pims.Api.Models.Concepts.ResearchFile;
    using Pims.Api.Policies;
    using Pims.Api.Services;
    using Pims.Core.Json;
    using Pims.Dal.Entities.Models;
    using Pims.Dal.Security;
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
        private readonly IResearchFileService _researchFileService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SearchController(Research) class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="researchFileService"></param>
        /// <param name="mapper"></param>
        ///
        public SearchController(IResearchFileService researchFileService, IMapper mapper)
        {
            _researchFileService = researchFileService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        #region Research List View Endpoints

        /// <summary>
        /// Get all the Research Files that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Research Files matching the filter.</returns>
        [HttpGet]
        [HasPermission(Permissions.ResearchFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ResearchFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "research", "file" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
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
        /// <returns>An array of research files matching the filter.</returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.ResearchFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ResearchFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "research", "file" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetResearchFiles([FromBody] ResearchFilterModel filter)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Research filter must contain valid values.");
            }

            var researchFiles = _researchFileService.GetPage((ResearchFilter)filter);
            return new JsonResult(_mapper.Map<PageModel<ResearchFileModel>>(researchFiles));
        }
        #endregion
        #endregion
    }
}
