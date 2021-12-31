using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

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
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LeaseController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public LeaseController(IPimsService pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
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
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult GetLease(int id)
        {
            var lease = _pimsService.Lease.Get(id);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(lease));
        }

        /// <summary>
        /// Add the specified lease. Allows the user to override the normal restriction on adding properties already associated to a lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [HasPermission(Permissions.LeaseAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult AddLease(Models.Lease.LeaseModel leaseModel, bool userOverride = false)
        {
            var leaseEntity = _mapper.Map<Pims.Dal.Entities.PimsLease>(leaseModel);
            var lease = _pimsService.Lease.Add(leaseEntity, userOverride);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(lease));
        }

        /// <summary>
        /// Update the specified lease, and attached properties.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult UpdateLease(Models.Lease.LeaseModel leaseModel, bool userOverride = false)
        {
            var leaseEntity = _mapper.Map<Pims.Dal.Entities.PimsLease>(leaseModel);
            _pimsService.Lease.Update(leaseEntity, false);
            var lease = _pimsService.Lease.UpdatePropertyLeases(leaseModel.Id, leaseModel.RowVersion, leaseEntity.PimsPropertyLeases, userOverride);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(lease));
        }
        #endregion
    }
}
