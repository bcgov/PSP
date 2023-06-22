using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Acquisition.Controllers
{
    /// <summary>
    /// InterestHolderController class, provides endpoints for interacting with acquisition file InterestHolders.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("acquisitionfiles")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class InterestHolderController : ControllerBase
    {
        #region Variables
        private readonly IAcquisitionFileService _acquisitionService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a InterestHolderController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="acquisitionService"></param>
        /// <param name="mapper"></param>
        ///
        public InterestHolderController(IAcquisitionFileService acquisitionService, IMapper mapper)
        {
            _acquisitionService = acquisitionService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the acquisition file InterestHolders.
        /// </summary>
        /// <returns>The interest holder items.</returns>
        [HttpGet("{id:long}/interestholders")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<InterestHolderModel>), 200)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetAcquisitionFileInterestHolders([FromRoute] long id)
        {
            var interestHolders = _acquisitionService.GetInterestHolders(id);
            return new JsonResult(_mapper.Map<List<Api.Models.Concepts.InterestHolderModel>>(interestHolders));
        }

        /// <summary>
        /// Update the acquisition file InterestHolders.
        /// </summary>
        /// <returns>The updated interest holder items.</returns>
        [HttpPut("{id:long}/interestholders")]
        [HasPermission(Permissions.AcquisitionFileView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(InterestHolderModel), 200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 409)]
        [SwaggerOperation(Tags = new[] { "acquisitionfile" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateInterestHolderFile([FromRoute] long id, [FromBody] List<InterestHolderModel> interestHolders)
        {
            var interestHolderEntities = _mapper.Map<List<Dal.Entities.PimsInterestHolder>>(interestHolders);
            var updatedInterestHolders = _acquisitionService.UpdateInterestHolders(id, interestHolderEntities);
            return new JsonResult(_mapper.Map<List<Dal.Entities.PimsInterestHolder>>(updatedInterestHolders));
        }

        #endregion
    }
}
