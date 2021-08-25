using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectType class, provides an entity for the datamodel to manage a list of project types.
    /// </summary>
    [MotiTable("PIMS_PROJECT_TYPE", "PRJTYP")]
    public class ProjectType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project status type.
        /// </summary>
        [Column("PROJECT_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of projects.
        /// </summary>
        public ICollection<Project> Projects { get; } = new List<Project>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectType class.
        /// </summary>
        public ProjectType() { }

        /// <summary>
        /// Create a new instance of a ProjectType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public ProjectType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
