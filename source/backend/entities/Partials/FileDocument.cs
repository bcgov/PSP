using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    public abstract class PimsFileDocument : StandardIdentityBaseAppEntity<long>
    {
        [NotMapped]
        public abstract long FileId { get; set; }
    }
}
