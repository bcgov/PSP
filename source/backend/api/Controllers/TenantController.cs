using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pims.Api.Policies;
using Pims.Dal;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Tenant;

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
        private readonly PimsOptions _pimsOptions;
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a TenantController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsOptions"></param>
        /// <param name="tenantRepository"></param>
        /// <param name="mapper"></param>
        public TenantController(IOptionsMonitor<PimsOptions> pimsOptions, ITenantRepository tenantRepository, IMapper mapper)
        {
            _pimsOptions = pimsOptions.CurrentValue;
            _tenantRepository = tenantRepository;
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
        [ProducesResponseType(204)]
        [SwaggerOperation(Tags = new[] { "tenants" })]
        public IActionResult Settings()
        {
            var tenant = _tenantRepository.TryGetTenantByCode(_pimsOptions.Tenant);

            if (tenant == null)
            {
                return new NoContentResult();
            }

            return new JsonResult(_mapper.Map<Model.TenantModel>(tenant));
        }

        /// <summary>
        /// Updates the tenant for the specified 'code'.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{code}")]
        [HasPermission(Permissions.SystemAdmin)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.TenantModel), 200)]
        [ProducesResponseType(403)]
        [SwaggerOperation(Tags = new[] { "tenants" })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "To support standardized routes (/{code})")]
        public IActionResult UpdateTenant(string code, Model.TenantModel model)
        {
            var tenant = _tenantRepository.UpdateTenant(_mapper.Map<Entity.PimsTenant>(model));
            return new JsonResult(_mapper.Map<Model.TenantModel>(tenant));
        }
        #endregion
    }
}
