namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// NoteModel class, provides a model to represent notes associated to entities.
    /// </summary>
    public class NoteModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The id for this note.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The note text contents.
        /// </summary>
        public string Note { get; set; }
        #endregion
    }
}
