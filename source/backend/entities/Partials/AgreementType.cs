using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAgreementType class, provides an entity for the datamodel to manage acquisition types.
    /// </summary>
    public partial class PimsAgreementType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify acquisition type.
        /// </summary>
        [NotMapped]
        public string Id { get => AgreementTypeCode; set => AgreementTypeCode = value; }
        #endregion
    }
}
