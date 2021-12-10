using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Helpers.Reporting;
using Pims.Api.Policies;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Pims.Api.Areas.Reports.Controllers
{
    /// <summary>
    /// LeaseController class, provides endpoints for generating reports.
    /// </summary>
    [Authorize]
    [ApiController]
    [Area("reports")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/leases")]
    [Route("[area]/leases")]
    public class LeaseController : ControllerBase
    {
        #region Variables
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ReportController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        public LeaseController(IPimsService pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        #region Export Leases
        /// <summary>
        /// Exports leases as CSV or Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["text/csv", "application/application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"]
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpGet]
        [HasPermission(Permissions.PropertyView)]
        [Produces(ContentTypes.CONTENT_TYPE_CSV, ContentTypes.CONTENT_TYPE_EXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "lease", "report" })]
        public IActionResult ExportLeases(bool all = false)
        {
            var uri = new Uri(this.Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return ExportLeases(new Lease.Models.Search.LeaseFilterModel(query), all);
        }

        /// <summary>
        /// Exports leases as CSV or Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["text/csv", "application/application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"]
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.PropertyView)]
        [Produces(ContentTypes.CONTENT_TYPE_CSV, ContentTypes.CONTENT_TYPE_EXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "lease", "report" })]
        public IActionResult ExportLeases([FromBody] Lease.Models.Search.LeaseFilterModel filter, bool all = false)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid()) throw new BadRequestException("Lease filter must contain valid values.");
            var accept = (string)this.Request.Headers["Accept"] ?? throw new BadRequestException($"HTTP request header 'Accept' is required.");

            if (accept != ContentTypes.CONTENT_TYPE_CSV && accept != ContentTypes.CONTENT_TYPE_EXCEL && accept != ContentTypes.CONTENT_TYPE_EXCELX)
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{accept}'.");

            //Create duplicate lease rows for every unique property lease, tenant, and term.
            filter.Quantity = all ? _pimsService.Lease.Count() : filter.Quantity;
            var page = _pimsService.Lease.GetPage((LeaseFilter)filter);
            var leaseTerms = _mapper.Map<IEnumerable<Models.Lease.LeaseModel>>(page.Items.SelectMany(l => l.PimsLeaseTerms, (lease, term) => (lease, term)));
            var leaseProperties = _mapper.Map<IEnumerable<Models.Lease.LeaseModel>>(page.Items.SelectMany(l => l.PimsPropertyLeases, (lease, property) => (lease, property)));
            var leaseTenants = _mapper.Map<IEnumerable<Models.Lease.LeaseModel>>(page.Items.SelectMany(l => l.PimsLeaseTenants, (lease, tenant) => (lease, tenant)));
            var leases = _mapper.Map<IEnumerable<Models.Lease.LeaseModel>>(page.Items.Where(l => !l.PimsLeaseTenants.Any() && !l.PimsLeaseTerms.Any() && !l.PimsPropertyLeases.Any()));
            var allLeases = leaseTerms.Concat(leaseProperties).Concat(leaseTenants).Concat(leases);

            return accept.ToString() switch
            {
                ContentTypes.CONTENT_TYPE_CSV => ReportHelper.GenerateCsv(allLeases),
                _ => ReportHelper.GenerateExcel(allLeases, "PIMS")
            };

        }

        #endregion
        #endregion
    }
}
