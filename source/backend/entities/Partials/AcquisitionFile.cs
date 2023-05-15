using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionFile class, provides an entity for the datamodel to manage acquisition files.
    /// </summary>
    public partial class PimsAcquisitionFile : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionFileId; set => this.AcquisitionFileId = value; }

        public bool PersonIsAssignedToFile(long personId)
        {
            return PimsAcquisitionFilePeople is not null && PimsAcquisitionFilePeople.Any(x => x.PersonId == personId);
        }
        #endregion
    }
}
