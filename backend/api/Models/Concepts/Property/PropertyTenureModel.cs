namespace Pims.Api.Models.Concepts
{
    public class PropertyTenureModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Property tenure id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// get/set - Tenure type code.
        /// </summary>
        public virtual TypeModel<string> PropertyTenureTypeCode { get; set; }

        #endregion
    }
}
