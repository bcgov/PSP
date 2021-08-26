using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// WorkflowType class, provides an entity for the datamodel to manage a list of workflow types.
    /// </summary>
    [MotiTable("PIMS_WORKFLOW_MODEL_TYPE", "WFLMDT")]
    public class WorkflowType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify workflow model type.
        /// </summary>
        [Column("WORKFLOW_MODEL_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - A collection of workflows.
        /// </summary>
        public ICollection<Workflow> Workflows { get; } = new List<Workflow>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a WorkflowType class.
        /// </summary>
        public WorkflowType() { }

        /// <summary>
        /// Create a new instance of a WorkflowType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public WorkflowType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
