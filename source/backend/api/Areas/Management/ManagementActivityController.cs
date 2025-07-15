using System;
using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Property;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Management.Controllers
{
    /// <summary>
    /// ManagementActivityController class, provides endpoints for interacting with management activities.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("management-activities")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ManagementActivityController : ControllerBase
    {
        #region Variables
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        /// <summary>
        /// Creates a new instance of a ManagementActivityController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ManagementActivityController(IPropertyService propertyService, IMapper mapper, ILogger<ManagementActivityController> logger)
        {
            _propertyService = propertyService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get the specified management activity.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{managementActivityId:long}")]
        [HasPermission(Permissions.ManagementView, Permissions.ActivityView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManagementActivityModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetManagementActivity(long managementActivityId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(ManagementActivityController),
                nameof(GetManagementActivity),
                User.GetUsername(),
                DateTime.Now);

            var activity = _propertyService.GetActivity(managementActivityId);

            return new JsonResult(_mapper.Map<ManagementActivityModel>(activity));
        }
    }
}
