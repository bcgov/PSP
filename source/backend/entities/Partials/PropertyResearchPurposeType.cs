using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropResearchPurposeType class, provides an entity for the datamodel to manage property research purpose types.
    /// </summary>
    public partial class PimsPropResearchPurposeType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify research file purpose type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropResearchPurposeTypeCode; set => PropResearchPurposeTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsPropResearchPurposeType class.
        /// </summary>
        /// <param name="id"></param>PimsPropResearchPurposeType
        public PimsPropResearchPurposeType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
