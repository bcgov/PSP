namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// EntityNoteModel class, provides a model to represent notes associated to entities.
    /// </summary>
    public class EntityNoteModel
    {
        #region Properties

        /// <summary>
        /// get/set - The id for this entity-note association.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The parent entity that owns this note. Notes are associated to parent entities.
        /// </summary>
        public ParentModel Parent { get; set; }

        /// <summary>
        /// get/set - The note model.
        /// </summary>
        public NoteModel Note { get; set; }

        public long RowVersion { get; set; }

        #endregion
    }
}
