
using System;
using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Notification;
using Pims.Api.Services;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Notification.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("notifications")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, IMapper mapper, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all notifications for the current user.
        /// </summary>
        [HttpGet("user")]
        [HasPermission(Permissions.NotificationView)]
        [Produces("application/json")]
        [SwaggerOperation(Tags = new[] { "notifications" })]
        public IActionResult GetUserNotifications()
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationController),
                nameof(GetUserNotifications),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationService.GetType());
            var username = User.GetUsername();
            var notifications = _notificationService.GetByUser(username);
            return Ok(_mapper.Map<IEnumerable<NotificationModel>>(notifications));
        }

        /// <summary>
        /// Search notifications for the current user that match the specified criteria.
        /// </summary>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>A list of notifications that match the criteria.</returns>
        [HttpPost("search")]
        [HasPermission(Permissions.NotificationView)]
        [Produces("application/json")]
        [SwaggerOperation(Tags = new[] { "notifications" })]
        public IActionResult SearchNotifications([FromBody] NotificationSearchCriteria criteria)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationController),
                nameof(SearchNotifications),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationService.GetType());

            var username = User.GetUsername();
            var notifications = _notificationService.Search(criteria, username);

            return Ok(_mapper.Map<IEnumerable<NotificationModel>>(notifications));
        }

        /// <summary>
        /// Get a single notification by id.
        /// </summary>
        [HttpGet("{id:long}")]
        [HasPermission(Permissions.NotificationView)]
        [Produces("application/json")]
        [SwaggerOperation(Tags = new[] { "notifications" })]
        public IActionResult GetNotification(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationController),
                nameof(GetNotification),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationService.GetType());

            var username = User.GetUsername();
            var response = _notificationService.GetByIdForUser(id, username);
            switch (response.Result)
            {
                case NotificationAccessResult.NotFound:
                    return NotFound();
                case NotificationAccessResult.Forbidden:
                    return Forbid();
                case NotificationAccessResult.Success:
                    return Ok(_mapper.Map<NotificationModel>(response.Notification));
                default:
                    return StatusCode(500);
            }
        }

        /// <summary>
        /// Create a notification.
        /// </summary>
        [HttpPost]
        [HasPermission(Permissions.NotificationAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(NotificationModel), 201)]
        [SwaggerOperation(Tags = new[] { "notifications" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult AddNotification([FromBody] NotificationModel notification)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationController),
                nameof(AddNotification),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationService.GetType());

            var username = User.GetUsername();
            var notificationEntity = _mapper.Map<PimsNotification>(notification);
            var result = _notificationService.Add(notificationEntity, username);

            return CreatedAtAction(
                nameof(GetNotification),
                new { id = result.NotificationId },
                _mapper.Map<NotificationModel>(result)
            );
        }

        /// <summary>
        /// Update an existing notification.
        /// </summary>
        [HttpPut("{id:long}")]
        [HasPermission(Permissions.NotificationEdit)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(NotificationModel), 200)]
        [SwaggerOperation(Tags = new[] { "notifications" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateNotification(long id, [FromBody] NotificationModel notification)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationController),
                nameof(UpdateNotification),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationService.GetType());

            var username = User.GetUsername();
            var notificationEntity = _mapper.Map<PimsNotification>(notification);
            notificationEntity.NotificationId = id;
            var result = _notificationService.Update(notificationEntity, username);

            return Ok(_mapper.Map<NotificationModel>(result));
        }

        /// <summary>
        /// Delete an existing notification.
        /// </summary>
        [HttpDelete("{id:long}")]
        [HasPermission(Permissions.NotificationDelete)]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Tags = new[] { "notifications" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult DeleteNotification(long id)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationController),
                nameof(DeleteNotification),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationService.GetType());

            var username = User.GetUsername();
            var deleted = _notificationService.Delete(id, username);
            if (!deleted)
            {
                throw new InvalidOperationException($"Failed to delete notification {id}.");
            }
            return NoContent();
        }
    }
}
