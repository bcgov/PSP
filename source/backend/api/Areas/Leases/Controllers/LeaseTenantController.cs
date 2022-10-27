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
        private readonly ILeaseRepository _leaseRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseTenantController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseRepository"></param>
        /// <param name="mapper"></param>
        ///
        public LeaseTenantController(ILeaseRepository leaseRepository, IMapper mapper)
        {
            _leaseRepository = leaseRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Update the specified tenants on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/tenants")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult UpdateTenants(long leaseId, Models.Lease.LeaseModel lease)
        {
            var tenantEntities = _mapper.Map<ICollection<Pims.Dal.Entities.PimsLeaseTenant>>(lease.Tenants);
            var updatedLease = _leaseRepository.UpdateLeaseTenants(leaseId, lease.RowVersion, tenantEntities);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(updatedLease));
        }
        #endregion
    }
}
