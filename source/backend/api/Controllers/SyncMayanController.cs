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
        /// Synchronizes incoming json document types with the PIMS db.
        /// </summary>
        [HttpPatch("sync/documenttype")]
        [HasPermission(Permissions.DocumentAdmin)]
        [ProducesResponseType(typeof(ExternalBatchResult), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult SyncDocumentTypes([FromBody] SyncModel model)
        {
            var result = _documentSyncService.SyncPimsDocumentTypes(model);
            return new JsonResult(result);
        }

        /// <summary>
        /// Synchronizes incoming json document metadata with mayan.
        /// </summary>
        [HttpPatch("sync/mayan/metadatatype")]
        [HasPermission(Permissions.DocumentAdmin)]
        [ProducesResponseType(typeof(ExternalBatchResult), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult SyncMetadataTypes([FromBody] SyncModel model)
        {
            var result = _documentSyncService.SyncMayanMetadataTypes(model);
            return new JsonResult(result);
        }

        /// <summary>
        /// Migrate incoming json document metadata with the mayan.
        /// </summary>
        [HttpPatch("migrate/mayan/metadatatype")]
        [HasPermission(Permissions.DocumentAdmin)]
        [ProducesResponseType(typeof(ExternalBatchResult), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult MigrateMetadataTypes([FromBody] SyncModel model)
        {
            var result = _documentSyncService.MigrateMayanMetadataTypes(model);
            return new JsonResult(result);
        }

        /// <summary>
        /// Synchronizes the pims db with Mayan.
        /// </summary>
        [HttpPatch("sync/mayan")]
        [HasPermission(Permissions.DocumentAdmin)]
        [ProducesResponseType(typeof(PimsDocumentTyp), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult SyncMayan([FromBody] SyncModel model)
        {
            var result = _documentSyncService.SyncPimsToMayan(model);
            return new JsonResult(result);
        }

        #endregion
    }
}
