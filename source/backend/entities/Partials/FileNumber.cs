
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsFileNumber class, provides an entity for the datamodel to manage Historic File Numbers.
    /// </summary>
    public partial class PimsFileNumber : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.FileNumberId; set => this.FileNumberId = value; }
        #endregion
    }
}
