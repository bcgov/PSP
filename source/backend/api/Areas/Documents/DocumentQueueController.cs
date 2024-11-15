using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Services;
using Pims.Core.Api.Policies;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// DocumentQueueController class, provides endpoints to handle document queue requests.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/documents/queue")]
    [Route("/documents")]
    public class DocumentQueueController : ControllerBase
    {
        #region Variables
        private readonly IDocumentQueueService _documentQueueService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentQueueController class.
        /// </summary>
        /// <param name="documentQueueService"></param>
        /// <param name="mapper"></param>
        public DocumentQueueController(IDocumentQueueService documentQueueService, IMapper mapper)
        {
            _documentQueueService = documentQueueService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Search for Document Queue items via filter.
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [HasPermission(Permissions.SystemAdmin)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<Models.Concepts.Document.DocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document-types" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDocumentTypes([FromBody] DocumentQueueFilter filter)
        {
            var queuedDocuments = _documentQueueService.SearchDocumentQueue(filter);
            var documentQueueModels = _mapper.Map<List<Models.Concepts.Document.DocumentQueueModel>>(queuedDocuments);
            return new JsonResult(documentQueueModels);
        }

        #endregion
    }
}
