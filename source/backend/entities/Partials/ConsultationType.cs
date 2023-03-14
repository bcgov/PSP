using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsConsultationType class, provides an entity for the datamodel to manage a consultation types.
    /// </summary>
    public partial class PimsConsultationType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to the consultation type.
        /// </summary>
        [NotMapped]
        public string Id { get => ConsultationTypeCode; set => ConsultationTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsConsultationType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsConsultationType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
