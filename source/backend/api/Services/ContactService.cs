using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Services.Interfaces;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class ContactService : IContactService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IContactRepository _contactRepository;

        public ContactService(ClaimsPrincipal user, ILogger<ContactService> logger, IContactRepository contactRepository)
        {
            _user = user;
            _logger = logger;
            _contactRepository = contactRepository;
        }

        public PimsContactMgrVw GetById(string id)
        {
            _logger.LogInformation("Getting contact with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.ContactView);
            return _contactRepository.GetById(id);
        }

        public Paged<PimsContactMgrVw> GetPage(ContactFilter filter)
        {
            _logger.LogInformation("Searching for contacts...");

            _logger.LogDebug("Contact search with filter", filter);
            _user.ThrowIfNotAuthorized(Permissions.ContactView);
            Paged<PimsContactMgrVw> contacts = _contactRepository.GetPage(filter);
            return contacts;
        }
    }
}
