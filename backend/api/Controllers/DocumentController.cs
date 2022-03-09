using Microsoft.AspNetCore.Mvc;
using Pims.Api.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// DocumentController class, provides endpoints to handle document requests.
    /// </summary>
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/documents/")]
    [Route("/documents")]
    public class DocumentController : ControllerBase
    {
        #region Variables
        private readonly IDocumentService _documentService;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ErrorController class.
        /// </summary>
        /// <param name="documentService"></param>
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Retrieves a list of documents.
        /// </summary>
        [HttpGet()]
        //[HasPermission(Permissions.PropertyAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult GetDocumentList()
        {
            var ast = _documentService.GetDocumentList();
            return new JsonResult(ast);
        }

        /// <summary>
        /// Retrieves a list of documents.
        /// </summary>
        [HttpGet("{documentId}/files/{fileId}/download")]
        //[HasPermission(Permissions.PropertyAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult DownloadFile(int documentId, int fileId)
        {
            var ast = _documentService.DownloadFile(documentId, fileId);
            return new JsonResult(ast);
        }
        #endregion
    }
}
