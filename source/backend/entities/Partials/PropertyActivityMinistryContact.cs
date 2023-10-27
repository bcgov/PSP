using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropActMinContact class, provides an entity for the datamodel to manage property activity ministry contacts.
    /// </summary>
    public partial class PimsPropActMinContact : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropActMinContactId; set => this.PropActMinContactId = value; }
        #endregion
    }
}
