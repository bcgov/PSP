using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// H120Category class, provides an entity for the datamodel to manage acquisition files.
    /// </summary>
    public partial class PimsH120Category : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.H120CategoryId; set => this.H120CategoryId = value; }
        #endregion
    }
}
