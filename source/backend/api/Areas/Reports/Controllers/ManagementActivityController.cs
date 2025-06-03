using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Management.Controllers;
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
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ManagementActivityController(IManagementActivityService managementActivityService, IMapper mapper, ILogger<ManagementActivityController> logger)
        {
            _managementActivityService = managementActivityService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Exports Management Activities as CSV or Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["text/csv", "application/application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"].
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpGet]
        [HasPermission(Permissions.ManagementView)]
        [Produces(ContentTypes.CONTENTTYPECSV, ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "management-activities", "report" })]
        public IActionResult ExportManagementActivities(bool all = false)
        {
            var uri = new Uri(Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return ExportManagementActivities(new ManagementActivityFilterModel(query), all);
        }

        /// <summary>
        /// Exports Management Activities as CSV or Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["text/csv", "application/application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"].
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.ManagementView)]
        [Produces(ContentTypes.CONTENTTYPECSV, ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "management-activities", "report" })]
        public IActionResult ExportManagementActivities([FromBody]ManagementActivityFilterModel filter, bool all = false)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ActivitySearchController),
                nameof(ExportManagementActivities),
                User.GetUsername(),
                DateTime.Now);

            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Lease filter must contain valid values.");
            }

            var acceptHeader = (string)Request.Headers["Accept"];
            if (acceptHeader != ContentTypes.CONTENTTYPECSV && acceptHeader != ContentTypes.CONTENTTYPEEXCEL && acceptHeader != ContentTypes.CONTENTTYPEEXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{acceptHeader}'.");
            }

            var allManagementActivities = _managementActivityService.GetPage((ManagementActivityFilter)filter, all);
            var flatActivities = _mapper.Map<IEnumerable<ManagementActivityReportModel>>(allManagementActivities);

            return acceptHeader.ToString() switch
            {
                ContentTypes.CONTENTTYPECSV => ReportHelper.GenerateCsv(flatActivities),
                _ => ReportHelper.GenerateExcel(flatActivities, "ManagementActities")
            };
        }
    }
}
