using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsWorkActivityCode class, provides an entity for the datamodel to manage a list of work activity codes.
    /// </summary>
    public partial class PimsWorkActivityCode : IFinancialCodeEntity
    {
        [NotMapped]
        public long Internal_Id { get => this.Id; set => this.Id = value; }
    }
}
