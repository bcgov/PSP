using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyContact class, provides an entity for the datamodel to manage property contacts.
    /// </summary>
    public partial class PimsPropertyContact : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyContactId; set => this.PropertyContactId = value; }
        #endregion
    }
}
