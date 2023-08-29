using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Areas.Reports.Models.Acquisition;
using Pims.Api.Areas.Reports.Models.Agreement;
using Pims.Api.Helpers.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Helpers.Reporting;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Reports.Controllers
{
    /// <summary>
    /// AcquisitionController class, provides endpoints for generating reports.
    /// </summary>
    [Authorize]
    [ApiController]
    [Area("reports")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/acquisition")]
    [Route("[area]/acquisition")]
    public class AcquisitionController : ControllerBase
    {
        #region Variables
        private readonly IAcquisitionFileService _acquisitionFileService;
        private readonly ClaimsPrincipal _user;
        private readonly ICompReqFinancialService _compReqFinancialService;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="acquisitionFileService"></param>
        /// <param name="user"></param>
        /// <param name="compReqFinancialService"></param>
        public AcquisitionController(IAcquisitionFileService acquisitionFileService, ClaimsPrincipal user, ICompReqFinancialService compReqFinancialService)
        {
            _acquisitionFileService = acquisitionFileService;
            _user = user;
            _compReqFinancialService = compReqFinancialService;
        }
        #endregion

        #region endpoints

        /// <summary>
        /// Exports acquisition as Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["text/csv", "application/application/vnd.ms-excel"].
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("agreements")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces(ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "acquisition", "report" })]
        public IActionResult ExportAgreements([FromBody] AcquisitionReportFilterModel filter)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");

            var acceptHeader = (string)Request.Headers["Accept"];

            if (acceptHeader != ContentTypes.CONTENTTYPEEXCEL && acceptHeader != ContentTypes.CONTENTTYPEEXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{acceptHeader}'.");
            }

            var agreements = _acquisitionFileService.SearchAgreements(filter);
            var reportAgreements = agreements.Select(agreement => new AgreementReportModel(agreement, _user));

            return ReportHelper.GenerateExcel(reportAgreements, "Agreement Export");
        }

        /// <summary>
        /// Exports compensation requisitions as Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["text/csv", "application/application/vnd.ms-excel"].
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("compensation-requisitions")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces(ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [SwaggerOperation(Tags = new[] { "acquisition", "report" })]
        public IActionResult ExportCompensationRequisitions([FromBody] AcquisitionReportFilterModel filter)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");

            var acceptHeader = (string)Request.Headers["Accept"];
            if (acceptHeader != ContentTypes.CONTENTTYPEEXCEL && acceptHeader != ContentTypes.CONTENTTYPEEXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{acceptHeader}'.");
            }

            var financials = _compReqFinancialService.SearchCompensationRequisitionFinancials(filter);
            if (financials is not null && financials.Any())
            {
                var reportTotals = new CompensationFinancialReportTotalsModel(financials);
                var reportFinancials = financials.Select(financial => new CompensationFinancialReportModel(financial, reportTotals, _user));

                return ReportHelper.GenerateExcel(reportFinancials, "Compensation Requisition Export");
            }
            else
            {
                // Return 204 "No Content" to signal the frontend that we did not find any matching records.
                return NoContent();
            }
        }
        #endregion
    }
}
