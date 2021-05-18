using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pims.Dal;
using Swashbuckle.AspNetCore.Annotations;
using Model = Pims.Api.Models.Tenant;
using Entity = Pims.Dal.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// TenantController class, provides endpoints for tenant settings.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/tenants")]
    [Route("tenants")]
    public class TenantController : ControllerBase
    {
        #region Variables
        private readonly Pims.Dal.PimsOptions _pimsOptions;
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TenantController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsOptions"></param>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        public TenantController(IOptionsMonitor<Pims.Dal.PimsOptions> pimsOptions, IPimsService pimsService, IMapper mapper)
        {
            _pimsOptions = pimsOptions.CurrentValue;
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Provides tenant settings that are configured for the environment.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.TenantModel), 200)]
        [SwaggerOperation(Tags = new[] { "tenants" })]
        public IActionResult Settings()
        {
            var tenant = _pimsService.Tenant.GetTenant(_pimsOptions.Tenant);
            return new JsonResult(_mapper.Map<Model.TenantModel>(tenant));
        }


        /// <summary>
        /// Updates the tenant for the specified 'code'.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{code}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.TenantModel), 200)]
        [SwaggerOperation(Tags = new[] { "tenants" })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "To support standardized routes (/{code})")]
        public IActionResult UpdateTenant(string code, Model.TenantModel model)
        {
            var tenant = _pimsService.Tenant.UpdateTenant(_mapper.Map<Entity.Tenant>(model));
            return new JsonResult(_mapper.Map<Model.TenantModel>(tenant));
        }
        #endregion
    }
}
