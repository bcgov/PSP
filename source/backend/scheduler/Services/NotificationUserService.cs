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
                AutoReplenishment = true // The limiter handles the timer internally
            };

            NotificationUserSearchFilterModel filter = new()
            {
                Quantity = _pushNotificationsJobOptions?.CurrentValue.EmailNotificationsBatchSize ?? 50,
                NotificationOutputTypeCode = NotificationOutputTypes.EMAIL.ToString(),
                MaxRetries = _pushNotificationsJobOptions?.CurrentValue.EmailNotificationsMaxRetriesAllowed ?? 3,
                NotificationSentDateTime = null,
                NotificationTriggerDate = DateOnly.FromDateTime(DateTime.UtcNow),
            };

            var searchResponse = await SearchEmailUserNotifications(filter);
            if (searchResponse?.ScheduledTaskResponseModel != null)
            {
                return searchResponse?.ScheduledTaskResponseModel;
            }

            // Request to PUSH notifications
            _logger.LogInformation($"Processing {searchResponse.SearchResults.Payload.Count} Emails notifications.");

            // 2. Process your notifications
            using RateLimiter limiter = new TokenBucketRateLimiter(limiterOptions);
            List<PushNotificationResponseModel> responses = new();
            foreach (var userNotification in searchResponse?.SearchResults?.Payload)
            {
                // 3. Acquire a token before making the call
                using RateLimitLease lease = await limiter.AcquireAsync(permitCount: 1);

                if (lease.IsAcquired)
                {
                    _logger.LogInformation("Token acquired. Processing {id}", userNotification.NotificationUserOutputId);

                    // Your existing logic
                    var response = await _notificationUserRepository.PushUserNotificationsAsync(userNotification);
                    PushNotificationResponseModel responseModel = HandlePushNotificationRequestResponse("PushUserNotificationsAsync", userNotification, response);
                    responses.Add(responseModel);
                }
                else
                {
                    _logger.LogWarning("Rate limit exceeded and queue is full for {id}", userNotification.NotificationUserOutputId);
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
                NotificationOutputTypeCode = NotificationOutputTypes.EMAIL.ToString(),
                MaxRetries = _pushNotificationsJobOptions?.CurrentValue.PimsNotificationsMaxRetriesAllowed ?? 3,
                NotificationSentDateTime = null,
                NotificationTriggerDate = DateOnly.FromDateTime(DateTime.UtcNow),
            };

            var searchResponse = await SearchEmailUserNotifications(filter);
            if (searchResponse?.ScheduledTaskResponseModel != null)
            {
                return searchResponse?.ScheduledTaskResponseModel;
            }

            // Request to PUSH notifications
            _logger.LogInformation($"Processing {searchResponse.SearchResults.Payload.Count} PIMS notifications.");

            IEnumerable<Task<PushNotificationResponseModel>> responses = searchResponse?.SearchResults?.Payload?.Select(not =>
            {
                _logger.LogInformation("Pushing notificatin with id {NotificationUserOutputId}", not.NotificationUserOutputId);

                return _notificationUserRepository.PushUserNotificationsAsync(not).ContinueWith(response => HandlePushNotificationRequestTaskResponse("PushPimsUserNotifications", not, response));
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

        private async Task<SearchNotificationsResponseModel> SearchEmailUserNotifications(NotificationUserSearchFilterModel filter)
        {
            BaseTaskResponseModel scheduledTaskResponseModel = null;
            var pendingEmailNotifications = await _notificationUserRepository.SearchUserNotificationsAsync(filter);
            if (pendingEmailNotifications?.Payload?.Count == 0)
            {
                _logger.LogInformation("No User Email notifications to process, skipping execution.");
                scheduledTaskResponseModel = new BaseTaskResponseModel() { Status = TaskResponseStatusTypes.SKIPPED, Message = "No emails to process, skipping execution." };
            }

            return new SearchNotificationsResponseModel() { ScheduledTaskResponseModel = scheduledTaskResponseModel, SearchResults = pendingEmailNotifications };
        }

        private PushNotificationResponseModel HandlePushNotificationRequestResponse(string httpMethodName, NotificationUserOutputModel notification, ExternalResponse<NotificationUserOutputModel> response)
        {
            if(response?.Status == ExternalResponseStatus.Success)
            {
                return new PushNotificationResponseModel() { ResponseStatus = response.Status, Message = response.Message };
            }
            else
            {
                _logger.LogError("Received error response from {httpMethodName} for push notification {notificationId} status {Status} message: {Message}", httpMethodName, notification?.NotificationUserOutputId, response?.Status, response?.Message);
                return new PushNotificationResponseModel() { ResponseStatus = response.Status, Message = response.Message };
            }
        }

        private PushNotificationResponseModel HandlePushNotificationRequestTaskResponse(string httpMethodName, NotificationUserOutputModel notification, Task<ExternalResponse<NotificationUserOutputModel>> response)
        {
            var responseObject = response?.Result;
            if (responseObject?.Status == ExternalResponseStatus.Success)
            {
                return new PushNotificationResponseModel() { ResponseStatus = responseObject.Status, Message = responseObject.Message };
            }
            else
            {
                _logger.LogError("Received error response from {httpMethodName} for push notification {notificationId} status {Status} message: {Message}", httpMethodName, notification?.NotificationUserOutputId, response?.Result?.Status, response?.Result?.Message);
                return new PushNotificationResponseModel() { ResponseStatus = responseObject.Status, Message = responseObject.Message };
            }
        }
    }
}
