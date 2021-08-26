using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectProperty class, provides an entity for the datamodel to manage project properties.
    /// </summary>
    [MotiTable("PIMS_PROJECT_PROPERTY", "PRJPRP")]
    public class ProjectProperty : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the project property.
        /// </summary>
        [Column("PROJECT_PROPERTY_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the project.
        /// </summary>
        [Column("PROJECT_ID")]
        public long ProjectId { get; set; }

        /// <summary>
        /// get/set - The project.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// get/set - The foreign key to the property.
        /// </summary>
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - The property.
        /// </summary>
        public Property Property { get; set; }

        /// <summary>
        /// get/set - Whether this user organization is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectProperty class.
        /// </summary>
        public ProjectProperty() { }

        /// <summary>
        /// Create a new instance of a ProjectProperty class.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="property"></param>
        public ProjectProperty(Project project, Property property)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.ProjectId = project.Id;
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
            this.PropertyId = property.Id;
        }
        #endregion
    }
}
