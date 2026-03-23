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

    }
}
