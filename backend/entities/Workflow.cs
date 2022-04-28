using System;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// Workflow class, provides an entity for the datamodel to manage workflows.
    /// </summary>
    public partial class PimsWorkflowModel : IDisableBaseAppEntity
    {
        #region Constructors
        /// <summary>
        /// Create a new instance of a Workflow class.
        /// </summary>
        /// <param name="workflowTypeId"></param>
        public PimsWorkflowModel(string workflowTypeId) : this()
        {
            if (string.IsNullOrWhiteSpace(workflowTypeId))
            {
                throw new ArgumentException($"Argument '{nameof(workflowTypeId)}' is required.", nameof(workflowTypeId));
            }

            this.WorkflowModelTypeCode = workflowTypeId;
        }
        #endregion
    }
}
