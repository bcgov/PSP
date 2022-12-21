using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsTenantType class, provides an entity for the datamodel to manage a list of Tenant types.
    /// </summary>
    public partial class PimsTenantType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify Person Profile type.
        /// </summary>
        [NotMapped]
        public string Id { get => TenantTypeCode; set => TenantTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAcqFlPersonProfileType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsTenantType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
