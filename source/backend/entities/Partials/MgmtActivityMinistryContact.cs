using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsMgmtActMinContact class, provides an entity for the datamodel to manage management activity ministry contacts.
    /// </summary>
    public partial class PimsMgmtActMinContact : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => MgmtActMinContactId; set => MgmtActMinContactId = value; }
    }
}
