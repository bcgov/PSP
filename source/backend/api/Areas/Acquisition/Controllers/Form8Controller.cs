using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Acquisition.Controllers
{
    /// <summary>
    /// Form8Controller class, provides endpoints for Acquisition File's form 8's.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("acquisitionfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class Form8Controller : ControllerBase
    {
        private readonly IAcquisitionFileService _acquisitionFileService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public Form8Controller(IAcquisitionFileService acquisitionService, IMapper mapper, ILogger<Form8Controller> logger)
        {
            _acquisitionFileService = acquisitionService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all Form 8's from the Acquisition File.
        /// </summary>
        /// <param name="id">Acquisition File Id.</param>
        /// <returns>List of all Form8's created for the acquisition file.</returns>
        [HttpGet("{id:long}/form8")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<Form8Model>), 200)]
        [SwaggerOperation(Tags = new[] { "form8" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetFileForm8s([FromRoute] long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(Form8Controller),
                nameof(GetFileForm8s),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_acquisitionFileService.GetType()}");

            var pimsForm8s = _acquisitionFileService.GetAcquisitionForm8s(id);
            var form8s = _mapper.Map<List<Form8Model>>(pimsForm8s);

            return new JsonResult(form8s);
        }

        /// <summary>
        /// Creates a new Form8 for the acquistion file.
        /// </summary>
        /// <param name="id">Acquisition File Id.</param>
        /// <param name="form8">Form8 Data Model.</param>
        /// <returns></returns>
        [HttpPost("{id:long}/form8")]
        [HasPermission(Permissions.AcquisitionFileEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Form8Model), 201)]
        [SwaggerOperation(Tags = new[] { "form8" })]
        public IActionResult AddForm8([FromRoute]long id, [FromBody]Form8Model form8)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(Form8Controller),
                nameof(AddForm8),
                User.GetUsername(),
                DateTime.Now);
            _logger.LogInformation($"Dispatching to service: {_acquisitionFileService.GetType()}");

            var form8Entity = _mapper.Map<PimsForm8>(form8);
            var newForm8 = _acquisitionFileService.AddForm8(id, form8Entity);

            return new JsonResult(_mapper.Map<Form8Model>(newForm8));
        }
    }
}
