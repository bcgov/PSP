using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsProjectPerson class, provides an entity for the datamodel to manage project persons.
    /// </summary>
    public partial class PimsProjectPerson : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.ProjectPersonId; set => this.ProjectPersonId = value; }
    }
}
