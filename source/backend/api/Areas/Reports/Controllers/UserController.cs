using System;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Constants;
using Pims.Core.Api.Exceptions;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Helpers.Reporting;
using Pims.Api.Models.Base;
using Pims.Core.Api.Policies;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Swashbuckle.AspNetCore.Annotations;
using EModel = Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Reports.Controllers
{
    /// <summary>
    /// UserController class, provides endpoints for generating reports.
    /// </summary>
    [Authorize]
    [ApiController]
    [Area("reports")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[area]/users")]
    [Route("[area]/users")]
    public class UserController : ControllerBase
    {
        #region Variables
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a UserController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints
        #region Export Users

        /// <summary>
        /// Exports users as CSV or Excel file.
        /// Include 'Accept' header to request the appropriate expor -
        ///     ["text/csv", "application/application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"].
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpGet]
        [HasPermission(Permissions.AdminUsers)]
        [Produces(ContentTypes.CONTENTTYPECSV, ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(typeof(PageModel<Models.User.UserModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 403)]
        [SwaggerOperation(Tags = new[] { "user", "report" })]
        public IActionResult ExportUsers(bool all = false)
        {
            var uri = new Uri(this.Request.GetDisplayUrl());
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            return ExportUsers(new EModel.UserFilter(query), all);
        }

        /// <summary>
        /// Exports users as CSV or Excel file.
        /// Include 'Accept' header to request the appropriate export -
        ///     ["text/csv", "application/application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"].
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpPost("filter")]
        [HasPermission(Permissions.AdminUsers)]
        [Produces(ContentTypes.CONTENTTYPECSV, ContentTypes.CONTENTTYPEEXCELX)]
        [ProducesResponseType(typeof(PageModel<Models.User.UserModel>), 200)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(Api.Models.ErrorResponseModel), 403)]
        [SwaggerOperation(Tags = new[] { "user", "report" })]
        public IActionResult ExportUsers([FromBody] EModel.UserFilter filter, bool all = false)
        {
            filter.ThrowBadRequestIfNull($"The request must include a filter.");
            var acceptHeader = (string)this.Request.Headers["Accept"];

            if (acceptHeader != ContentTypes.CONTENTTYPECSV && acceptHeader != ContentTypes.CONTENTTYPEEXCEL && acceptHeader != ContentTypes.CONTENTTYPEEXCELX)
            {
                throw new BadRequestException($"Invalid HTTP request header 'Accept:{acceptHeader}'.");
            }

            filter.Quantity = all ? _userRepository.Count() : filter.Quantity;
            filter.Page = 1;
            var page = _userRepository.GetAllByFilter(filter);
            var report = _mapper.Map<PageModel<Models.User.UserModel>>(page);

            return acceptHeader.ToString() switch
            {
                ContentTypes.CONTENTTYPECSV => ReportHelper.GenerateCsv(report.Items),
                _ => ReportHelper.GenerateExcel(report.Items, "PIMS")
            };
        }
        #endregion
        #endregion
    }
}
