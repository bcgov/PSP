using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Areas.Contact.Models.Search;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Policies;
using Pims.Api.Services.Interfaces;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Contact.Controllers
{
    /// <summary>
    /// SearchController class, provides endpoints for searching contacts.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("contacts")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class SearchController : ControllerBase
    {
        #region Variables
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SearchController(Contacts) class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="contactService"></param>
        /// <param name="mapper"></param>
        ///
        public SearchController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        #region Paged Contact Endpoints

        /// <summary>
        /// Get all the contacts that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of contacts matching the filter.</returns>
        [HttpGet]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ContactSummaryModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "contact" })]
        public IActionResult GetContacts()
        {
            var uri = new Uri(this.Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return GetContacts(new ContactFilterModel(query));
        }

        /// <summary>
        /// Get all the contacts that satisfy the filter parameters.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>An array of contacts matching the filter.</returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.ContactView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ContactSummaryModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "contact" })]
        public IActionResult GetContacts([FromBody] ContactFilterModel filter)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Contact filter must contain valid values.");
            }

            Paged<Dal.Entities.PimsContactMgrVw> contacts = _contactService.GetPage((ContactFilter)filter);
            return new JsonResult(_mapper.Map<Api.Models.PageModel<ContactSummaryModel>>(contacts));
        }
        #endregion
        #endregion
    }
}
