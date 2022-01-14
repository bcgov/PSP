using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models;
using Pims.Dal;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// SystemConstantController class, provides endpoints to retrieve system information api.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/systemConstant")]
    [Route("systemConstant")]
    public class SystemConstantController : ControllerBase
    {
        #region Variables
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a SystemConstantController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        public SystemConstantController(IPimsService pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Returns the system constants.
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SystemConstantModel), 200)]
        [SwaggerOperation(Tags = new[] { "systemConstant" })]
        public IActionResult Environment()
        {
            var systemConstants = _mapper.Map<SystemConstantModel[]>(_pimsService.SystemConstant.GetAll());
            return new JsonResult(systemConstants);
        }
        #endregion
    }
}
