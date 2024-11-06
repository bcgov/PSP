using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsSubfileInterestType class, provides an entity for the datamodel to manage acquisition sub file interest types.
    /// </summary>
    public partial class PimsSubfileInterestType : ITypeEntity<string>
    {
        public PimsSubfileInterestType(string id)
            : this()
        {
            Id = id;
        }

        public PimsSubfileInterestType()
        {
        }

        /// <summary>
        /// get/set - Primary key to identify acquisition sub file interets type.
        /// </summary>
        [NotMapped]
        public string Id { get => SubfileInterestTypeCode; set => SubfileInterestTypeCode = value; }
    }
}
