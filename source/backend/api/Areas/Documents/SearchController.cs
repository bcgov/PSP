using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Documents
{
    /// <summary>
    /// SearchController class, provides endpoints for searching documents.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("documents")]
    [Route("v{version:apiVersion}/[area]/search")]
    [Route("[area]/search")]
    public class SearchController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IPropertyService propertyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SearchController(IDocumentService documentService, IPropertyService propertyService, IMapper mapper, ILogger<SearchController> logger)
        {
            _documentService = documentService;
            this.propertyService = propertyService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all the Documents that satisfy the filter parameters.
        /// </summary>
        /// <returns>An array of Documents matching the filter.</returns>
        [HttpGet]
        [HasPermission(Permissions.DocumentView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DocumentSearchResultModel>), 200)]
        [ProducesResponseType(typeof(Models.ErrorResponseModel), 400)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDocuments([FromQuery] DocumentSearchFilterModel filter)
        {
            // RECOMMENDED - Add valuable metadata to logs
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(SearchController),
                nameof(GetDocuments),
                User.GetUsername(),
                DateTime.Now);

            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            if (!filter.IsValid())
            {
                throw new BadRequestException("Document filter must contain valid values.");
            }

            // RECOMMENDED - Log communications between components
            _logger.LogInformation("Dispatching to service: {Service}", _documentService.GetType());
            var documents = _documentService.GetPage(filter);

            // Transform all properties to lat/long for returned documents that have properties, this is required for the front end to properly display the property locations.
            foreach (var document in documents.Items)
            {
                var propertyDocuments = document.PimsPropertyDocuments ?? new List<PimsPropertyDocument>();
                document.PimsPropertyDocuments = TransformAllPropertiesToLatLong(propertyDocuments);
            }

            return new JsonResult(_mapper.Map<PageModel<DocumentSearchResultModel>>(documents));
        }

        /// <summary>
        /// Returns the spatial location and boundary polygons in lat/long (4326) for a list of document properties.
        /// The spatial values will be modified in-place.
        /// </summary>
        /// <param name="propertyDocuments">The document properties to re-project.</param>
        /// <returns>The document properties with transformed spatial locations.</returns>
        private ICollection<PimsPropertyDocument> TransformAllPropertiesToLatLong(ICollection<PimsPropertyDocument> propertyDocuments)
        {
            if (propertyDocuments == null)
            {
                return propertyDocuments;
            }

            foreach (var propertyDocument in propertyDocuments)
            {
                propertyDocument.Property = propertyService.TransformPropertyToLatLong(propertyDocument.Property);
            }

            return propertyDocuments;
        }

    }
}
