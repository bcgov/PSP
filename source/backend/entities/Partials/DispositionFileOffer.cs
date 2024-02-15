using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionOffer class, provides an entity for the datamodel.
    /// </summary>
    public partial class PimsDispositionOffer : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.DispositionOfferId; set => this.DispositionOfferId = value; }
    }
}
