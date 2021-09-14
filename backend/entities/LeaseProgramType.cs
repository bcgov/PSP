using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseProgramType class, provides an entity for the datamodel to manage a list of lease program types.
    /// </summary>
    [MotiTable("PIMS_LEASE_PROGRAM_TYPE", "LSPRGT")]
    public class LeaseProgramType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease program type.
        /// </summary>
        [Column("LEASE_PROGRAM_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of leases.
        /// </summary>
        public ICollection<Lease> Leases { get; } = new List<Lease>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseProgramType class.
        /// </summary>
        public LeaseProgramType() { }

        /// <summary>
        /// Create a new instance of a LeaseProgramType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeaseProgramType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
