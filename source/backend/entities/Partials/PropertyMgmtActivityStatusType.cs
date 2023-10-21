using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropMgmtActivityStatusType class, provides an entity for the datamodel to manage management activities status types.
    /// </summary>
    public partial class PimsPropMgmtActivityStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key.
        /// </summary>
        [NotMapped]
        public string Id { get => PropMgmtActivityStatusTypeCode; set => PropMgmtActivityStatusTypeCode = value; }
        #endregion

    }
}
