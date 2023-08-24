using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Acquisition.Models.Search;
using Pims.Api.Helpers.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Helpers.Reporting;
using Pims.Api.Models;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Acquisition.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("acquisitionfiles")]
    [Route("v{version:apiVersion}/[area]/export")]
    [Route("[area]/export")]
    public class ReportController : ControllerBase
    {
        private readonly IAcquisitionFileService _acquisitionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ReportController(IAcquisitionFileService acquisitionService, IMapper mapper, ILogger<ReportController> logger)
        {
            _acquisitionService = acquisitionService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces(ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 409)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile", "report" })]
        public IActionResult ExportLeases([FromQuery]AcquisitionFilterModel filter)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Acquisition files filter must contain valid values.");
            }

            var acceptHeader = (string)Request.Headers["Accept"];
            if (acceptHeader != ContentTypes.CONTENTTYPEEXCEL && acceptHeader != ContentTypes.CONTENTTYPEEXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{acceptHeader}'.");
            }

            var acquisitionFileData = _acquisitionService.GetAcquisitionFileExport((AcquisitionFilter)filter);

            return ReportHelper.GenerateExcel(acquisitionFileData, "PIMS_Acquisition_Files");
        }
    }
}
