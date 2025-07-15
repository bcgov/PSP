using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Management.Models;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Property;
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
    [Area("management-activities")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class ActivitySearchController : ControllerBase
    {
        private readonly IManagementActivityService _managementActivityService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new instance of a SearchController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="managementActivityService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public ActivitySearchController(IManagementActivityService managementActivityService, IMapper mapper, ILogger<SearchController> logger)
        {
            _managementActivityService = managementActivityService;
            _mapper = mapper;
            _logger = logger;
        }

        #region Management List View Endpoints

        /// <summary>
        /// Gets all the Management Files that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Management Files matching the filter.</returns>
        [HttpGet]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ManagementActivityFilterModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "management-activities" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementActivities()
        {
            var uri = new Uri(Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return GetManagementActivitiesPaged(new ManagementActivityFilterModel(query));
        }

        /// <summary>
        /// Gets all the Management Files that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Management Files matching the filter.</returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.ManagementView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ManagementActivityModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "managementfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementActivitiesPaged([FromBody]ManagementActivityFilterModel filter)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ActivitySearchController),
                nameof(GetManagementActivitiesPaged),
                User.GetUsername(),
                DateTime.Now);

            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Management Activities filter must contain valid values.");
            }

            _logger.LogInformation("Dispatching to service: {Service}", _managementActivityService.GetType());

            var managementActivities = _managementActivityService.GetPage((ManagementActivityFilter)filter);

            return new JsonResult(_mapper.Map<PageModel<ManagementActivityModel>>(managementActivities));
        }

        #endregion
    }
}
