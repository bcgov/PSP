using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionAgreement class, provides an entity for the datamodel to manage disposition agreements.
    /// </summary>
    public partial class PimsDispositionAgreement : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.DispositionAgreementId; set => this.DispositionAgreementId = value; }
        #endregion

    }
}
