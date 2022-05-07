namespace Pims.Api.Models.Concepts
{
    public class PropertyRoadModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Property road id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// get/set - Road type code.
        /// </summary>
        public virtual TypeModel<string> PropertyRoadTypeCode { get; set; }

        #endregion
    }
}
