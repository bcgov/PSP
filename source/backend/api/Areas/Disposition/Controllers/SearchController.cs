using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Disposition.Models.Search;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.DispositionFile;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Disposition.Controllers
{
    /// <summary>
    /// SearchController class, provides endpoints for searching disposition files.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("dispositionfiles")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class SearchController : ControllerBase
    {
        #region Variables
        private readonly IDispositionFileService _dispositionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SearchController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="dispositionService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public SearchController(IDispositionFileService dispositionService, IMapper mapper, ILogger<DispositionFileController> logger)
        {
            _dispositionService = dispositionService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Disposition List View Endpoints

        /// <summary>
        /// Gets all the Disposition Files that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Disposition Files matching the filter.</returns>
        [HttpGet]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DispositionFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFiles()
        {
            var uri = new Uri(Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return GetDispositionFiles(new DispositionFilterModel(query));
        }

        /// <summary>
        /// Gets all the Disposition Files that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Disposition Files matching the filter.</returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.DispositionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DispositionFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "dispositionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDispositionFiles([FromBody] DispositionFilterModel filter)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SearchController),
                nameof(GetDispositionFiles),
                User.GetUsername(),
                DateTime.Now);

            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Disposition filter must contain valid values.");
            }

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionService.GetType());

            var dispositionFiles = _dispositionService.GetPage((DispositionFilter)filter);
            return new JsonResult(_mapper.Map<PageModel<DispositionFileModel>>(dispositionFiles));
        }

        #endregion
    }
}
