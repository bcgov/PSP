using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsYearlyFinancialCode class, provides an entity for the datamodel to manage a list of yearly financial codes.
    /// </summary>
    public partial class PimsYearlyFinancialCode : IFinancialCodeEntity
    {
        [NotMapped]
        public long Internal_Id { get => this.Id; set => this.Id = value; }
    }
}
