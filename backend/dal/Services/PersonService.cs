using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Services
{
    public class PersonService : BaseService, IPersonService
    {
        readonly Repositories.IPersonRepository _personRepository;

        /// <summary>
        /// Creates a new instance of a PersonService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="personRepository"></param>
        public PersonService(ClaimsPrincipal user, ILogger<BaseService> logger, Repositories.IPersonRepository personRepository) : base(user, logger)
        {
            _personRepository = personRepository;
        }

        public PimsPerson GetPerson(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.ContactView);
            return _personRepository.Get(id);
        }

        public PimsPerson AddPerson(PimsPerson person, bool userOverride)
        {
            person.ThrowIfNull(nameof(person));
            this.User.ThrowIfNotAuthorized(Permissions.ContactAdd);

            var createdPerson = _personRepository.Add(person, userOverride);
            _personRepository.CommitTransaction();

            return GetPerson(createdPerson.Id);
        }

        public PimsPerson UpdatePerson(PimsPerson person, long rowVersion)
        {
            person.ThrowIfNull(nameof(person));
            this.User.ThrowIfNotAuthorized(Permissions.ContactEdit);
            ValidateRowVersion(person.Id, rowVersion);

            var updatedPerson = _personRepository.Update(person);
            _personRepository.CommitTransaction();

            return GetPerson(updatedPerson.Id);
        }

        public void ValidateRowVersion(long personId, long rowVersion)
        {
            if (_personRepository.GetRowVersion(personId) != rowVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this contact, please refresh the application and retry.");
            }
        }
    }
}
