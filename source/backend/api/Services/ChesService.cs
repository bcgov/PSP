#nullable enable
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Ches;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Config;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Repositories.Ches;

namespace Pims.Api.Services
{
    /// <summary>
    /// Default implementation of IEmailService using CHES.
    /// </summary>
    public class ChesService : IEmailService
    {
        private readonly IEmailRepository _chesRepository;
        private readonly ILogger<ChesService> _logger;
        private readonly ChesConfig _chesConfig;

        public ChesService(IEmailRepository chesRepository, ILogger<ChesService> logger, ChesConfig chesConfig)
        {
            _chesRepository = chesRepository;
            _logger = logger;
            _chesConfig = chesConfig;
        }

        public async Task<ExternalResponse<EmailResponse>> SendEmailAsync(EmailRequest request)
        {
            if (request == null)
            {
                return new ExternalResponse<EmailResponse>
                {
                    Status = ExternalResponseStatus.Error,
                    Payload = new EmailResponse(),
                    Message = "Email request cannot be null",
                };
            }

            if (request.To == null || request.To.Count == 0)
            {
                return new ExternalResponse<EmailResponse>
                {
                    Status = ExternalResponseStatus.Error,
                    Payload = new EmailResponse(),
                    Message = "Email request must have at least one recipient",
                };
            }

            if (string.IsNullOrEmpty(request.From))
            {
                request.From = _chesConfig.FromEmail;
            }

            _logger.LogInformation("Email send requested. Recipient count: {recipientCount}.", request.To?.Count ?? 0);

            ExternalResponse<EmailResponse>? response = await _chesRepository.SendEmailAsync(request);

            if (response == null || response.Payload == null)
            {
                return new ExternalResponse<EmailResponse>
                {
                    Status = ExternalResponseStatus.Error,
                    Payload = new EmailResponse(),
                    Message = "Error sending email",
                };
            }
            return response;
        }
    }
}
