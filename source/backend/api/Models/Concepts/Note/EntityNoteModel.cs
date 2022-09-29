namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// EntityNoteModel class, provides a model to represent notes associated to entities.
    /// </summary>
    public class EntityNoteModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The id for this entity-note association.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The parent entity that owns this note. Notes are associated to parent entities.
        /// </summary>
        public NoteParentModel Parent { get; set; }

        /// <summary>
        /// get/set - The note model.
        /// </summary>
        public NoteModel Note { get; set; }

        #endregion
    }
}
