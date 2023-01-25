using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Exceptions;
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
        public IActionResult GetFinancialCodes()
        {
            var allCodes = _financialCodeService.GetAllFinancialCodes();
            return new JsonResult(allCodes);
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

        #endregion
    }
}
