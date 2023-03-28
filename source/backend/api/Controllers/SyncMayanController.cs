using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Mayan.Sync;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// SyncMayanController class, provides endpoints to handle syncronization between mayan and pims.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/documents/")]
    [Route("/documents")]
    public class SyncMayanController : ControllerBase
    {
        #region Variables
        private readonly IDocumentSyncService _documentSyncService;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SyncMayanController class.
        /// </summary>
        /// <param name="documentSyncService"></param>
        public SyncMayanController(IDocumentSyncService documentSyncService)
        {
            _documentSyncService = documentSyncService;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Synchronizes the mayan document types directly with Mayan.
        /// </summary>
        [HttpPatch("sync/mayan/documenttype")]
        [HasPermission(Permissions.DocumentAdmin)]
        [ProducesResponseType(typeof(ExternalBatchResult), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult SyncMayanDocumentTypes([FromBody] SyncModel model)
        {
            var result = _documentSyncService.SyncMayanDocumentTypes(model);
            return new JsonResult(result);
        }

        /// <summary>
        /// Synchronizes the mayan metadata directly with Mayan.
        /// </summary>
        [HttpPatch("sync/mayan/metadatatype")]
        [HasPermission(Permissions.DocumentAdmin)]
        [ProducesResponseType(typeof(ExternalBatchResult), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult SyncMayanMetadataTypes([FromBody] SyncModel model)
        {
            var result = _documentSyncService.SyncMayanMetadataTypes(model);
            return new JsonResult(result);
        }

        /// <summary>
        /// Synchronizes the document types from Mayan to PIMS.
        /// </summary>
        [HttpPatch("sync/backend/documenttype")]
        [HasPermission(Permissions.DocumentAdmin)]
        [ProducesResponseType(typeof(PimsDocumentTyp), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public async Task<IActionResult> SyncDocumentTypes([FromBody] SyncModel model)
        {
            var result = await _documentSyncService.SyncBackendDocumentTypes(model);
            return new JsonResult(result);
        }

        #endregion
    }
}
