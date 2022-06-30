namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// GenericNoteModel class, provides a model to represent notes associated to entities.
    /// </summary>
    public class GenericNoteModel
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

        public long RowVersion { get; set; }

        #endregion
    }
}
