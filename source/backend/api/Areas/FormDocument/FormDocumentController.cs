using System.Collections.Generic;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// FormDocumentController class, provides endpoints to handle document generation requests.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/formDocument/")]
    [Route("/formDocument")]
    public class FormDocumentController : ControllerBase
    {
        #region Variables

        private readonly IFormDocumentService _formDocumentService;

        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a FormDocumentController class.
        /// </summary>
        /// <param name="formDocumentService"></param>
        /// <param name="mapper"></param>
        public FormDocumentController(IFormDocumentService formDocumentService, IMapper mapper)
        {
            _formDocumentService = formDocumentService;
            _mapper = mapper;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Gets the form document types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HasPermission(Permissions.FormView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<FormDocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "form-document" })]
        public IActionResult GetFormDocumentTypes()
        {
            var supportedFileTypes = _formDocumentService.GetAllFormDocumentTypes();
            return new JsonResult(supportedFileTypes);
        }

        /// <summary>
        /// Add the specified form document.
        /// </summary>
        /// <param name="fileType">The type of the file.</param>
        /// <param name="formFileModel">The form to add.</param>
        /// <returns></returns>
        [HttpPost("{fileType}")]
        [HasPermission(Permissions.FormAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FormDocumentFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "form" })]
        public IActionResult AddFormDocumentFile(FileType fileType, [FromBody] FormDocumentFileModel formFileModel)
        {
            switch (fileType)
            {
                case FileType.Acquisition:
                    PimsFormType pimsEntity = _mapper.Map<PimsFormType>(formFileModel.FormDocumentType);
                    var createdFormModel = _mapper.Map<FormDocumentFileModel>(_formDocumentService.AddAcquisitionForm(pimsEntity, formFileModel.FileId));
                    return new JsonResult(createdFormModel);
                default:
                    throw new BadRequestException("File type must be a known type");
            }
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
        [ProducesResponseType(typeof(FormDocumentFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "form" })]
        public IActionResult GetFileForms(FileType fileType, long fileId)
        {
            IEnumerable<FormDocumentFileModel> forms;
            forms = fileType switch
            {
                FileType.Acquisition => _mapper.Map<IEnumerable<FormDocumentFileModel>>(_formDocumentService.GetAcquisitionForms(fileId)),
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
        [ProducesResponseType(typeof(FormDocumentFileModel), 200)]
        [SwaggerOperation(Tags = new[] { "form" })]
        public IActionResult GetFileForm(FileType fileType, long fileFormId)
        {
            FormDocumentFileModel form;
            form = fileType switch
            {
                FileType.Acquisition => _mapper.Map<FormDocumentFileModel>(_formDocumentService.GetAcquisitionForm(fileFormId)),
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
                FileType.Acquisition => _formDocumentService.DeleteAcquisitionFileForm(fileFormId),
                _ => throw new BadRequestException("File type must be a known type"),
            };
            return new JsonResult(deleted);
        }
        #endregion
    }
}
