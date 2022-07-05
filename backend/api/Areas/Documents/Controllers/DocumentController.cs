
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace Pims.Api.Areas.Notes.Controllers
{
    /// <summary>
    /// NoteController class, provides endpoints for interacting with notes.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("documents")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class DocumentController : ControllerBase
    {
        #region Variables
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="documentService"></param>
        /// <param name="mapper"></param>
        ///
        public DocumentController(IDocumentService documentService, IMapper mapper)
        {
            _documentService = documentService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the document types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("document-types")]
        [HasPermission(Permissions.DocumentView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<DocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document-types" })]
        public IActionResult GetDocumentTypes()
        {
            var documentTypes = _documentService.GetPIMSDocumentTypes();

            var mappedDocumentTypes = _mapper.Map<List<DocumentTypeModel>>(documentTypes);
            return new JsonResult(mappedDocumentTypes);
        }

        #endregion
    }
}
