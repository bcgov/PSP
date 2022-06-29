
namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// GenericNoteModel class, provides a model to represent notes.
    /// </summary>
    public class GenericNoteModel : BaseModel
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

        #endregion

    }
}
