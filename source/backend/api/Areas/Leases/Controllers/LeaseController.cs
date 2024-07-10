using System;
using System.Collections.Generic;
using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts.File;
using Pims.Api.Models.Concepts.Lease;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// LeaseController class, provides endpoints for interacting with leases.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("leases")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class LeaseController : ControllerBase
    {
        #region Variables
        private readonly ILeaseService _leaseService;
        private readonly IMapper _mapper;
        private readonly ILogger<LeaseController> _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public LeaseController(ILeaseService leaseService, IMapper mapper, ILogger<LeaseController> logger)
        {
            _mapper = mapper;
            _leaseService = leaseService;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the lease for the specified primary key 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LeaseModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetLease(int id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseController),
                nameof(GetLease),
                User.GetUsername(),
                DateTime.Now);

            var lease = _leaseService.GetById(id);
            var mapped = _mapper.Map<LeaseModel>(lease);
            return new JsonResult(mapped);
        }

        /// <summary>
        /// Gets the specified lease last updated-by information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}/updateInfo")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dal.Entities.Models.LastUpdatedByModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetLastUpdatedBy(long id)
        {
            var lastUpdated = _leaseService.GetLastUpdateInformation(id);
            return new JsonResult(lastUpdated);
        }

        /// <summary>
        /// Add the specified lease. Allows the user to override the normal restriction on adding properties already associated to a lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.LeaseAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LeaseModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddLease(LeaseModel leaseModel, [FromQuery] string[] userOverrideCodes)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseController),
                nameof(AddLease),
                User.GetUsername(),
                DateTime.Now);

            var leaseEntity = _mapper.Map<Pims.Dal.Entities.PimsLease>(leaseModel);
            var userOverrides = userOverrideCodes.Select(x => UserOverrideCode.Parse(x));
            var lease = _leaseService.Add(leaseEntity, userOverrides);

            return new JsonResult(_mapper.Map<LeaseModel>(lease));
        }

        /// <summary>
        /// Update the specified lease, and attached properties.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LeaseModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateLease(LeaseModel leaseModel, [FromQuery] string[] userOverrideCodes)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(LeaseController),
                nameof(UpdateLease),
                User.GetUsername(),
                DateTime.Now);

            var leaseEntity = _mapper.Map<Dal.Entities.PimsLease>(leaseModel);
            var userOverrides = userOverrideCodes.Select(x => UserOverrideCode.Parse(x));
            var updatedLease = _leaseService.Update(leaseEntity, userOverrides);

            return new JsonResult(_mapper.Map<LeaseModel>(updatedLease));
        }

        /// <summary>
        /// Get the lease checklist items.
        /// </summary>
        /// <returns>The checklist items.</returns>
        [HttpGet("{id:long}/checklist")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<FileChecklistItemModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetLeaseChecklistItems([FromRoute] long id)
        {
            var checklist = _leaseService.GetChecklistItems(id);

            return new JsonResult(_mapper.Map<IEnumerable<FileChecklistItemModel>>(checklist));
        }

        /// <summary>
        /// Update the lease checklist.
        /// </summary>
        /// <returns>Updated lease.</returns>
        [HttpPut("{id:long}/checklist")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LeaseModel), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateLeaseChecklist([FromRoute] long id, [FromBody] IList<FileChecklistItemModel> checklistItems)
        {
            if (checklistItems.Any(x => x.FileId != id))
            {
                throw new BadRequestException("All checklist items file id must match the Lease's id");
            }

            if (checklistItems.Count == 0)
            {
                throw new BadRequestException("Checklist items must be greater than zero");
            }

            var checklistItemEntities = _mapper.Map<IList<Dal.Entities.PimsLeaseChecklistItem>>(checklistItems);
            var updatedLease = _leaseService.UpdateChecklistItems(id, checklistItemEntities);

            return new JsonResult(_mapper.Map<LeaseModel>(updatedLease));
        }

        #endregion
    }
}
