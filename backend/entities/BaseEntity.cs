using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// BaseEntity class, provides a way to inherit shared properties on all entities.
    /// </summary>
    public abstract class BaseEntity : IBaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        [Column("CONCURRENCY_CONTROL_NUMBER", Order = 100)]
        public long RowVersion { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a BaseEntity class.
        /// Initializes the default values.
        /// </summary>
        public BaseEntity()
        {
        }
        #endregion
    }
}
