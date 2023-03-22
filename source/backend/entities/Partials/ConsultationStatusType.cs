using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsConsultationStatusType class, provides an entity for the datamodel to manage consultation status types.
    /// </summary>
    public partial class PimsConsultationStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to consultation status type.
        /// </summary>
        [NotMapped]
        public string Id { get => ConsultationStatusTypeCode; set => ConsultationStatusTypeCode = value; }
        #endregion
    }
}
