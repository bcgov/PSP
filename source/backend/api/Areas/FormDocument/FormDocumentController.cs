using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// FormDocumentController class, provides endpoints to handle document generation requests.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/formDocument/")]
    [Route("/formDocument")]
    public class FormDocumentController : ControllerBase
    {
        #region Variables
        private readonly IFormDocumentService _formDocumentService;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a FormDocumentController class.
        /// </summary>
        /// <param name="formDocumentService"></param>
        public FormDocumentController(IFormDocumentService formDocumentService)
        {
            _formDocumentService = formDocumentService;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Gets the Form Document types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // [HasPermission(Permissions.GenerateDocuments)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<FormDocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "form-document" })]
        public IActionResult GetFormDocumentTypes()
        {
            var supportedFileTypes = _formDocumentService.GetAllFormDocumentTypes();
            return new JsonResult(supportedFileTypes);
        }

        #endregion
    }
}
