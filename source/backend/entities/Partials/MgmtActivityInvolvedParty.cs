using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsMgmtActInvolvedParty class, provides an entity for the datamodel to manage activity involved parties.
    /// </summary>
    public partial class PimsMgmtActInvolvedParty : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => MgmtActInvolvedPartyId; set => MgmtActInvolvedPartyId = value; }
    }
}
