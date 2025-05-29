using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Management.Models.Search;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.ManagementFile;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Management.Controllers
{
    /// <summary>
    /// SearchController class, provides endpoints for searching management files.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("managementfiles")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class SearchController : ControllerBase
    {
        #region Variables
        private readonly IManagementFileService _managementService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SearchController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="managementService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public SearchController(IManagementFileService managementService, IMapper mapper, ILogger<SearchController> logger)
        {
            _managementService = managementService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Management List View Endpoints

        /// <summary>
        /// Gets all the Management Files that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Management Files matching the filter.</returns>
        [HttpGet]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ManagementFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "managementfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementFiles()
        {
            var uri = new Uri(Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return GetManagementFiles(new ManagementFilterModel(query));
        }

        /// <summary>
        /// Gets all the Management Files that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Management Files matching the filter.</returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ManagementFileModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "managementfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementFiles([FromBody] ManagementFilterModel filter)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SearchController),
                nameof(GetManagementFiles),
                User.GetUsername(),
                DateTime.Now);

            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Management filter must contain valid values.");
            }

            _logger.LogInformation("Dispatching to service: {Service}", _managementService.GetType());

            var managementFiles = _managementService.GetPage((ManagementFilter)filter);
            return new JsonResult(_mapper.Map<PageModel<ManagementFileModel>>(managementFiles));
        }

        #endregion
    }
}
