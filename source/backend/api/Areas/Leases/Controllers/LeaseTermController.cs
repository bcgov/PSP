using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Api.Services;
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
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseTermController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="leaseTermService"></param>
        /// <param name="mapper"></param>
        ///
        public LeaseTermController(ILeaseTermService leaseTermService, IMapper mapper)
        {
            _leaseTermService = leaseTermService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Update the specified term on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{leaseId:long}/term")]
        [HasPermission(Permissions.LeaseAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult AddTerm(long leaseId, [FromBody] Models.Lease.TermModel termModel)
        {
            var termEntity = _mapper.Map<PimsLeaseTerm>(termModel);
            var updatedLease = _leaseTermService.AddTerm(leaseId, termModel.LeaseRowVersion, termEntity);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(updatedLease));
        }

        /// <summary>
        /// Update the specified term on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{leaseId:long}/term/{termId:long}")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult UpdateTerm(long leaseId, long termId, [FromBody] Models.Lease.TermModel termModel)
        {
            var termEntity = _mapper.Map<PimsLeaseTerm>(termModel);
            var updatedLease = _leaseTermService.UpdateTerm(leaseId, termId, termModel.LeaseRowVersion, termEntity);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(updatedLease));
        }

        /// <summary>
        /// Delete the specified term on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{leaseId:long}/term")]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult DeleteTerm(long leaseId, Models.Lease.TermModel termModel)
        {
            var termEntity = _mapper.Map<PimsLeaseTerm>(termModel);
            var updatedLease = _leaseTermService.DeleteTerm(leaseId, termModel.LeaseRowVersion, termEntity);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(updatedLease));
        }
        #endregion
    }
}
