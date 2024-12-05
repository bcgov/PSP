using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Lease;
using Pims.Core.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// LeasePeriodController class, provides endpoints for interacting with lease property periods.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class LeasePeriodController : ControllerBase
    {
        #region Variables
        private readonly ILeasePeriodService _LeasePeriodService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeasePeriodController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="LeasePeriodService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public LeasePeriodController(ILeasePeriodService LeasePeriodService, IMapper mapper, ILogger<PropertyImprovementController> logger)
        {
            _LeasePeriodService = LeasePeriodService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Update the specified period on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{leaseId:long}/periods")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeasePeriodModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetPeriods(long leaseId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeasePeriodController),
                nameof(GetPeriods),
                User.GetUsername(),
                DateTime.Now);

            var periods = _LeasePeriodService.GetPeriods(leaseId);

            return new JsonResult(_mapper.Map<IEnumerable<LeasePeriodModel>>(periods));
        }

        /// <summary>
        /// Update the specified period on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{leaseId:long}/periods")]
        [HasPermission(Permissions.LeaseAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LeasePeriodModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddPeriod(long leaseId, [FromBody] LeasePeriodModel periodModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeasePeriodController),
                nameof(AddPeriod),
                User.GetUsername(),
                DateTime.Now);

            var periodEntity = _mapper.Map<PimsLeasePeriod>(periodModel);
            var updatedLease = _LeasePeriodService.AddPeriod(leaseId, periodEntity);

            return new JsonResult(_mapper.Map<LeasePeriodModel>(updatedLease));
        }

        /// <summary>
        /// Update the specified period on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/periods/{periodId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LeasePeriodModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdatePeriod(long leaseId, long periodId, [FromBody] LeasePeriodModel periodModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeasePeriodController),
                nameof(UpdatePeriod),
                User.GetUsername(),
                DateTime.Now);

            var periodEntity = _mapper.Map<PimsLeasePeriod>(periodModel);
            var updatedLease = _LeasePeriodService.UpdatePeriod(leaseId, periodId, periodEntity);

            return new JsonResult(_mapper.Map<LeasePeriodModel>(updatedLease));
        }

        /// <summary>
        /// Delete the specified period on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{leaseId:long}/periods")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LeasePeriodModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeletePeriod(long leaseId, LeasePeriodModel periodModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeasePeriodController),
                nameof(DeletePeriod),
                User.GetUsername(),
                DateTime.Now);

            var periodEntity = _mapper.Map<PimsLeasePeriod>(periodModel);
            var updatedLease = _LeasePeriodService.DeletePeriod(leaseId, periodEntity);

            return new JsonResult(_mapper.Map<LeasePeriodModel>(updatedLease));
        }
        #endregion
    }
}
