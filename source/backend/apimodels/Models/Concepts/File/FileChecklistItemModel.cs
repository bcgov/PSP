using Pims.Api.Models.Base;

namespace Pims.Api.Models.Models.Concepts.File
{
    public class FileChecklistItemModel : BaseAuditModel
    {
        /// <summary>
        /// get/set - Checklist item id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - File id.
        /// </summary>
        public long FileId { get; set; }

        /// <summary>
        /// get/set - Checklist item type.
        /// </summary>
        public FileChecklistItemTypeModel ItemType { get; set; }

        /// <summary>
        /// get/set - Checklist item status type code.
        /// </summary>
        public CodeTypeModel<string> StatusTypeCode { get; set; }
    }
}
