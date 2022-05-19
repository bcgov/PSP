namespace Pims.Api.Models.Concepts
{
    public class PropertyAdjacentLandModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Property adjacent land id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent property id.
        /// </summary>
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - Adjacent land type code.
        /// </summary>
        public virtual TypeModel<string> PropertyAdjacentLandTypeCode { get; set; }

        #endregion
    }
}
