using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropActInvolvedParty class, provides an entity for the datamodel to manage property activity involved parties.
    /// </summary>
    public partial class PimsPropActInvolvedParty : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropActInvolvedPartyId; set => this.PropActInvolvedPartyId = value; }
        #endregion
    }
}
