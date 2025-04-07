using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class ExpropriationEventService : IExpropriationEventService
    {
        private readonly IExpropriationEventRepository _expropriationEventRepository;
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new instance of a ExpropriationEventService class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="expropriationEventRepository"></param>
        public ExpropriationEventService(ClaimsPrincipal user, ILogger<ExpropriationEventService> logger, IExpropriationEventRepository expropriationEventRepository)
        {
            _user = user;
            _logger = logger;
            _expropriationEventRepository = expropriationEventRepository;
        }

        public IEnumerable<PimsExpropOwnerHistory> GetExpropriationEvents(long acquisitionFileId)
        {
            _logger.LogInformation("Getting acquisition file expropriation events with AcquisitionFile Id: {Id}", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            return _expropriationEventRepository.GetExpropriationEventsByAcquisitionFile(acquisitionFileId);
        }

        public PimsExpropOwnerHistory GetExpropriationEventById(long expropriationHistoryId)
        {
            _logger.LogInformation("Getting expropriation event with Id: {Id}", expropriationHistoryId);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileView);

            return _expropriationEventRepository.GetExpropriationEventById(expropriationHistoryId);
        }

        public PimsExpropOwnerHistory AddExpropriationEvent(long acquisitionFileId, PimsExpropOwnerHistory expropriationEvent)
        {
            _logger.LogInformation("Adding expropriation event with AcquisitionFile Id: {Id}", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);

            // Add expropriation event to the event history
            var addedEvent = _expropriationEventRepository.AddExpropriationEvent(expropriationEvent);
            _expropriationEventRepository.CommitTransaction();

            return addedEvent;
        }

        public PimsExpropOwnerHistory UpdateExpropriationEvent(long acquisitionFileId, PimsExpropOwnerHistory expropriationEvent)
        {
            _logger.LogInformation("Updating expropriation event with AcquisitionFile Id: {Id}", acquisitionFileId);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);

            // Update the expropriation event
            var updatedEvent = _expropriationEventRepository.UpdateExpropriationEvent(expropriationEvent);
            _expropriationEventRepository.CommitTransaction();

            return updatedEvent;
        }

        public bool DeleteExpropriationEvent(long acquisitionFileId, long expropriationEventId)
        {
            _logger.LogInformation("Deleting expropriation event with ExpropriationHistoryId: {Id}", expropriationEventId);
            _user.ThrowIfNotAuthorized(Permissions.AcquisitionFileEdit);

            // Delete the expropriation event
            var deleteResult = _expropriationEventRepository.TryDeleteExpropriationEvent(acquisitionFileId, expropriationEventId);
            _expropriationEventRepository.CommitTransaction();

            return deleteResult;
        }
    }
}
