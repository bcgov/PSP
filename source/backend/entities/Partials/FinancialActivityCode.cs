using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsFinancialActivityCode class, provides an entity for the datamodel to manage a list of financial activity codes.
    /// </summary>
    public partial class PimsFinancialActivityCode : IFinancialCodeEntity
    {
        [NotMapped]
        public long Internal_Id { get => this.Id; set => this.Id = value; }
    }
}
