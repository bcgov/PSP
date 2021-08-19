using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AddressType class, provides an entity for the datamodel to manage a list of address usage types.
    /// </summary>
    [MotiTable("PIMS_ADDRESS_USAGE_TYPE", "ADUSGT")]
    public class AddressType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify address usage type.
        /// </summary>
        [Column("ADDRESS_USAGE_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - A collection of addresses.
        /// </summary>
        public ICollection<Address> Addresses { get; } = new List<Address>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a AddressType class.
        /// </summary>
        public AddressType() { }

        /// <summary>
        /// Create a new instance of a AddressType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public AddressType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
