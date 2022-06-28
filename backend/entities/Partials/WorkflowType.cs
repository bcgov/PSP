using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// WorkflowType class, provides an entity for the datamodel to manage a list of workflow types.
    /// </summary>
    public partial class PimsWorkflowModelType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify workflow model type.
        /// </summary>
        [NotMapped]
        public string Id { get => WorkflowModelTypeCode; set => WorkflowModelTypeCode = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a WorkflowType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsWorkflowModelType(string id) : this()
        {
            Id = id;
        }
        #endregion
    }
}
