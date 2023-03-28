using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Models.Lookup;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pims.Api.Areas.Forms.Controllers
{
    /// <summary>
    /// FormController class, provides endpoints for interacting with forms.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Area("forms")]
    [Route("v{version:apiVersion}/[area]")]
    [Route("[area]")]
    public class FormController : ControllerBase
    {
        #region Variables
        private readonly IFormService _formService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a FormController class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="formService"></param>
        /// <param name="mapper"></param>
        ///
        public FormController(IFormService formService, IMapper mapper)
        {
            _formService = formService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Add the specified form.
        /// </summary>
        /// <param name="fileType">The type of the file.</param>
        /// <param name="formFileModel">The form to add.</param>
        /// <returns></returns>
        [HttpPost("{fileType}")]
        [HasPermission(Permissions.FormAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FileFormModel), 200)]
        [SwaggerOperation(Tags = new[] { "form" })]
        public IActionResult AddFormFile(FileType fileType, [FromBody] FileFormModel formFileModel)
        {
            FileFormModel createdFormModel;
            createdFormModel = fileType switch
            {
                FileType.Acquisition => _mapper.Map<FileFormModel>(_formService.AddAcquisitionForm(formFileModel.FormTypeCode, formFileModel.FileId)),
                _ => throw new BadRequestException("File type must be a known type"),
            };
            return new JsonResult(createdFormModel);
        }

        /// <summary>
        /// Get all the forms corresponding to the passed file id.
        /// </summary>
        /// <param name="fileType">The type of the file.</param>
        /// <param name="fileId">The file to retrieve forms for.</param>
        /// <returns></returns>
        [HttpGet("{fileType}/file/{fileId}")]
        [HasPermission(Permissions.FormView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FileFormModel), 200)]
        [SwaggerOperation(Tags = new[] { "form" })]
        public IActionResult GetFileForms(FileType fileType, long fileId)
        {
            IEnumerable<FileFormModel> forms;
            forms = fileType switch
            {
                FileType.Acquisition => _mapper.Map<IEnumerable<FileFormModel>>(_formService.GetAcquisitionForms(fileId)),
                _ => throw new BadRequestException("File type must be a known type"),
            };
            return new JsonResult(forms);
        }

        /// <summary>
        /// Get the form corresponding to the passed file form id.
        /// </summary>
        /// <param name="fileType">The type of the file.</param>
        /// <param name="fileFormId">The form to add.</param>
        /// <returns></returns>
        [HttpGet("{fileType}/{fileFormId}")]
        [HasPermission(Permissions.FormView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FileFormModel), 200)]
        [SwaggerOperation(Tags = new[] { "form" })]
        public IActionResult GetFileForm(FileType fileType, long fileFormId)
        {
            FileFormModel form;
            form = fileType switch
            {
                FileType.Acquisition => _mapper.Map<FileFormModel>(_formService.GetAcquisitionForm(fileFormId)),
                _ => throw new BadRequestException("File type must be a known type"),
            };
            return new JsonResult(form);
        }

        /// <summary>
        /// Deletes the file form.
        /// </summary>
        /// <param name="fileType">The type of the file.</param>
        /// <param name="fileFormId">Used to identify the form and delete it.</param>
        /// <returns></returns>
        [HttpDelete("{fileType}/{fileFormId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.FormDelete)]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "activity" })]
        public IActionResult DeleteFileForm(FileType fileType, long fileFormId)
        {
            var deleted = fileType switch
            {
                FileType.Acquisition => _formService.DeleteAcquisitionFileForm(fileFormId),
                _ => throw new BadRequestException("File type must be a known type"),
            };
            return new JsonResult(deleted);
        }
        #endregion
    }
}
