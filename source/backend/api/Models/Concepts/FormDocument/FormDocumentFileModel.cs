namespace Pims.Api.Models.Concepts
{
    public class FormDocumentFileModel : BaseAppModel
    {
        public long? Id { get; set; }

        public long FileId { get; set; }

        public FormDocumentTypeModel FormDocumentType { get; set; }
    }
}
