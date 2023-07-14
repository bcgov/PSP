using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsInthldrPropInterest class, provides an entity for the datamodel to manage interest holder properties.
    /// </summary>
    public partial class PimsInthldrPropInterest : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PimsInthldrPropInterestId; set => this.PimsInthldrPropInterestId = value; }
        #endregion
    }
}
