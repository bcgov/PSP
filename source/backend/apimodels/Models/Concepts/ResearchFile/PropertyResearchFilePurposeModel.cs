using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.ResearchFile
{
    public class PropertyResearchFilePurposeModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - Property research file purpose id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Property research file purpose type code.
        /// </summary>
        public virtual CodeTypeModel<string> PropertyResearchPurposeTypeCode { get; set; }

        #endregion
    }
}
