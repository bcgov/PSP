using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectStatusType class, provides an entity for the datamodel to manage a list of project status types.
    /// </summary>
    [MotiTable("PIMS_PROJECT_STATUS_TYPE", "PRJSTY")]
    public class ProjectStatusType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project status type.
        /// </summary>
        [Column("PROJECT_STATUS_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of projects.
        /// </summary>
        public ICollection<Project> Projects { get; } = new List<Project>();

        /// <summary>
        /// get/set - A code to identify a group of related status.
        /// </summary>
        [Column("CODE_GROUP")]
        public string CodeGroup { get; set; }

        /// <summary>
        /// get/set - Text to display the status.
        /// </summary>
        [Column("TEXT")]
        public string Text { get; set; }

        /// <summary>
        /// get/set - Whether this status is a milestone.
        /// </summary>
        [Column("IS_MILESTONE")]
        public bool IsMilestone { get; set; }

        /// <summary>
        /// get/set - Whether this status is a terminal status.
        /// </summary>
        [Column("IS_TERMINAL")]
        public bool IsTerminal { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectStatusType class.
        /// </summary>
        public ProjectStatusType() { }

        /// <summary>
        /// Create a new instance of a ProjectStatusType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codeGroup"></param>
        /// <param name="description"></param>
        /// <param name="text"></param>
        public ProjectStatusType(string id, string codeGroup, string description, string text) : base(id, description)
        {
            this.CodeGroup = codeGroup ?? throw new ArgumentNullException(nameof(codeGroup));
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
        }
        #endregion
    }
}
