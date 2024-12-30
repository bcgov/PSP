using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Scheduler.Services;

namespace Pims.Scheduler.Controllers
{
    /// <summary>
    /// DocumentSchedulerController class, allows a caller to create a job to process pending PIMS documents.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v{version:apiVersion}/documents")]
    [Route("/documents")]
    public class DocumentSchedulerController : ControllerBase
    {
        #region Variables

        private readonly ILogger _logger;
        private readonly IDocumentQueueService _documentQueueService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PimsGeoserverController class.
        /// </summary>
        public DocumentSchedulerController(ILogger<DocumentSchedulerController> logger, IDocumentQueueService documentQueueService, IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            _documentQueueService = documentQueueService;
            _backgroundJobClient = backgroundJobClient;
        }
        #endregion

        #region Endpoints

        [Route("queued")]
        [HasPermission(Permissions.SystemAdmin)]
        public Task ProcessQueuedDocuments()
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DocumentSchedulerController),
                nameof(ProcessQueuedDocuments),
                User.GetUsername(),
                DateTime.Now);

            // TODO: this is a placeholder only
            _backgroundJobClient.Enqueue(() => _documentQueueService.UploadQueuedDocuments());

            return Task.CompletedTask;
        }

        #endregion
    }
}
