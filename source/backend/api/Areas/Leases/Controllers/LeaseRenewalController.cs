
using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Lease;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// LeaseRenewalController class, provides endpoints for interacting with lease renewals.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class LeaseRenewalController : ControllerBase
    {
        #region Variables
        private readonly ILeaseService _leaseService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseRenewalController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public LeaseRenewalController(ILeaseService leaseService, IMapper mapper, ILogger<LeaseRenewalController> logger)
        {
            _leaseService = leaseService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the renewals on the passed lease id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{leaseId:long}/renewals")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseRenewalModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetRenewals(long leaseId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseRenewalController),
                nameof(GetRenewals),
                User.GetUsername(),
                DateTime.Now);

            var leaseRenewals = _leaseService.GetRenewalsByLeaseId(leaseId);

            return new JsonResult(_mapper.Map<IEnumerable<LeaseRenewalModel>>(leaseRenewals));
        }
        #endregion
    }
}
