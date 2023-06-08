using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsInterestHolderInterestType class, provides an entity for the datamodel to manage interest holder types.
    /// </summary>
    public partial class PimsInterestHolderInterestType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify interest holder type.
        /// </summary>
        [NotMapped]
        public string Id { get => InterestHolderInterestTypeCode; set => InterestHolderInterestTypeCode = value; }
        #endregion
    }
}
