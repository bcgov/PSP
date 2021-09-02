using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// TaskType class, provides an entity for the datamodel to manage a list of task types.
    /// </summary>
    [MotiTable("PIMS_TASK_TEMPLATE_TYPE", "TSKTMT")]
    public class TaskType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify task template type.
        /// </summary>
        [Column("TASK_TEMPLATE_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - A collection of tasks.
        /// </summary>
        public ICollection<Task> Tasks { get; } = new List<Task>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a TaskType class.
        /// </summary>
        public TaskType() { }

        /// <summary>
        /// Create a new instance of a TaskType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public TaskType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
