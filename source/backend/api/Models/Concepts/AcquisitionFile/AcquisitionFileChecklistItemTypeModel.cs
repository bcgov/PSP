namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileChecklistItemTypeModel : BaseModel
    {
        /// <summary>
        /// get/set - Checklist item code value.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - Section to which the item belongs.
        /// </summary>
        public string SectionCode { get; set; }

        /// <summary>
        /// get/set - A description of the checklist item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Checklist item descriptive tooltip presented to the user.
        /// </summary>
        public string Hint { get; set; }

        /// <summary>
        /// get/set - Whether this checklist item is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The display order.
        /// </summary>
        public int? DisplayOrder { get; set; }
    }
}
