using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ActInstPropAcqFile class, provides an entity for the datamodel to manage acquisition properties.
    /// </summary>
    public partial class PimsActInstPropAcqFile : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ActInstPropAcqFileId; set => this.ActInstPropAcqFileId = value; }
        #endregion
    }
}
