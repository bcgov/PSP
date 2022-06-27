using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectRiskType class, provides an entity for the datamodel to manage a list of project risk types.
    /// </summary>
    public partial class PimsProjectRiskType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project risk type.
        /// </summary>
        [NotMapped]
        public string Id { get => ProjectRiskTypeCode; set => ProjectRiskTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a ProjectRiskType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsProjectRiskType(string id) : this()
        {
            Id = id;
        }
        #endregion
    }
}
