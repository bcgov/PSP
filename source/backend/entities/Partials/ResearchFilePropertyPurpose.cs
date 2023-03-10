using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPrfPropResearchPurposeType class, provides an entity for the datamodel to manage the relationship of research files' property types.
    /// </summary>
    public partial class PimsPrfPropResearchPurposeType : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify of the property purpose.
        /// </summary>
        [NotMapped]
        public override long Internal_Id { get => PrfPropResearchPurposeId; set => PrfPropResearchPurposeId = value; }
        #endregion
    }
}
