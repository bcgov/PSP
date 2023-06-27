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
    public class PropertyImprovementController : ControllerBase
    {
        #region Variables
        private readonly ILeaseService _leaseService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyImprovementController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public PropertyImprovementController(ILeaseService leaseService, IMapper mapper, ILogger<PropertyImprovementController> logger)
        {
            _leaseService = leaseService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the improvements for the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{leaseId:long}/improvements")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PropertyImprovementModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetImprovements(long leaseId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyImprovementController),
                nameof(GetImprovements),
                User.GetUsername(),
                DateTime.Now);

            var improvements = _leaseService.GetImprovementsByLeaseId(leaseId);

            return new JsonResult(_mapper.Map<IEnumerable<PropertyImprovementModel>>(improvements));
        }

        /// <summary>
        /// Update the specified improvements on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/improvements")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PropertyImprovementModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateImprovements(long leaseId, IEnumerable<PropertyImprovementModel> improvements)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyImprovementController),
                nameof(UpdateImprovements),
                User.GetUsername(),
                DateTime.Now);

            var improvementEntities = _mapper.Map<ICollection<Pims.Dal.Entities.PimsPropertyImprovement>>(improvements);
            var updatedImprovements = _leaseService.UpdateImprovementsByLeaseId(leaseId, improvementEntities);

            return new JsonResult(_mapper.Map<IEnumerable<PropertyImprovementModel>>(updatedImprovements));
        }
        #endregion
    }
}
