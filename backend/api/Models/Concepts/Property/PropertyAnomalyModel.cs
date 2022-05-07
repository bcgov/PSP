namespace Pims.Api.Models.Concepts
{
    public class PropertyAnomalyModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Property anomaly id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// get/set - Anomaly type code.
        /// </summary>
        public virtual TypeModel<string> PropertyAnomalyTypeCode { get; set; }

        #endregion
    }
}
