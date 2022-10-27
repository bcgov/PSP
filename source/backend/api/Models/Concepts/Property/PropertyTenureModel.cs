namespace Pims.Api.Models.Concepts
{
    public class PropertyTenureModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - Property tenure id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent property id.
        /// </summary>
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - Tenure type code.
        /// </summary>
        public virtual TypeModel<string> PropertyTenureTypeCode { get; set; }

        #endregion
    }
}
