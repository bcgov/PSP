using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Contact.Controllers
{
    /// <summary>
    /// ContactController class, provides endpoints for interacting with contacts.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("contacts")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class ContactController : ControllerBase
    {
        #region Variables
        private readonly IContactRepository _contactRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ContactController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="contactRepository"></param>
        /// <param name="personRepository"></param>
        /// <param name="organizationRepository"></param>
        /// <param name="mapper"></param>
        ///
        public ContactController(
            IContactRepository contactRepository,
            IPersonRepository personRepository,
            IOrganizationRepository organizationRepository,
            IMapper mapper)
        {
            _contactRepository = contactRepository;
            _personRepository = personRepository;
            _organizationRepository = organizationRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the contact for the specified primary key 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Contact.ContactModel>), 200)]
        [SwaggerOperation(Tags = new[] { "contact" })]
        public IActionResult GetContact(string id)
        {
            var contactView = _contactRepository.Get(id);

            if (id.StartsWith("P"))
            {
                var person = _personRepository.Get(contactView.PersonId.Value);
                var mappedPerson = _mapper.Map<Models.Contact.ContactModel>(person);
                mappedPerson.Id = contactView.Id;
                return new JsonResult(mappedPerson);
            }
            else
            {
                var organization = _organizationRepository.Get(contactView.OrganizationId.Value);
                var mappedOrganization = _mapper.Map<Models.Contact.ContactModel>(organization);
                mappedOrganization.Id = contactView.Id;
                return new JsonResult(mappedOrganization);
            }
        }
        #endregion
    }
}
