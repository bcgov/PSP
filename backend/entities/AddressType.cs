using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

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
        public PimsAddressUsageType(string id):this()
        {
            Id = id;
        }
        #endregion
    }

    public static class AddressUsageTypes
    {
        public const string Mailing = "MAILADDR";
        public const string ProofOfInsurance = "INSURE";
        public const string RentalPayment = "RENTAL";
        public const string PropertyNotification = "PROPNOTIFY";
        public const string Physical = "PHYSADDR";
    }
}
