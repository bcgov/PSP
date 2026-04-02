using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Models.Concepts.Notification;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ClaimsPrincipal _user;

        private readonly IUserRepository _userRepository;
        private readonly ILogger<NotificationService> _logger;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(
            ClaimsPrincipal user,
            IUserRepository userRepository,
            ILogger<NotificationService> logger,
            INotificationRepository notificationRepository)
        {
            _user = user;
            _userRepository = userRepository;
            _logger = logger;
            _notificationRepository = notificationRepository;
        }

        public IEnumerable<PimsNotification> GetByUser(string username)
        {
            _logger.LogInformation("Getting notifications for user {Username}", username);
            _user.ThrowIfNotAllAuthorized(Permissions.NotificationView);

            var user = GetUserByUsername(username);
            return _notificationRepository.GetByUser(user.UserId);
        }

        public NotificationAccessResponse GetByIdForUser(long notificationId, string username)
        {
            _logger.LogInformation("Getting notification with id {NotificationId} for user {Username}", notificationId, username);
            _user.ThrowIfNotAllAuthorized(Permissions.NotificationView);

            var user = _userRepository.GetByUsername(username);
            if (user == null)
            {
                return new NotificationAccessResponse { Result = NotificationAccessResult.Forbidden };
            }

            var notification = _notificationRepository.GetById(notificationId);
            if (notification == null)
            {
                return new NotificationAccessResponse { Result = NotificationAccessResult.NotFound };
            }

            if (!notification.IsOwnedByUser(user.UserId))
            {
                return new NotificationAccessResponse { Result = NotificationAccessResult.Forbidden };
            }

            return new NotificationAccessResponse { Result = NotificationAccessResult.Success, Notification = notification };
        }

        public PimsNotification Add(PimsNotification notification, string username)
        {
            _logger.LogInformation("Creating notification {Notification} for user {Username}", notification, username);
            _user.ThrowIfNotAllAuthorized(Permissions.NotificationAdd);
            notification.ThrowIfNull(nameof(notification));
            notification.ThrowIfInvalidFileAndSubId();

            var user = GetUserByUsername(username);
            var newNotification = _notificationRepository.Add(notification, user.UserId);
            _notificationRepository.CommitTransaction();

            return newNotification;
        }

        public PimsNotification Update(PimsNotification notification, string username)
        {
            _logger.LogInformation("Updating notification with id {NotificationId} for user {Username}", notification.NotificationId, username);

            _user.ThrowIfNotAllAuthorized(Permissions.NotificationEdit);
            notification.ThrowIfNull(nameof(notification));
            ValidateVersion(notification.NotificationId, notification.ConcurrencyControlNumber);

            var user = _userRepository.GetByUsername(username) ?? throw new KeyNotFoundException($"User '{username}' not found in PIMS_USERS.");
            var existingNotification = _notificationRepository.GetById(notification.NotificationId)
                ?? throw new KeyNotFoundException($"Notification '{notification.NotificationId}' not found.");

            if (!existingNotification.IsOwnedByUser(user.UserId))
            {
                throw new UnauthorizedAccessException("User does not own this notification.");
            }
            notification.ThrowIfInvalidFileAndSubId();

            return _notificationRepository.Update(notification, user.UserId);
        }

        public bool Delete(long notificationId, string username)
        {
            _user.ThrowIfNotAllAuthorized(Permissions.NotificationDelete);

            var user = GetUserByUsername(username);

            var existingNotification = _notificationRepository.GetById(notificationId)
                ?? throw new KeyNotFoundException($"Notification '{notificationId}' not found.");

            if (!existingNotification.IsOwnedByUser(user.UserId))
            {
                throw new UnauthorizedAccessException("User does not own this notification.");
            }

            var deleted = _notificationRepository.Delete(notificationId, user.UserId);
            if (deleted)
            {
                _notificationRepository.CommitTransaction();
            }
            return deleted;
        }

        private void ValidateVersion(long notificationId, long? notificationVersion)
        {
            long currentRowVersion = _notificationRepository.GetRowVersion(notificationId);
            if (currentRowVersion != notificationVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this notification, please refresh the application and retry.");
            }
        }

        private PimsUser GetUserByUsername(string username)
        {
            return _userRepository.GetByUsername(username) ?? throw new KeyNotFoundException($"User '{username}' not found in PIMS_USERS.");
        }
    }
}
