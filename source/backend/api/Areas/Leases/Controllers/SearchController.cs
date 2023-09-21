using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Lease.Models.Search;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// SearchController class, provides endpoints for searching leases.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class SearchController : ControllerBase
    {
        #region Variables
        private readonly ILeaseService _leaseService;
        private readonly IMapper _mapper;
        private readonly ILogger<LeaseController> _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SearchController(LIS) class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public SearchController(ILeaseService leaseService, IMapper mapper, ILogger<LeaseController> logger)
        {
            _leaseService = leaseService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints
        #region Lease List View Endpoints

        /// <summary>
        /// Get all the leases that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of leases matching the filter.</returns>
        [HttpGet]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetLeases()
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SearchController),
                nameof(GetLeases),
                User.GetUsername(),
                DateTime.Now);

            var uri = new Uri(this.Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return GetLeases(new LeaseFilterModel(query));
        }

        /// <summary>
        /// Get all the leases that satisfy the filter parameters.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>An array of leases matching the filter.</returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetLeases([FromBody] LeaseFilterModel filter)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SearchController),
                nameof(GetLeases),
                User.GetUsername(),
                DateTime.Now);

            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Property filter must contain valid values.");
            }

            var leases = _leaseService.GetPage((LeaseFilter)filter);
            return new JsonResult(_mapper.Map<Api.Models.PageModel<LeaseModel>>(leases));
        }
        #endregion
        #endregion
    }
}
