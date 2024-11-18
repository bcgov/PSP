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
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// LeaseStakeholderController class, provides endpoints for interacting with lease stakeholder.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class LeaseStakeholderController : ControllerBase
    {
        #region Variables
        private readonly ILeaseService _leaseService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseStakeholderController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public LeaseStakeholderController(ILeaseService leaseService, IMapper mapper, ILogger<PropertyImprovementController> logger)
        {
            _leaseService = leaseService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the specified stakeholders on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{leaseId:long}/stakeholders")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseStakeholderModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetStakeholders(long leaseId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseStakeholderController),
                nameof(GetStakeholders),
                User.GetUsername(),
                DateTime.Now);

            var updatedLease = _leaseService.GetStakeholdersByLeaseId(leaseId);

            return new JsonResult(_mapper.Map<IEnumerable<LeaseStakeholderModel>>(updatedLease));
        }

        /// <summary>
        /// Update the specified stakeholders on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/stakeholders")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseStakeholderModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateStakeholders(long leaseId, IEnumerable<LeaseStakeholderModel> stakeholders)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseStakeholderController),
                nameof(UpdateStakeholders),
                User.GetUsername(),
                DateTime.Now);

            var stakeholderEntities = _mapper.Map<ICollection<Pims.Dal.Entities.PimsLeaseStakeholder>>(stakeholders);
            var updatedLease = _leaseService.UpdateStakeholdersByLeaseId(leaseId, stakeholderEntities);

            return new JsonResult(_mapper.Map<IEnumerable<LeaseStakeholderModel>>(updatedLease));
        }

        /// <summary>
        /// Get all stakeholders types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("stakeholdertypes")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseStakeholderTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetStakeholderTypes()
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseStakeholderController),
                nameof(GetStakeholderTypes),
                User.GetUsername(),
                DateTime.Now);

            var stakeholderTypes = _leaseService.GetAllStakeholderTypes();

            return new JsonResult(_mapper.Map<IEnumerable<LeaseStakeholderTypeModel>>(stakeholderTypes));
        }
        #endregion
    }
}
