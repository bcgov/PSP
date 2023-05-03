using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
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
        public IActionResult GetFinancialCodesByType(FinancialCodeTypes type)
        {
            return new JsonResult(_financialCodeService.GetFinancialCodesByType(type));
        }
        #endregion
    }
}
