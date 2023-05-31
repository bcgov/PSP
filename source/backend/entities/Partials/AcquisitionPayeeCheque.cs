using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqPayeeCheque class, provides an entity for the datamodel to manage compensation payee cheques.
    /// </summary>
    public partial class PimsAcqPayeeCheque : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AcqPayeeChequeId; set => this.AcqPayeeChequeId = value; }
        #endregion
    }
}