using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly INotificationUserOutputRepository _notificationUserOutputRepository;
        private readonly IEmailRepository _chesRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NotificationUserService(ClaimsPrincipal user, ILogger<DocumentQueueService> logger, INotificationUserOutputRepository userOutputRepository, IEmailRepository chesRepository, IAcquisitionFileRepository acqFileRepository, IDispositionFileRepository dispFileRepository, IWebHostEnvironment webHostEnvironment)
            : base(null, logger)
        {
            _user = user;
            _logger = logger;
            _notificationUserOutputRepository = userOutputRepository;
            _chesRepository = chesRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<PimsNotificationUserOutput> SearchNotificationUser(NotificationUserSearchFilterModel filter)
        {
            _logger.LogInformation("Searching all agreements matching the filter: {filter} ", filter);
            _user.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            return _notificationUserOutputRepository.GetAllByFilter(filter);
        }

        public async Task PushNotificationUser(long notificationUserId)
        {
            _logger.LogInformation("Pushing notification with Id: {notificationUserId} ", notificationUserId);
            _user.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            var userNotification = _notificationUserOutputRepository.GetById(notificationUserId);
            if (userNotification.NotificationSentDt is not null)
            {
                return;
            }

            userNotification.NotificationRetryCnt = userNotification.NotificationRetryCnt.HasValue ? ++userNotification.NotificationRetryCnt : 1;

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
                            userNotification.NotificationSentDt = DateTime.UtcNow;
                        }
                        break;
                    case ExternalResponseStatus.Error:
                    case ExternalResponseStatus.NotExecuted:
                        {
                            userNotification.NotificationErrorDt = DateOnly.FromDateTime(DateTime.UtcNow);
                            userNotification.NotificationErrorReason = chesResponse.Message;
                        }
                        break;
                }
            }
            else
            {
                userNotification.NotificationSentDt = DateTime.UtcNow;
            }

            _ = await _notificationUserOutputRepository.UpdateAsync(userNotification);

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
        }

        private static string GetNotificationSource(PimsNotification notification)
        {
            return notification.NotificationTypeCode switch
            {
                nameof(NotificationTypes.TAKE_SRW)
                or nameof(NotificationTypes.TAKE_LAT)
                or nameof(NotificationTypes.TAKE_LTC)
                or nameof(NotificationTypes.TAKE_LPYBLE)
                or nameof(NotificationTypes.NOC)
                or nameof(NotificationTypes.EXPROPH_APPEFFDT)
                or nameof(NotificationTypes.AGMT_SIGND)
                    => $"Acquisition File #: {notification.AcquisitionFile.FileNumberFormatted}",

                nameof(NotificationTypes.L_RENEWAL)
                    => $"Lease File #: {notification.Lease.LFileNo} ",

                nameof(NotificationTypes.L_INSURANCE)
                    => GetInsuranceSource(notification),

                nameof(NotificationTypes.L_CONSULTFN)
                    => GetConsultationSource(notification),

                _ => string.Empty,
            };
        }

        private static string GetInsuranceSource(PimsNotification notification)
        {
            var lease = notification.Lease;

            if (lease.PimsLeaseStakeholders.Count > 0 &&
                lease.LeasePayRvblTypeCode == "RCVBL")
            {
                var tenants = GetTenants(lease);

                return $"Lease File #: {lease.LFileNo} " +
                    $"with {tenants} as Tenants. " +
                    $"Insurance for: {notification.Insurance.InsuranceTypeCodeNavigation.Description}";
            }

            return $"Lease File #: {lease.LFileNo} " +
                $"and Insurance for: {notification.Insurance.InsuranceTypeCodeNavigation.Description}";
        }

        private static string GetConsultationSource(PimsNotification notification)
        {
            var lease = notification.Lease;

            if (lease.PimsLeaseStakeholders.Count > 0)
            {
                var tenants = GetTenants(lease);

                return $"Lease File #: {lease.LFileNo} " +
                    $"with {tenants} as Tenants and " +
                    $"First Nation Consultation with status: " +
                    $"{notification.LeaseConsultation.ConsultationOutcomeTypeCodeNavigation.Description}";
            }

            return $"Lease File #: {lease.LFileNo} " +
                $"and First Nation Consultation with status: " +
                $"{notification.LeaseConsultation.ConsultationStatusTypeCodeNavigation.Description}";
        }

        private static string GetTenants(PimsLease lease)
        {
            return string.Join(
                ", ",
                lease.PimsLeaseStakeholders
                    .Select(stakeholder =>
                        stakeholder.PersonId.HasValue
                            ? stakeholder.Person.GetFullName()
                            : stakeholder.Organization?.Name)
                    .Where(name => !string.IsNullOrWhiteSpace(name))
            );
        }
    }
}
