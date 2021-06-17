namespace Pims.Api.Areas.Project.Models.Status
{
    /// <summary>
    /// WorkflowModel class, provides a model to represent the project workflows.
    /// </summary>
    public class WorkflowModel : Api.Models.CodeModel
    {
        /// <summary>
        /// get/set - Primary key to identify workflow.
        /// </summary>
        public long Id { get; set; }
    }
}
