using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal;
using Pims.Dal.Security;
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
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PersonController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public PersonController(IPimsService pimsService, IMapper mapper)
        {
            _pimsService = pimsService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Get the contact for the specified primary key 'id'.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(Permissions.ContactAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Areas.Contact.Models.Contact.ContactModel), 201)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "person" })]
        public IActionResult AddPerson([FromBody] Models.Person.PersonCreateModel model)
        {
            var entity = _mapper.Map<Dal.Entities.PimsPerson>(model);
            var created = _pimsService.Person.Add(entity);
            var response = _mapper.Map<Areas.Contact.Models.Contact.ContactModel>(created);

            return CreatedAtAction(nameof(Areas.Contact.Controllers.ContactController.GetContact), nameof(Areas.Contact.Controllers.ContactController), new { id = response.Id }, response);
        }
        #endregion
    }
}
