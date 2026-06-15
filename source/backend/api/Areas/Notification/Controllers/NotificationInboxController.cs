using System;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Notification;
using Pims.Api.Services;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Notification.Controllers
{
    /// <summary>
    /// User-facing inbox surface. Drives the in-app notification popover and bell badge.
    ///
    /// All routes are scoped to the calling user — UserId is never accepted from the
    /// request body or query string; it is always resolved server-side from the JWT claim
    /// via User.GetUsername(). This prevents one user querying another user's deliveries.
    ///
    /// A delivery is a single dispatch of a notification through one channel (in-app or
    /// email). Only deliveries where NotificationSentDt IS NOT NULL are surfaced here —
    /// pending deliveries are invisible to the user surface.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("notifications/inbox")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class NotificationInboxController : ControllerBase
    {
        private readonly INotificationInboxService _notificationInboxService;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationInboxController> _logger;

        public NotificationInboxController(
            INotificationInboxService notificationInboxService,
            IMapper mapper,
            ILogger<NotificationInboxController> logger)
        {
            _notificationInboxService = notificationInboxService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a paged list of sent in-app deliveries for the current user.
        /// Drives the notification popover in the top navigation bar.
        /// Only deliveries where NotificationSentDt IS NOT NULL are returned —
        /// the service enforces this filter unconditionally.
        /// </summary>
        /// <param name="page">1-based page number (default 1).</param>
        /// <param name="quantity">Page size (default 10).</param>
        [HttpGet]
        [HasPermission(Permissions.NotificationView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PageModel<NotificationInboxItemModel>), 200)]
        [SwaggerOperation(Tags = new[] { "notifications-inbox" })]
        public IActionResult GetInbox(
            [FromQuery] int page = 1,
            [FromQuery] int quantity = 10)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationInboxController),
                nameof(GetInbox),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationInboxService.GetType());

            var username = User.GetUsername();
            var inbox = _notificationInboxService.GetUserInbox(username, page, quantity);

            return Ok(_mapper.Map<PageModel<NotificationInboxItemModel>>(inbox));
        }

        /// <summary>
        /// Get the count of unread in-app deliveries for the current user.
        /// Drives the bell badge in the top navigation bar.
        /// Returns a single integer with no pagination overhead.
        /// </summary>
        [HttpGet("unread-count")]
        [HasPermission(Permissions.NotificationView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(int), 200)]
        [SwaggerOperation(Tags = new[] { "notifications-inbox" })]
        public IActionResult GetUnreadCount()
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationInboxController),
                nameof(GetUnreadCount),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationInboxService.GetType());

            var username = User.GetUsername();
            var count = _notificationInboxService.GetUnreadCount(username);

            return Ok(new { UnreadCount = count });
        }

        /// <summary>
        /// Mark a single in-app delivery as read or unread for the current user.
        /// The service layer verifies the delivery belongs to the calling user.
        /// </summary>
        /// <param name="outputId">Delivery identifier.</param>
        /// <param name="isRead">True to mark as read, false to mark as unread (default true).</param>
        [HttpPatch("{outputId:long}/read")]
        [HasPermission(Permissions.NotificationEdit)]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Tags = new[] { "notifications-inbox" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult UpdateReadStatus(long outputId, [FromBody] bool isRead = true)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationInboxController),
                nameof(UpdateReadStatus),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationInboxService.GetType());

            var username = User.GetUsername();
            _notificationInboxService.UpdateReadStatus(outputId, username, isRead);

            return NoContent();
        }

        /// <summary>
        /// Mark all sent in-app deliveries as read for the current user.
        /// Bulk convenience operation — equivalent to calling PATCH notifications/inbox/{outputId}/read
        /// for every unread delivery but in a single round trip.
        /// </summary>
        [HttpPatch("read-all")]
        [HasPermission(Permissions.NotificationEdit)]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Tags = new[] { "notifications-inbox" })]
        public IActionResult MarkAllRead()
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationInboxController),
                nameof(MarkAllRead),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationInboxService.GetType());

            var username = User.GetUsername();
            _notificationInboxService.MarkAllRead(username);

            return NoContent();
        }

        /// <summary>
        /// Gets a single in-app delivery for the current user.
        /// The service layer verifies the delivery belongs to the calling user.
        /// </summary>
        /// <param name="outputId">Delivery identifier.</param>
        [HttpGet("{outputId:long}")]
        [HasPermission(Permissions.NotificationView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(NotificationInboxItemModel), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Tags = new[] { "notifications-inbox" })]
        public IActionResult GetNotificationOutput(long outputId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationInboxController),
                nameof(GetNotificationOutput),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationInboxService.GetType());

            var username = User.GetUsername();
            var output = _notificationInboxService.GetNotificationOutputById(outputId, username);
            if (output == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<NotificationInboxItemModel>(output));
        }

        /// <summary>
        /// Delete a single in-app delivery for the current user.
        /// The service layer verifies the delivery belongs to the calling user.
        /// </summary>
        /// <param name="outputId">Delivery identifier.</param>
        [HttpDelete("{outputId:long}")]
        [HasPermission(Permissions.NotificationDelete)]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Tags = new[] { "notifications-inbox" })]
        public IActionResult DeleteNotificationOutput(long outputId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationInboxController),
                nameof(DeleteNotificationOutput),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationInboxService.GetType());

            var username = User.GetUsername();
            var deleted = _notificationInboxService.DeleteNotificationOutput(outputId, username);

            return new JsonResult(deleted);
        }
    }
}
