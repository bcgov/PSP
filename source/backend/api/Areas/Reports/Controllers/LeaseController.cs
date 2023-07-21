using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Report;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Areas.Reports.Models.Lease;
using Pims.Api.Helpers.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Helpers.Reporting;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

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
        private readonly ILookupRepository _lookupRepository;
        private readonly ILeaseService _leaseService;
        private readonly ILeaseReportsService _leaseReportService;
        private readonly ILeasePaymentService _leasePaymentService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ReportController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="lookupRepository"></param>
        /// <param name="leaseService"></param>
        /// <param name="leaseReportService"></param>
        /// <param name="leasePaymentService"></param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="mapper"></param>
        public LeaseController(ILookupRepository lookupRepository, ILeaseService leaseService, ILeaseReportsService leaseReportService, ILeasePaymentService leasePaymentService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _lookupRepository = lookupRepository;
            _leaseService = leaseService;
            _leaseReportService = leaseReportService;
            _leasePaymentService = leasePaymentService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Endpoints
        #region Export Leases

        /// <summary>
        /// Exports leases as CSV or Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["text/csv", "application/application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"].
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpGet]
        [HasPermission(Permissions.LeaseView)]
        [Produces(ContentTypes.CONTENTTYPECSV, ContentTypes.CONTENTTYPEEXCELX)]
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
        ///     ["text/csv", "application/application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"].
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.LeaseView)]
        [Produces(ContentTypes.CONTENTTYPECSV, ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "lease", "report" })]
        public IActionResult ExportLeases([FromBody] Lease.Models.Search.LeaseFilterModel filter, bool all = false)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Lease filter must contain valid values.");
            }

            var acceptHeader = (string)this.Request.Headers["Accept"];

            if (acceptHeader != ContentTypes.CONTENTTYPECSV && acceptHeader != ContentTypes.CONTENTTYPEEXCEL && acceptHeader != ContentTypes.CONTENTTYPEEXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{acceptHeader}'.");
            }

            var flatLeases = GetCrossJoinLeases(filter, all);

            return acceptHeader.ToString() switch
            {
                ContentTypes.CONTENTTYPECSV => ReportHelper.GenerateCsv(flatLeases),
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
        [Produces(ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "lease", "report" })]
        public IActionResult ExportAggregatedLeases(int fiscalYearStart)
        {
            if (fiscalYearStart < 1900)
            {
                throw new BadRequestException("Fiscal year invalid.");
            }

            IEnumerable<PimsLease> leasesForFiscal = _leaseReportService.GetAggregatedLeaseReport(fiscalYearStart);
            var programs = _lookupRepository.GetAllLeaseProgramTypes();
            var regions = _lookupRepository.GetAllRegions();

            AggregatedLeasesModel model = new AggregatedLeasesModel(leasesForFiscal, fiscalYearStart, programs, regions);

            using (var template = new XLTemplate(Path.Combine(_webHostEnvironment.ContentRootPath, "Resources", "AggregatedLeasesTemplate.xlsx")))
            {
                template.AddVariable(model);
                template.Generate();
                var stream = new MemoryStream();
                template.SaveAs(stream);
                stream.Position = 0;

                return new FileStreamResult(stream, ContentTypes.CONTENTTYPEEXCELX);
            }
        }

        /// <summary>
        /// Exports lease payments as CSV or Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["application/application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"].
        /// </summary>
        /// <returns></returns>
        [HttpGet("payments")]
        [HasPermission(Permissions.PropertyView)]
        [Produces(ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "lease", "payements", "report" })]
        public IActionResult ExportLeasePayments(int fiscalYearStart)
        {
            var acceptHeader = (string)this.Request.Headers["Accept"];

            if (acceptHeader != ContentTypes.CONTENTTYPEEXCEL && acceptHeader != ContentTypes.CONTENTTYPEEXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{acceptHeader}'.");
            }

            DateTime startDate = fiscalYearStart.ToFiscalYearDate();
            var allPayments = _leasePaymentService.GetAllByDateRange(startDate, startDate.AddYears(1).AddDays(-1)); // Add years will give you the equivalent month, except for 29th/ 28th of leap years which is not the case here.
            var paymentItems = _mapper.Map<IEnumerable<Api.Models.Concepts.LeasePaymentReportModel>>(allPayments);

            return acceptHeader.ToString() switch
            {
                ContentTypes.CONTENTTYPECSV => ReportHelper.GenerateCsv<Api.Models.Concepts.LeasePaymentReportModel>(paymentItems.OrderBy(p => p.Region).ThenBy(p => p.LFileNumber).ThenByDescending(p => p.PaymentReceivedDate)),
                _ => ReportHelper.GenerateExcel(paymentItems, $"LeaseLicense_Payments")
            };
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
            var page = _leaseService.GetPage((LeaseFilter)filter, all);
            var allLeases = page.Items.SelectMany(l => l.PimsLeaseTerms.DefaultIfEmpty(), (lease, term) => (lease, term))
                .SelectMany(lt => lt.lease.PimsPropertyLeases.DefaultIfEmpty(), (leaseTerm, property) => (leaseTerm.term, leaseTerm.lease, property))
                .SelectMany(ltp => ltp.lease.PimsLeaseTenants.DefaultIfEmpty(), (leaseTermProperty, tenant) => (leaseTermProperty.term, leaseTermProperty.lease, leaseTermProperty.property, tenant));
            var flatLeases = _mapper.Map<IEnumerable<LeaseModel>>(allLeases);
            return flatLeases;
        }
        #endregion
    }
}
