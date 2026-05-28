using System.Threading.Tasks;
using Pims.Api.Models.Ches;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Services
{
    /// <summary>
    /// IEmailService interface, defines the functionality for email service.
    /// </summary>
    public interface IEmailService
    {
        Task<ExternalResponse<EmailResponse>> SendEmailAsync(EmailRequest request);
    }
}