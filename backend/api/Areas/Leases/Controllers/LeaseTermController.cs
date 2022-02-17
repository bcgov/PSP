using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

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
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LeaseTermController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public LeaseTermController(IPimsService pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
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
            var updatedLease = _pimsService.LeaseTermService.AddTerm(leaseId, termModel.LeaseRowVersion, termEntity);

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
            var updatedLease = _pimsService.LeaseTermService.UpdateTerm(leaseId, termId, termModel.LeaseRowVersion, termEntity);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(updatedLease));
        }

        /// <summary>
        /// Delete the specified term on the passed lease.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{leaseId:long}/term")]
        [HasPermission(Permissions.LeaseDelete)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Lease.LeaseModel>), 200)]
        [SwaggerOperation(Tags = new[] { "lease" })]
        public IActionResult DeleteTerm(long leaseId, Models.Lease.TermModel termModel)
        {
            var termEntity = _mapper.Map<PimsLeaseTerm>(termModel);
            var updatedLease = _pimsService.LeaseTermService.DeleteTerm(leaseId, termModel.LeaseRowVersion, termEntity);

            return new JsonResult(_mapper.Map<Models.Lease.LeaseModel>(updatedLease));
        }
        #endregion
    }
}
