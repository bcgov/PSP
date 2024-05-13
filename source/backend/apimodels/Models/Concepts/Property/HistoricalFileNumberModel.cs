using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Property
{
    public class HistoricalFileNumberModel : BaseAuditModel
    {
        /// <summary>
        /// get/set - The property for the number.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The property Id for the number.
        /// </summary>
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - The property object for the number.
        /// </summary>
        public PropertyModel Property { get; set; }

        /// <summary>
        /// get/set - The type of file.
        /// </summary>
        public CodeTypeModel<string> FileNumberTypeCode { get; set; }

        /// <summary>
        /// get/set - The number value.
        /// </summary>
        public string FileNumber { get; set; }

        /// <summary>
        /// get/set - Other type that's not registered.
        /// </summary>
        public string OtherFileNumberType { get; set; }

        /// <summary>
        /// get/set - Whether is disabled.
        /// </summary>
        public bool? IsDisabled { get; set; }
    }
}
