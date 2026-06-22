using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class NotificationInboxService : INotificationInboxService
    {
        private readonly ClaimsPrincipal _user;
        private readonly IUserRepository _userRepository;
        private readonly INotificationInboxRepository _notificationInboxRepository;
        private readonly ILogger<NotificationInboxService> _logger;

        public NotificationInboxService(
            ClaimsPrincipal user,
            IUserRepository userRepository,
            INotificationInboxRepository notificationInboxRepository,
            ILogger<NotificationInboxService> logger)
        {
            _user = user;
            _userRepository = userRepository;
            _notificationInboxRepository = notificationInboxRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public Paged<PimsNotificationUserOutput> GetUserInbox(string username, int page, int quantity)
        {
            _logger.LogInformation("Getting notificationinbox deliveries for user {Username}", username);
            _user.ThrowIfNotAllAuthorized(Permissions.NotificationView);

            var user = GetUserByUsername(username);
            return _notificationInboxRepository.GetUserInboxDeep(user.UserId, page, quantity);
        }

        /// <inheritdoc />
        public int GetUnreadCount(string username)
        {
            _logger.LogInformation("Getting unread notification count for user {Username}", username);
            _user.ThrowIfNotAllAuthorized(Permissions.NotificationView);

            var user = GetUserByUsername(username);
            return _notificationInboxRepository.GetUnreadCount(user.UserId);
        }

        /// <inheritdoc />
        public PimsNotificationUserOutput GetNotificationOutputById(long outputId, string username)
        {
            _logger.LogInformation("Getting notification delivery {OutputId} for user {Username}", outputId, username);
            _user.ThrowIfNotAllAuthorized(Permissions.NotificationView);

            var user = GetUserByUsername(username);
            return _notificationInboxRepository.GetNotificationOutputByIdDeep(outputId, user.UserId);
        }

        /// <inheritdoc />
        public PimsNotificationUserOutput UpdateReadStatus(long outputId, string username, bool isRead)
        {
            _logger.LogInformation(
                "Marking notification {OutputId} as {ReadState} for user {Username}",
                outputId,
                isRead ? "read" : "unread",
                username);

            _user.ThrowIfNotAllAuthorized(Permissions.NotificationEdit);

            var user = GetUserByUsername(username);

            var updated = _notificationInboxRepository.UpdateReadStatus(outputId, user.UserId, isRead);
            _notificationInboxRepository.CommitTransaction();

            return updated;
        }

        /// <inheritdoc />
        public void MarkAllRead(string username)
        {
            _logger.LogInformation("Marking all deliveries as read for user {Username}", username);
            _user.ThrowIfNotAllAuthorized(Permissions.NotificationEdit);

            var user = GetUserByUsername(username);
            _notificationInboxRepository.MarkAllRead(user.UserId);
            _notificationInboxRepository.CommitTransaction();
        }

        /// <inheritdoc />
        public bool DeleteNotificationOutput(long outputId, string username)
        {
            _logger.LogInformation("Deleting delivery {OutputId} for user {Username}", outputId, username);
            _user.ThrowIfNotAllAuthorized(Permissions.NotificationDelete);

            var user = GetUserByUsername(username);

            bool result = _notificationInboxRepository.DeleteNotificationOutput(outputId, user.UserId);
            if (result)
            {
                _notificationInboxRepository.CommitTransaction();
            }

            return result;
        }

        private PimsUser GetUserByUsername(string username)
        {
            return _userRepository.GetByUsername(username)
                ?? throw new KeyNotFoundException($"User '{username}' not found in PIMS_USERS.");
        }
    }
}
