using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts.FinancialCode;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// FinancialCodeController class, provides endpoints for managing financial codes.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/financial-codes")]
    [Route("/financial-codes")]
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
        /// Gets financial codes of the supplied type from the datastore.
        /// </summary>
        /// <returns>An array with financial codes.</returns>
        [HttpGet("{type}")]
        [Produces("application/json")]
        [HasPermission(Permissions.ProjectView)]
        [ProducesResponseType(typeof(IEnumerable<FinancialCodeModel>), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "financialcodes" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetFinancialCodesByType(FinancialCodeTypes type)
        {
            return new JsonResult(_financialCodeService.GetFinancialCodesByType(type));
        }

        /// <summary>
        /// Gets financial Activity Type Codes.
        /// </summary>
        /// <returns>An array with financial activity types codes.</returns>
        [HttpGet("financial-activities")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<FinancialCodeModel>), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "financialcodes" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetFinancialActivityCodes()
        {
            return new JsonResult(_financialCodeService.GetFinancialCodesByType(FinancialCodeTypes.FinancialActivity));
        }

        /// <summary>
        /// Gets Chart of Acccounts Type Codes.
        /// </summary>
        /// <returns>An array with chart of accounts types codes.</returns>
        [HttpGet("chart-accounts")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<FinancialCodeModel>), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "financialcodes" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetChartOfAccounts()
        {
            return new JsonResult(_financialCodeService.GetFinancialCodesByType(FinancialCodeTypes.ChartOfAccounts));
        }

        /// <summary>
        /// Gets Responsibility Type Codes.
        /// </summary>
        /// <returns>An array with Responsibility types codes.</returns>
        [HttpGet("responsibilities")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<FinancialCodeModel>), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "financialcodes" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetResponsibilities()
        {
            return new JsonResult(_financialCodeService.GetFinancialCodesByType(FinancialCodeTypes.Responsibility));
        }

        /// <summary>
        /// Gets Yearly Financials Type Codes.
        /// </summary>
        /// <returns>An array with yearly financials types codes.</returns>
        [HttpGet("yearly-financials")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<FinancialCodeModel>), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "financialcodes" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetYearlyFinancials()
        {
            return new JsonResult(_financialCodeService.GetFinancialCodesByType(FinancialCodeTypes.YearlyFinancial));
        }
        #endregion
    }
}
