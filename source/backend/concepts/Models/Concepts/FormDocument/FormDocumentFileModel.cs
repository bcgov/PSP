using Pims.Api.Concepts.Models.Base;

namespace Pims.Api.Concepts.Models.Concepts.FormDocument
{
    public class FormDocumentFileModel : BaseAuditModel
    {
        public long? Id { get; set; }

        public long FileId { get; set; }

        public FormDocumentTypeModel FormDocumentType { get; set; }
    }
}
