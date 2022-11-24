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
        private readonly ILeaseRepository _leaseRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyImprovementController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseRepository"></param>
        /// <param name="mapper"></param>
        ///
        public PropertyImprovementController(ILeaseRepository leaseRepository, IMapper mapper)
        {
            _leaseRepository = leaseRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Update the specified improvements on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/improvements")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult UpdateImprovements(long leaseId, Models.Lease.LeaseModel lease)
        {
            var improvementEntities = _mapper.Map<ICollection<Pims.Dal.Entities.PimsPropertyImprovement>>(lease.Improvements);
            var updatedLease = _leaseRepository.UpdateLeaseImprovements(leaseId, lease.RowVersion, improvementEntities);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(updatedLease));
        }
        #endregion
    }
}
