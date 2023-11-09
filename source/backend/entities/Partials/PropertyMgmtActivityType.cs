using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropMgmtActivityType class, provides an entity for the datamodel to manage management activities types.
    /// </summary>
    public partial class PimsPropMgmtActivityType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key.
        /// </summary>
        [NotMapped]
        public string Id { get => PropMgmtActivityTypeCode; set => PropMgmtActivityTypeCode = value; }
        #endregion

    }
}
