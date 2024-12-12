using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities;
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
    [Route("/documents/queue")]
    public class DocumentQueueController : ControllerBase
    {
        #region Variables
        private readonly IDocumentQueueService _documentQueueService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentQueueController class.
        /// </summary>
        /// <param name="documentQueueService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public DocumentQueueController(IDocumentQueueService documentQueueService, IMapper mapper, ILogger<DocumentQueueController> logger)
        {
            _documentQueueService = documentQueueService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Update a Queued Document.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{documentQueueId:long}")]
        [HasPermission(Permissions.SystemAdmin)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<DocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document-types" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult Update(long documentQueueId, [FromBody] DocumentQueueModel documentQueue)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DocumentQueueController),
                nameof(Update),
                User.GetUsername(),
                DateTime.Now);

            documentQueue.ThrowIfNull(nameof(documentQueue));
            if (documentQueueId != documentQueue.Id)
            {
                throw new BadRequestException("Invalid document queue id.");
            }

            var queuedDocuments = _documentQueueService.Update(_mapper.Map<PimsDocumentQueue>(documentQueue));
            var updatedDocumentQueue = _mapper.Map<DocumentQueueModel>(queuedDocuments);
            return new JsonResult(updatedDocumentQueue);
        }

        /// <summary>
        /// Poll a queud document to check on the upload status.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{documentQueueId:long}/poll")]
        [HasPermission(Permissions.SystemAdmin)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<DocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document-types" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> Poll(long documentQueueId, [FromBody] DocumentQueueModel documentQueue)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DocumentQueueController),
                nameof(Poll),
                User.GetUsername(),
                DateTime.Now);

            documentQueue.ThrowIfNull(nameof(documentQueue));
            if (documentQueueId != documentQueue.Id)
            {
                throw new BadRequestException("Invalid document queue id.");
            }

            var queuedDocuments = await _documentQueueService.PollForDocument(_mapper.Map<PimsDocumentQueue>(documentQueue));
            var updatedDocumentQueue = _mapper.Map<DocumentQueueModel>(queuedDocuments);
            return new JsonResult(updatedDocumentQueue);
        }

        /// <summary>
        /// Upload a Queued Document.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{documentQueueId:long}/upload")]
        [HasPermission(Permissions.SystemAdmin)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<DocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document-types" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> Upload(long documentQueueId, [FromBody] DocumentQueueModel documentQueue)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DocumentQueueController),
                nameof(Upload),
                User.GetUsername(),
                DateTime.Now);

            documentQueue.ThrowIfNull(nameof(documentQueue));
            if (documentQueueId != documentQueue.Id)
            {
                throw new BadRequestException("Invalid document queue id.");
            }

            var queuedDocuments = await _documentQueueService.Upload(_mapper.Map<PimsDocumentQueue>(documentQueue));
            var updatedDocumentQueue = _mapper.Map<DocumentQueueModel>(queuedDocuments);
            return new JsonResult(updatedDocumentQueue);
        }

        /// <summary>
        /// Search for Document Queue items via filter.
        /// </summary>
        /// <returns></returns>
        [HttpPost("search")]
        [HasPermission(Permissions.SystemAdmin)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<DocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document-types" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult SearchQueuedDocuments([FromBody] DocumentQueueFilter filter)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(DocumentQueueController),
                nameof(SearchQueuedDocuments),
                User.GetUsername(),
                DateTime.Now);

            var queuedDocuments = _documentQueueService.SearchDocumentQueue(filter);
            var documentQueueModels = _mapper.Map<List<DocumentQueueModel>>(queuedDocuments);
            return new JsonResult(documentQueueModels);
        }

        #endregion
    }
}
