using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Http.Configuration;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentQueueService implementation provides document queue managing capabilities.
    /// </summary>
    public class DocumentQueueService : BaseService, IDocumentQueueService
    {
        private readonly IDocumentQueueRepository documentQueueRepository;
        private readonly IOptionsMonitor<AuthClientOptions> keycloakOptions;

        public DocumentQueueService(
            ClaimsPrincipal user,
            ILogger<DocumentService> logger,
            IDocumentQueueRepository documentQueueRepository,
            IOptionsMonitor<AuthClientOptions> options)
            : base(user, logger)
        {
            this.documentQueueRepository = documentQueueRepository;
            this.keycloakOptions = options;
        }

        public IEnumerable<PimsDocumentQueue> SearchDocumentQueue(DocumentQueueFilter filter)
        {
            this.Logger.LogInformation("Retrieving queued PIMS documents using filter {filter}", filter);
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.SystemAdmin, this.keycloakOptions);

            return documentQueueRepository.GetAllByFilter(filter);
        }
    }
}
