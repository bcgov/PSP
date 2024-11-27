using Microsoft.AspNetCore.Mvc;
using Pims.Api.Services;
using Swashbuckle.AspNetCore.Annotations;
using Model = Pims.Api.Models.Health;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// HealthController class, provides endpoints to check the health of the api.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/health")]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        private readonly IEnvironmentService _environmentService;

        #region Constructors

        /// <summary>
        /// Creates a new instances of a HealthController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="environmentService"></param>
        public HealthController(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Return environment information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("env")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.EnvModel), 200)]
        [SwaggerOperation(Tags = new[] { "health" })]
        public IActionResult Environment()
        {
            var environment = _environmentService.GetEnvironmentVariables();

            return new JsonResult(environment);
        }
        #endregion
    }
}
