using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pims.Api.Helpers.Exceptions;
using Pims.Dal.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pims.Api.Controllers
{
    [Route("problem")]
    public class ProblemErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Index()
        {
            var code = HttpStatusCode.InternalServerError;
            var message = "An unhandled error has occurred.";
            var ex = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            if (ex is SecurityTokenException)
            {
                code = HttpStatusCode.Unauthorized;
                message = "The authentication token is invalid.";
            }
            else if (ex is SecurityTokenValidationException)
            {
                code = HttpStatusCode.Unauthorized;
                message = "The authentication token is invalid.";
            }
            else if (ex is SecurityTokenExpiredException)
            {
                code = HttpStatusCode.Unauthorized;
                message = "The authentication token has expired.";
            }
            else if (ex is SecurityTokenNotYetValidException)
            {
                code = HttpStatusCode.Unauthorized;
                message = "The authentication token not yet valid.";
            }
            else if (ex is DbUpdateConcurrencyException)
            {
                code = HttpStatusCode.BadRequest;
                message = "Data may have been modified or deleted since item was loaded.";

            }
            else if (ex is DbUpdateException)
            {
                code = HttpStatusCode.BadRequest;
                message = "An error occurred while updating this item.";

            }
            else if (ex is KeyNotFoundException)
            {
                code = HttpStatusCode.NotFound;
                message = "Item does not exist.";

            }
            else if (ex is ConcurrencyControlNumberMissingException)
            {
                code = HttpStatusCode.BadRequest;
                message = "Item cannot be updated without a row version.";

            }
            else if (ex is NotAuthorizedException)
            {
                code = HttpStatusCode.Forbidden;
                message = "User is not authorized to perform this action.";

            }
            else if (ex is Core.Exceptions.ConfigurationException)
            {
                code = HttpStatusCode.InternalServerError;
                message = "Application configuration details invalid or missing.";

            }
            else if (ex is BadRequestException || ex is InvalidOperationException)
            {
                code = HttpStatusCode.BadRequest;
                message = ex.Message;

            }
            else if (ex is UserOverrideException)
            {
                code = HttpStatusCode.Conflict;
                message = ex.Message;

            }
            return Problem(statusCode:(int)code,detail:message);
        }
    }
}
