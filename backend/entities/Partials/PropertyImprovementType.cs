using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyImprovementType class, provides an entity for the datamodel to manage a list of property improvement types.
    /// </summary>
    public partial class PimsPropertyImprovementType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify property improvement type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyImprovementTypeCode; set => PropertyImprovementTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PropertyImprovementType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsPropertyImprovementType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
