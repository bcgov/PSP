using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Api.Services.Interfaces;
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
        private readonly IContactService _contactService;
        private readonly IPersonRepository _personRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ContactController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="contactService"></param>
        /// <param name="personRepository"></param>
        /// <param name="organizationRepository"></param>
        /// <param name="mapper"></param>
        ///
        public ContactController(
            IContactService contactService,
            IPersonRepository personRepository,
            IOrganizationRepository organizationRepository,
            IMapper mapper)
        {
            _contactService = contactService;
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
            var contactView = _contactService.GetById(id);

            if (id.StartsWith("P"))
            {
                var person = _personRepository.GetById(contactView.PersonId.Value);
                var mappedPerson = _mapper.Map<Models.Contact.ContactModel>(person);
                mappedPerson.Id = contactView.Id;
                return new JsonResult(mappedPerson);
            }
            else
            {
                var organization = _organizationRepository.GetById(contactView.OrganizationId.Value);
                var mappedOrganization = _mapper.Map<Models.Contact.ContactModel>(organization);
                mappedOrganization.Id = contactView.Id;
                return new JsonResult(mappedOrganization);
            }
        }
        #endregion
    }
}
