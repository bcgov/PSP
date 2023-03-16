using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AddressType class, provides an entity for the datamodel to manage a list of address usage types.
    /// </summary>
    public partial class PimsAddressUsageType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify address usage type.
        /// </summary>
        [NotMapped]
        public string Id { get => AddressUsageTypeCode; set => AddressUsageTypeCode = value; }

        /// <summary>
        /// get - A collection of addresses.
        /// </summary>
        [NotMapped]
        public ICollection<PimsAddress> Addresses { get; } = new List<PimsAddress>();
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a AddressType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAddressUsageType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
