using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Services;
using Pims.Core.Api.Policies;
using Pims.Core.Json;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Property.Controllers
{
    /// <summary>
    /// PropertyLookupController class, provides orchestrated property lookup endpoints.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("properties")]
    [Route("v{version:apiVersion}/[area]/lookup")]
    [Route("[area]/lookup")]
    public class PropertyLookupController : ControllerBase
    {
        private readonly IPropertyLookupService _propertyLookupService;

        public PropertyLookupController(IPropertyLookupService propertyLookupService)
        {
            _propertyLookupService = propertyLookupService;
        }

        /// <summary>
        /// Lookup a property by PID in PIMS first, then PMBC when not found in PIMS.
        /// </summary>
        /// <param name="pid">The property PID.</param>
        /// <returns>A lookup result with source indicators and record data.</returns>
        [HttpGet("pid/{pid}")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> GetPropertyLookupByPid(string pid)
        {
            var result = await _propertyLookupService.LookupByPidAsync(pid);
            return new JsonResult(result);
        }
    }
}
