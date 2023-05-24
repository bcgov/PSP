using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// CompensationController class, provides endpoints to handle compensation requests.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/compensations/")]
    [Route("/compensations")]
    public class CompensationController : ControllerBase
    {
        #region Variables

        private readonly ICompensationService _compensationService;

        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public CompensationController(ICompensationService compensationService, IMapper mapper)
        {
            _compensationService = compensationService;
            _mapper = mapper;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Get all the compensations corresponding to the passed file id.
        /// </summary>
        /// <param name="fileId">The file to retrieve compensations for.</param>
        /// <returns></returns>
        [HttpGet("/acquisitionFile/{fileId}/compensations")]
        [HasPermission(Permissions.CompensationRequisitionView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CompensationModel>), 200)]
        [SwaggerOperation(Tags = new[] { "compensation" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetFileCompensations(long fileId)
        {
            var pimsCompensations = _compensationService.GetAcquisitionCompensations(fileId);
            var compensations = _mapper.Map<List<CompensationModel>>(pimsCompensations);
            return new JsonResult(compensations);
        }

        /// <summary>
        /// Deletes the compensation with the matching id.
        /// </summary>
        /// <param name="compensationId">Used to identify the compensation and delete it.</param>
        /// <returns></returns>
        [HttpDelete("{compensationId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.CompensationRequisitionDelete)]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "compensation" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteCompensation(long compensationId)
        {
            var result = _compensationService.DeleteCompensation(compensationId);
            return new JsonResult(result);
        }
        #endregion
    }
}
