namespace Pims.Api.Models.Concepts
{
    public class PropertyAdjacentLandModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Property adjacent land id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// get/set - Adjacent land type code.
        /// </summary>
        public virtual TypeModel<string> PropertyAdjacentLandTypeCode { get; set; }

        #endregion
    }
}
