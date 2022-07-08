using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Dummy.Controllers
{
    /// <summary>
    /// ModelController class, provides endpoints for interacting with leases.
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Area("models")]
    [Route("[area]")]
    public class ModelController : ControllerBase
    {

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ModelController class, initializes it with the specified arguments.
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
        [HttpGet("dummy/person/{id:long}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Pims.Api.Models.Concepts.PersonModel), 200)]
        [SwaggerOperation(Tags = new[] { "person" })]
        public IActionResult GetPerson(int id)
        {
            return new JsonResult(id);
        }

        /// <summary>
        /// Get the lease for the specified primary key 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("dummy/research/{id:long}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Pims.Api.Models.Concepts.ResearchFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "researchfile" })]
        public IActionResult GetResearchFile(int id)
        {
            return new JsonResult(id);
        }

        #endregion
    }
}
