namespace Pims.Api.Models.Concepts
{
    public class ResearchFilePropertyModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The relationship's disabled status flag.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The relationship's disabled status flag.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// get/set - The relationship's property.
        /// </summary>
        public PropertyModel Property { get; set; }

        /// <summary>
        /// get/set - The relationship's research file.
        /// </summary>
        public ResearchFileModel ResearchFile { get; set; }
        #endregion
    }
}
