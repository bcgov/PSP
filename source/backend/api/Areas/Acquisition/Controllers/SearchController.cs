namespace Pims.Api.Areas.Acquisition.Controllers
{
    using System;
    using System.Collections.Generic;
    using MapsterMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Pims.Api.Areas.Acquisition.Models.Search;
    using Pims.Api.Helpers.Exceptions;
    using Pims.Api.Helpers.Extensions;
    using Pims.Api.Models.Concepts;
    using Pims.Api.Policies;
    using Pims.Api.Services;
    using Pims.Core.Extensions;
    using Pims.Dal.Entities.Models;
    using Pims.Dal.Security;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// SearchController class, provides endpoints for searching acquisition files.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("acquisitionfiles")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class SearchController : ControllerBase
    {
        #region Variables
        private readonly IAcquisitionFileService acquisitionService;
        private readonly IMapper mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SearchController(Acquisition) class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="acquisitionService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public SearchController(IAcquisitionFileService acquisitionService, IMapper mapper, ILogger<SearchController> logger)
        {
            this.acquisitionService = acquisitionService;
            this.mapper = mapper;
            this._logger = logger;
        }
        #endregion

        #region Endpoints
        #region Acquisition List View Endpoints

        /// <summary>
        /// Gets all the Acquisition Files that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Acquisition Files matching the filter.</returns>
        [HttpGet]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AcquisitionFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult GetAcquisitionFiles()
        {
            var uri = new Uri(this.Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return GetAcquisitionFiles(new AcquisitionFilterModel(query));
        }

        /// <summary>
        /// Gets all the Acquisition Files that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Acquisition Files matching the filter.</returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AcquisitionFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        public IActionResult GetAcquisitionFiles([FromBody] AcquisitionFilterModel filter)
        {
            // RECOMMENDED - Add valuable metadata to logs
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SearchController),
                nameof(GetAcquisitionFiles),
                User.GetUsername(),
                DateTime.Now);

            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Acquisition filter must contain valid values.");
            }

            // RECOMMENDED - Log communications between components
            _logger.LogInformation("Dispatching to service: {Service}", acquisitionService.GetType());

            var acquisitionFiles = acquisitionService.GetPage((AcquisitionFilter)filter);
            return new JsonResult(mapper.Map<Api.Models.PageModel<AcquisitionFileModel>>(acquisitionFiles));
        }
        #endregion
        #endregion
    }
}
