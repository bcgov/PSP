using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectNote class, provides an entity for the datamodel to manage project notes.
    /// </summary>
    [MotiTable("PIMS_PROJECT_NOTE", "PROJNT")]
    public class ProjectNote : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key identity insert.
        /// </summary>
        [Column("PROJECT_NOTE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the owning project.
        /// </summary>
        [Column("PROJECT_ID")]
        public long ProjectId { get; set; }

        /// <summary>
        /// get/set - the project that this note belongs to.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// get/set - The type of note.
        /// </summary>
        [Column("NOTE_TYPE")]
        public NoteTypes NoteType { get; set; }

        /// <summary>
        /// get/set - The note.
        /// </summary>
        [Column("NOTE")]
        public string Note { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectNote class.
        /// </summary>
        public ProjectNote() { }

        /// <summary>
        /// Create a new instance of a ProjectNote class, initializes with specified arguments.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="type"></param>
        /// <param name="note"></param>
        public ProjectNote(Project project, NoteTypes type, string note)
        {
            this.Note = note ?? throw new ArgumentNullException(nameof(project));
            this.NoteType = type;
            this.ProjectId = project?.Id ?? throw new ArgumentNullException(nameof(project));
            this.Project = project;
        }
        #endregion
    }
}
