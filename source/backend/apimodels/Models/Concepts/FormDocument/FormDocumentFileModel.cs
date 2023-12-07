using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.FormDocument
{
    public class FormDocumentFileModel : BaseAuditModel
    {
        public long? Id { get; set; }

        public long FileId { get; set; }

        public FormDocumentTypeModel FormDocumentType { get; set; }
    }
}
