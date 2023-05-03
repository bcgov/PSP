using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsCompReqH120 class, provides an entity for the datamodel to compensation requisition financial information.
    /// </summary>
    public partial class PimsCompReqH120 : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.CompReqFinActivity; set => this.CompReqFinActivity = value; }
    }
}
