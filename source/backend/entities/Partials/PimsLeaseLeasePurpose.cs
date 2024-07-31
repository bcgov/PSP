using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseLeasePurpose class, provides an entity for the datamodel to manage Lease Purposes types.
    /// </summary>
    public partial class PimsLeaseLeasePurpose : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => LeaseLeasePurposeId; set => LeaseLeasePurposeId = value; }
    }
}
