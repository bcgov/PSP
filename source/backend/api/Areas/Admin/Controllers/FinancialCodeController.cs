using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.FinancialCode;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Json;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Admin.Controllers
{
    /// <summary>
    /// FinancialCodeController class, provides endpoints for managing financial codes.
    /// </summary>
    [HasPermission(Permissions.SystemAdmin)]
    [Authorize]
    [ApiController]
    [Area("admin")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/financial-codes")]
    [Route("[area]/financial-codes")]
    public class FinancialCodeController : ControllerBase
    {
        #region Variables
        private readonly IFinancialCodeService _financialCodeService;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a FinancialCodeController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="financialCodeService"></param>
        public FinancialCodeController(IFinancialCodeService financialCodeService)
        {
            _financialCodeService = financialCodeService;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Gets all the financial codes from the datastore.
        /// </summary>
        /// <returns>An array with all financial codes.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<FinancialCodeModel>), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "admin-financialcodes" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetFinancialCodes()
        {
            var allCodes = _financialCodeService.GetAllFinancialCodes();
            return new JsonResult(allCodes);
        }

        /// <summary>
        /// Retrieves the financial code with the specified id.
        /// </summary>
        /// <param name="type">The financial code type.</param>
        /// <param name="codeId">The id of the financial code to retrieve.</param>
        /// <returns>The financial code.</returns>
        [HttpGet("{type}/{codeId:long}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FinancialCodeModel), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 409)]
        [SwaggerOperation(Tags = new[] { "admin-financialcodes" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetFinancialCode(FinancialCodeTypes type, long codeId)
        {
            return new JsonResult(_financialCodeService.GetById(type, codeId));
        }

        /// <summary>
        /// Adds the specified financial code to the datastore.
        /// </summary>
        /// <param name="type">The financial code type.</param>
        /// <param name="codeModel">The financial code to add.</param>
        /// <returns>The created financial code.</returns>
        [HttpPost("{type}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FinancialCodeModel), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 409)]
        [SwaggerOperation(Tags = new[] { "admin-financialcodes" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddFinancialCode(FinancialCodeTypes type, [FromBody] FinancialCodeModel codeModel)
        {
            try
            {
                var createdCode = _financialCodeService.Add(type, codeModel);
                return new JsonResult(createdCode);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }

        /// <summary>
        /// Updates the financial code with the specified id.
        /// </summary>
        /// <param name="type">The financial code type.</param>
        /// <param name="codeId">The id of the financial code to update.</param>
        /// <param name="codeModel">Updated financial code values.</param>
        /// <returns>The updated financial code.</returns>
        [HttpPut("{type}/{codeId:long}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FinancialCodeModel), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 409)]
        [SwaggerOperation(Tags = new[] { "admin-financialcodes" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateFinancialCode(FinancialCodeTypes type, long codeId, [FromBody] FinancialCodeModel codeModel)
        {
            try
            {
                var updatedCode = _financialCodeService.Update(type, codeModel);
                return new JsonResult(updatedCode);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }

        #endregion
    }
}
