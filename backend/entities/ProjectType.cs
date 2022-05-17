using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectType class, provides an entity for the datamodel to manage a list of project types.
    /// </summary>
    public partial class PimsProjectType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project status type.
        /// </summary>
        [NotMapped]
        public string Id { get => ProjectTypeCode; set => ProjectTypeCode = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsProjectType(string id) : this()
        {
            Id = id;
        }
        #endregion
    }
}
