using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Ches;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Models.Concepts.Notification;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Repositories.Ches;
using Pims.Core.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class NotificationUserService : BaseService, INotificationUserService
    {
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _user;
        private readonly INotificationUserRepository _notificationUserRepository;
        private readonly IEmailRepository _chesRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NotificationUserService(ClaimsPrincipal user, ILogger<DocumentQueueService> logger, INotificationUserRepository userRepository, IEmailRepository chesRepository, IAcquisitionFileRepository acqFileRepository, IDispositionFileRepository dispFileRepository, IWebHostEnvironment webHostEnvironment)
            : base(null, logger)
        {
            _user = user;
            _logger = logger;
            _notificationUserRepository = userRepository;
            _chesRepository = chesRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<PimsNotificationUserOutput> SearchNotificationUser(NotificationUserSearchFilterModel filter)
        {
            _logger.LogInformation("Searching all agreements matching the filter: {filter} ", filter);
            _user.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            return _notificationUserRepository.GetAllByFilter(filter);
        }

        public async Task PushNotificationUser(long notificationUserId)
        {
            _logger.LogInformation("Pushing notification with Id: {notificationUserId} ", notificationUserId);
            _user.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            var userNotification = _notificationUserRepository.GetById(notificationUserId);
            if (userNotification.NotificationSentDt is not null)
            {
                return;
            }

            userNotification.NotificationRetryCnt = userNotification.NotificationRetryCnt.HasValue ? ++userNotification.NotificationRetryCnt : 1;
            var updatedNotification = _notificationUserRepository.Update(userNotification);
            _notificationUserRepository.CommitTransaction();

            if (userNotification.NotificationOutputTypeCode == NotificationOutputTypes.EMAIL.ToString())
            {
                EmailRequest emailRequest = await GenerateEmailRequest(userNotification);
                ExternalResponse<EmailResponse> chesResponse;
                if (emailRequest is null)
                {
                    chesResponse = new() { Status = ExternalResponseStatus.Error, Message = "PIMS internal error when building email, Recipient not found" };
                }
                else
                {
                    chesResponse = await _chesRepository.SendEmailAsync(emailRequest);
                }

                switch (chesResponse.Status)
                {
                    case ExternalResponseStatus.Success:
                        {
                            updatedNotification.NotificationSentDt = DateTime.UtcNow;
                        }
                        break;
                    case ExternalResponseStatus.Error:
                    case ExternalResponseStatus.NotExecuted:
                        {
                            updatedNotification.NotificationErrorDt = DateOnly.FromDateTime(DateTime.UtcNow);
                            updatedNotification.NotificationErrorReason = chesResponse.Message;
                        }
                        break;
                }
            }
            else
            {
                updatedNotification.NotificationSentDt = DateTime.UtcNow;
            }

            _notificationUserRepository.Update(updatedNotification);
            _notificationUserRepository.CommitTransaction();

            return;
        }

        private async Task<EmailRequest> GenerateEmailRequest(PimsNotificationUserOutput userNotification)
        {
            var emailToContactAddress = userNotification.NotificationUser.User?.Person?.GetEmail();
            var emailToUsername = userNotification.NotificationUser.User?.Person?.GetFullName();
            var notificationTypeCodeDescription = userNotification.NotificationUser.Notification.NotificationTypeCodeNavigation.Description;

            if (emailToContactAddress is null)
            {
                return null;
            }

            string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Resources", "EmailNotification.html");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("HTML template not found.");
            }

            // Load asynchronously
            string templateBody = await File.ReadAllTextAsync(path);

            EmailRequest newEmail = new()
            {
                Subject = $"PIMS System {notificationTypeCodeDescription}",
                Body = templateBody
                            .Replace("{{userName}}", emailToUsername)
                            .Replace("{{notificationType}}", notificationTypeCodeDescription)
                            .Replace("{{notificationSource}}", GetNotificationSource(userNotification.NotificationUser.Notification)),
            };

            newEmail.To.Add(emailToContactAddress);

            return newEmail;

            static string GetNotificationSource(PimsNotification notification)
            {
                string source;
                if(notification.AcquisitionFileId.HasValue)
                {
                    source = $"Acquisition File #: {notification.AcquisitionFile.FileNumberFormatted}";
                }
                else if(notification.DispositionFileId.HasValue)
                {
                    source = $"Disposition File #: D-{notification.DispositionFile.FileNumber}";
                }
                else if(notification.ResearchFileId.HasValue)
                {
                    source = $"Research File #: R-{notification.ResearchFile.RfileNumber}";
                }
                else if(notification.ManagementFileId.HasValue)
                {
                    source = $"Management File #: M-{notification.ManagementFile.ManagementFileId}";
                }
                else if(notification.LeaseId.HasValue)
                {
                    source = $"Lease #: L-{notification.Lease.LFileNo}";
                }
                else
                {
                    source = string.Empty;
                }

                return source;
            }
        }
    }
}
