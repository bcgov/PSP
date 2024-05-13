using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsHistoricalFileNumber class, provides an entity for the datamodel to manage Historical File Numbers.
    /// </summary>
    public partial class PimsHistoricalFileNumber : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.HistoricalFileNumberId; set => this.HistoricalFileNumberId = value; }
        #endregion
    }
}
