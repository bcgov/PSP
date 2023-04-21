namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileChecklistItemModel : BaseAppModel
    {
        /// <summary>
        /// get/set - Checklist item id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Acquisition file id.
        /// </summary>
        public long AcquisitionFileId { get; set; }

        /// <summary>
        /// get/set - Checklist item type.
        /// </summary>
        public AcquisitionFileChecklistItemTypeModel ItemType { get; set; }

        /// <summary>
        /// get/set - Checklist item status type code.
        /// </summary>
        public TypeModel<string> StatusTypeCode { get; set; }
    }
}
