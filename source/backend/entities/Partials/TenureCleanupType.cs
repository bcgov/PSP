using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsTenureCleanupType class, provides an entity for the datamodel to manage Tenure Cleanup Types.
    /// </summary>
    public partial class PimsTenureCleanupType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify acquisition file progress status type.
        /// </summary>
        [NotMapped]
        public string Id { get => TenureCleanupTypeCode; set => TenureCleanupTypeCode = value; }

        public PimsTenureCleanupType(string id)
            : this()
        {
            Id = id;
        }

        public PimsTenureCleanupType()
        {
        }
    }
}
