using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseTerm class, provides an entity for the datamodel to manage lease terms.
    /// </summary>
    public partial class PimsLeaseTerm : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Id { get => this.LeaseTermId; set => this.LeaseTermId = value; }
        #endregion
    }
}
