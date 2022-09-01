using System.Linq;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Dal;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Persons.Controllers
{
    /// <summary>
    /// PersonController class, provides endpoints for interacting with person contacts (individuals).
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("persons")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PersonController : ControllerBase
    {
        #region Variables
        private readonly IPimsService _pimsService;
        private readonly IPimsRepository _pimsRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PersonController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="pimsRepository"></param>
        /// <param name="mapper"></param>
        ///
        public PersonController(IPimsService pimsService, IPimsRepository pimsRepository, IMapper mapper)
        {
            _pimsService = pimsService;
            _pimsRepository = pimsRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the person for the specified primary key 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.Person.PersonModel), 200)]
        [SwaggerOperation(Tags = new[] { "person" })]
        public IActionResult GetPerson(int id)
        {
            var person = _pimsService.PersonService.GetPerson(id);
            return new JsonResult(_mapper.Map<Models.Person.PersonModel>(person));
        }

        /// <summary>
        /// Add the specified person to the datastore.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.ContactAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Areas.Contact.Models.Contact.ContactModel), 201)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "person" })]
        public IActionResult AddPerson([FromBody] Models.Person.PersonModel model, bool userOverride = false)
        {
            // Business rule - support country free-form value if country code is "Other". Ignore field otherwise.
            var otherCountry = _pimsRepository.Lookup.GetCountries().FirstOrDefault(x => x.Code == Dal.Entities.CountryCodes.Other);
            foreach (var address in model?.Addresses)
            {
                if (otherCountry != null && address != null && address.CountryId != otherCountry.CountryId)
                {
                    address.CountryOther = null;
                }
            }

            var entity = _mapper.Map<Dal.Entities.PimsPerson>(model);

            try
            {
                var created = _pimsService.PersonService.AddPerson(entity, userOverride);
                var response = _mapper.Map<Areas.Contact.Models.Contact.PersonModel>(created);

                return new JsonResult(response);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }

        /// <summary>
        /// Update the specified contact, and attached properties.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [HasPermission(Permissions.ContactEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.Person.PersonModel), 200)]
        [SwaggerOperation(Tags = new[] { "person" })]
        public IActionResult UpdatePerson([FromBody] Models.Person.PersonModel personModel)
        {
            var personEntity = _mapper.Map<Pims.Dal.Entities.PimsPerson>(personModel);
            var updatedPerson = _pimsService.PersonService.UpdatePerson(personEntity, personModel.RowVersion);

            return new JsonResult(_mapper.Map<Models.Person.PersonModel>(updatedPerson));
        }
        #endregion
    }
}
