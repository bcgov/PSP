
namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// Note class, provides a model to represent notes.
    /// </summary>
    public class GenericNote
    {
        #region Properties

        /// <summary>
        /// get/set - The model id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The parent's id. Notes are associated to parent entities
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// get/set - The note text contents.
        /// </summary>
        public string NoteText { get; set; }

        /// <summary>
        /// get/set - The concurrency number.
        /// </summary>
        public long RowVersion { get; set; }

        #endregion

    }
}
