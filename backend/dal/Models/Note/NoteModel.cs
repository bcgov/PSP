
namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// NoteModel class, provides a model to represent notes associated to entities.
    /// </summary>
    public class NoteModel
    {
        #region Properties

        /// <summary>
        /// get/set - The id for this note.
        /// </summary>
        public long Id { get; set; }

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
