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
    /// LeaseTenantController class, provides endpoints for interacting with lease tenants.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class LeaseTenantController : ControllerBase
    {
        #region Variables
        private readonly ILeaseService _leaseService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseTenantController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public LeaseTenantController(ILeaseService leaseService, IMapper mapper, ILogger<PropertyImprovementController> logger)
        {
            _leaseService = leaseService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the specified tenants on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{leaseId:long}/tenants")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseTenantModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetTenants(long leaseId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseTenantController),
                nameof(GetTenants),
                User.GetUsername(),
                DateTime.Now);

            var updatedLease = _leaseService.GetTenantsByLeaseId(leaseId);

            return new JsonResult(_mapper.Map<IEnumerable<LeaseTenantModel>>(updatedLease));
        }

        /// <summary>
        /// Update the specified tenants on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/tenants")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseTenantModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateTenants(long leaseId, IEnumerable<LeaseTenantModel> tenants)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseTenantController),
                nameof(UpdateTenants),
                User.GetUsername(),
                DateTime.Now);

            var tenantEntities = _mapper.Map<ICollection<Pims.Dal.Entities.PimsLeaseTenant>>(tenants);
            var updatedLease = _leaseService.UpdateTenantsByLeaseId(leaseId, tenantEntities);

            return new JsonResult(_mapper.Map<IEnumerable<LeaseTenantModel>>(updatedLease));
        }
        #endregion
    }
}
