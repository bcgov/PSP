using System.Threading.Tasks;
using Pims.Api.Models.Ches;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Repositories.Ches
{
    public interface IEmailRepository
    {
        Task<ExternalResponse<EmailResponse>> SendEmailAsync(EmailRequest request);
    }
}