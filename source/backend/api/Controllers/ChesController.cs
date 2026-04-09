using Microsoft.AspNetCore.Mvc;

namespace Pims.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/ches")]
    [Route("/ches")]
    public class ChesController : ControllerBase
    {
    }
}
