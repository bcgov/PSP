using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Leases.Controllers
{
    /// <summary>
    /// InsuranceController class, provides endpoints for interacting with insurances.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("insurances")]
    [Route("v{version:apiVersion}/leases/{leaseId}/[area]")]
    [Route("/leases/{leaseId}/[area]")]
    public class InsuranceController : ControllerBase
    {
        #region Variables
        private readonly ILeaseService _leaseService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a InsuranceController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="leaseService"></param>
        /// <param name="logger"></param>
        ///
        public InsuranceController(IMapper mapper, ILeaseService leaseService, ILogger<InsuranceController> logger)
        {
            _mapper = mapper;
            _leaseService = leaseService;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Updates a list of insurance for a lease.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<InsuranceModel>), 200)]
        [SwaggerOperation(Tags = new[] { "insurance" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateInsurance(int leaseId, IEnumerable<InsuranceModel> insurances)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(InsuranceController),
                nameof(UpdateInsurance),
                User.GetUsername(),
                DateTime.Now);

            var updatedEntities = _leaseService.UpdateInsuranceByLeaseId(leaseId, _mapper.Map<IEnumerable<PimsInsurance>>(insurances));

            var insuranceModels = _mapper.Map<IEnumerable<InsuranceModel>>(updatedEntities);

            return new JsonResult(insuranceModels);
        }

        /// <summary>
        /// Get a list of insurance for a lease.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HasPermission(Permissions.LeaseEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<InsuranceModel>), 200)]
        [SwaggerOperation(Tags = new[] { "insurance" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetInsurance(int leaseId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyImprovementController),
                nameof(GetInsurance),
                User.GetUsername(),
                DateTime.Now);

            var insuranceModels = _mapper.Map<IEnumerable<InsuranceModel>>(_leaseService.GetInsuranceByLeaseId(leaseId));

            return new JsonResult(insuranceModels);
        }
        #endregion
    }
}
