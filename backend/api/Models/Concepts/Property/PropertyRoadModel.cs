namespace Pims.Api.Models.Concepts
{
    public class PropertyRoadModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - Property road id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent property id.
        /// </summary>
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - Road type code.
        /// </summary>
        public virtual TypeModel<string> PropertyRoadTypeCode { get; set; }

        #endregion
    }
}
