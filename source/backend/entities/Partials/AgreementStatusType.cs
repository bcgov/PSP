using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAgreementStatusType class, provides an entity for the datamodel to manage agreement status types.
    /// </summary>
    public partial class PimsAgreementStatusType : ITypeEntity<string, bool?>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify agreement status type.
        /// </summary>
        [NotMapped]
        public string Id { get => AgreementStatusTypeCode; set => AgreementStatusTypeCode = value; }
        #endregion
    }
}
