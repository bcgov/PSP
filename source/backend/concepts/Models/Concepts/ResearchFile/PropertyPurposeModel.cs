using Pims.Api.Concepts.Models.Base;

namespace Pims.Api.Concepts.Models.Concepts.ResearchFile
{
    public class PropertyPurposeModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The property purpose type code.
        /// </summary>
        public TypeModel<string> PropertyPurposeType { get; set; }

        /// <summary>
        /// get/set - The research file property id.
        /// </summary>
        public long PropertyResearchFileId { get; set; }
        #endregion
    }
}
