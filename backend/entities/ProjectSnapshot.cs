using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectSnapshot class, provides an entity for the datamodel to manage project snapshots which are used for reporting.
    /// </summary>
    [MotiTable("PIMS_PROJECT_SNAPSHOT", "PRJSNP")]
    public class ProjectSnapshot : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key provides a unique identity for the project snapshot.
        /// </summary>
        [Column("PROJECT_SNAPSHOT_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the owning project.
        /// </summary>
        [Column("PROJECT_ID")]
        public long ProjectId { get; set; }

        /// <summary>
        /// get/set - The project this snapshot is from.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// get/set - The date that this snapshot was taken.
        /// </summary>
        [Column("SNAPSHOT_ON")]
        public DateTime SnapshotOn { get; set; }

        /// <summary>
        /// get/set - Additional serialized metadata.
        /// </summary>
        [Column("METADATA")]
        public string Metadata { get; set; }

        #region Financials
        /// <summary>
        /// get/set - The net book value.
        /// </summary>
        [Column("NET_BOOK")]
        public decimal? NetBook { get; set; }

        /// <summary>
        /// get/set - The market value.
        /// </summary>
        [Column("MARKET")]
        public decimal? Market { get; set; }

        /// <summary>
        /// get/set - The assessed value.
        /// </summary>
        [Column("ASSESSED")]
        public decimal? Assessed { get; set; }

        /// <summary>
        /// get/set - The appraised value.
        /// </summary>
        [Column("APPRAISED")]
        public decimal? Appraised { get; set; }
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectSnapshot class.
        /// </summary>
        public ProjectSnapshot() { }

        /// <summary>
        /// Create a new instance of a ProjectSnapshot class, initializes with specified arguments
        /// </summary>
        /// <param name="project"></param>
        public ProjectSnapshot(Project project)
        {
            this.ProjectId = project?.Id ?? throw new ArgumentNullException(nameof(project));

            this.SnapshotOn = DateTime.UtcNow;
            this.Project = project;

            this.NetBook = project.NetBook;
            this.Market = project.Market;
            this.Assessed = project.Assessed;
            this.Appraised = project.Appraised;

            this.Metadata = project.Metadata;
        }
        #endregion
    }
}
