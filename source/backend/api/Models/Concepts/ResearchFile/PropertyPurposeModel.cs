namespace Pims.Api.Models.Concepts
{
    public class PropertyPurposeModel : BaseModel
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
