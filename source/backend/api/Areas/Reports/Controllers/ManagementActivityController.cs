using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Management.Models;
using Pims.Api.Areas.Reports.Models.Management;
using Pims.Api.Helpers.Constants;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Helpers.Reporting;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Reports.Controllers
{
    /// <summary>
    /// ManagementActivityController class, provides endpoints for generating reports.
    /// </summary>
    [Authorize]
    [ApiController]
    [Area("reports")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/management-activities")]
    [Route("[area]/management-activities")]
    public class ManagementActivityController : ControllerBase
    {
        private readonly IManagementActivityService _managementActivityService;
        private readonly IPropertyService _propertyService;
        private readonly ILogger _logger;

        public ManagementActivityController(IManagementActivityService managementActivityService, IPropertyService propertyService, ILogger<ManagementActivityController> logger)
        {
            _managementActivityService = managementActivityService;
            _propertyService = propertyService;
            _logger = logger;
        }

        /// <summary>
        /// Generates the Management Activities Overview Report as an Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["application/application/vnd.ms-excel"].
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="acceptHeader"></param>
        /// <returns></returns>
        [HttpPost("overview")]
        [HasPermission(Permissions.ManagementView)]
        [Produces(ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "management-activities", "report" })]
        public IActionResult ExportManagementActivitiesOverview([FromBody] ManagementActivityFilterModel filter, [FromHeader(Name = "Accept")] string acceptHeader)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(ExportManagementActivitiesOverview),
                User.GetUsername(),
                DateTime.Now);

            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Management activity filter must contain valid values.");
            }

            if (acceptHeader != ContentTypes.CONTENTTYPEEXCEL && acceptHeader != ContentTypes.CONTENTTYPEEXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{acceptHeader}'.");
            }

            var allManagementActivities = _managementActivityService.SearchManagementActivities((ManagementActivityFilter)filter);
            if (allManagementActivities is null || allManagementActivities.Count == 0)
            {
                // Return 204 "No Content" to signal the frontend that we did not find any matching records.
                return NoContent();
            }

            // Transform all properties to lat/long for returned activities that have properties, this is required for the front end to properly display the property locations.
            foreach (var activity in allManagementActivities)
            {
                var activityProperties = activity.PimsManagementActivityProperties ?? new List<PimsManagementActivityProperty>();
                activity.PimsManagementActivityProperties = TransformAllPropertiesToLatLong(activityProperties);
            }

            var reportActivities = allManagementActivities.Select(a => new ManagementActivityOverviewReportModel(a));

            return ReportHelper.GenerateExcel(reportActivities, "Management Activities Overview");
        }

        /// <summary>
        /// Generates the Management Activity Invoices Report as an Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["application/application/vnd.ms-excel"].
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="acceptHeader"></param>
        /// <returns></returns>
        [HttpPost("invoices")]
        [HasPermission(Permissions.ManagementView)]
        [Produces(ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "management-activities", "report" })]
        public IActionResult ExportManagementActivityInvoices([FromBody] ManagementActivityFilterModel filter, [FromHeader(Name = "Accept")] string acceptHeader)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(ExportManagementActivityInvoices),
                User.GetUsername(),
                DateTime.Now);

            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Management activity filter must contain valid values.");
            }

            if (acceptHeader != ContentTypes.CONTENTTYPEEXCEL && acceptHeader != ContentTypes.CONTENTTYPEEXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{acceptHeader}'.");
            }

            var allInvoices = _managementActivityService.SearchManagementActivityInvoices((ManagementActivityFilter)filter);
            if (allInvoices is null || allInvoices.Count == 0)
            {
                // Return 204 "No Content" to signal the frontend that we did not find any matching records.
                return NoContent();
            }

            // Transform all properties to lat/long for returned invoices that have properties, this is required for the front end to properly display the property locations.
            foreach (var invoice in allInvoices)
            {
                var activityProperties = invoice.ManagementActivity.PimsManagementActivityProperties ?? new List<PimsManagementActivityProperty>();
                invoice.ManagementActivity.PimsManagementActivityProperties = TransformAllPropertiesToLatLong(activityProperties);
            }

            var reportInvoices = allInvoices.Select(i => new ManagementActivityInvoicesReportModel(i));

            return ReportHelper.GenerateExcel(reportInvoices, "Management Activity Invoices");
        }

        /// <summary>
        /// Returns the spatial location and boundary polygons in lat/long (4326) for a list of activity properties.
        /// The spatial values will be modified in-place.
        /// </summary>
        /// <param name="activityProperties">The activity properties to re-project.</param>
        /// <returns>The activity properties with transformed spatial locations.</returns>
        private ICollection<PimsManagementActivityProperty> TransformAllPropertiesToLatLong(ICollection<PimsManagementActivityProperty> activityProperties)
        {
            if (activityProperties == null)
            {
                return activityProperties;
            }

            foreach (var activityProperty in activityProperties)
            {
                activityProperty.Property = _propertyService.TransformPropertyToLatLong(activityProperty.Property);
            }

            return activityProperties;
        }

    }
}
