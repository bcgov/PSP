using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Lease;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Property.Controllers
{
    /// <summary>
    /// PropertyImprovementController class, provides endpoints for interacting with lease property improvements.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("properties")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class PropertyImprovementController : ControllerBase
    {
        #region Variables
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyImprovementController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="propertyService"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        ///
        public PropertyImprovementController(IPropertyService propertyService, IMapper mapper, ILogger<PropertyImprovementController> logger)
        {
            _propertyService = propertyService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the improvements for the Property.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{propertyId:long}/improvements")]
        [HasPermission(Permissions.LeaseView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PropertyImprovementModel>), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetImprovements(long propertyId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyImprovementController),
                nameof(GetImprovements),
                User.GetUsername(),
                DateTime.Now);

            var improvements = _propertyService.GetImprovementsByPropertyId(propertyId);

            return new JsonResult(_mapper.Map<IEnumerable<PropertyImprovementModel>>(improvements));
        }

        /// <summary>
        /// Create the Improvement to the Property.
        /// </summary>
        /// <returns>Collection of Property Improvements.</returns>
        [HttpPost("{propertyId:long}/improvements")]
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyImprovementModel), 201)]
        [TypeFilter(typeof(NullJsonResultFilter))]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult AddPropertyImprovement([FromRoute] long propertyId, [FromBody] PropertyImprovementModel improvement)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyImprovementController),
                nameof(AddPropertyImprovement),
                User.GetUsername(),
                DateTime.Now);

            if (propertyId != improvement.PropertyId)
            {
                throw new BadRequestException("Invalid PropertyId.");
            }

            var newImprovement = _propertyService.AddPropertyImprovement(_mapper.Map<PimsPropertyImprovement>(improvement));

            return new JsonResult(_mapper.Map<PropertyImprovementModel>(newImprovement));
        }

        /// <summary>
        /// Get the Property Improvement by Id.
        /// </summary>
        /// <returns>Instance of Property Improvement by Id.</returns>
        [HttpGet("{propertyId:long}/improvements/{propertyImprovementId:long}")]
        [HasPermission(Permissions.PropertyView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyImprovementModel), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult GetPropertyImprovementById([FromRoute] long propertyId, [FromRoute] long propertyImprovementId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyImprovementController),
                nameof(GetPropertyImprovementById),
                User.GetUsername(),
                DateTime.Now);

            var improvement = _propertyService.GetPropertyImprovementByID(propertyId, propertyImprovementId);

            return new JsonResult(_mapper.Map<PropertyImprovementModel>(improvement));
        }

        /// <summary>
        /// Update the Property Improvement.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="propertyImprovementId"></param>
        /// <param name="propertyImprovement"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException">BadRequest for Missmatching Ids.</exception>
        [HttpPut("{propertyId:long}/improvements/{propertyImprovementId:long}")]
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PropertyImprovementModel), 200)]
        [TypeFilter(typeof(NullJsonResultFilter))]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult UpdatePropertyImprovement([FromRoute] long propertyId, [FromRoute] long propertyImprovementId, [FromBody] PropertyImprovementModel propertyImprovement)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyImprovementController),
                nameof(UpdatePropertyImprovement),
                User.GetUsername(),
                DateTime.Now);

            if (propertyId != propertyImprovement.PropertyId || propertyImprovementId != propertyImprovement.Id)
            {
                throw new BadRequestException("Invalid PropertyId.");
            }

            var updatedEntity = _propertyService.UpdatePropertyImprovement(propertyId, _mapper.Map<PimsPropertyImprovement>(propertyImprovement));

            return new JsonResult(_mapper.Map<PropertyImprovementModel>(updatedEntity));
        }

        /// <summary>
        /// Delete the Property Improvement.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{propertyId:long}/improvements/{propertyImprovementId:long}")]
        [HasPermission(Permissions.PropertyEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "property" })]
        public IActionResult DeletePropertyImprovement([FromRoute] long propertyId, [FromRoute] long propertyImprovementId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PropertyImprovementController),
                nameof(DeletePropertyImprovement),
                User.GetUsername(),
                DateTime.Now);

            var result = _propertyService.DeletePropertyImprovement(propertyId, propertyImprovementId);

            return new JsonResult(result);
        }

        #endregion
    }
}
