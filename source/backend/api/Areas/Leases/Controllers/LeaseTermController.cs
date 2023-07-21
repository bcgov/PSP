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

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// LeaseTermController class, provides endpoints for interacting with lease property terms.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class LeaseTermController : ControllerBase
    {
        #region Variables
        private readonly ILeaseTermService _leaseTermService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseTermController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseTermService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public LeaseTermController(ILeaseTermService leaseTermService, IMapper mapper, ILogger<PropertyImprovementController> logger)
        {
            _leaseTermService = leaseTermService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Update the specified term on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{leaseId:long}/terms")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<LeaseTermModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetTerms(long leaseId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseTermController),
                nameof(GetTerms),
                User.GetUsername(),
                DateTime.Now);

            var terms = _leaseTermService.GetTerms(leaseId);

            return new JsonResult(_mapper.Map<IEnumerable<LeaseTermModel>>(terms));
        }

        /// <summary>
        /// Update the specified term on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{leaseId:long}/terms")]
        [HasPermission(Permissions.LeaseAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LeaseTermModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddTerm(long leaseId, [FromBody] LeaseTermModel termModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseTermController),
                nameof(AddTerm),
                User.GetUsername(),
                DateTime.Now);

            var termEntity = _mapper.Map<PimsLeaseTerm>(termModel);
            var updatedLease = _leaseTermService.AddTerm(leaseId, termEntity);

            return new JsonResult(_mapper.Map<LeaseTermModel>(updatedLease));
        }

        /// <summary>
        /// Update the specified term on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/terms/{termId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LeaseTermModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateTerm(long leaseId, long termId, [FromBody] LeaseTermModel termModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseTermController),
                nameof(UpdateTerm),
                User.GetUsername(),
                DateTime.Now);

            var termEntity = _mapper.Map<PimsLeaseTerm>(termModel);
            var updatedLease = _leaseTermService.UpdateTerm(leaseId, termId, termEntity);

            return new JsonResult(_mapper.Map<LeaseTermModel>(updatedLease));
        }

        /// <summary>
        /// Delete the specified term on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{leaseId:long}/terms")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LeaseTermModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteTerm(long leaseId, LeaseTermModel termModel)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseTermController),
                nameof(DeleteTerm),
                User.GetUsername(),
                DateTime.Now);

            var termEntity = _mapper.Map<PimsLeaseTerm>(termModel);
            var updatedLease = _leaseTermService.DeleteTerm(leaseId, termEntity);

            return new JsonResult(_mapper.Map<LeaseTermModel>(updatedLease));
        }
        #endregion
    }
}
