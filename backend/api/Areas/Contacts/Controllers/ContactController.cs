using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Policies;
using Pims.Dal;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

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
        private readonly IPimsService _pimsService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ContactController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="pimsService"></param>
        /// <param name="mapper"></param>
        ///
        public ContactController(IPimsService pimsService, IMapper mapper)
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
        [HttpGet("{id:string}")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Models.Contact.ContactModel>), 200)]
        [SwaggerOperation(Tags = new[] { "contact" })]
        public IActionResult GetContact(string id)
        {
            var contact = _pimsService.Contact.Get(id);

            return new JsonResult(_mapper.Map<Models.Contact.ContactModel>(contact));
        }
        #endregion
    }
}
