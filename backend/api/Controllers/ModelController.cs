using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Lease.Controllers
{
    /// <summary>
    /// LeaseController class, provides endpoints for interacting with leases.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    [Area("models")]
    [Route("[area]")]
    public class ModelController : ControllerBase
    {

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LeaseController class, initializes it with the specified arguments.
        /// </summary>
        public ModelController()
        {
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
        [ProducesResponseType(typeof(Pims.Api.Models.Concepts.PersonModel), 200)]
        [SwaggerOperation(Tags = new[] { "person" })]
        public IActionResult GetPerson(int id)
        {
            return new JsonResult(id);
        }


        #endregion
    }
}
