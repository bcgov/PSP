using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.CompensationRequisition.Controllers
{
    /// <summary>
    /// H120CategoryController class, provides endpoints to handle h120 categories.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("H120Category")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class H120CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IH120CategoryService _h120CategoryService;

        public H120CategoryController(IMapper mapper, ILogger<CompensationRequisitionController> logger, IH120CategoryService h120CategoryService)
        {
            _mapper = mapper;
            _logger = logger;
            _h120CategoryService = h120CategoryService;
        }

        /// <summary>
        /// Gets all the compensation requisition financial categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<H120CategoryModel>), 200)]
        [SwaggerOperation(Tags = new[] { "h120" })]
        public IActionResult GetH120Categories()
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(H120CategoryController),
                nameof(GetH120Categories),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation("Dispatching to service: {Service}", _h120CategoryService.GetType());

            var h120Categories = _h120CategoryService.GetAll();

            return new JsonResult(_mapper.Map<IEnumerable<H120CategoryModel>>(h120Categories));
        }
    }
}
