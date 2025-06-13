using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    public abstract class PimsNoteRelationship : StandardIdentityBaseAppEntity<long>
    {
        [NotMapped]
        public abstract long ParentId { get; set; }

        [NotMapped]
        public abstract long InternalNoteId { get; set; }
    }
}
