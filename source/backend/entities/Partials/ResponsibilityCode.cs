using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsResponsibilityCode class, provides an entity for the datamodel to manage a list of responsibility codes.
    /// </summary>
    public partial class PimsResponsibilityCode : IFinancialCodeEntity
    {
        [NotMapped]
        public long Internal_Id { get => this.Id; set => this.Id = value; }
    }
}
