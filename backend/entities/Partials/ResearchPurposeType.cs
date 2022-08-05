using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsResearchPurposeType class, provides an entity for the datamodel to manage research purpose types.
    /// </summary>
    public partial class PimsResearchPurposeType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify research file purpose type.
        /// </summary>
        [NotMapped]
        public string Id { get => ResearchPurposeTypeCode; set => ResearchPurposeTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsResearchPurposeType class.
        /// </summary>
        /// <param name="id"></param>PimsResearchPurposeType
        public PimsResearchPurposeType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
