using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Notification;
using Pims.Api.Models.Models.Concepts.Notification;
using Pims.Api.Models.Requests.Http;
using Pims.Core.Api.Services;
using Pims.Scheduler.Configuration;
using Pims.Scheduler.Models;
using Pims.Scheduler.Models.Base;
using Pims.Scheduler.Models.Notifications;
using Pims.Scheduler.Repositories;
using Pims.Scheduler.Services.Interfaces;

namespace Pims.Scheduler.Services
{
    public class NotificationUserService : BaseService, INotificationUserService
    {
        private readonly ILogger _logger;
        private readonly IPimsNotificationUserRepository _notificationUserRepository;
        private readonly IOptionsMonitor<PushNotificationsJobOptions> _pushNotificationsJobOptions;

        public NotificationUserService(ILogger<NotificationUserService> logger, IPimsNotificationUserRepository notificationUserRepository, IOptionsMonitor<PushNotificationsJobOptions> pushNotificationsJobOptions)
            : base(null, logger)
        {
            _logger = logger;
            _notificationUserRepository = notificationUserRepository;
            _pushNotificationsJobOptions = pushNotificationsJobOptions;
        }

        public async Task<BaseTaskResponseModel> PushEmailUserNotifications()
        {
            // 1. Configure the limiter
            var limiterOptions = new TokenBucketRateLimiterOptions
            {
                TokenLimit = 30,                // Maximum tokens the bucket can hold
                QueueLimit = 10000,              // How many requests can wait in line if bucket is empty
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1), // How often to add tokens
                TokensPerPeriod = 30,           // How many tokens to add each period
                AutoReplenishment = true, // The limiter handles the timer internally
            };

            NotificationUserSearchFilterModel filter = new()
            {
                Quantity = _pushNotificationsJobOptions?.CurrentValue.EmailNotificationsBatchSize ?? 50,
                NotificationOutputTypeCode = NotificationOutputTypes.EMAIL.ToString(),
                MaxRetries = _pushNotificationsJobOptions?.CurrentValue.EmailNotificationsMaxRetriesAllowed ?? 3,
                NotificationSentDateTime = null,
                NotificationTriggerDate = DateOnly.FromDateTime(DateTime.UtcNow),
            };

            var searchResponse = await SearchUserNotifications(filter);
            if (searchResponse?.ScheduledTaskResponseModel != null)
            {
                return searchResponse.ScheduledTaskResponseModel;
            }

            // Request to PUSH notifications
            _logger.LogInformation("Processing {count} Emails notifications.", searchResponse?.SearchResults?.Payload?.Count ?? 0);

            // 2. Process your notifications
            using RateLimiter limiter = new TokenBucketRateLimiter(limiterOptions);
            List<PushNotificationResponseModel> responses = new();
            foreach (var userNotification in searchResponse?.SearchResults?.Payload ?? Enumerable.Empty<NotificationOutputModel>())
            {
                // 3. Acquire a token before making the call
                using RateLimitLease lease = await limiter.AcquireAsync(permitCount: 1);

                if (lease.IsAcquired)
                {
                    _logger.LogInformation("Token acquired. Processing {id}", userNotification.Id);

                    var response = await _notificationUserRepository.PushUserNotificationsAsync(userNotification);
                    PushNotificationResponseModel responseModel = HandlePushNotificationRequestResponse("PushUserNotificationsAsync", userNotification, response);
                    responses.Add(responseModel);
                }
                else
                {
                    _logger.LogWarning("Rate limit exceeded and queue is full for {id}", userNotification.Id);
                }
            }

            // Return results
            return new BaseTaskResponseModel() { Status = GetMergedStatus(responses) };
        }

        public async Task<BaseTaskResponseModel> PushPimsUserNotifications()
        {
            NotificationUserSearchFilterModel filter = new()
            {
                Quantity = _pushNotificationsJobOptions?.CurrentValue.PimsNotificationsBatchSize ?? 50,
                NotificationOutputTypeCode = NotificationOutputTypes.PIMS.ToString(),
                MaxRetries = _pushNotificationsJobOptions?.CurrentValue.PimsNotificationsMaxRetriesAllowed ?? 3,
                NotificationSentDateTime = null,
                NotificationTriggerDate = DateOnly.FromDateTime(DateTime.UtcNow),
            };

            var searchResponse = await SearchUserNotifications(filter);
            if (searchResponse?.ScheduledTaskResponseModel != null)
            {
                return searchResponse?.ScheduledTaskResponseModel;
            }

            // Request to PUSH notifications
            _logger.LogInformation("Processing {count} PIMS notifications.", searchResponse?.SearchResults?.Payload?.Count ?? 0);

            IEnumerable<Task<PushNotificationResponseModel>> responses = searchResponse?.SearchResults?.Payload?.Select(notification =>
            {
                _logger.LogInformation("Pushing notification with id {Id}", notification.Id);

                return _notificationUserRepository.PushUserNotificationsAsync(notification).ContinueWith(response => HandlePushNotificationRequestTaskResponse("PushPimsUserNotifications", notification, response));
            });

            var results = await Task.WhenAll(responses);

            return new BaseTaskResponseModel() { Status = GetMergedStatus(results) };
        }

        private static TaskResponseStatusTypes GetMergedStatus(IEnumerable<PushNotificationResponseModel> responses)
        {
            if (responses.All(r => r.ResponseStatus == ExternalResponseStatus.Success))
            {
                return TaskResponseStatusTypes.SUCCESS;
            }
            else if (responses.All(r => r.ResponseStatus == ExternalResponseStatus.Error || r.ResponseStatus == ExternalResponseStatus.NotExecuted))
            {
                return TaskResponseStatusTypes.ERROR;
            }

            return TaskResponseStatusTypes.PARTIAL;
        }

        private async Task<SearchNotificationsResponseModel> SearchUserNotifications(NotificationUserSearchFilterModel filter)
        {
            BaseTaskResponseModel scheduledTaskResponseModel = null;
            var pendingNotifications = await _notificationUserRepository.SearchUserNotificationsAsync(filter);
            if (pendingNotifications?.Payload?.Count == 0)
            {
                _logger.LogInformation("No User notifications to process, skipping execution.");
                scheduledTaskResponseModel = new BaseTaskResponseModel() { Status = TaskResponseStatusTypes.SKIPPED, Message = "No notifications to process, skipping execution." };
            }

            return new SearchNotificationsResponseModel() { ScheduledTaskResponseModel = scheduledTaskResponseModel, SearchResults = pendingNotifications };
        }

        private PushNotificationResponseModel HandlePushNotificationRequestResponse(string httpMethodName, NotificationOutputModel notification, ExternalResponse<NotificationOutputModel> response)
        {
            if (response?.Status == ExternalResponseStatus.Success)
            {
                return new PushNotificationResponseModel() { ResponseStatus = response.Status, Message = response.Message };
            }
            else
            {
                _logger.LogError("Received error response from {httpMethodName} for push notification {NotificationId} status {Status} message: {Message}", httpMethodName, notification?.Id, response?.Status, response?.Message);
                return new PushNotificationResponseModel() { ResponseStatus = response?.Status ?? ExternalResponseStatus.Error, Message = response?.Message ?? "Unknown error" };
            }
        }

        private PushNotificationResponseModel HandlePushNotificationRequestTaskResponse(string httpMethodName, NotificationOutputModel notification, Task<ExternalResponse<NotificationOutputModel>> response)
        {
            var responseObject = response?.Result;
            if (responseObject?.Status == ExternalResponseStatus.Success)
            {
                return new PushNotificationResponseModel() { ResponseStatus = responseObject.Status, Message = "Ok" };
            }
            else
            {
                _logger.LogError("Received error response from {httpMethodName} for push notification {NotificationId} status {Status} message: {Message}", httpMethodName, notification?.Id, response?.Result?.Status, response?.Result?.Message);
                return new PushNotificationResponseModel() { ResponseStatus = responseObject?.Status ?? ExternalResponseStatus.Error, Message = responseObject?.Message ?? "Unknown error" };
            }
        }
    }
}
