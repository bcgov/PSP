using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Mapping.Converters;
using Pims.Api.Policies;
using Pims.Dal.Security;
using Pims.Ltsa;
using Pims.Ltsa.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model = Pims.Ltsa.Models;

namespace Pims.Api.Areas.Tools.Controllers
{
    /// <summary>
    /// LtsaController class, provides endpoints to integrate with Ltsa.
    /// </summary>
    [Authorize]
    [ApiController]
    [Area("tools")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/ltsa")]
    [Route("[area]/ltsa")]
    public class LtsaController : ControllerBase
    {
        #region Variables
        private readonly ILtsaService _ltsaService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LtsaController class.
        /// </summary>
        /// <param name="ltsaService"></param>
        /// <param name="mapper"></param>
        public LtsaController(ILtsaService ltsaService, IMapper mapper)
        {
            _ltsaService = ltsaService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Make a request to Ltsa for title summaries that match the specified `search`.
        /// </summary>
        /// <param name="pid">the parcel identifier to search for</param>
        /// <returns>An array of title summary matches.</returns>
        [HttpGet("titleSummaries")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.TitleSummariesResponse), 200)]
        [ProducesResponseType(typeof(Pims.Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "tools-ltsa" })]
        [HasPermission(Permissions.PropertyEdit)]
        public async Task<IActionResult> FindTitleSummariesAsync(string pid)
        {
            var result = await _ltsaService.GetTitleSummariesAsync(ParcelConverter.ConvertPID(pid));
            return new JsonResult(result);
        }

        /// <summary>
        /// Post a new order using default parameters and the passed in titleNumber.
        /// </summary>
        /// <param name="titleNumber">the title number to create the order for</param>
        /// <param name="landTitleDistrictCode">the land title district code</param>
        /// <returns>The order created within LTSA</returns>
        [HttpPost("titleOrder")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.OrderWrapper<Model.TitleOrder>), 200)]
        [ProducesResponseType(typeof(Pims.Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "tools-ltsa" })]
        [HasPermission(Permissions.PropertyEdit)]
        public async Task<IActionResult> PostTitleOrderAsync(string titleNumber, string landTitleDistrictCode)
        {
            var result = await _ltsaService.PostTitleOrder(titleNumber, landTitleDistrictCode);
            return new JsonResult(result);
        }

        /// <summary>
        /// Post a new order using default parameters and the passed in titleNumber.
        /// </summary>
        /// <param name="titleNumber">the title number to create the order for</param>
        /// <returns>The order created within LTSA</returns>
        [HttpPost("parcelInfoOrder")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.OrderWrapper<Model.ParcelInfoOrder>), 200)]
        [ProducesResponseType(typeof(Pims.Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "tools-ltsa" })]
        [HasPermission(Permissions.PropertyEdit)]
        public async Task<IActionResult> PostParcelInfoOrderAsync(string titleNumber)
        {
            var result = await _ltsaService.PostParcelInfoOrder(titleNumber);
            return new JsonResult(result);
        }

        /// <summary>
        /// Post a new order using default parameters and the passed in titleNumber.
        /// </summary>
        /// <param name="strataPlanNumber">the title number to create the order for</param>
        /// <returns>The order created within LTSA</returns>
        [HttpPost("spcpOrder")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.OrderWrapper<Model.SpcpOrder>), 200)]
        [ProducesResponseType(typeof(Pims.Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "tools-ltsa" })]
        [HasPermission(Permissions.PropertyEdit)]
        public async Task<IActionResult> PostSpcpOrderAsync(string strataPlanNumber)
        {
            var result = await _ltsaService.PostSpcpOrder(strataPlanNumber);
            return new JsonResult(result);
        }

        /// <summary>
        /// Get title and parcel information from LTSA by posting a title and parcel info order.
        /// </summary>
        /// <param name="pid">the pid to retrieve parcel and title information for</param>
        /// <returns>The orders created within LTSA</returns>
        [HttpPost("ltsaFields")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.LtsaOrders>), 200)]
        [ProducesResponseType(typeof(Pims.Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "tools-ltsa" })]
        [HasPermission(Permissions.PropertyEdit)]
        public async Task<IActionResult> PostLtsaFields(string pid)
        {
            var result = await _ltsaService.PostLtsaFields(pid);
            return new JsonResult(result);
        }
        #endregion
    }
}
