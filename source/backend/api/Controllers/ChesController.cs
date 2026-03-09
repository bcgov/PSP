using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Ches;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;

namespace Pims.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/ches/")]
    [Route("/ches/")]
    public class ChesController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ChesController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Send an email using CHES service.
        /// </summary>
        [HttpPost("email")]
        [ProducesResponseType(typeof(EmailResponse), 200)]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            var result = await _emailService.SendEmailAsync(request);
            if (result.Status == ExternalResponseStatus.Error)
            {
                return StatusCode(500, new { error = result.Message, details = result.Payload });
            }
            return Ok(result);
        }
    }
}
