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
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// PropertyImprovementController class, provides endpoints for interacting with lease property improvements.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PropertyLeaseController : ControllerBase
    {
        #region Variables
        private readonly ILeaseService _leaseService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyLeaseController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public PropertyLeaseController(ILeaseService leaseService, IMapper mapper, ILogger<PropertyLeaseController> logger)
        {
            _leaseService = leaseService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the properties for the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{leaseId:long}/properties")]
        [HasPermission(Permissions.LeaseView)]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PropertyImprovementModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetProperties(long leaseId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyLeaseController),
                nameof(GetProperties),
                User.GetUsername(),
                DateTime.Now);

            var properties = _leaseService.GetPropertiesByLeaseId(leaseId);

            return new JsonResult(_mapper.Map<IEnumerable<PropertyLeaseModel>>(properties));
        }
        #endregion
    }
}
