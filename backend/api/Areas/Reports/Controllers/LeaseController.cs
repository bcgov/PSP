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
using Pims.Dal.Entities;
using Pims.Api.Areas.Reports.Models.Lease;
using ClosedXML.Report;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Pims.Dal.Services;

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
        private readonly IPimsRepository _pimsRepository;
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ReportController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="pimsRepository"></param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="mapper"></param>
        public LeaseController(IPimsRepository pimsRepository, IPimsService pimsService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _pimsRepository = pimsRepository;
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
        [HasPermission(Permissions.LeaseView)]
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
        [HasPermission(Permissions.LeaseView)]
        [Produces(ContentTypes.CONTENT_TYPE_CSV, ContentTypes.CONTENT_TYPE_EXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "lease", "report" })]
        public IActionResult ExportLeases([FromBody] Lease.Models.Search.LeaseFilterModel filter, bool all = false)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Lease filter must contain valid values.");
            }

            var accept = (string)this.Request.Headers["Accept"] ?? throw new BadRequestException($"HTTP request header 'Accept' is required.");

            if (accept != ContentTypes.CONTENT_TYPE_CSV && accept != ContentTypes.CONTENT_TYPE_EXCEL && accept != ContentTypes.CONTENT_TYPE_EXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{accept}'.");
            }

            var flatLeases = GetCrossJoinLeases(filter, all);

            return accept.ToString() switch
            {
                ContentTypes.CONTENT_TYPE_CSV => ReportHelper.GenerateCsv(flatLeases),
                _ => ReportHelper.GenerateExcel(flatLeases, "PIMS")
            };
        }

        /// <summary>
        /// Exports leases aggregated by program and region as an Excel file, only including leases that are part of the passed fiscal year.
        /// 
        /// </summary>
        /// <param name="fiscalYearStart"></param>
        /// <returns></returns>
        [HttpGet("aggregated")]
        [HasPermission(Permissions.LeaseView)]
        [Produces(ContentTypes.CONTENT_TYPE_EXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "lease", "report" })]
        public IActionResult ExportAggregatedLeases(int fiscalYearStart)
        {
            if (fiscalYearStart < 1900)
            {
                throw new BadRequestException("Fiscal year invalid.");
            }

            IEnumerable<PimsLease> leasesForFiscal = _pimsService.LeaseReportsService.GetAggregatedLeaseReport(fiscalYearStart);
            var programs = _pimsRepository.Lookup.GetLeaseProgramTypes();
            var regions = _pimsRepository.Lookup.GetRegions();

            AggregatedLeasesModel model = new AggregatedLeasesModel(leasesForFiscal, fiscalYearStart, programs, regions);

            using (var template = new XLTemplate(Path.Combine(_webHostEnvironment.ContentRootPath, "Resources", "AggregatedLeasesTemplate.xlsx")))
            {
                template.AddVariable(model);
                template.Generate();
                var stream = new MemoryStream();
                template.SaveAs(stream);
                stream.Position = 0;

                return new FileStreamResult(stream, ContentTypes.CONTENT_TYPE_EXCELX);
            }
        }

        #endregion

        /// <summary>
        /// Create duplicate lease rows for every unique property lease, tenant, and term.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public IEnumerable<LeaseModel> GetCrossJoinLeases(Lease.Models.Search.LeaseFilterModel filter, bool all = false)
        {
            filter.Quantity = all ? _pimsRepository.Lease.Count() : filter.Quantity;
            var page = _pimsRepository.Lease.GetPage((LeaseFilter)filter);
            var allLeases = page.Items.SelectMany(l => l.PimsLeaseTerms.DefaultIfEmpty(), (lease, term) => (lease, term))
                .SelectMany(lt => lt.lease.PimsPropertyLeases.DefaultIfEmpty(), (leaseTerm, property) => (leaseTerm.term, leaseTerm.lease, property))
                .SelectMany(ltp => ltp.lease.PimsLeaseTenants.DefaultIfEmpty(), (leaseTermProperty, tenant) => (leaseTermProperty.term, leaseTermProperty.lease, leaseTermProperty.property, tenant));
            var flatLeases = _mapper.Map<IEnumerable<LeaseModel>>(allLeases);
            return flatLeases;
        }
        #endregion
    }
}
