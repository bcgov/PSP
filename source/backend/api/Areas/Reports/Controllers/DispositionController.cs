using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Disposition.Models.Search;
using Pims.Api.Helpers.Constants;
using Pims.Core.Api.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Helpers.Reporting;
using Pims.Core.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Reports.Controllers
{
    /// <summary>
    /// DispositionController class, provides endpoints for generating reports.
    /// </summary>
    [Authorize]
    [ApiController]
    [Area("reports")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/Disposition")]
    [Route("[area]/Disposition")]
    public class DispositionController : ControllerBase
    {
        #region Variables
        private readonly IDispositionFileService _dispositionFileService;
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DispositionController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="dispositionFileService"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DispositionController(IDispositionFileService dispositionFileService, ClaimsPrincipal user, ILogger<DispositionController> logger)
        {
            _dispositionFileService = dispositionFileService;
            _user = user;
            _logger = logger;
        }
        #endregion

        #region endpoints

        /// <summary>
        /// Get the Excel Report for Disposition Files.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Excel File Blob that matches filter criteria.</returns>
        [HttpGet]
        [HasPermission(Permissions.DispositionView)]
        [Produces(ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [SwaggerOperation(Tags = new[] { "Dispositionfile", "report" })]
        public IActionResult ExportDispositionFiles([FromQuery] DispositionFilterModel filter)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DispositionController),
                nameof(ExportDispositionFiles),
                _user.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _dispositionFileService.GetType());

            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Disposition files filter must contain valid values.");
            }

            var acceptHeader = (string)Request.Headers["Accept"];
            if (acceptHeader != ContentTypes.CONTENTTYPEEXCEL && acceptHeader != ContentTypes.CONTENTTYPEEXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{acceptHeader}'.");
            }

            var dispositionFileData = _dispositionFileService.GetDispositionFileExport((DispositionFilter)filter);
            if (dispositionFileData.Count.Equals(0))
            {
                return NoContent();
            }

            return ReportHelper.GenerateExcel(dispositionFileData, "Disposition File Export");
        }
        #endregion
    }
}
