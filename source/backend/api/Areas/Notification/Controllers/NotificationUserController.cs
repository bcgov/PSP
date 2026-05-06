using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Notification;
using Pims.Api.Models.Models.Concepts.Notification;
using Pims.Api.Services;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Json;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Areas.Notification.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("user-notifications")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class NotificationUserController : ControllerBase
    {

        private readonly INotificationUserService _notificationUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationController> _logger;

        public NotificationUserController(INotificationUserService notificationService, IMapper mapper, ILogger<NotificationController> logger)
        {
            _notificationUserService = notificationService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("search")]
        [HasPermission(Permissions.SystemAdmin)]
        [Produces("application/json")]
        [SwaggerOperation(Tags = new[] { "user-notifications" })]
        [ProducesResponseType(typeof(List<NotificationUserOutputModel>), 200)]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult SearchUserNotifications([FromBody]NotificationUserSearchFilterModel filter)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationController),
                nameof(SearchUserNotifications),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationUserService.GetType());
            var notifications = _notificationUserService.SearchNotificationUser(filter);

            return Ok(_mapper.Map<IEnumerable<NotificationUserOutputModel>>(notifications));
        }

        [HttpPut("{notificationUserId:long}/push")]
        [HasPermission(Permissions.SystemAdmin)]
        [Produces("application/json")]
        [SwaggerOperation(Tags = new[] { "user-notifications" })]
        [ProducesResponseType(typeof(List<NotificationUserOutputModel>), 200)]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> PushUserNotificationsAsync([FromRoute] long notificationUserId)
        {
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(NotificationController),
                nameof(SearchUserNotifications),
                User.GetUsername(),
                DateTime.Now);

            _logger.LogInformation("Dispatching to service: {Service}", _notificationUserService.GetType());
            await _notificationUserService.PushNotificationUser(notificationUserId);

            return NoContent();
        }
    }
}
