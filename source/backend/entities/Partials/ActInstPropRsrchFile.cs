using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ActInstPropRsrchFile class, provides an entity for the datamodel to manage reseach properties.
    /// </summary>
    public partial class PimsActInstPropRsrchFile : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ActInstPropRsrchFileId; set => this.ActInstPropRsrchFileId = value; }
        #endregion
    }
}
