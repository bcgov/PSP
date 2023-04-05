using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAgreement class, provides an entity for the datamodel to manage agreements.
    /// </summary>
    public partial class PimsAgreement : StandardIdentityBaseAppEntity<string>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override string Internal_Id { get => this.AgreementId; set => this.AgreementId = value; }
        #endregion
    }
}
