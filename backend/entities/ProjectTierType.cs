using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectTierType class, provides an entity for the datamodel to manage a list of project tier types.
    /// </summary>
    public partial class PimsProjectTierType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project tier type.
        /// </summary>
        [NotMapped]
        public string Id { get => ProjectTierTypeCode; set => ProjectTierTypeCode = value; }

        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectTierType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsProjectTierType(string id) : this()
        {
            Id = id;
        }
        #endregion
    }
}
