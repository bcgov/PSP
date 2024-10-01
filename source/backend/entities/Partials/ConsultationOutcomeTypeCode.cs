using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsConsultationOutcomeType class, provides an entity for the datamodel to manage Consultation outcome types.
    /// </summary>
    public partial class PimsConsultationOutcomeType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify Consultation outcome type.
        /// </summary>
        [NotMapped]
        public string Id { get => ConsultationOutcomeTypeCode; set => ConsultationOutcomeTypeCode = value; }
        #endregion

        #region Constructors

        public PimsConsultationOutcomeType() { }

        /// <summary>
        /// Create a new instance of a PimsConsultationOutcomeType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsConsultationOutcomeType(string id)
        {
            Id = id;
        }
        #endregion
    }
}
