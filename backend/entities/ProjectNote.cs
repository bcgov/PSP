using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectNote class, provides an entity for the datamodel to manage project notes.
    /// </summary>
    [MotiTable("PIMS_PROJECT_NOTE", "PROJNT")]
    public class ProjectNote : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project note.
        /// </summary>
        [Column("PROJECT_NOTE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the project.
        /// </summary>
        [Column("PROJECT_ID")]
        public long ProjectId { get; set; }

        /// <summary>
        /// get/set - The project the note belongs to.
        /// </summary>
        public Project Project { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectNote class.
        /// </summary>
        public ProjectNote() { }

        /// <summary>
        /// Create a new instance of a ProjectNote class.
        /// </summary>
        /// <param name="project"></param>
        public ProjectNote(Project project)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.ProjectId = project.Id;
        }
        #endregion
    }
}
