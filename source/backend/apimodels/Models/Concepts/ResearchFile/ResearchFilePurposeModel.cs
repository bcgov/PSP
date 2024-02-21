using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.ResearchFile
{
    public class ResearchFilePurposeModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - Research file purpose id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Purpose type code.
        /// </summary>
        public virtual CodeTypeModel<string> ResearchPurposeTypeCode { get; set; }

        #endregion
    }
}
