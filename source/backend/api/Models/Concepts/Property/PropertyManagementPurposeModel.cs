namespace Pims.Api.Models.Concepts
{
    public class PropertyManagementPurposeModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent property id.
        /// </summary>
        public long PropertyId { get; set; }

        // TODO: Remove this from DB schema

        /// <summary>
        /// get/set - Whether this relationship is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The property purpose type code.
        /// </summary>
        public TypeModel<string> PropertyPurposeTypeCode { get; set; }

        #endregion
    }
}
