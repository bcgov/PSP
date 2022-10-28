using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal.Repositories;
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
        private readonly ILeaseRepository _leaseRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseRepository"></param>
        /// <param name="mapper"></param>
        ///
        public LeaseController(ILeaseRepository leaseRepository, IMapper mapper)
        {
            _leaseRepository = leaseRepository;
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
            var lease = _leaseRepository.Get(id);
            var mapped = _mapper.Map<Models.Lease.LeaseModel>(lease);
            return new JsonResult(mapped);
        }

        /// <summary>
        /// Add the specified lease. Allows the user to override the normal restriction on adding properties already associated to a lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.LeaseAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult AddLease(Models.Lease.LeaseModel leaseModel, bool userOverride = false)
        {
            var leaseEntity = _mapper.Map<Pims.Dal.Entities.PimsLease>(leaseModel);
            var lease = _leaseRepository.Add(leaseEntity, userOverride);

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
            _leaseRepository.Update(leaseEntity, false);
            var lease = _leaseRepository.UpdatePropertyLeases(leaseModel.Id, leaseModel.RowVersion, leaseEntity.PimsPropertyLeases, userOverride);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(lease));
        }
        #endregion
    }
}
