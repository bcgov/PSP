using System;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Areas.Autocomplete.Models;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Policies;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Autocomplete.Controllers
{
    /// <summary>
    /// AutocompleteController class, provides endpoints for retrieving Autocomplete predictions.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("autocomplete")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class AutocompleteController : ControllerBase
    {
        #region Variables
        private readonly IPimsRepository _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a AutocompleteController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public AutocompleteController(IPimsRepository pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Retrieves autocomplete predictions based on the supplied autocomplete request.
        /// </summary>
        /// <returns>An array of contacts matching the filter</returns>
        [HttpGet("organizations")]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AutocompleteResponseModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "autocomplete" })]
        public IActionResult GetOrganizationPredictions()
        {
            var uri = new Uri(this.Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return GetOrganizationPredictions(new AutocompletionRequestModel(query));
        }

        /// <summary>
        /// Retrieves autocomplete predictions based on the supplied autocomplete request.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>An array of contacts matching the filter</returns>
        [HttpPost("organizations")]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.AutocompleteResponseModel), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "autocomplete" })]
        public IActionResult GetOrganizationPredictions([FromBody] AutocompletionRequestModel filter)
        {
            filter.ThrowBadRequestIfNull($"The request must include an autocomplete request.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Autocomplete request must contain valid values.");
            }

            var predictions = _pimsService.Autocomplete.GetOrganizationPredictions(filter);

            return new JsonResult(_mapper.Map<Models.AutocompleteResponseModel>(predictions));
        }
        #endregion
    }
}
